using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCActionManager : SSActionManager, ISSActionCallback, IActionManager
{
    FollowPlayerAction follow;
    MovePlayerAction movePlayer;
    MoveMonsterAction monsterAction;
    protected new void Start()
    {
    }

    //移动玩家
    public void MovePlayer(GameObject player, float speed, float direction)
    {
        PlayerManager playerManager = player.GetComponent<PlayerManager>();
        if (playerManager == null)
            return;
        playerManager.speed = speed;
        playerManager.direction = direction;
        if (!playerManager.moveable)
        {
            playerManager.moveable = true;
            movePlayer = MovePlayerAction.GetSSAction();
            RunAction(player, movePlayer, this);
        }
    }

    //跟随玩家
    public void FollowPlayer(GameObject follower, float distanceAway, float distanceUp, float speed)
    {
        FollowManager followManager = follower.GetComponent<FollowManager>();
        if (followManager == null)
            return;
        if (!followManager.followable)
        {
            followManager.followable = true;
            follow = FollowPlayerAction.GetSSAction(distanceAway, distanceUp, speed);
            RunAction(follower, follow, this);
        }
    }

    //巡逻
    public void MoveMonster(GameObject monster, float speed)
    {
        MonsterManager monsterManager = monster.GetComponent<MonsterManager>();
        if (monsterManager == null)
            return;
        if (!monsterManager.moveable)
        {
            monsterManager.moveable = true;
            monsterAction = MoveMonsterAction.GetSSAction(speed);
            RunAction(monster, monsterAction, this);
        }
    }

    //回调函数
    public void SSActionEvent(SSAction source,
    SSActionEventType events = SSActionEventType.Competed,
    int intParam = 0,
    string strParam = null,
    Object objectParam = null)
    {
        
    }
}
