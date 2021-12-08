using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieGrasp : MonoBehaviour
{
    Advent info;
    Animator animator;
    float timer;
    GameObject enemy;
    // Start is called before the first frame update
    void Start()
    {
        info = GameObject.Find("AdventInfo").GetComponent<Advent>();
        animator = GetComponent<Animator>();
        timer = (float)info.point;
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

            StartCoroutine(bindCoroutine(enemy.GetComponent<Enemy>()));
        }
        catch
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            animator.SetTrigger("End");
            StartCoroutine(endCoroutine(enemy.GetComponent<Enemy>()));
        }
        Vector3 addPos = new Vector3(0, 1, 0);
        try
        {
            transform.position = enemy.transform.position + addPos;
        }
        catch
        {
            Destroy(this.gameObject);
        }
    }

    private IEnumerator bindCoroutine(Enemy target)
    {
        yield return new WaitForSeconds(0f);
        target.moveSpeed *= 0.2f;
        target.jumpPower *= 0.2f;
    }

    private IEnumerator endCoroutine(Enemy target)
    {
        yield return new WaitForSeconds(0f);
        target.moveSpeed *= 5f;
        target.jumpPower *= 5f;
        Destroy(this.gameObject);
    }
}
