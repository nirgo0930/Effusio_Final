using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRino : Enemy
{
    public enum State
    {
        Idle,
        Run,
        HitWall,
    };
    public State currentState = State.Idle;
    WaitForSeconds Delay1000 = new WaitForSeconds(1f);
    public bool isWall = false;

    void Awake()
    {
        base.Awake();
        bossName = "BossRino";
        ackDistance = 0f;
        moveSpeed = 100f;
        jumpPower = 0f;
        currentHp = 100;
        atkCoolTime = 3f;
        atkCoolTimeCalc = atkCoolTime;

        if (this.tag == "Summon")
        {
            target = new GameObject();
            target.tag = "Enemy";
            StartCoroutine(Effusio());
        }
        else
        {
            StartCoroutine(FSM());
        }
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
        yield return null;
        MyAnimSetTrigger("Idle");
        yield return new WaitForSecondsRealtime(2f);

        while (!IsTargetDir())
        {
            MonsterFlip();
        }

        if (canAtk && IsTargetDir())
        {
            MyAnimSetTrigger("Run");
            yield return Delay1000;
            rb.velocity = Vector2.zero;
            currentState = State.Run;
        }
    }
    IEnumerator Run()
    {
        yield return null;
        if (!isHit)
        {
            if (!isWall)
            {
                MyAnimSetTrigger("Run");
                rb.AddForce(new Vector2(-transform.localScale.x * moveSpeed, 0), ForceMode2D.Force);
            }

        }
        else
        {
            yield return null;
            currentState = State.Idle;
        }

    }

    IEnumerator HitWall()
    {

        MyAnimSetTrigger("HitWall");
        isWall = false;

        rb.velocity = Vector2.zero;
        rb.AddForce(new Vector2(transform.localScale.x * 10f, 0), ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.5f);

        currentState = State.Idle;
    }

    void OnCollisionEnter2D(Collision2D other)
    {

        if (other.gameObject.tag == "Ground")
        {
            currentState = State.HitWall;
        }
    }

    IEnumerator Effusio()
    {
        yield return new WaitForSeconds(2f);

        MyAnimSetTrigger("Run");

        rb.velocity = new Vector2(-transform.localScale.x * 5f, 0);

        yield return null;
    }

}