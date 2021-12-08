using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soul : MonoBehaviour
{
    public string bossName;
    protected GameObject target;
    private void Start()
    {
        target = GameObject.FindWithTag("Player");
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerController playerController = target.GetComponent<PlayerController>();
            string[] idList = playerController.effusioIdList;
            for (int i = 0; i < 3; i++)
            {
                if (idList[i].Equals("Empty"))
                {
                    playerController.effusioSelect = i;
                    playerController.selectEffusioId = bossName;
                    playerController.effusioIdList[i] = bossName;
                    playerController.effusio.loadEffusioInfo(bossName);
                    if (i == 0) { playerController.playerInfo.effusioId_a = bossName; }
                    else if (i == 1) { playerController.playerInfo.effusioId_b = bossName; }
                    else if (i == 2) { playerController.playerInfo.effusioId_c = bossName; }

                    GameObject.Find("GameManager").GetComponent<GameManager>().Refresh();
                    GameObject.Find("GameManager").GetComponent<GameManager>().potal.SetActive(true);
                    Destroy(this.gameObject);
                    return;
                }
            }
        }
    }
}