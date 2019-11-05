using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleRing : MonoBehaviour
{
    ParticleSystem particleSystem;              //粒子系统
    ParticleSystem.Particle[] particlesArray;   //粒子数组
    public int count = 3000;                    //粒子数量
    public float size = 0.1f;                   //粒子大小
    public float maxRadius = 12f;               //最大半径
    public float minRadius = 7f;                //最小半径
    public float speed = 2f;                    //初始速度
    public float pingPong = 0.02f;              //游离范围
    public Gradient gradient;                   //颜色
    CirclePosition[] circles;                   //粒子位置
    public float time = 0;                      //时间

    class CirclePosition
    {
        public float radius = 0f, angle = 0f, time = 0f, targetRadius = 0f;
        public CirclePosition(float radius, float angle, float time, float targetRadius)
        {
            this.radius = radius;                   //半径    
            this.angle = angle;                     //角度
            this.time = time;                       //运行时间                              
            this.targetRadius = targetRadius;       //约束半径
        }
    }

    void Start()
    {
        particlesArray = new ParticleSystem.Particle[count];
        circles = new CirclePosition[count];
        particleSystem = gameObject.GetComponent<ParticleSystem>();
        particleSystem.maxParticles = count;
        particleSystem.startSize = size;
        particleSystem.Emit(count);
        particleSystem.GetParticles(particlesArray);
        float midRadius = (maxRadius + minRadius) / 2;
        float minRate = Random.Range(1.0f, midRadius / minRadius);
        float maxRate = Random.Range(midRadius / maxRadius, 1.0f);
        for (int i = 0; i < count; i++)
        {
            //设置半径
            float radius = Random.Range(minRadius * minRate, maxRadius * maxRate);
            //设置角度
            float angle = (float)i / count * 360f;

            float theta = angle / 180 * Mathf.PI;
            //保存粒子初位置
            circles[i] = new CirclePosition(radius, angle, (float)i / count * 360f, radius);
            //设置粒子初位置
            particlesArray[i].position = new Vector3(radius * Mathf.Cos(theta), radius * Mathf.Sin(theta), 0);
        }
        particleSystem.SetParticles(particlesArray, particlesArray.Length);
    }

    void Update()
    {
        //增加时间，并计算当前应该显示的粒子的最低索引
        time = (time + Time.deltaTime) > 5 ? 5 : (time + Time.deltaTime);
        int tar = count - (int)(Mathf.Pow(time / 5, 3) * count);

        for (int i = 0; i < count; ++i)
        {
            //粒子角度增加
            circles[i].angle = (circles[i].angle - Random.Range(0.4f, 0.6f) + 360f) % 360f;

            float theta = circles[i].angle / 180 * Mathf.PI;

            //粒子半径变动
            circles[i].time += Time.deltaTime;
            circles[i].radius += Mathf.PingPong(circles[i].time / minRadius / maxRadius, pingPong) - pingPong / 2.0f;

            //约束粒子半径，使得粒子有向初半径位移的趋势
            if (circles[i].radius < circles[i].targetRadius)
                circles[i].radius += 0.01f;
            else if (circles[i].radius > circles[i].targetRadius)
                circles[i].radius -= 0.01f;

            //将粒子锁定至最大半径与最小半径之间
            if (circles[i].radius < minRadius)
                circles[i].radius += Time.deltaTime;
            else if (circles[i].radius>maxRadius)
                circles[i].radius -= Time.deltaTime;

            particlesArray[i].position = new Vector3(circles[i].radius * Mathf.Cos(theta), circles[i].radius * Mathf.Sin(theta), 0);  

            //设置粒子颜色
            if (i < tar)
                particlesArray[i].startColor = gradient.Evaluate(0.5f);             //粒子全透明
            else
            {
                float deep = circles[i].angle / 360f > 0.6f ? (circles[i].angle / 360f) : (circles[i].angle / 360f < 0.4f ? circles[i].angle / 360f : 0.4f);    //不允许全透明
                particlesArray[i].startColor = gradient.Evaluate(deep);
            }

        }
        particleSystem.SetParticles(particlesArray, particlesArray.Length);
    }
}
