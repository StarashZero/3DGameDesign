using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaController : MonoBehaviour
{
    public delegate void FollowAction(GameObject follower, float distanceAway, float distanceUp, float speed);      
    public delegate void MonsterMoveAction(GameObject monster, float speed);
    public delegate void Victory();
    public static event FollowAction followAction;              //跟随事件发布
    public static event MonsterMoveAction monsterMoveAction;    //巡逻事件发布
    public static event Victory victory;                        //胜利事件发布

    MonsterFactory monsterFactory;
    GameObject[] monsters;
    int playerArea;
    void Start()
    {
        monsterFactory = Singleton<MonsterFactory>.Instance;
        playerArea = -1;
        monsters = new GameObject[5];
        GameStart();
    }

    //游戏开始，初始化Monster
    public void GameStart()
    {
        for (int temp = 0; temp < 5; temp++)
        {
            monsters[temp] = monsterFactory.GetMonster(temp);
        }
    }

    //设置玩家区域
    public void SetArea(int area)
    {
        playerArea = area;
    }

    //释放所有Monster
    public void FreeAll()
    {
        monsterFactory.FreeAll();
    }

    void Update()
    {
        int cnt = 0;
        for (int temp = 0; temp < 5; temp++)
        {
            if (!monsters[temp].activeSelf)
                continue;
            //当玩家进入区域时，唤醒该区域的Monster，使其追击玩家，否则巡逻
            if (temp == playerArea && monsters[temp].GetComponent<FollowManager>().followable == false)
            {
                monsters[temp].GetComponent<MonsterManager>().moveable = false;
                monsters[temp].GetComponent<MonsterManager>().SetSpeed(monsters[temp].GetComponent<FollowManager>().speed);
                followAction(monsters[temp], 0, 0, monsters[temp].GetComponent<FollowManager>().speed);
            }
            else if (temp != playerArea)
            {
                monsters[temp].GetComponent<FollowManager>().followable = false;
                monsters[temp].GetComponent<MonsterManager>().SetSpeed(0.5f);
                monsterMoveAction(monsters[temp], 0.5f);
            }
            cnt++;
        }
        //如果所有怪兽都不再存在，发布胜利事件
        if (cnt == 0)
            victory();
    }
}
