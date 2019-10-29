using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowManager : MonoBehaviour
{
    public bool followable;         //是否可跟随
    public bool stop;               //是否暂停
    public bool lookat;             //是否朝向玩家
    public float speed;             //跟随速度
    void Start()
    {
        followable = true;
        lookat = true;
        stop = false;
    }

}
