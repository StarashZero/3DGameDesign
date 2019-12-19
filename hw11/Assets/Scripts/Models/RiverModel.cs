using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverModel
{
    private GameObject river;               //河流的游戏对象

    public RiverModel(Vector3 position)
    {
        river = GameObject.FindGameObjectWithTag("river");
    }
}
