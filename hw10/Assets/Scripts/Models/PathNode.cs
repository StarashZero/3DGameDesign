using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    public static readonly int BOAT_PRIESTS = 0;            //船上牧师
    public static readonly int BOAT_DEVILS = 1;             //船上恶魔
    public static readonly int LEFT_PRIESTS = 2;            //左岸牧师
    public static readonly int RIGHT_PRIESTS = 3;           //右岸牧师
    public static readonly int LEFT_DEVILS = 4;             //左岸恶魔
    public static readonly int RIGHT_DEVILS = 5;            //右岸恶魔
    public static readonly int BOAT_PLACE = 6;              //船只位置
    PathNode parentNode;                                    //父节点
    PathNode nextNode;                                      //路径下一节点
    public int[] state;                                     //状态

    public PathNode(int[] state)
    {
        parentNode = null;
        nextNode = null;
        this.state = (int[])state.Clone();
    }

    public PathNode(PathNode other)
    {
        parentNode = other;
        nextNode = null;
        state = (int[])other.state.Clone();
    }

    //重载Hash函数，使其对state唯一
    public override int GetHashCode()
    {
        int res = 0;
        for (int i = 0; i < 7; i++)
        {
            res += state[i] * (int)Mathf.Pow(10,i);
        }
        return res;
    }

    public PathNode GetParent()
    {
        return parentNode;
    }

    public void SetParent(PathNode parent)
    {
        parentNode = parent;
    }

    public PathNode GetNext()
    {
        return nextNode;
    }

    public void SetNext(PathNode next)
    {
        nextNode = next;
    }

    //获得当前状态
    //0 游戏进行
    //1 游戏胜利
    //2 游戏结束
    public int GetState()
    {
        if (state[RIGHT_PRIESTS] == 3)
            return 1;
        int leftPriests, leftDevils, rightPriests, rightDevils;
        leftPriests = state[LEFT_PRIESTS] + state[BOAT_PRIESTS] * (1 - state[BOAT_PLACE]);
        leftDevils = state[LEFT_DEVILS] + state[BOAT_DEVILS] * (1 - state[BOAT_PLACE]);
        rightPriests = state[RIGHT_PRIESTS] + (state[BOAT_PRIESTS] * state[BOAT_PLACE]);
        rightDevils = state[RIGHT_DEVILS] + (state[BOAT_DEVILS] * state[BOAT_PLACE]);
        if ((leftPriests!=0&& leftPriests < leftDevils) || (rightPriests!=0&& rightPriests < rightDevils))
            return 2;
        return 0;
    }

    //是否可以移动
    public bool CanMove(int state_place)
    {
        if (state_place == LEFT_PRIESTS || state_place == LEFT_DEVILS)
        {
            return (state[BOAT_PLACE] == 0 && (state[BOAT_PRIESTS] + state[BOAT_DEVILS] < 2) && state[state_place] > 0);
        }
        else if (state_place == RIGHT_PRIESTS || state_place == RIGHT_DEVILS)
        {
            return (state[BOAT_PLACE] == 1 && (state[BOAT_PRIESTS] + state[BOAT_DEVILS] < 2) && state[state_place] > 0);
        }
        else if (state_place == BOAT_PRIESTS || state_place == BOAT_DEVILS)
        {
            return (state[state_place] > 0);
        }
        else if (state_place == BOAT_PLACE)
        {
            return (state[BOAT_PRIESTS] + state[BOAT_DEVILS] > 0);
        }
        return false;
    }

    //移动
    public bool Move(int state_place)
    {
        bool canMove = CanMove(state_place);
        if (!canMove)
            return false;
        if (state_place == LEFT_PRIESTS || state_place == RIGHT_PRIESTS)
        {
            state[state_place]--;
            state[BOAT_PRIESTS]++;
        }
        else if (state_place == LEFT_DEVILS || state_place == RIGHT_DEVILS)
        {
            state[state_place]--;
            state[BOAT_DEVILS]++;
        }
        else if (state_place == BOAT_PRIESTS)
        {
            state[state_place]--;
            state[state[BOAT_PLACE] == 0 ? LEFT_PRIESTS : RIGHT_PRIESTS]++;
        }
        else if (state_place == BOAT_DEVILS)
        {
            state[state_place]--;
            state[state[BOAT_PLACE] == 0 ? LEFT_DEVILS : RIGHT_DEVILS]++;
        }
        else if (state_place == BOAT_PLACE)
        {
            state[BOAT_PLACE] = 1 - state[BOAT_PLACE];
        }
        return true;
    }

}
