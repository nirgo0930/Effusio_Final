using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrandFire : Enemy
{
    public enum State
    {
        Idle,
        TpEnd,
        Attack,
        Move,
        Tp,
    };

    public State currentState = State.Idle;
    public Transform[] wallCheck;

    private void Awake()
    {
        base.Awake();
        bossName = "GrandFire";
        ackDistance = 3;
        moveSpeed = 0.5f;
        jumpPower = 0f;
        currentHp = 120;
        atkCoolTime = 3f;
        atkCoolTimeCalc = atkCoolTime;

         if (this.tag == "Summon")
        {
            target = new GameObject();
            target.tag = "Enemy";
            StartCoroutine(Effusio());
            // StartCoroutine(Attack());
            // Destroy(gameObject, 1f);
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
    IEnumerator Attack()
    {
        if (!isHit)
        {
            canAtk = false;
            MyAnimSetTrigger("Attack");
            yield return new WaitForSeconds(1f);
            currentState = State.Idle;
        }
    }
    IEnumerator Tp()
    {
        MyAnimSetTrigger("Tp");
        yield return new WaitForSeconds(1f);
        currentState = State.TpEnd;
    }
    IEnumerator TpEnd()
    {
        MyAnimSetTrigger("TpEnd");
        yield return new WaitForSeconds(2f);
        currentState = State.Attack;
    }

    void TpPosision()
    {
        Vector3 newPosision;

        if (checkLeft())
        {
            newPosision = new Vector3(target.transform.position.x - 1.5f, target.transform.position.y + 2f, 0);
        }
        else
        {
            newPosision = new Vector3(target.transform.position.x + 1.5f, target.transform.position.y + 2f, 0);
        }
        this.transform.position = newPosision;

        //StartCoroutine(TpEnd());
        while (!IsTargetDir())
        {
            MonsterFlip();
        }
    }


    IEnumerator Idle()
    {
        rb.velocity=Vector2.zero;
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
                currentState = State.Move;
            }
            else if (8 <= distance)
            {
                currentState = State.Tp;
            }
        }

    }
    IEnumerator Move()
    {
        distance = Vector2.Distance(target.transform.position, transform.position);
        while (distance >= ackDistance)
        {
            distance = Vector2.Distance(target.transform.position, transform.position);
            MyAnimSetTrigger("Move");
            rb.velocity = new Vector2(-transform.localScale.x * moveSpeed, 0.2f);

            if (Physics2D.OverlapCircle(wallCheck[0].position, 0.01f, layerMask) && Physics2D.OverlapCircle(wallCheck[1].position, 0.01f, layerMask))
            {
                MonsterFlip();
            }
            if (canAtk)
            {
                if (ackDistance <= distance && distance < 8)
                {
                    currentState = State.Move;
                    break;
                }
                else if (8 <= distance)
                {
                    rb.velocity = Vector2.zero;
                    currentState = State.Tp;
                    break;
                }
            }
            else
            {
                rb.velocity = Vector2.zero;
                currentState = State.Idle;
                break;
            }

            yield return null;
        }
        if (distance < ackDistance)
        {
            rb.velocity = Vector2.zero;
            currentState = State.Attack;
        }
    }
    bool checkLeft()
    {
        if (Physics2D.Raycast(target.transform.position, new Vector2(-1, 0), 2f, layerMask))
        {
            return true;
        }
        return false;
    }

     IEnumerator Effusio()
    {
        yield return new WaitForSeconds(2f);

        MyAnimSetTrigger("Attack");

        yield return null;
    }
}