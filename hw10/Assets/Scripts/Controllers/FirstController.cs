using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstController : MonoBehaviour, ISceneController, IUserAction
{
    public CCActionManager actionManager;
    public LandModelController rightLandController;                        //右岸控制器
    public LandModelController leftLandController;                         //左岸控制器
    public RiverModel riverModel;                                              //河流Model
    public BoatModelController boatController;                                  //船控制器
    public RoleModelController[] roleControllers;                         //人物控制器集合
    public PathController pathController;
    public bool isRuning;                                                      //游戏进行状态
    public float time;                                                         //游戏进行时间

    public void JudgeCallback(bool isRuning, string message)
    {
        this.gameObject.GetComponent<UserGUI>().gameMessage = message;
        this.gameObject.GetComponent<UserGUI>().time = (int)time;
        this.isRuning = isRuning;
    }


    //导入资源
    public void LoadResources()
    {
        //人物初始化
        roleControllers = new RoleModelController[6];
        for (int i = 0; i < 6; i++)
        {
            roleControllers[i] = new RoleModelController();
            roleControllers[i].CreateRole(PositionModel.roles[i], i < 3 ? true : false, i);
        }
        //左右岸初始化
        leftLandController = new LandModelController();
        leftLandController.CreateLand("left_land", PositionModel.left_land);
        rightLandController = new LandModelController();
        rightLandController.CreateLand("right_land", PositionModel.right_land);
        //将人物添加并定位至左岸  
        foreach (RoleModelController roleModelController in roleControllers)
        {
            roleModelController.GetRoleModel().role.transform.localPosition = leftLandController.AddRole(roleModelController.GetRoleModel());
        }
        //河流Model实例化
        riverModel = new RiverModel(PositionModel.river);
        //船初始化
        boatController = new BoatModelController();
        boatController.CreateBoat(PositionModel.left_boat);

        pathController = new PathController();
        //数据初始化
        isRuning = true;
        time = 60;
    }

    //移动船
    public void MoveBoat()
    {
        //判断当前游戏是否在进行，同时是否有对象正在移动
        if ((!isRuning) || actionManager.IsMoving())
            return;
        //判断船在左侧还是右侧
        Vector3 destination = boatController.GetBoatModel().isRight ? PositionModel.left_boat : PositionModel.right_boat;
        actionManager.MoveBoat(boatController.GetBoatModel().boat, destination, 5);
        /*        if (boatRoleController.GetBoatModel().isRight)
                    moveController.SetMove(PositionModel.left_boat, boatRoleController.GetBoatModel().boat);
                else
                    moveController.SetMove(PositionModel.right_boat, boatRoleController.GetBoatModel().boat);*/
        //移动后，将船的位置取反
        boatController.GetBoatModel().isRight = !boatController.GetBoatModel().isRight;
    }

    //移动人物    
    public void MoveRole(RoleModel roleModel)
    {
        //判断当前游戏是否在进行，同时是否有对象正在移动
        if ((!isRuning) || actionManager.IsMoving())
            return;

        Vector3 destination, mid_destination;
        if (roleModel.isInBoat)
        {
            //若人在船上，则将其移向岸上
            if (boatController.GetBoatModel().isRight)
                destination = rightLandController.AddRole(roleModel);
            else
                destination = leftLandController.AddRole(roleModel);
            if (roleModel.role.transform.localPosition.y > destination.y)
                mid_destination = new Vector3(destination.x, roleModel.role.transform.localPosition.y, destination.z);
            else
                mid_destination = new Vector3(roleModel.role.transform.localPosition.x, destination.y, destination.z);
            actionManager.MoveRole(roleModel.role, mid_destination, destination, 5);
            roleModel.isRight = boatController.GetBoatModel().isRight;
            boatController.RemoveRole(roleModel);
        }
        else
        {
            //若人在岸上，则将其移向船
            if (boatController.GetBoatModel().isRight == roleModel.isRight)
            {
                if (roleModel.isRight)
                {
                    rightLandController.RemoveRole(roleModel);
                }
                else
                {
                    leftLandController.RemoveRole(roleModel);
                }
                destination = boatController.AddRole(roleModel);
                if (roleModel.role.transform.localPosition.y > destination.y)
                    mid_destination = new Vector3(destination.x, roleModel.role.transform.localPosition.y, destination.z);
                else
                    mid_destination = new Vector3(roleModel.role.transform.localPosition.x, destination.y, destination.z);
                actionManager.MoveRole(roleModel.role, mid_destination, destination, 5);
            }
        }
    }

    //游戏重置
    public void Restart()
    {
        //对各数据进行初始化
        time = 60;
        leftLandController.CreateLand("left_land", PositionModel.left_land);
        rightLandController.CreateLand("right_land", PositionModel.right_land);
        for (int i = 0; i < 6; i++)
        {
            roleControllers[i].CreateRole(PositionModel.roles[i], i < 3 ? true : false, i);
            roleControllers[i].GetRoleModel().role.transform.localPosition = leftLandController.AddRole(roleControllers[i].GetRoleModel());
        }
        boatController.CreateBoat(PositionModel.left_boat);
        GetComponent<JudgeController>().Init();
        isRuning = true;
    }

    //获得帮助
    public void Tips()
    {
        PathNode node = pathController.Search(new int[] { boatController.GetBoatModel().priestNum, boatController.GetBoatModel().devilNum, leftLandController.GetLandModel().priestNum,
        rightLandController.GetLandModel().priestNum, leftLandController.GetLandModel().devilNum, rightLandController.GetLandModel().devilNum, boatController.GetBoatModel().isRight?1:0});
        if (node.GetState() == 0)
        {
            PathNode nNode = node.GetNext();
            TransferState(node, nNode);
        }
    }

    //状态转换  
    private void TransferState(PathNode bNode, PathNode eNode)
    {
        int[] subs = new int[7];
        //求差,找到转换类型
        for (int i = 0; i < 7; i++)
        {
            subs[i] = eNode.state[i] - bNode.state[i];
        }
        if (subs[PathNode.BOAT_PLACE] != 0)
        {
            //船只移动
            MoveBoat();
        }
        else if (subs[PathNode.BOAT_PRIESTS] < 0)
        {
            //船上牧师移动
            for (int i = 0; i < 2; i++)
            {
                if (boatController.GetBoatModel().roles[i]!=null && boatController.GetBoatModel().roles[i].isPriest)
                {
                    MoveRole(boatController.GetBoatModel().roles[i]);
                    break;
                }
            }
        }
        else if (subs[PathNode.BOAT_DEVILS] < 0)
        {
            //船上恶魔移动
            for (int i = 0; i < 2; i++)
            {
                if (boatController.GetBoatModel().roles[i] != null && !boatController.GetBoatModel().roles[i].isPriest)
                {
                    MoveRole(boatController.GetBoatModel().roles[i]);
                    break;
                }
            }
        }
        else if (subs[PathNode.LEFT_PRIESTS] < 0)
        {
            //左岸牧师移动
            for (int i = 0; i < 6; i++)
            {
                if (roleControllers[i].GetRoleModel().isPriest&& !roleControllers[i].GetRoleModel().isInBoat&& !roleControllers[i].GetRoleModel().isRight)
                {
                    MoveRole(roleControllers[i].GetRoleModel());
                    break;
                }
            }
        }
        else if (subs[PathNode.LEFT_DEVILS] < 0)
        {
            //左岸恶魔移动
            for (int i = 0; i < 6; i++)
            {
                if (!roleControllers[i].GetRoleModel().isPriest&& !roleControllers[i].GetRoleModel().isInBoat&& !roleControllers[i].GetRoleModel().isRight)
                {
                    MoveRole(roleControllers[i].GetRoleModel());
                    break;
                }
            }
        }
        else if (subs[PathNode.RIGHT_PRIESTS] < 0)
        {
            //右岸牧师移动
            for (int i = 0; i < 6; i++)
            {
                if (roleControllers[i].GetRoleModel().isPriest && !roleControllers[i].GetRoleModel().isInBoat && roleControllers[i].GetRoleModel().isRight)
                {
                    MoveRole(roleControllers[i].GetRoleModel());
                    break;
                }
            }
        }
        else if (subs[PathNode.RIGHT_DEVILS] < 0)
        {
            //右岸恶魔移动
            for (int i = 0; i < 6; i++)
            {
                if (!roleControllers[i].GetRoleModel().isPriest && !roleControllers[i].GetRoleModel().isInBoat && roleControllers[i].GetRoleModel().isRight)
                {
                    MoveRole(roleControllers[i].GetRoleModel());
                    break;
                }
            }
        }
    }

    void Awake()
    {
        SSDirector.GetInstance().CurrentSenceController = this;
        LoadResources();
        this.gameObject.AddComponent<UserGUI>();
        this.gameObject.AddComponent<CCActionManager>();
        this.gameObject.AddComponent<JudgeController>();
    }

    void Update()
    {
        if (isRuning)
        {
            time -= Time.deltaTime;
            this.gameObject.GetComponent<UserGUI>().time = (int)time;
        }
    }

}
