using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public float health = 0.5f;

    void OnGUI()
    {
        //初步计算血条位置
        Vector3 worldPos = new Vector3(transform.position.x, transform.position.y -0.2f, transform.position.z);
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        //计算血条大小比例
        float distance = (transform.position.z - Camera.main.transform.position.z - 10);
        float newScale = (distance < 0 ? 1 : 1 / (1 + distance)) * 0.5f;
        //生产HorizontalScrollbar
        GUI.HorizontalScrollbar(new Rect(new Rect(screenPos.x - 100*newScale, screenPos.y, 200*newScale, 20*newScale)), 0.0f, health, 0.0f, 1.0f);
    }
}
