using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCActionManager : SSActionManager, ISSActionCallback
{
    //是否正在运动
    private bool isMoving = false;
    //船移动动作类
    public CCMoveToAction moveBoatAction;
    //人移动动作类(需要组合)
    public CCSequenceAction moveRoleAction;
    //控制器
    public FirstController controller;

    protected new void Start()
    {
        controller = (FirstController)SSDirector.GetInstance().CurrentSenceController;
        controller.actionManager = this;
    }

    public bool IsMoving()
    {
        return isMoving;
    }

    //移动船
    public void MoveBoat(GameObject boat, Vector3 target, float speed)
    {
        if (isMoving)
            return;
        isMoving = true;
        moveBoatAction = CCMoveToAction.GetSSAction(target, speed);
        this.RunAction(boat, moveBoatAction, this);
    }

    //移动人
    public void MoveRole(GameObject role, Vector3 mid_destination, Vector3 destination, int speed)
    {
        if (isMoving)
            return;
        isMoving = true;
        moveRoleAction = CCSequenceAction.GetSSAction(0, 0, new List<SSAction> { CCMoveToAction.GetSSAction(mid_destination, speed), CCMoveToAction.GetSSAction(destination, speed) });
        this.RunAction(role, moveRoleAction, this);
    }

    //回调函数
    public void SSActionEvent(SSAction source,
    SSActionEventType events = SSActionEventType.Competed,
    int intParam = 0,
    string strParam = null,
    Object objectParam = null)
    {
        isMoving = false;
    }
}
