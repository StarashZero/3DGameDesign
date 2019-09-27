using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleModelController :ClickAction
{
    RoleModel roleModel;                                       
    IUserAction userAction;                                     

    public RoleModelController()
    {
        userAction = SSDirector.GetInstance().CurrentSenceController as IUserAction;
    }

    public void CreateRole(Vector3 position, bool isPriest, int tag)
    {
        if (roleModel == null)
            roleModel = new RoleModel();
        roleModel.Init(position, isPriest, tag);
        roleModel.role.GetComponent<Click>().setClickAction(this);
    }

    public RoleModel GetRoleModel()
    {
        return roleModel;
    }

    public void DealClick()
    {
        userAction.MoveRole(roleModel);
    }
}
