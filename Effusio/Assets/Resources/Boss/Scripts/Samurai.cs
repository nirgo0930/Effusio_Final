using System.Collections;
using UnityEngine;

public class Samurai : Enemy
{
    public enum State
    {
        Idle,
        Run,
        Attack,
        Skill,
        Attack1,
        Attack2,
        mid

    };
    public State currentState = State.Idle;
    public static float skillDis;
    Vector2 newPosition;
    float newdis;
    public bool isSkill = false;
    // public bool isSkill = false;

    public Transform[] wallCheck;

    void Awake()
    {
        base.Awake();
        bossName = "Samurai";
        ackDistance = 3.6f;
        moveSpeed = 1.5f;
        jumpPower = 8f;
        currentHp = 70;
        atkCoolTime = 3f;
        atkCoolTimeCalc = atkCoolTime;

        if (this.tag == "Summon")
        {
            StartCoroutine(Attack());
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
        MyAnimSetTrigger("Idle");
        yield return new WaitForSeconds(1f);
        distance = Vector2.Distance(target.transform.position, transform.position);
        while (!IsTargetDir())
        {
            MonsterFlip();
        }

        if (canAtk)
        {
            if (0 < distance && distance < ackDistance)
            {
                currentState = State.Attack;
            }
            else if (ackDistance <= distance && distance < 8)
            {
                currentState = State.Skill;
            }
            else if (8 <= distance)
            {
                currentState = State.Run;
            }
        }



    }
    IEnumerator Run()
    {

        MyAnimSetTrigger("Run");
        distance = Vector2.Distance(target.transform.position, transform.position);
        if (canAtk && IsTargetDir())
        {

            rb.velocity = new Vector2(-transform.localScale.x * moveSpeed, 0);
            if (distance < ackDistance)
            {
                rb.velocity = Vector2.zero;
                currentState = State.Attack;
            }
            else if (ackDistance <= distance && distance < 8)
            {

                currentState = State.Skill;
            }
        }
        else
        {
            yield return new WaitForSeconds(1f);
            currentState = State.Idle;
        }
    }

    IEnumerator Attack()
    {
        yield return null;
        print("Attack");
        if (!isHit)
        {
            if (distance <= ackDistance)
            {
                canAtk = false;
                MyAnimSetTrigger("Attack1");
                yield return new WaitForSeconds(1f);
                MyAnimSetTrigger("Attack2");
                yield return new WaitForSeconds(1f);

                currentState = State.Idle;
            }
            else
            {
                currentState = State.Run;
            }

        }
    }
    IEnumerator Attack1()
    {
        MyAnimSetTrigger("Attack1");
        yield return null;

    }
    IEnumerator Attack2()
    {

        MyAnimSetTrigger("Attack2");
        yield return null;
    }
    IEnumerator mid()
    {
        MyAnimSetTrigger("mid");
        yield return null;

    }

    IEnumerator Skill()
    {
        yield return null;
        rb.velocity = Vector2.zero;

        skillDis = -transform.localScale.x * 2f;

        newPosition = new Vector2(transform.position.x + skillDis, transform.position.y);
        StartCoroutine(Attack1());
        yield return new WaitForSecondsRealtime(3f);
        isSkill = true;

        rb.velocity = new Vector2(-transform.localScale.x * 100f, transform.localScale.y);

        yield return new WaitForSeconds(0.1f);

        StartCoroutine(mid());
        yield return new WaitForSecondsRealtime(0.2f);

        StartCoroutine(Attack2());
        yield return new WaitForSeconds(0.5f);

        if (this.gameObject.tag == "Summon")
        {
            Destroy(this.gameObject);
        }
        currentState = State.Idle;
    }

    private void Update()
    {
        if (isSkill)
        {
            if (skillDis < 0)
            {
                if (transform.position.x < newPosition.x)
                {
                    rb.velocity = Vector2.zero;
                    isSkill = false;
                }
            }
            else
            {
                if (transform.position.x > newPosition.x)
                {
                    rb.velocity = Vector2.zero;
                    isSkill = false;
                }
            }
        }
    }
}
