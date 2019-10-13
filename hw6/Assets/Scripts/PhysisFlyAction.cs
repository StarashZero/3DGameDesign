using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysisFlyAction : SSAction
{
    float speed;            //水平速度
    Vector3 direction;      //飞行方向

    //生产函数(工厂模式)
    public static PhysisFlyAction GetSSAction(Vector3 direction, float speed)
    {
        PhysisFlyAction action = ScriptableObject.CreateInstance<PhysisFlyAction>();
        action.speed = speed;
        action.direction = direction;
        return action;
    }

    public override void Start()
    {
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
        //为物体增加水平初速度
        gameObject.GetComponent<Rigidbody>().velocity = speed * direction;
    }

    public override void Update()
    {
        //如果飞碟到达底部，则动作结束，进行回调
        if (this.transform.position.y < -6)
        {
            this.destroy = true;
            this.enable = false;
            this.callback.SSActionEvent(this);
        }

    }
}
