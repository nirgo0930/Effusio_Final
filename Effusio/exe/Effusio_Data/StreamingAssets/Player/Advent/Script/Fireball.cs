using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    GameObject player;
    GameObject enemy;
    Advent info;
    public Rigidbody2D fbRb;
    public float turn;
    public int damage;
    float timer = 10f;
    float max_Speed = 10f;
    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        info = GameObject.Find("AdventInfo").GetComponent<Advent>();
        damage = info.point;

        List<GameObject> FoundObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));

        try
        {
            enemy = FoundObjects[0];
            float shortDis = Vector3.Distance(gameObject.transform.position, enemy.transform.position);

            foreach (GameObject found in FoundObjects)
            {
                float Distance = Vector3.Distance(gameObject.transform.position, found.transform.position);
                if (Distance < shortDis)
                {
                    enemy = found;
                    shortDis = Distance;
                }
            }
            fbRb = GetComponent<Rigidbody2D>();
            fbRb.gravityScale = 0;

            Vector3 dir = (enemy.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            GetComponent<Rigidbody2D>().velocity = dir * max_Speed;
        }
        catch (Exception)
        {
            print("else");
            Vector3 dir = new Vector3(player.transform.localScale.x, 0, 0).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            GetComponent<Rigidbody2D>().velocity = dir * max_Speed;
        }
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0) { Destroy(this.gameObject); }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            max_Speed = max_Speed * 0.9f;
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Enemy target = other.gameObject.GetComponent<Enemy>();

            Destroy(this.gameObject);
            target.TakeDamage(damage);
        }
    }
}