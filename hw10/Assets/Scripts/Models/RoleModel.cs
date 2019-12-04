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
        if (role != null)
            Object.DestroyImmediate(role);
        this.isPriest = isPriest;
        this.tag = tag;
        isRight = false;
        isInBoat = false;
        role = GameObject.Instantiate(Resources.Load("Prefabs/" + (isPriest ? "priest" : "devil"), typeof(GameObject))) as GameObject;
        role.transform.localScale = new Vector3(1, 1, 1);
        role.transform.position = position;
        role.name = "role" + tag;
        role.AddComponent<Click>();
        role.AddComponent<BoxCollider>();
    }
}
