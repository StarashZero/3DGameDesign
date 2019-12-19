using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleModel{ 
    public GameObject role;             //角色的游戏对象
    public bool isPriest;               //区分角色是牧师还是恶魔
    public int tag;                     //给对象标号，方便查找
    public bool isRight;                //区分角色是在左侧还是右侧
    public bool isInBoat;               //区分角色是在船上还是在岸上

    //初始化函数
    public void Init(Vector3 position, bool isPriest, int tag)
    {
        this.isPriest = isPriest;
        this.tag = tag;
        isRight = false;
        isInBoat = false;
        if (role == null)
        {
            role = GameObject.FindGameObjectWithTag("role" + tag);
        }
        role.transform.localPosition = position;
    }
}
