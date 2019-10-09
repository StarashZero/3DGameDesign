# **Unity实现简易打飞碟(Hit UFO)**  
## **前言**  
这是中山大学数据科学与计算机学院2019年3D游戏编程与设计的第四次作业  
所有项目与代码已上传至github当中，欢迎大家访问。  
github个人主页: [https://starashzero.github.io](https://starashzero.github.io)  
3D游戏编程与设计主页: [https://starashzero.github.io/3DGameDesign](https://starashzero.github.io/3DGameDesign)  
本次作业项目地址: [https://github.com/StarashZero/3DGameDesign/tree/master/hw5](https://github.com/StarashZero/3DGameDesign/tree/master/hw5)  
## **游戏简介**  
鼠标作为武器，打爆飞碟!   
![](picture/1.png)  
~~<center>(照例丑到爆的UI)</center>~~  

## **游戏玩法** 
点击飞出来的UFO(各种圆盘)即可得分。 

## **游戏规则**  
1. 正常模式共有五轮飞碟，飞碟数列遵循斐波那契数列，当所有飞碟发射完毕时游戏结束。  
2. 无限模式拥有无限轮数 ~~(其实就是重复)~~  
3. 红色飞碟为1分，绿色飞碟为2分，蓝色飞碟为3分 ~~(RGB!)~~   

## **游戏试玩**  
![](picture/1.gif)   

## **项目要求**  
参考[潘老师的博客第五章](https://pmlpml.github.io/unity3d-learning/05-interaction-with-gameworld)最后一部分  
关于程序的要求如下:   
* 游戏有 n 个 round，每个 round 都包括10 次 trial；
* 每个 trial 的飞碟的色彩、大小、发射位置、速度、角度、同时出现的个数都可能不同。它们由该 round 的 ruler 控制；
* 每个 trial 的飞碟有随机性，总体难度随 round 上升；
* 鼠标点中得分，得分规则按色彩、大小、速度不同计算，规则可自由设定  
* 使用带缓存的工厂模式管理不同飞碟的生产与回收，该工厂必须是场景单实例的！具体实现见参考资源 Singleton 模板类
* 近可能使用前面 MVC 结构实现人机交互与游戏模型分离  

## **项目代码结构**
大部分文件的用途都跟其他几次作业相似，因此这里就不进行解释了。 
![](picture/2.png)   
UML图:  
![](picture/3.png) 

## **各部分代码解释**  
* **Singleton.cs**  
    ```java
    public class Singleton<T> : MonoBehaviour where T: MonoBehaviour
    {
        protected static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = (T)FindObjectOfType(typeof(T));
                    if (instance == null)
                    {
                        Debug.LogError("An instance of " + typeof(T) + " is needed in the scene, but there is none");
                    }
                }
                return instance;
            }
        }
    }
    ```  
    场景单实例类，当所需的实例第一次被需要时，在场景内搜索该实例，下一次使用时不需要搜索直接返回。  
* **DiskData.cs**  
    ```java
    public class DiskData : MonoBehaviour
    {
        public float speed;         //水平速度
        public int points;          //得分
        public Vector3 direction;   //初始方向
    }
    ```  
    飞碟数据，携带飞碟的飞行速度、得分、以及飞行方向。  
* **DiskFactory.cs**  
    ```java
    public class DiskFactory : MonoBehaviour
    {
        public GameObject disk_Prefab;              //飞碟预制

        private List<DiskData> used;                //正被使用的飞碟
        private List<DiskData> free;                //空闲的飞碟

        public void Start()
        {
            used = new List<DiskData>();
            free = new List<DiskData>();
            disk_Prefab = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Disk_Prefab"), Vector3.zero, Quaternion.identity);
            disk_Prefab.SetActive(false);
        }

        public GameObject GetDisk(int round)
        {
            GameObject disk;
            //如果有空闲的飞碟，则直接使用，否则生成一个新的
            if (free.Count > 0)
            {
                disk = free[0].gameObject;
                free.Remove(free[0]);
            }
            else
            {
                disk = GameObject.Instantiate<GameObject>(disk_Prefab, Vector3.zero, Quaternion.identity);
                disk.AddComponent<DiskData>();
            }

            //按照round来设置飞碟属性
            //飞碟的等级 = 0~2之间的随机数 * 轮次数
            //0~4:  红色飞碟  
            //4~7:  绿色飞碟  
            //7~10: 蓝色飞碟
            float level = UnityEngine.Random.Range(0, 2f) * (round + 1);
            if (level < 4)
            {
                disk.GetComponent<DiskData>().points = 1;
                disk.GetComponent<DiskData>().speed = 4.0f;
                disk.GetComponent<DiskData>().direction = new Vector3(UnityEngine.Random.Range(-1f, 1f) > 0 ? 2 : -2, 1, 0);
                disk.GetComponent<Renderer>().material.color = Color.red;
            }
            else if (level > 7)
            {
                disk.GetComponent<DiskData>().points = 3;
                disk.GetComponent<DiskData>().speed = 8.0f;
                disk.GetComponent<DiskData>().direction = new Vector3(UnityEngine.Random.Range(-1f, 1f) > 0 ? 2 : -2, 1, 0);
                disk.GetComponent<Renderer>().material.color = Color.blue;
            }
            else
            {
                disk.GetComponent<DiskData>().points = 2;
                disk.GetComponent<DiskData>().speed = 6.0f;
                disk.GetComponent<DiskData>().direction = new Vector3(UnityEngine.Random.Range(-1f, 1f) > 0 ? 2 : -2, 1, 0);
                disk.GetComponent<Renderer>().material.color = Color.green;
            }

            used.Add(disk.GetComponent<DiskData>());

            return disk;
        }

        public void FreeDisk(GameObject disk)
        {
            //找到使用中的飞碟，将其踢出并加入到空闲队列
            foreach (DiskData diskData in used)
            {
                if (diskData.gameObject.GetInstanceID() == disk.GetInstanceID())
                {
                    disk.SetActive(false);
                    free.Add(diskData);
                    used.Remove(diskData);
                    break;
                }

            }
        }
    }
    ```  
    飞碟工厂，负责生产与释放飞碟。  
    + GetDisk:  
    GetDisk用于生产飞碟，首先从free空闲队列中查找是否有可用的飞碟，如果没有则新建一个飞碟。  
    飞碟属性的设置依赖轮次数，将一个0~2之间的随机数乘以轮次数获得level，再根据level来设置属性。  
    这样就具有了轮次越多优质飞碟越多的特性，且前期不会产生高分飞碟。  
    + FreeDisk:  
    FreeDisk用于释放飞碟，将飞碟从used队列中移除并添加到free队列中。  

* **SSAction.cs**
    ```java
    public class SSAction : ScriptableObject
    {
        public bool enable = true;
        public bool destroy = false;

        public GameObject gameObject { get; set; }
        public Transform transform { get; set; }
        public ISSActionCallback callback { get; set; }

        protected SSAction()
        {

        }

        // Start is called before the first frame update
        public virtual void Start()
        {
            throw new System.NotImplementedException();
        }

        // Update is called once per frame
        public virtual void Update()
        {
            throw new System.NotImplementedException();
        }
    }
    ```  
    动作的基类，与之前的项目用途一致。  
* **CCFlyAction.cs**  
    ```java
    public class CCFlyAction : SSAction
    {
        float gravity;          //重力加速度
        float speed;            //水平速度
        Vector3 direction;      //飞行方向
        float time;             //时间

        //生产函数(工厂模式)
        public static CCFlyAction GetSSAction(Vector3 direction, float speed)
        {
            CCFlyAction action = ScriptableObject.CreateInstance<CCFlyAction>();
            action.gravity = 9.8f;
            action.time = 0;
            action.speed = speed;
            action.direction = direction;
            return action;
        }

        public override void Start()
        {
            
        }

        public override void Update()
        {
            time += Time.deltaTime;
            transform.Translate(Vector3.down * gravity * time * Time.deltaTime);
            transform.Translate(direction * speed * Time.deltaTime);
            //如果飞碟到达底部，则动作结束，进行回调
            if (this.transform.position.y < -6)
            {
                this.destroy = true;
                this.enable = false;
                this.callback.SSActionEvent(this);
            }
            
        }
    }
    ```  
    飞行动作，将飞行拆分成水平和垂直两个方向的运动，水平速度恒定，垂直方向施加重力加速度。  
    当飞碟到达底部时，动作结束，将进行回调。  
* **ISSActionCallback**  
    ```java
    public enum SSActionEventType : int { Started, Competed }
    public interface ISSActionCallback
    {
        //回调函数
        void SSActionEvent(SSAction source,
            SSActionEventType events = SSActionEventType.Competed,
            int intParam = 0,
            string strParam = null,
            Object objectParam = null);
    }
    ```  
    回调函数接口，与之前项目用途一致。  
* **SSActionManager.cs**  
    ```java
    public class SSActionManager : MonoBehaviour
    {
        //动作集，以字典形式存在
        private Dictionary<int, SSAction> actions = new Dictionary<int, SSAction>();
        //等待被加入的动作队列(动作即将开始)
        private List<SSAction> waitingAdd = new List<SSAction>();
        //等待被删除的动作队列(动作已完成)
        private List<int> waitingDelete = new List<int>();

        protected void Update()
        {
            //将waitingAdd中的动作保存
            foreach (SSAction ac in waitingAdd)
                actions[ac.GetInstanceID()] = ac;
            waitingAdd.Clear();

            //运行被保存的事件
            foreach (KeyValuePair<int, SSAction> kv in actions)
            {
                SSAction ac = kv.Value;
                if (ac.destroy)
                {
                    waitingDelete.Add(ac.GetInstanceID());
                }
                else if (ac.enable)
                {
                    ac.Update();
                }
            }

            //销毁waitingDelete中的动作
            foreach (int key in waitingDelete)
            {
                SSAction ac = actions[key];
                actions.Remove(key);
                Destroy(ac);
            }
            waitingDelete.Clear();
        }

        //准备运行一个动作，将动作初始化，并加入到waitingAdd
        public void RunAction(GameObject gameObject, SSAction action, ISSActionCallback manager)
        {
            action.gameObject = gameObject;
            action.transform = gameObject.transform;
            action.callback = manager;
            waitingAdd.Add(action);
            action.Start();
        }

        // Start is called before the first frame update
        protected void Start()
        {

        }

    }
    ```  
    动作管理者的基类，和之前项目用途一致。  

* **CCActionManager.cs**
    ```java
    public class CCActionManager : SSActionManager, ISSActionCallback
    {
        //飞行动作
        public CCFlyAction flyAction;
        //控制器
        public FirstController controller;

        protected new void Start()
        {
            controller = (FirstController)SSDirector.GetInstance().CurrentScenceController;
            controller.actionManager = this;
        }

        public void Fly(GameObject disk, float speed, Vector3 direction)
        {
            flyAction = CCFlyAction.GetSSAction(direction, speed);
            RunAction(disk, flyAction, this);
        }

        //回调函数
        public void SSActionEvent(SSAction source,
        SSActionEventType events = SSActionEventType.Competed,
        int intParam = 0,
        string strParam = null,
        Object objectParam = null)
        {
            //飞碟结束飞行后进行回收
            controller.diskFactory.FreeDisk(source.gameObject);
        }
    }
    ```  
    飞行动作管理者，负责生成飞行动作，并接受飞行动作的回调信息，使飞碟被回收。  
* **IUserAction.cs**  
    ```java  
    public interface IUserAction
    {
        void Hit(Vector3 position);
        void Restart();
        void SetMode(bool isInfinite);
    }
    ```  
    用户动作接口，提供点击、重置、选择模式三个函数的接口。  
* **SSDirector.cs**  
    ```java
    public class SSDirector : System.Object
    {
        private static SSDirector _instance;
        public ISceneController CurrentScenceController { get; set; }
        public static SSDirector GetInstance()
        {
            if (_instance == null)
            {
                _instance = new SSDirector();
            }
            return _instance;
        }
    }
    ```  
    导演类，与之前项目用途一致。
* **ISceneController.cs**  
    ```java
    public interface ISceneController
    {
        void LoadResources();
    }
    ```  
    场景控制类接口，与之前项目用途一致。  
* **FirstController.cs**  
    ```java
    public class FirstController : MonoBehaviour, ISceneController, IUserAction
    {
        public CCActionManager actionManager;                   //动作管理者
        public DiskFactory diskFactory;                         //飞碟工厂
        int[] roundDisks;           //对应轮次的飞碟数量
        bool isInfinite;            //游戏当前模式
        int points;                 //游戏当前分数
        int round;                  //游戏当前轮次
        int sendCnt;                //当前已发送的飞碟数量
        float sendTime;             //发送时间

        void Start()
        {
            LoadResources();
        }

        public void LoadResources()
        {
            SSDirector.GetInstance().CurrentScenceController = this;
            gameObject.AddComponent<DiskFactory>();
            gameObject.AddComponent<CCActionManager>();
            gameObject.AddComponent<UserGUI>();
            diskFactory = Singleton<DiskFactory>.Instance;
            sendCnt = 0;
            round = 0;
            sendTime = 0;
            points = 0;
            isInfinite = false;
            roundDisks = new int[] { 3, 5, 8, 13, 21 };
        }

        public void SendDisk()
        {
            //从工厂生成一个飞碟
            GameObject disk = diskFactory.GetDisk(round);
            //设置飞碟的随机位置
            disk.transform.position = new Vector3(-disk.GetComponent<DiskData>().direction.x * 7, UnityEngine.Random.Range(0f, 8f), 0);
            disk.SetActive(true);
            //设置飞碟的飞行动作
            actionManager.Fly(disk, disk.GetComponent<DiskData>().speed, disk.GetComponent<DiskData>().direction);
        }

        public void Hit(Vector3 position)
        {
            Camera ca = Camera.main;
            Ray ray = ca.ScreenPointToRay(position);

            RaycastHit[] hits;
            hits = Physics.RaycastAll(ray);

            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit hit = hits[i];
                if (hit.collider.gameObject.GetComponent<DiskData>() != null)
                {
                    //将飞碟移至底端，触发飞行动作的回调
                    hit.collider.gameObject.transform.position = new Vector3(0, -7, 0);
                    //积分
                    points += hit.collider.gameObject.GetComponent<DiskData>().points;
                    //更新GUI数据
                    gameObject.GetComponent<UserGUI>().points = points;
                }
            }
        }

        public void Restart()
        {
            gameObject.GetComponent<UserGUI>().gameMessage = "";
            round = 0;
            sendCnt = 0;
            points = 0;
            gameObject.GetComponent<UserGUI>().points = points;
        }

        public void SetMode(bool isInfinite)
        {
            this.isInfinite = isInfinite;
        }

        void Update()
        {
            sendTime += Time.deltaTime;
            //每隔1s发送一次飞碟
            if (sendTime > 1)
            {
                sendTime = 0;
                //每次发送至多5个飞碟
                for (int i = 0; i < 5 && sendCnt < roundDisks[round]; i++)
                {
                    sendCnt++;
                    SendDisk();
                }
                //判断是否需要重置轮次，不需要则输出游戏结束
                if (sendCnt == roundDisks[round] && round == roundDisks.Length - 1)
                {
                    if (isInfinite)
                    {
                        round = 0;
                        sendCnt = 0;
                        gameObject.GetComponent<UserGUI>().gameMessage = "";
                    }
                    else
                    {
                        gameObject.GetComponent<UserGUI>().gameMessage = "Game Over!";
                    }
                }
                //更新轮次
                if (sendCnt == roundDisks[round] && round < roundDisks.Length - 1)
                {
                    sendCnt = 0;
                    round++;
                }
            }
        }
    }

    ```  
    场景控制器，负责游戏主要逻辑。  
    + SendDisk:  
    SendDisk用于发射一个飞碟，首先从工厂获得一个飞碟，再为其设置初始位置和飞行动作。  
    + Hit:  
    Hit用于处理用户的点击动作，将用户点击到的飞碟移除，并计算分数。  
    + Restart:  
    Restart用于重置游戏。  
    + Update:  
    Update用于发射飞碟与更新状态，飞碟每1s发射一次，每次做多5只，避免太过拥挤，当飞碟发射完毕后判断是否重置或者结束游戏。  
* **UserGUI.cs**  
    ```java
    public class UserGUI : MonoBehaviour
    {
        private IUserAction userAction;
        public string gameMessage;
        public int points;
        void Start()
        {
            points = 0;
            gameMessage = "";
            userAction = SSDirector.GetInstance().CurrentScenceController as IUserAction;
        }

        void OnGUI()
        {
            //小字体初始化
            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.white;
            style.fontSize = 30;

            //大字体初始化
            GUIStyle bigStyle = new GUIStyle();
            bigStyle.normal.textColor = Color.white;
            bigStyle.fontSize = 50;

            GUI.Label(new Rect(300, 30, 50, 200), "Hit UFO", bigStyle);
            GUI.Label(new Rect(20, 0, 100, 50), "Points: " + points, style);
            GUI.Label(new Rect(310, 100, 50, 200), gameMessage, style);
            if (GUI.Button(new Rect(20, 50, 100, 40), "Restart"))
            {
                userAction.Restart();
            }
            if (GUI.Button(new Rect(20, 100, 100, 40), "Normal Mode"))
            {
                userAction.SetMode(false);
            }
            if (GUI.Button(new Rect(20, 150, 100, 40), "Infinite Mode"))
            {
                userAction.SetMode(true);
            }
            if (Input.GetButtonDown("Fire1"))
            {
                userAction.Hit(Input.mousePosition);
            }
        }
    }

    ```  
    界面类，构建UI并捕捉用户动作。  
