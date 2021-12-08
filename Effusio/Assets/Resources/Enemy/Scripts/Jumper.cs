using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumper : Enemy
{
    public enum State
    {
        Idle,
        Jump
    };
    public State currentState = State.Idle;

    public Transform[] wallCheck;

    WaitForSeconds Delay500 = new WaitForSeconds(0.5f);


    void Awake()
    {
        base.Awake();
        ackDistance = 5f;
        moveSpeed = 1f;
        jumpPower = 10f;
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
        MyAnimSetTrigger("Idle");
        yield return new WaitForSecondsRealtime(2f);

        while (!IsTargetDir())
        {
            MonsterFlip();
        }
        if (IsTargetDir())
        {
            MyAnimSetTrigger("Jump");
            rb.velocity = new Vector2(-transform.localScale.x * moveSpeed, jumpPower);
            yield return Delay500;
            currentState = State.Idle;
        }
    }
   
    private void Update()
    {
    }
}
