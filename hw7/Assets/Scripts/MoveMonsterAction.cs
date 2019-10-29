using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMonsterAction : SSAction
{
    MonsterManager monsterManager;
    float speed;                    //速度
    float counter;                  //计数器

    public static MoveMonsterAction GetSSAction(float speed)
    {
        MoveMonsterAction action = ScriptableObject.CreateInstance<MoveMonsterAction>();
        action.speed = speed;
        return action;
    }

    public override void Start()
    {
        counter = 0;
        monsterManager = gameObject.GetComponent<MonsterManager>();
    }

    public override void Update()
    {
        if (monsterManager.moveable)
        {
            if (monsterManager.stop)
                return;
            counter += Time.deltaTime;
            //向前移动一段距离
            gameObject.transform.Translate(0, 0, speed * 2.6f * Time.deltaTime);
            //每3s换一个方向或撞墙时
            if (counter > 3 || monsterManager.change)
            {
                monsterManager.change = false;
                gameObject.transform.Rotate(0, Random.Range(45, 135), 0);
                counter = 0;
            }
        }
        else
        {
            this.destroy = true;
            this.enable = false;
            this.callback.SSActionEvent(this);
        }

    }
}
