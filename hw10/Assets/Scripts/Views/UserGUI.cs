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
        style.alignment = TextAnchor.MiddleCenter;

        //大字体初始化
        GUIStyle bigStyle = new GUIStyle();
        bigStyle.normal.textColor = Color.white;
        bigStyle.fontSize = 50;
        bigStyle.alignment = TextAnchor.MiddleCenter;

        GUI.Label(new Rect(Screen.width/2-200, 30, 400, 50), "Priests and Devils", bigStyle);

        GUI.Label(new Rect(Screen.width/2-100, 100, 200, 50), gameMessage, style);

        GUI.Label(new Rect(0, 0, 150, 50), "Time: " + time, style);

        if(GUI.Button(new Rect(Screen.width - 120, 10, 100, 50), "Restart"))
        {
            userAction.Restart();
        }
        if (GUI.Button(new Rect(Screen.width / 2 - 50, 160, 100, 50), "Tips"))
        {
            userAction.Tips();
        }

    }
}
