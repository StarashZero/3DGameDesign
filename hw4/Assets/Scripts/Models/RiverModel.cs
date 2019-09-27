using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverModel
{
    private GameObject river;               //河流的游戏对象

    public RiverModel(Vector3 position)
    {
        river = Object.Instantiate(Resources.Load("Prefabs/river", typeof(GameObject))) as GameObject;
        river.name = "river";
        river.transform.position = position;
        river.transform.localScale = new Vector3(15, 2, 3);
    }
}
