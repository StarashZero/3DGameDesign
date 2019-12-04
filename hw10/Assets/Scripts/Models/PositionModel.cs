using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionModel
{
    public static Vector3 right_land = new Vector3(15, -4, 0);                  //右侧岸的位置
    public static Vector3 left_land = new Vector3(-13, -4, 0);                  //左侧岸的位置
    public static Vector3 river = new Vector3(1, -(float)5.5, 0);               //河流的位置
    public static Vector3 right_boat = new Vector3(7, -(float)3.8, 0);          //船在右边的位置
    public static Vector3 left_boat = new Vector3(-5, -(float)3.8, 0);          //船在左边的位置
    //角色在岸上的相对位置(6个)
    public static Vector3[] roles = new Vector3[]{new Vector3((float)-0.2, (float)0.7, 0) ,new Vector3((float)-0.1, (float)0.7,0),
    new Vector3(0, (float)0.7,0),new Vector3((float)0.1, (float)0.7,0),new Vector3((float)0.2, (float)0.7,0),new Vector3((float)0.3, (float)0.7,0)};
    //角色在船上的相对位置(2个)
    public static Vector3[] boatRoles = new Vector3[] { new Vector3((float)-0.1, (float)1.2, 0), new Vector3((float)0.2, (float)1.2, 0) };
}
