using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public delegate void FreeMe(GameObject follower);
    public static event FreeMe freeAction;                  //释放事件发布
    public delegate void DealDamage(GameObject player);
    public static event DealDamage dealDamage;              //受伤事件发布


    public bool moveable;           //释放可移动
    public bool stop;               //释放暂停移动
    public int health;              //当前血量
    Animator animator;
    FollowManager followManager;
    float damageCounter;            //受伤保护计数器
    float hitCounter;               //攻击冷却
    private void Start()
    {
        moveable = false;
        animator = gameObject.GetComponent<Animator>();
        followManager = gameObject.GetComponent<FollowManager>();
        damageCounter = 0;
        stop = false;
    }

    //设置速度(动画)
    public void SetSpeed(float speed)
    {
        if (animator == null)
            return;
        animator.SetFloat("speed", speed);
    }

    //跳跃
    public void Jump()
    {
        if (animator == null)
            return;
        animator.SetTrigger("jumpTrigger");
    }

    //攻击
    public void Hit()
    {
        if (animator == null)
            return;
        animator.SetTrigger("Attack 01");
    }

    //受伤
    public void Damage()
    {
        if (animator == null)
            return;
        animator.SetTrigger("Take Damage");
    }

    //死亡
    public void Die()
    {
        if (animator == null)
            return;
        animator.SetTrigger("Die");
    }

    //归位
    public void ToIdle()
    {
        if (animator == null)
            return;
        animator.SetTrigger("ToIdle");
    }

    //判断动画状态
    public bool IsName(string name)
    {
        if (animator == null)
            return false;
        return animator.GetCurrentAnimatorStateInfo(0).IsName(name);
    }

    //玩家攻击判定
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "PlayerHitRange" && damageCounter > 2)
        {
            PlayerManager parentManger = other.gameObject.GetComponentInParent<PlayerManager>();
            //判断玩家是否正在攻击，避免误判
            if (!parentManger.IsName("hit01"))
                return;
            Damage();
            followManager.stop = true;
            damageCounter = 0;
            health--;
            //发布受伤事件
            dealDamage(gameObject);
            if (health == 0)
            {
                Die();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "PlayerHitRange" && damageCounter > 2)
        {
            PlayerManager parentManger = other.gameObject.GetComponentInParent<PlayerManager>();
            if (!parentManger.IsName("hit01"))
                return;
            Damage();
            followManager.stop = true;
            damageCounter = 0;
            health--;
            dealDamage(gameObject);
            if (health == 0)
            {
                Die();
            }
        }
    }

    //碰撞事件，与玩家碰撞时触发攻击操作
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && hitCounter > 4)
        {
            Hit();
            followManager.stop = true;
            hitCounter = 0;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && hitCounter > 4)
        {
            Hit();
            followManager.stop = true;
            hitCounter = 0;
        }
    }

    private void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Die") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f)
        {
            ToIdle();
            //释放操作发布
            freeAction(gameObject);
        }
        damageCounter = (damageCounter + Time.deltaTime) > 5 ? 5 : damageCounter + Time.deltaTime;
        hitCounter = (hitCounter + Time.deltaTime) > 5 ? 5 : hitCounter + Time.deltaTime;
        if (followManager.stop && !IsName("Attack 01"))
            followManager.stop = false;
    }
}
