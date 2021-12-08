using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera : MonoBehaviour
{
    public GameObject player;

    public float offsetX = 0f;
    public float offsetY = 2f;
    public float offsetZ = -30f;

    Vector3 cameraPostion;
    public bool check;
    void Start()
    {
        cameraPostion = new Vector3(0f, 0f, 0f);
        player = GameObject.FindWithTag("Player");
        check = true;
    }
    void LateUpdate()
    {
        if (check)
        {
            player = GameObject.FindWithTag("Player");
            if (player == null) { check = true; }
            else { check = false; }
        }
        cameraPostion.x = player.transform.position.x + offsetX;
        cameraPostion.y = player.transform.position.y + offsetY;
        cameraPostion.z = player.transform.position.z + offsetZ;

        transform.position = cameraPostion;
    }
}
