using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trunk : Enemy
{
    public enum State
    {
        Idle,
        Run,
        Attack,
    };
    public State currentState = State.Idle;

    public Transform[] wallCheck;
    public Transform genPoint;
    public GameObject Bullet;
    public Bullet bulletInfo;
    

    WaitForSeconds Delay1000 = new WaitForSeconds(1f);
    private void Start()
    {
        bulletInfo = GetComponent<Bullet>();
    }

    void Awake()
    {
        base.Awake();
        ackDistance = 15f;
        moveSpeed = 2f;
        jumpPower = 8f;
        currentHp = 40;
        atkCoolTime = 3f;
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
        yield return null;
        MyAnimSetTrigger("Idle");
        if (Random.value > 0.5f)
        {
            MonsterFlip();
        }
        yield return Delay1000;
        currentState = State.Run;
    }
    IEnumerator Run()
    {
        yield return null;
        float runTime = Random.Range(2f, 3f);
        while (runTime >= 0f)
        {
            runTime -= Time.deltaTime;
            
            if (!isHit)
            {
                MyAnimSetTrigger("Run");
                rb.velocity = new Vector2(-transform.localScale.x * moveSpeed, rb.velocity.y);

                if (!Physics2D.OverlapCircle(wallCheck[0].position, 0.01f, layerMask) &&
                Physics2D.OverlapCircle(wallCheck[1].position, 0.01f, layerMask) &&
                 !Physics2D.Raycast(transform.position, -transform.localScale.x * transform.right, 1f, layerMask))
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpPower);
                }
                else if (Physics2D.OverlapCircle(wallCheck[1].position, 0.01f, layerMask))
                {
                    MonsterFlip();
                }
                if (canAtk && IsTargetDir())
                {
                    if (Vector2.Distance(transform.position, target.transform.position) < ackDistance)
                    {
                        currentState = State.Attack;
                        break;
                    }
                }

            }
            yield return null;
        }
        if (currentState != State.Attack)
        {
            if (Random.value > 0.5f)
            {
                MonsterFlip();
            }
            else
            {
                currentState = State.Idle;
            }
        }
    }
    IEnumerator Attack()
    {
        yield return null;

        canAtk = false;
        rb.velocity = new Vector2(0, jumpPower);
        MyAnimSetTrigger("Attack");

        yield return Delay1000;
        currentState = State.Idle;
    }
    void Fire()
    {
        GameObject bulletClone = Instantiate(Bullet, genPoint.position, transform.rotation);
        Rigidbody2D bulletClone_rb = bulletClone.GetComponent<Rigidbody2D>();
        bulletClone_rb.velocity = transform.right * -transform.localScale.x * 17f;
        bulletClone.transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y);
    }
}
