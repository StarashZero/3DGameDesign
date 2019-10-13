using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreRecorder
{
    int points;                 //游戏当前分数

    public ScoreRecorder()
    {
        points = 0;
    }

    public void Record(DiskData disk)
    {
        points += disk.points;
    }

    public int GetPoints()
    {
        return points;
    }
    public void Reset()
    {
        points = 0;
    }
}
