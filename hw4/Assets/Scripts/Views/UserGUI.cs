using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGUI : MonoBehaviour
{
    private IUserAction userAction;
    public string gameMessage;
    public int time;
    void Start()
    {
        gameMessage = "";
        time = 60;
        userAction = SSDirector.GetInstance().CurrentSenceController as IUserAction;
    }

    void OnGUI()
    {
        //小字体初始化
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.white;
        style.fontSize = 30;

        //大字体初始化
        GUIStyle bigStyle = new GUIStyle();
        bigStyle.normal.textColor = Color.white;
        bigStyle.fontSize = 50;

        GUI.Label(new Rect(200, 30, 50, 200), "Priests and Devils", bigStyle);

        GUI.Label(new Rect(320, 100, 50, 200), gameMessage, style);

        GUI.Label(new Rect(0, 0, 100, 50), "Time: " + time, style);

        if(GUI.Button(new Rect(340, 160, 100, 50), "Restart"))
        {
            userAction.Restart();
        }
    }
}
