using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterFactory : MonoBehaviour
{
    public GameObject monster_Prefab;              

    List<MonsterManager> used;               
    List<MonsterManager> free;
    Vector3[] areaPositions;
    void Start()
    {
        used = new List<MonsterManager>();
        free = new List<MonsterManager>();
        monster_Prefab = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Boximon Fiery"), Vector3.zero, Quaternion.identity);
        monster_Prefab.SetActive(false);
        areaPositions = new Vector3[] { new Vector3(6,-1.5f,-3), new Vector3(-3, -1.5f, -3), new Vector3(6, -1.5f, -12), new Vector3(-7.5f, -1.5f, -12), new Vector3(-12, -1.5f, 6)};
    }

    //订阅MonsterManager释放事件
    private void OnEnable()
    {
        MonsterManager.freeAction += FreeMonster;
    }

    //取消订阅
    private void OnDisable()
    {
        MonsterManager.freeAction -= FreeMonster;
    }

    //生成Monster
    public GameObject GetMonster(int area)
    {
        GameObject monster;
        if (free.Count > 0)
        {
            monster = free[0].gameObject;
            free.Remove(free[0]);
        }
        else
        {
            monster = GameObject.Instantiate<GameObject>(monster_Prefab, Vector3.zero, Quaternion.identity);
            monster.AddComponent<FollowManager>();
            monster.AddComponent<MonsterManager>();
        }
        monster.SetActive(true);
        monster.transform.position = areaPositions[area];

        if (area < 3)
        {
            monster.transform.localScale = new Vector3(1, 1, 1);
            monster.GetComponent<FollowManager>().speed = 0.8f;
            monster.GetComponent<MonsterManager>().health = 2;
        }else if (area == 3)
        {
            monster.transform.localScale = new Vector3(2, 2, 2);
            monster.GetComponent<FollowManager>().speed = 1;
            monster.GetComponent<MonsterManager>().health = 3;
        }
        else
        {
            monster.transform.localScale = new Vector3(3, 3, 3);
            monster.GetComponent<FollowManager>().speed = 1.2f;
            monster.GetComponent<MonsterManager>().health = 5;
        }

        used.Add(monster.GetComponent<MonsterManager>());

        return monster;
    }

    //释放Monster
    public void FreeMonster(GameObject monster)
    {
        foreach (MonsterManager monsterManger in used)
        {
            if (monsterManger.gameObject.GetInstanceID() == monster.GetInstanceID())
            {
                monster.GetComponent<FollowManager>().followable = false;
                monster.GetComponent<MonsterManager>().moveable = false;
                monster.SetActive(false);
                free.Add(monsterManger);
                used.Remove(monsterManger);
                break;
            }

        }
    }

    //释放所有Monster
    public void FreeAll()
    {
        while(used.Count!=0)
        {
            used[0].gameObject.GetComponent<FollowManager>().followable = false;
            used[0].moveable = false;
            used[0].gameObject.SetActive(false);
            free.Add(used[0]);
            used.Remove(used[0]);
        }
    }
}
