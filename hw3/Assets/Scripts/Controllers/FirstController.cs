using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstController : MonoBehaviour, ISceneController, IUserAction
{
    private LandModelController rightLandRoleController;                        //右岸控制器
    private LandModelController leftLandRoleController;                         //左岸控制器
    private RiverModel riverModel;                                              //河流Model
    private BoatController boatRoleController;                                  //船控制器
    private RoleModelController[] roleModelControllers;                         //人物控制器集合
    private MoveController moveController;                                      //移动控制器
    private bool isRuning;                                                      //游戏进行状态
    private float time;                                                         //游戏进行时间

    //导入资源
    public void LoadResources()
    {
        //人物初始化
        roleModelControllers = new RoleModelController[6];
        for (int i = 0; i < 6; i++)
        {
            roleModelControllers[i] = new RoleModelController();
            roleModelControllers[i].CreateRole(PositionModel.roles[i], i < 3 ? true : false, i);
        }
        //左右岸初始化
        leftLandRoleController = new LandModelController();
        leftLandRoleController.CreateLand("left_land", PositionModel.left_land);
        rightLandRoleController = new LandModelController();
        rightLandRoleController.CreateLand("right_land", PositionModel.right_land);
        //将人物添加并定位至左岸  
        foreach (RoleModelController roleModelController in roleModelControllers)
        {
            roleModelController.GetRoleModel().role.transform.localPosition = leftLandRoleController.AddRole(roleModelController.GetRoleModel());
        }
        //河流Model实例化
        riverModel = new RiverModel(PositionModel.river);
        //船初始化
        boatRoleController = new BoatController();
        boatRoleController.CreateBoat(PositionModel.left_boat);
        //移动控制器实例化
        moveController = new MoveController();
        //数据初始化
        isRuning = true;
        time = 60;
    }

    //移动船
    public void MoveBoat()
    {
        //判断当前游戏是否在进行，同时是否有对象正在移动
        if ((!isRuning) || moveController.GetIsMoving())
            return;
        //判断船在左侧还是右侧
        if (boatRoleController.GetBoatModel().isRight)
            moveController.SetMove(PositionModel.left_boat, boatRoleController.GetBoatModel().boat);
        else
            moveController.SetMove(PositionModel.right_boat, boatRoleController.GetBoatModel().boat);
        //移动后，将船的位置取反
        boatRoleController.GetBoatModel().isRight = !boatRoleController.GetBoatModel().isRight;
    }

    //移动人物    
    public void MoveRole(RoleModel roleModel)
    {
        //判断当前游戏是否在进行，同时是否有对象正在移动
        if ((!isRuning) || moveController.GetIsMoving())
            return;
 
        if (roleModel.isInBoat)
        {
            //若人在船上，则将其移向岸上
            if (boatRoleController.GetBoatModel().isRight)
                moveController.SetMove(rightLandRoleController.AddRole(roleModel), roleModel.role);
            else
                moveController.SetMove(leftLandRoleController.AddRole(roleModel), roleModel.role);
            roleModel.isRight = boatRoleController.GetBoatModel().isRight;
            boatRoleController.RemoveRole(roleModel);
        }
        else
        {
            //若人在岸上，则将其移向船
            if (boatRoleController.GetBoatModel().isRight == roleModel.isRight)
            {
                if (roleModel.isRight)
                {
                    rightLandRoleController.RemoveRole(roleModel);
                }
                else
                {
                    leftLandRoleController.RemoveRole(roleModel);
                }
                moveController.SetMove(boatRoleController.AddRole(roleModel), roleModel.role);
            }
        }
    }

    //游戏重置
    public void Restart()
    {
        //对各数据进行初始化
        time = 60;
        leftLandRoleController.CreateLand("left_land", PositionModel.left_land);
        rightLandRoleController.CreateLand("right_land", PositionModel.right_land);
        for (int i = 0; i < 6; i++)
        {
            roleModelControllers[i].CreateRole(PositionModel.roles[i], i < 3 ? true : false, i);
            roleModelControllers[i].GetRoleModel().role.transform.localPosition = leftLandRoleController.AddRole(roleModelControllers[i].GetRoleModel());
        }
        boatRoleController.CreateBoat(PositionModel.left_boat);
        isRuning = true;
    }

    //检测游戏状态
    public void Check()
    {
        if (!isRuning)
            return;
        this.gameObject.GetComponent<UserGUI>().gameMessage = "";
        //判断是否已经胜利
        if (rightLandRoleController.GetLandModel().priestNum == 3)
        {
            this.gameObject.GetComponent<UserGUI>().gameMessage = "You Win!";
            isRuning = false;
        }
        else
        {
            //判断是否已经失败
            /*
             leftPriestNum: 左边牧师数量
             leftDevilNum: 左边恶魔数量
             rightPriestNum: 右边牧师数量
             rightDevilNum: 右边恶魔数量
             若任意一侧，牧师数量不为0且牧师数量少于恶魔数量，则游戏失败
             */
            int leftPriestNum, leftDevilNum, rightPriestNum, rightDevilNum;
            leftPriestNum = leftLandRoleController.GetLandModel().priestNum + (boatRoleController.GetBoatModel().isRight ? 0 : boatRoleController.GetBoatModel().priestNum);
            leftDevilNum = leftLandRoleController.GetLandModel().devilNum + (boatRoleController.GetBoatModel().isRight ? 0 : boatRoleController.GetBoatModel().devilNum);
            if (leftPriestNum != 0 && leftPriestNum < leftDevilNum)
            {
                this.gameObject.GetComponent<UserGUI>().gameMessage = "Game Over!";
                isRuning = false;
            }
            rightPriestNum = rightLandRoleController.GetLandModel().priestNum + (boatRoleController.GetBoatModel().isRight ? boatRoleController.GetBoatModel().priestNum : 0);
            rightDevilNum = rightLandRoleController.GetLandModel().devilNum + (boatRoleController.GetBoatModel().isRight ? boatRoleController.GetBoatModel().devilNum : 0);
            if (rightPriestNum != 0 && rightPriestNum < rightDevilNum)
            {
                this.gameObject.GetComponent<UserGUI>().gameMessage = "Game Over!";
                isRuning = false;
            }
        }
    }



    void Awake()
    {
        SSDirector.GetInstance().CurrentSenceController = this;
        LoadResources();
        this.gameObject.AddComponent<UserGUI>();
    }

    void Update()
    {
        if (isRuning)
        {
            time -= Time.deltaTime;
            this.gameObject.GetComponent<UserGUI>().time = (int)time;
            if (time <= 0)
            {
                this.gameObject.GetComponent<UserGUI>().time = 0;
                this.gameObject.GetComponent<UserGUI>().gameMessage = "Game Over!";
                isRuning = false;
            }
        }
    }

}
