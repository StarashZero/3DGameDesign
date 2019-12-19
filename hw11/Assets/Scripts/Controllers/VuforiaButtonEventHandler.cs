using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using Vuforia;

[System.Obsolete]
public class VuforiaButtonEventHandler : MonoBehaviour, IVirtualButtonEventHandler
{
    public VirtualButtonBehaviour help;
    public VirtualButtonBehaviour restart;
    IUserAction action;

    // Start is called before the first frame update
    [System.Obsolete]
    void Start()
    {
        help.RegisterEventHandler(this);
        restart.RegisterEventHandler(this);
        action = SSDirector.GetInstance().CurrentSenceController as IUserAction;
    }

    private void Update()
    {
        
    }


    public void OnButtonPressed(VirtualButtonBehaviour vb)
    {
        vb.gameObject.GetComponentInChildren<MeshRenderer>().material.color = Color.red;
        switch (vb.VirtualButtonName)
        {
            case "restart":
                action.Restart();
                break;
            case "help":
                action.Tips();
                break;
            default:
                break;
        }
    }

    public void OnButtonReleased(VirtualButtonBehaviour vb)
    {
        vb.gameObject.GetComponentInChildren<MeshRenderer>().material.color = Color.white;
    }
}
