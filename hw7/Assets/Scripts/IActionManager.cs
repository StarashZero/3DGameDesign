using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IActionManager
{
    void MovePlayer(GameObject player, float speed, float direction);
    void FollowPlayer(GameObject follower, float distanceAway, float distanceUp, float speed);

    void MoveMonster(GameObject monster, float speed);
}
