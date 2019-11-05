using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleController : MonoBehaviour
{
    GameObject particleSea;
    GameObject particleRing;
    // Start is called before the first frame update
    void Start()
    {
        //生成粒子光环
        particleRing = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/ParticleRing"), new Vector3(0, 0, 15), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        //当粒子光环完全出现后生成粒子海洋
        if (particleSea == null && particleRing.GetComponent<ParticleRing>().time == 5)
            particleSea = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/ParticleSea"), new Vector3(-45, -8, -5), Quaternion.identity);
    }

    private void OnGUI()
    {
        //当用户点击按钮后，改变粒子光环的最大半径
        if (particleSea != null&&GUI.Button(new Rect(Screen.width/2-20,Screen.height/2-10,40,40), "+"))
        {
            particleRing.GetComponent<ParticleRing>().maxRadius = particleRing.GetComponent<ParticleRing>().maxRadius == 12 ? 9 : 12;
        }
    }
}
