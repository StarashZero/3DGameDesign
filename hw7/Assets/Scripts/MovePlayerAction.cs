using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayerAction : SSAction
{
    PlayerManager playerManager;
    float counter;                  //计数器

    //生产函数(工厂模式)
    public static MovePlayerAction GetSSAction()
    {
        MovePlayerAction action = ScriptableObject.CreateInstance<MovePlayerAction>();
        return action;
    }

    public override void Start()
    {
        playerManager = gameObject.GetComponent<PlayerManager>();
        counter = 0;
    }

    public override void Update()
    {
        if (playerManager.moveable)
        {
            //攻击时不允许移动
            if (playerManager.IsName("hit01"))
                return;
            float speed = playerManager.speed;
            //玩家后退时将玩家旋转180度并让摄像头不再面向玩家
            if (speed < -0.1)
            {
                speed = -speed;
                if (GameObject.FindWithTag("MainCamera").GetComponent<FollowManager>().lookat)
                {
                    gameObject.transform.Rotate(0, 180, 0);
                    GameObject.FindWithTag("MainCamera").GetComponent<FollowManager>().lookat = false;
                }
            }
            else if (speed > 0.1)
            {
                if (!GameObject.FindWithTag("MainCamera").GetComponent<FollowManager>().lookat)
                {
                    gameObject.transform.Rotate(0, 180, 0);
                    GameObject.FindWithTag("MainCamera").GetComponent<FollowManager>().lookat = true;
                }
            }
            //速度>0.7时增加计数器，1.5s后转为跑步速度，否则最高为0.7
            if (speed >= 0.7)
            {
                counter = (counter + Time.deltaTime) > 1.5f ? 1.5f : counter + Time.deltaTime;
                speed = counter == 1.5f ? speed : 0.7f;
            }
            else
            {
                counter = (counter - Time.deltaTime) < 0 ? 0 : counter - Time.deltaTime;
            }
            //向前移动
            gameObject.transform.Translate(0, 0, speed * 3.6f * Time.deltaTime);
            playerManager.speed = playerManager.speed > 0 ? playerManager.speed - Time.deltaTime : playerManager.speed + Time.deltaTime;
            //进行旋转
            gameObject.transform.Rotate(0, playerManager.direction * 70 * Time.deltaTime, 0);
            playerManager.direction = 0;
            //设置玩家状态机速度
            playerManager.SetSpeed(speed);
        }
        else
        {
            this.destroy = true;
            this.enable = false;
            this.callback.SSActionEvent(this);
        }

    }
}
