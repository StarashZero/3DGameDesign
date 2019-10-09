using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
