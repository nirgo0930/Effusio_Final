using System.Collections;
using UnityEngine;

public class Goblin : Enemy
{
    public enum State
    {
        Idle,
        Run,
        Attack,
        Jump,
        Wander
    };
    public State currentState = State.Idle;

    public Transform[] wallCheck;

    void Awake()
    {
        base.Awake();
        ackDistance = 1.5f;
        moveSpeed = 1;
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
        print("Idle");
        MyAnimSetTrigger("Idle");
        yield return new WaitForSeconds(1f);
        distance = Vector2.Distance(target.transform.position, transform.position);
        //print("���� : �÷��̾�� �Ÿ� = " + distance);

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
                currentState = State.Run;
            }
            else if (8 <= distance)
            {
                currentState = State.Wander;
            }
        }

    }
    IEnumerator Run()
    {
        print("Run");
        distance = Vector2.Distance(target.transform.position, transform.position);
        while (distance >= ackDistance)
        {
            distance = Vector2.Distance(target.transform.position, transform.position);
            print("������ : �÷��̾�� �Ÿ� = " + distance);

            MyAnimSetTrigger("Run");
            rb.velocity = new Vector2(-transform.localScale.x * moveSpeed, rb.velocity.y);

            if (Physics2D.OverlapCircle(wallCheck[0].position, 0.4f, layerMask) &&
            Physics2D.OverlapCircle(wallCheck[1].position, 0.4f, layerMask))
            {
                MonsterFlip();
            }
            

            if (canAtk)
            {
                if (ackDistance <= distance && distance < 8)
                {
                    currentState = State.Run;
                    break;
                }
                else if (8 <= distance)
                {
                    rb.velocity = Vector2.zero;
                    currentState = State.Idle;
                    break;
                }
            }

        }
        if (distance < ackDistance)
        {
            rb.velocity = Vector2.zero;
            currentState = State.Attack;
        }
        yield return null;
    }
    IEnumerator Attack()
    {
        print("Attack");
        if (!isHit)
        {
            if (distance <= ackDistance)
            {
                canAtk = false;
                MyAnimSetTrigger("Attack");
                yield return new WaitForSeconds(1f);
                currentState = State.Idle;
            }

        }


    }
    IEnumerator Wander()
    {
        print("Chase");
        distance = Vector2.Distance(target.transform.position, transform.position);
        while (distance >= 8)
        {
            distance = Vector2.Distance(target.transform.position, transform.position);

            float flag = Random.Range(0, 3);

            if (flag == 0)
            {
                float flip = Random.Range(0, 2);
                MyAnimSetTrigger("Idle");
                yield return new WaitForSecondsRealtime(3f);
                if (flip==0)
                {
                    MonsterFlip();
                }
            }
            else if (flag == 1)
            {
                float runTime = Random.Range(2,4);
                if (runTime == 2)
                {
                    MonsterFlip();
                }
                while (runTime >= 0)
                {
                    distance = Vector2.Distance(target.transform.position, transform.position);
                    if (distance < 8)
                    {
                        break;
                    }
                    runTime -= Time.deltaTime;
                    
                    if (!isHit)
                    {
                        MyAnimSetTrigger("Run");
                        rb.velocity = new Vector2(-transform.localScale.x * moveSpeed, rb.velocity.y);
                        //if (runTime == 2)
                        //{
                        //    rb.velocity = new Vector2(-transform.localScale.x * moveSpeed, rb.velocity.y);
                        //}
                        //else if (runTime == 3)
                        //{
                        //    rb.velocity = new Vector2(-transform.localScale.x * moveSpeed*-1, rb.velocity.y);
                        //}

                        if (Physics2D.OverlapCircle(wallCheck[0].position, 0.01f, layerMask) &&
                        Physics2D.OverlapCircle(wallCheck[1].position, 0.01f, layerMask))
                        {
                            MonsterFlip();
                        }
                    }
                    yield return null;
                }
            }
        }
        if(distance< 8)
        {
            rb.velocity = Vector2.zero;
            currentState = State.Run;
        }

    }

    private void Update()
    {
        GroundCheck();
        //if (!isHit && isGround && !IsPlayingAnim("Run"))
        //{
        //    boxCollider.offset = boxColliderOffset;
        //    MyAnimSetTrigger("Idle");
        //}
    }
}
