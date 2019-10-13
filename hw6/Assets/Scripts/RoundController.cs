using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundController : MonoBehaviour
{
    FirstController controller;
    IActionManager actionManager;                   //动作管理者
    DiskFactory diskFactory;                         //飞碟工厂
    ScoreRecorder scoreRecorder;
    UserGUI userGUI;
    int[] roundDisks;           //对应轮次的飞碟数量
    bool isInfinite;            //游戏当前模式
    int round;                  //游戏当前轮次
    int sendCnt;                //当前已发送的飞碟数量
    float sendTime;             //发送时间

    void Start()
    {
        controller = (FirstController)SSDirector.GetInstance().CurrentScenceController;
        actionManager = Singleton<CCActionManager>.Instance;
        diskFactory = Singleton<DiskFactory>.Instance;
        scoreRecorder = new ScoreRecorder();
        userGUI = Singleton<UserGUI>.Instance;
        sendCnt = 0;
        round = 0;
        sendTime = 0;
        isInfinite = false;
        roundDisks = new int[] { 3, 5, 8, 13, 21 };
    }

    public void Reset()
    {
        sendCnt = 0;
        round = 0;
        sendTime = 0;
        scoreRecorder.Reset();
    }

    public void Record(DiskData disk)
    {
        scoreRecorder.Record(disk);
    }

    public int GetPoints()
    {
        return scoreRecorder.GetPoints();
    }

    public void SetMode(bool isInfinite)
    {
        this.isInfinite = isInfinite;
    }

    public void SetFlyMode(bool isPhysis)
    {
        actionManager = isPhysis ? Singleton<PhysisActionManager>.Instance : Singleton<CCActionManager>.Instance as IActionManager;
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

    // Update is called once per frame
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
                    userGUI.SetMessage("");
                }
                else
                {
                    userGUI.SetMessage("Game Over!");
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
