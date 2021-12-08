using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEye : Enemy
{
    public enum State
    {
        Flight,
        FlightLeft,
        FlightRight,
        FlightUp

    };
    public State currentState = State.Flight;
    public float wanderDis;
    public Vector3 targetPos;

    void Awake()
    {
        base.Awake();
        ackDistance = 0f;
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

    IEnumerator Flight()
    {

        if (!isHit)
        {
            while (!IsTargetDir())
            {
                MonsterFlip();
            }

            if (transform.position.x < target.transform.position.x)
            {
                currentState = State.FlightRight;
            }
            else if (transform.position.x > target.transform.position.x)
            {
                currentState = State.FlightLeft;
            }
        }

        rb.velocity = Vector2.zero;

        yield return null;
    }

    IEnumerator FlightLeft()
    {
        if (!isHit)
        {
            distance = Vector2.Distance(target.transform.position, transform.position);
            float randVal = Random.Range(0.8f, 1.5f);
            wanderDis = -transform.localScale.x * randVal;

            targetPos = new Vector3(target.transform.position.x + wanderDis, target.transform.position.y - 0.1f, target.transform.position.z);

            transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed);

            if (transform.position.x < targetPos.x + 1f)
            {
                currentState = State.Flight;
            }
        }
        rb.velocity = Vector2.zero;

        yield return null;
    }
    IEnumerator FlightRight()
    {

        if (!isHit)
        {
            MyAnimSetTrigger("Flight");

            distance = Vector2.Distance(target.transform.position, transform.position);

            float randVal = Random.Range(0.8f, 1.5f);

            wanderDis = -transform.localScale.x * randVal;

            targetPos = new Vector3(target.transform.position.x + wanderDis, target.transform.position.y - 0.1f, target.transform.position.z);

            transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed);

            if (transform.position.x > targetPos.x - 1f)
            {
                currentState = State.Flight;
            }
        }

        rb.velocity = Vector2.zero;

        yield return null;
    }
}
