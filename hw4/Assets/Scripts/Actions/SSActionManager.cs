using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSActionManager : MonoBehaviour
{
    //动作集，以字典形式存在
    private Dictionary<int, SSAction> actions = new Dictionary<int, SSAction>();
    //等待被加入的动作队列(动作即将开始)
    private List<SSAction> waitingAdd = new List<SSAction>();
    //等待被删除的动作队列(动作已完成)
    private List<int> waitingDelete = new List<int>();

    protected void Update()
    {
        //将waitingAdd中的动作保存
        foreach (SSAction ac in waitingAdd)
            actions[ac.GetInstanceID()] = ac;
        waitingAdd.Clear();

        //运行被保存的事件
        foreach (KeyValuePair<int, SSAction> kv in actions)
        {
            SSAction ac = kv.Value;
            if (ac.destroy)
            {
                waitingDelete.Add(ac.GetInstanceID());
            }else if (ac.enable)
            {
                ac.Update();
            }
        }

        //销毁waitingDelete中的动作
        foreach (int key in waitingDelete)
        {
            SSAction ac = actions[key];
            actions.Remove(key);
            Destroy(ac);
        }
        waitingDelete.Clear();
    }

    //准备运行一个动作，将动作初始化，并加入到waitingAdd
    public void RunAction(GameObject gameObject, SSAction action, ISSActionCallback manager)
    {
        action.gameObject = gameObject;
        action.transform = gameObject.transform;
        action.callback = manager;
        waitingAdd.Add(action);
        action.Start();
    }

    // Start is called before the first frame update
    protected void Start()
    {

    }

}
