using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life : MonoBehaviour
{
    public int healVal;
    protected GameObject target;
    private void Start()
    {
        target = GameObject.FindWithTag("Player");
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Destroy(this.gameObject);
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerController playerController = target.GetComponent<PlayerController>();
            PlayerInfo info = playerController.GetComponent<PlayerController>().playerInfo;
            info.hp = Math.Min(info.hp + 1, info.maxHp);
            Destroy(this.gameObject);

        }
    }
}