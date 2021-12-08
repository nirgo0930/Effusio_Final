using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    GameObject player;
    Advent info;
    float timer;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        info = GameObject.Find("AdventInfo").GetComponent<Advent>();
        timer = (float)info.point;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            Destroy(this.gameObject);
        }
        player.layer = LayerMask.NameToLayer("PlayerAttacked");
        transform.position = player.transform.position;
    }
    void OnDestroy()
    {
        player.layer = LayerMask.NameToLayer("Player");
    }
}
