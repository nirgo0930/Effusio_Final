using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    protected GameObject target;
    private void Start()
    {
        target = GameObject.FindWithTag("Player");
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            Destroy(this.gameObject);
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerController playerController = target.GetComponent<PlayerController>();

            Destroy(this.gameObject);
            playerController.hurt(damage);
        }
    }
}