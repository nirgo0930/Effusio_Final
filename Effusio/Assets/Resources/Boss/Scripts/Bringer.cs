using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bringer : Enemy
{
    public enum State
    {
        Idle,
        Walk,
        Attack,
        Cast
    };
    public State currentState = State.Idle;
    public GameObject thunder;

    private void Awake()
    {
        base.Awake();
        bossName = "Bringer";
        moveSpeed = 1f;
        jumpPower = 0f;
        currentHp = 120;
        atkCoolTime = 3f;
        atkCoolTimeCalc = atkCoolTime;

        if (this.tag == "Summon")
        {
            try
            {
                List<GameObject> FoundObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
                target = FoundObjects[0];
                target.tag = "Enemy";
                float shortDis = Vector3.Distance(gameObject.transform.position, target.transform.position);

                foreach (GameObject found in FoundObjects)
                {
                    float Distance = Vector3.Distance(gameObject.transform.position, found.transform.position);
                    if (Distance < shortDis)
                    {
                        target = found;
                        shortDis = Distance;
                    }
                }
            }
            catch(Exception)
            {
                Destroy(this.gameObject, 1f);
            }


            Spell();
            Destroy(this, 5f);
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
        if (!isHit && isGround)
        {
            canAtk = false;
            MyAnimSetTrigger("Attack");
            yield return new WaitForSeconds(1.5f);
            currentState = State.Idle;
        }
    }
    IEnumerator Walk()
    {
        MyAnimSetTrigger("Walk");
        float distance = Vector2.Distance(target.transform.position, transform.position);
        if (canAtk && IsTargetDir())
        {
            rb.velocity = new Vector2(-transform.localScale.x * moveSpeed, 0);
            if (1 < distance && distance < 2.4)
            {
                rb.velocity = Vector2.zero;
                currentState = State.Attack;
            }
            else if (7 < distance && distance < 10)
            {
                rb.velocity = Vector2.zero;
                currentState = State.Cast;
            }
        }
        else
        {
            yield return new WaitForSeconds(1f);
            currentState = State.Idle;
        }
    }

    IEnumerator Cast()
    {
        canAtk = false;
        if (!isHit)
        {
            MyAnimSetTrigger("Cast");
            yield return new WaitForSeconds(1f);
        }
        if (!isHit)
        {
            MyAnimSetTrigger("Cast");
            yield return new WaitForSeconds(1f);
        }
        if (!isHit)
        {
            MyAnimSetTrigger("Cast");
            yield return new WaitForSeconds(1f);
        }
        currentState = State.Idle;
    }

    IEnumerator Idle()
    {
        MyAnimSetTrigger("Idle");
        yield return new WaitForSeconds(1f);

        while (!IsTargetDir())
        {
            MonsterFlip();
        }

        currentState = State.Walk;
    }
    void Spell()
    {
        StartCoroutine(Thunder());
    }

    IEnumerator Thunder()
    {
        Vector3 genPoint = new Vector3(target.transform.position.x, target.transform.position.y + 3.5f, 0);
        
        thunder.GetComponent<BringerThunder>().target = target;
        GameObject thunderClone = Instantiate(thunder, genPoint, Quaternion.identity);;
        
        yield return new WaitForSeconds(1f);
        Destroy(thunderClone);
    }

}