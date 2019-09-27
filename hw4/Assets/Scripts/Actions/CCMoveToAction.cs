using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCMoveToAction : SSAction
{
    //目的地
    public Vector3 target;
    //速度
    public float speed;

    private CCMoveToAction()
    {

    }

    //生产函数(工厂模式)
    public static CCMoveToAction GetSSAction(Vector3 target, float speed)
    {
        CCMoveToAction action = ScriptableObject.CreateInstance<CCMoveToAction>();
        action.target = target;
        action.speed = speed;
        return action;
    }

    // Start is called before the first frame update
    public override void Start()
    {
        
    }

    // Update is called once per frame
    public override void Update()
    {
        //判断是否符合移动条件
        if (this.gameObject == null || this.transform.localPosition == target)
        {
            this.destroy = true;
            this.callback.SSActionEvent(this);
            return;
        }
        //移动
        this.transform.localPosition = Vector3.MoveTowards(this.transform.localPosition, target, speed * Time.deltaTime);
    }
}
