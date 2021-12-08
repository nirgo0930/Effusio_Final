using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class StartTown : MonoBehaviour
{
    public GameObject selectAdventWindow;
    Vector3 selectAdventWindowPostion;
    public GameObject player;
    public float offsetX = 0f;
    public float offsetY = 2;
    public float offsetZ = -5f;
    public bool check;
    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        selectAdventWindow.SetActive(false);
        check = true;
    }

    private void Update()
    {
        if (check)
        {
            player = GameObject.FindWithTag("Player");
            if (player == null) { check = true; }
            else { check = false; }
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            selectAdventWindowPostion.x = player.transform.position.x + offsetX;
            selectAdventWindowPostion.y = player.transform.position.y + offsetY;
            selectAdventWindowPostion.z = player.transform.position.z + offsetZ;
            selectAdventWindow.transform.position = selectAdventWindowPostion;


            player.GetComponent<PlayerController>().playerInfo.savePlayerInfo();
            if (player.GetComponent<PlayerController>().playerInfo.curStage.Equals("Town"))
            {
                Time.timeScale = 0;
                selectAdventWindow.SetActive(true);
            }
            else
            {
                player.GetComponent<PlayerController>().selectAdvent(player.GetComponent<PlayerController>().playerInfo.adventId);
                nextMap(player.GetComponent<PlayerController>().playerInfo.curStage, new Vector2(2, 2));
            }
        }
    }

    public void nextMap(string mapName, Vector2 startPoint)
    {
        player.transform.position = startPoint;
#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
        GameObject.Find("GameManager").GetComponent<GameManager>().skillObj.SetActive(true);
        GameObject.Find("GameManager").GetComponent<GameManager>().Refresh();
        SceneManager.LoadScene(mapName);
    }

    public void clickAdvent1()
    {
        Time.timeScale = 1;
        player.GetComponent<PlayerController>().selectAdvent("Shield");
        nextMap("Stage1", new Vector2(2, 2));
    }

    public void clickAdvent2()
    {
        Time.timeScale = 1;
        player.GetComponent<PlayerController>().selectAdvent("Fireball");
        nextMap("Stage1", new Vector2(2, 2));
    }

    public void clickAdvent3()
    {
        Time.timeScale = 1;
        player.GetComponent<PlayerController>().selectAdvent("Heal");
        nextMap("Stage1", new Vector2(2, 2));
    }

    public void clickAdvent4()
    {
        Time.timeScale = 1;
        player.GetComponent<PlayerController>().selectAdvent("ZombieGrasp");
        nextMap("Stage1", new Vector2(2, 2));
    }

    public void closeAdvent()
    {
        Time.timeScale = 1;
        selectAdventWindow.SetActive(false);
    }
}
