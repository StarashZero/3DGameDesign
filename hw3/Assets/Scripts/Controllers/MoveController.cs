using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController
{
    private GameObject moveObject;                      //移动对象

    //判断当前是否在移动
    public bool GetIsMoving()
    {
        return (moveObject != null && moveObject.GetComponent<Move>().isMoving);
    }

    //设置新的移动
    public void SetMove(Vector3 destination, GameObject moveObject)
    {
        //判断新的对象是否已携带Move脚本,若不携带，则为其添加
        Move test;
        this.moveObject = moveObject;
        if (!moveObject.TryGetComponent<Move>(out test))
            moveObject.AddComponent<Move>();
        //设置目的地
        this.moveObject.GetComponent<Move>().destination = destination;
        //设置中转地址
        if (this.moveObject.transform.localPosition.y > destination.y)
            this.moveObject.GetComponent<Move>().mid_destination = new Vector3(destination.x, this.moveObject.transform.localPosition.y, destination.z);
        else
            this.moveObject.GetComponent<Move>().mid_destination = new Vector3(this.moveObject.transform.localPosition.x, destination.y, this.moveObject.transform.localPosition.z);
    }
}
