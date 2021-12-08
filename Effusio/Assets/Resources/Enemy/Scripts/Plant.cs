using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : Enemy
{
    public enum State
    {
        Idle,
        Attack
    };
    public State currentState = State.Idle;

    public Transform genPoint;
    public GameObject Bullet;



    WaitForSeconds Delay1000 = new WaitForSeconds(1f);

    void Awake()
    {
        base.Awake();
        ackDistance = 4.2f;
        moveSpeed = 0;
        jumpPower = 8f;
        currentHp = 40;
        atkCoolTime = 6f;
        atkCoolTimeCalc = atkCoolTime;



        StartCoroutine(FSM());
    }

    IEnumerator FSM()
    {
        while (true)
        {
            yield return StartCoroutine(currentState.ToString());

        }
    }
    IEnumerator Idle()
    {
        distance = Vector2.Distance(target.transform.position, transform.position);
        MyAnimSetTrigger("Idle");
        if (!IsTargetDir())
        {
            MonsterFlip();
        }
        yield return Delay1000;
        if (!isHit)
        {
            if (canAtk && distance < ackDistance)
            {
                currentState = State.Attack;
            }
            else
            {
                currentState = State.Idle;
            }

        }

    }
    IEnumerator Attack()
    {
        canAtk = false;
        MyAnimSetTrigger("Attack");

        yield return Delay1000;
        MyAnimSetTrigger("Attack");

        yield return Delay1000;
        MyAnimSetTrigger("Attack");

        yield return Delay1000;
        currentState = State.Idle;
    }
    void Fire()
    {
        GameObject bulletClone = Instantiate(Bullet, genPoint.position, transform.rotation);
        Rigidbody2D bulletClone_rb = bulletClone.GetComponent<Rigidbody2D>();
        bulletClone_rb.velocity = transform.right * -transform.localScale.x * 3f;
        bulletClone.transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y);
    }
}
