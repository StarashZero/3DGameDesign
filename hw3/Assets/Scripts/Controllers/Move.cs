using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public bool isMoving = false;                   //判断当前对象是否在移动
    public float speed = 5;                         //移动速度
    public Vector3 destination;                     //目的地
    public Vector3 mid_destination;                 //中转地址   

    void Update()
    {
        //已到达目的地，不进行移动
        if (transform.localPosition == destination)
        {
            isMoving = false;
            return;
        }

        isMoving = true;
        if (transform.localPosition.x != destination.x && transform.localPosition.y != destination.y)
        {
            //还未到达中转地址，向中转地址移动
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, mid_destination, speed * Time.deltaTime);
        }
        else
        {
            //以到达中转地址，向目的地移动
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, destination, speed * Time.deltaTime);
        }
    }
}
