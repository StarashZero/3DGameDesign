using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstController : MonoBehaviour, ISceneController, IUserAction
{
    GameObject map;                     //地图管理
    GameObject player;                  //玩家管理
    UserGUI userGUI;                    //用户交互
    IActionManager actionManager;       //动作管理
    AreaController areaController;      //怪兽管理
    void Start()
    {
        SSDirector.GetInstance().CurrentScenceController = this;
        gameObject.AddComponent<MonsterFactory>();
        gameObject.AddComponent<AreaController>();
        areaController = Singleton<AreaController>.Instance;
        gameObject.AddComponent<CCActionManager>();
        gameObject.AddComponent<UserGUI>();
        userGUI = Singleton<UserGUI>.Instance;
        actionManager = Singleton<CCActionManager>.Instance;
        LoadResources();
        GameObject.FindWithTag("MainCamera").AddComponent<FollowManager>();
        actionManager.FollowPlayer(GameObject.FindWithTag("MainCamera"), 3.5f, 5f, 6);
    }

    //订阅各事件
    private void OnEnable()
    {
        AreaController.followAction += FollowAction;
        AreaController.monsterMoveAction += MonsterMoveAction;
        AreaController.victory += Victory;
        PlayerManager.dealDamage += DealPlayerDamage;
        MonsterManager.dealDamage += DealMonsterDamage;
        DetectPlace.setArea += SetArea;
    }

    //取消订阅
    private void OnDisable()
    {
        AreaController.followAction -= FollowAction;
        AreaController.monsterMoveAction -= MonsterMoveAction;
        AreaController.victory -= Victory;
        PlayerManager.dealDamage -= DealPlayerDamage;
        MonsterManager.dealDamage -= DealMonsterDamage;
        DetectPlace.setArea -= SetArea;
    }

    //加载资源
    public void LoadResources()
    {
        if (map == null)
            map = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Map"), Vector3.zero, Quaternion.identity);
        if (player == null)
        {
            player = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/SapphiArtchan"), Vector3.zero, Quaternion.identity);
            player.AddComponent<PlayerManager>();
        }
        player.GetComponent<PlayerManager>().health = 10;
        userGUI.SetPlayerHealth(10);
        userGUI.SetPoints(0);
        player.transform.position = new Vector3(6, -1.5f, 5);
        actionManager.MovePlayer(player, 0, 0);
    }

    //处理胜利事件
    public void Victory()
    {
        if (userGUI.gameOver|| userGUI.victory)
            return;
        userGUI.victory = true;
        areaController.FreeAll();
        player.GetComponent<PlayerManager>().moveable = false;
        player.GetComponent<PlayerManager>().Win();
    }

    //处理玩家受伤事件
    public void DealPlayerDamage(GameObject sender)
    {
        userGUI.SetPlayerHealth(sender.GetComponent<PlayerManager>().health);
        if (sender.GetComponent<PlayerManager>().health == 0)
        {
            areaController.FreeAll();
            sender.GetComponent<PlayerManager>().moveable = false;
            userGUI.gameOver = true;
            userGUI.victory = false;
        }
    }

    //处理怪兽受伤事件
    public void DealMonsterDamage(GameObject sender)
    {
        userGUI.SetMonsterHealth(sender.GetComponent<MonsterManager>().health);
        userGUI.AddPoints(sender.GetComponent<MonsterManager>().health == 0 ? 2 : 1);
    }

    //玩家移动
    public void MovePlayer(float speed, float direction)
    {
        if (player.GetComponent<PlayerManager>().moveable)
            actionManager.MovePlayer(player, speed, direction);
    }

    //玩家跳跃
    public void Jump()
    {
        player.GetComponent<PlayerManager>().Jump();
    }

    //玩家攻击
    public void Hit()
    {
        player.GetComponent<PlayerManager>().Hit();
    }

    //处理怪兽追击事件
    public void FollowAction(GameObject follower, float distanceAway, float distanceUp, float speed)
    {
        actionManager.FollowPlayer(follower, distanceAway, distanceUp, speed);
    }

    //处理怪兽巡逻事件
    public void MonsterMoveAction(GameObject monster, float speed)
    {
        actionManager.MoveMonster(monster, speed);
    }

    //游戏重开
    public void Restart()
    {
        LoadResources();
        player.GetComponent<PlayerManager>().Revive();
        areaController.GameStart();
        userGUI.gameOver = false;
        userGUI.victory = false;
    }

    //设置玩家区域
    public void SetArea(float x, float y)
    {
        int playerArea;
        x += 4.5f;
        y += 4.5f;
        if ((int)x == 9 && (int)y == 9)
            playerArea = -1;
        else if ((int)x == 9 && (int)y == 0)
            playerArea = 0;
        else if ((int)x == 0 && (int)y == 0)
            playerArea = 1;
        else if ((int)x == 9 && (int)y == -9)
            playerArea = 2;
        else if (((int)x == 0 || (int)x == -9) && (int)y == -9)
            playerArea = 3;
        else
            playerArea = 4;
        areaController.SetArea(playerArea);

    }

    void Update()
    {

    }
}
