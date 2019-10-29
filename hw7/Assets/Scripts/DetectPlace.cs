using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlace : MonoBehaviour
{
    public delegate void SetArea(float x, float y);
    public static event SetArea setArea;                  //区域事件发布

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            setArea(transform.position.x, transform.position.z);
        }
    }

}
