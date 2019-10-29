using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerAction : SSAction
{
    float distanceAway;             //与玩家的水平距离
    float distanceUp;               //与玩家的垂直距离
    float speed;                    //跟随速度

    Vector3 targetPosition; 

    Transform player;

    //生产函数(工厂模式)
    public static FollowPlayerAction GetSSAction(float distanceAway, float distanceUp, float speed)
    {
        FollowPlayerAction action = ScriptableObject.CreateInstance<FollowPlayerAction>();
        action.distanceAway = distanceAway;
        action.distanceUp = distanceUp;
        action.speed = speed;
        return action;
    }

    public override void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    public override void Update()
    {
        if (gameObject.GetComponent<FollowManager>().followable)
        {
            if (gameObject.GetComponent<FollowManager>().stop)
                return;
            targetPosition = player.position + Vector3.up * distanceUp + (gameObject.GetComponent<FollowManager>().lookat?-player.forward * distanceAway: player.forward * distanceAway);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * speed * 1.6f);
            transform.LookAt(player);
        }
        else
        {
            this.destroy = true;
            this.enable = false;
            this.callback.SSActionEvent(this);
        }
    }
}
