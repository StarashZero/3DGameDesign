using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandModel
{
    public GameObject land;                     //岸的游戏对象
    public int priestNum, devilNum;             //岸上牧师与恶魔的数量

    public void Init(string name, Vector3 position)
    {
        if (land == null)
        {
            land = GameObject.FindGameObjectWithTag(name);
        }
        priestNum = devilNum = 0;
        land.transform.localPosition = position;
    }
}
