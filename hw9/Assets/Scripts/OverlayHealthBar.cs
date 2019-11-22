using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverlayHealthBar : MonoBehaviour
{
    private Slider healthBar;
    void Start()
    {
        healthBar = GetComponentInChildren<Slider>();
    }

    void Update()
    {
        //计算healthBar位置
        Vector3 worldPos = new Vector3(transform.position.x, transform.position.y + 1.8f, transform.position.z);
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        healthBar.transform.position = new Vector3(screenPos.x, screenPos.y, screenPos.z);
        //计算scale比例  
        float distance = (transform.position.z - Camera.main.transform.position.z - 10);
        float newScale = (distance<0?1: 1/(1+distance)) *0.5f;
        healthBar.transform.localScale = new Vector3(newScale, newScale, 1);
    }
}
