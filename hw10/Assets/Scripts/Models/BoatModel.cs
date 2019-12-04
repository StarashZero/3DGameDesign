using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatModel {
    public GameObject boat;                     //船对象
    public RoleModel[] roles;                   //船上的角色的指针
    public bool isRight;                        //判断船在左侧还是右侧
    public int priestNum, devilNum;             //船上牧师与恶魔的数量

    public void Init(Vector3 position)
    {
        if (boat != null)
            Object.DestroyImmediate(boat);
        priestNum = devilNum = 0;
        roles = new RoleModel[2];
        boat = GameObject.Instantiate(Resources.Load("Prefabs/boat", typeof(GameObject))) as GameObject;
        boat.name = "boat";
        boat.transform.position = position;
        boat.transform.localScale = new Vector3(4, (float)1.5, 3);
        boat.AddComponent<BoxCollider>();
        boat.AddComponent<Click>();
        isRight = false;
    }
}
