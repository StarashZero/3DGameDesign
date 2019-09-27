using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandModelController
{
    private LandModel landModel;

    public void CreateLand(string name, Vector3 position)
    {
        if (landModel==null)
            landModel = new LandModel();
        landModel.Init(name, position);
        landModel.priestNum = landModel.devilNum = 0;
    } 

    public LandModel GetLandModel()
    {
        return landModel;
    }

    //将人物添加到岸上，返回角色在岸上的相对坐标
    public Vector3 AddRole(RoleModel roleModel)
    {
        if (roleModel.isPriest)
            landModel.priestNum++;
        else
            landModel.devilNum++;
        roleModel.role.transform.parent = landModel.land.transform;
        roleModel.isInBoat = false;
        return PositionModel.roles[roleModel.tag];
    }

    //将角色从岸上移除
    public void RemoveRole(RoleModel roleModel)
    {
        if (roleModel.isPriest)
            landModel.priestNum--;
        else
            landModel.devilNum--;
    }
}
