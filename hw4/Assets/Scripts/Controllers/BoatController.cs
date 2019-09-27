using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatModelController : ClickAction
{
    BoatModel boatModel;
    IUserAction userAction;

    public BoatModelController()
    {
        userAction = SSDirector.GetInstance().CurrentSenceController as IUserAction;
    }
    public void CreateBoat(Vector3 position)
    {
        if (boatModel == null)
            boatModel = new BoatModel();
        boatModel.Init(position);
        boatModel.boat.GetComponent<Click>().setClickAction(this);
    }

    public BoatModel GetBoatModel()
    {
        return boatModel;
    }

    //将角色加到船上，返回接下来角色应该到达的位置
    public Vector3 AddRole(RoleModel roleModel)
    {
        //船上有两个位置，分别判断两个位置是否为空
        if (boatModel.roles[0] == null)
        {
            boatModel.roles[0] = roleModel;
            roleModel.isInBoat = true;
            roleModel.role.transform.parent = boatModel.boat.transform;
            if (roleModel.isPriest)
                boatModel.priestNum++;
            else
                boatModel.devilNum++;
            return PositionModel.boatRoles[0];

        }
        if (boatModel.roles[1] == null)
        {
            boatModel.roles[1] = roleModel;
            roleModel.isInBoat = true;
            roleModel.role.transform.parent = boatModel.boat.transform;
            if (roleModel.isPriest)
                boatModel.priestNum++;
            else
                boatModel.devilNum++;
            return PositionModel.boatRoles[1];
        }
        return roleModel.role.transform.localPosition;
    }

    //将角色从船上移除
    public void RemoveRole(RoleModel roleModel)
    {
        //船上有两个位置,分别判断两个位置当中有没有要移除的角色
        if (boatModel.roles[0] == roleModel)
        {
            boatModel.roles[0] = null;
            if (roleModel.isPriest)
                boatModel.priestNum--;
            else
                boatModel.devilNum--;
        }
        if (boatModel.roles[1] == roleModel)
        {
            boatModel.roles[1] = null;
            if (roleModel.isPriest)
                boatModel.priestNum--;
            else
                boatModel.devilNum--;
        }
    }

    public void DealClick()
    {
        if (boatModel.roles[0] != null || boatModel.roles[1] != null)
            userAction.MoveBoat();
    }
}
