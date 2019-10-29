using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public delegate void DealDamage(GameObject player);
    public static event DealDamage dealDamage;              //受伤事件发布

    public float speed;             //当前速度
    public float direction;         //当前方向位移
    public bool moveable;           //是否可移动          
    public int health;              //当前血量
    float jumpCounter;              //跳跃冷却计数器
    float hitCounter;               //攻击冷却计数器
    float damageCounter;            //受伤保护计数器

    Animator animator;
    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        speed = 0;
        direction = 0;
        jumpCounter = hitCounter = damageCounter = 0;
    }

    //设置速度(动画)
    public void SetSpeed(float speed)
    {
        animator.SetFloat("speed",speed);
    }

    //跳跃
    public void Jump()
    {
        if (jumpCounter > 2)
        {
            animator.ResetTrigger("jumpTrigger");
            animator.SetTrigger("jumpTrigger");
            jumpCounter = 0;
        }
    }

    //攻击
    public void Hit()
    {
        if (hitCounter > 2 && !IsName("jump"))
        {
            animator.SetTrigger("hitTrigger");
            hitCounter = 0;
        }
    }

    //受伤
    public void Damage()
    {
        animator.SetTrigger("damageTrigger");
    }

    //死亡
    public void Die()
    {
        animator.SetBool("running", false);
        animator.SetTrigger("KOTrigger");
    }

    //胜利
    public void Win()
    {
        animator.SetBool("running", false);
        animator.SetTrigger("winTrigger");
    }

    //归位
    public void ToIdle()
    {
        animator.SetTrigger("toIdle");
    }

    //判断动画状态
    public bool IsName(string name)
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(name);
    }

    //复活
    public void Revive()
    {
        ToIdle();
        animator.SetBool("running", true);
    }

    //受伤判定
    private void OnTriggerEnter(Collider other)
    {
        //如果接受到怪兽的攻击Trigger判定则进行处理
        if (other.gameObject.name == "MonsterHitRange" && !IsName("jump") && damageCounter > 1)
        {
            MonsterManager parentManager = other.gameObject.GetComponentInParent<MonsterManager>();
            //只有怪兽攻击时Trigger有效，避免误判
            if (!parentManager.IsName("Attack 01"))
                return;
            Damage();
            damageCounter = 0;
            health--;
            if (health <= 0)
            {
                Die();
            }
            //发布受伤事件
            dealDamage(gameObject);
        }
    }


    private void Update()
    {
        //及时进行归位消除，避免出现toIdle长时间存在的情况
        if (!IsName("KO_big") && !IsName("winpose"))
            animator.ResetTrigger("toIdle");
        hitCounter = (hitCounter + Time.deltaTime) > 5 ? 5 : hitCounter + Time.deltaTime;
        damageCounter = (damageCounter + Time.deltaTime) > 5 ? 5 : damageCounter + Time.deltaTime;
        jumpCounter = (jumpCounter + Time.deltaTime) > 5 ? 5 : jumpCounter + Time.deltaTime;
    }
}
