using System.Collections;
using UnityEngine;

public class BringerThunder : MonoBehaviour
{
    public BoxCollider2D boxCollider;
    public Animator anim;
    public GameObject target;
    public int damage = 10;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();

        StartCoroutine(Idle());
        StartCoroutine(Fall());
    }

    IEnumerator Idle()
    {
        anim.SetTrigger("Idle");
        yield return new WaitForSecondsRealtime(3f);
    }
    IEnumerator Fall()
    {
        anim.SetTrigger("Fall");
        yield return new WaitForSecondsRealtime(1.1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (target.tag.Equals(collision.tag))
        {
            if (target.tag.Equals("Player"))
            {
                PlayerController player = collision.gameObject.GetComponent<PlayerController>();
                player.hurt(1);
            }
            else
            {
                Enemy enemy = collision.gameObject.GetComponent<Enemy>();
                enemy.TakeDamage(damage);
            }
        }
    }
}
