using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCSequenceAction : SSAction, ISSActionCallback
{
    //动作序列
    public List<SSAction> sequence;
    //重复次数
    public int repeat = -1;
    //动作开始指针
    public int start = 0;

    //生产函数(工厂模式)
    public static CCSequenceAction GetSSAction(int repeat, int start, List<SSAction> sequence)
    {
        CCSequenceAction action = ScriptableObject.CreateInstance<CCSequenceAction>();
        action.repeat = repeat;
        action.start = start;
        action.sequence = sequence;
        return action;
    }

    //对序列中的动作进行初始化
    public override void Start()
    {
        foreach (SSAction action in sequence)
        {
            action.gameObject = this.gameObject;
            action.transform = this.transform;
            action.callback = this;
            action.Start();
        }
    }

    //运行序列中的动作
    public override void Update()
    {
        if (sequence.Count == 0)
            return;
        if (start < sequence.Count)
        {
            sequence[start].Update();
        }
    }

    //回调处理，当有动作完成时触发
    public void SSActionEvent(SSAction source,
        SSActionEventType events = SSActionEventType.Competed,
        int Param = 0,
        string strParam = null,
        Object objectParam = null)
    {
        source.destroy = false;
        this.start++;
        if (this.start >= sequence.Count)
        {
            this.start = 0;
            if (repeat > 0)
                repeat--;
            if (repeat == 0)
            {
                this.destroy = true;
                this.callback.SSActionEvent(this);
            }
        }
    }

    void OnDestroy()
    {

    }
}
