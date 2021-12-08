using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blue : Enemy
{
    public Transform[] wallCheck;

    private void Awake()
    {
        base.Awake();
        jumpPower = 10f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        rb.velocity = new Vector2(-transform.localScale.x * moveSpeed, rb.velocity.y);

        Debug.DrawRay(transform.position, -transform.localScale.x * transform.right, new Color(1, 0, 0), 1);
        
        if (!Physics2D.OverlapCircle(wallCheck[0].position, 0.01f, layerMask) &&
                Physics2D.OverlapCircle(wallCheck[1].position, 0.01f, layerMask) &&
                !Physics2D.Raycast(transform.position, -transform.localScale.x * transform.right, 0.5f, layerMask))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
        }
        else if (Physics2D.OverlapCircle(wallCheck[0].position, 0.01f, layerMask) && Physics2D.OverlapCircle(wallCheck[1].position, 0.01f, layerMask))
        {
            MonsterFlip();
        }
        if (isHit && !IsTargetDir() && IsPlayingAnim("Hit")) 
        {
            MonsterFlip();
        }

    }
}