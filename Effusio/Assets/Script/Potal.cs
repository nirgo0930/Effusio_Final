using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class Potal : MonoBehaviour
{
    public GameObject player;
    public string curStage;
    // Start is called before the first frame update
    void Start()
    {
        Scene scene = SceneManager.GetActiveScene();
        curStage = scene.name;
        player = GameObject.FindWithTag("Player");
        player.GetComponent<PlayerController>().playerInfo.curStage = this.curStage;
        player.GetComponent<PlayerController>().playerInfo.savePlayerInfo();
        GameObject.Find("GameManager").GetComponent<GameManager>().potal = this.gameObject;
#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
        if (curStage.Contains("BossStage"))
        {
            transform.position = new Vector2(17, 1);
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player" && Input.GetKeyDown(KeyCode.UpArrow))
        {

            nextMap();
        }
    }

    void nextMap()
    {
        if (curStage.Equals("Stage1"))
        {
            player.transform.position = new Vector3(2, 2, 0);

            player.GetComponentInChildren<PlayerInfo>().savePlayerInfo();
#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif
            SceneManager.LoadScene("BossStage1");
        }
        else if (curStage.Equals("BossStage1"))
        {
            print("move");
            player.transform.position = new Vector3(2, 2, 0);

            player.GetComponentInChildren<PlayerInfo>().savePlayerInfo();
#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif
            SceneManager.LoadScene("Stage2");
        }
        else if (curStage.Equals("Stage2"))
        {
            print("move");
            player.transform.position = new Vector3(2, 2, 0);

            player.GetComponentInChildren<PlayerInfo>().savePlayerInfo();
#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif
            SceneManager.LoadScene("BossStage2");
        }
        else if (curStage.Equals("BossStage2"))
        {
            print("move");
            player.transform.position = new Vector3(2, 2, 0);

            player.GetComponentInChildren<PlayerInfo>().savePlayerInfo();
#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif
            SceneManager.LoadScene("Stage3");
        }
        else if (curStage.Equals("Stage3"))
        {
            print("move");
            player.transform.position = new Vector3(2, 2, 0);

            player.GetComponentInChildren<PlayerInfo>().savePlayerInfo();
#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif
            SceneManager.LoadScene("BossStage3");
        }
        else if (curStage.Equals("BossStage3"))
        {
            print("move");
            player.transform.position = new Vector3(2, 2, 0);

            player.GetComponentInChildren<PlayerInfo>().savePlayerInfo();
#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif
            SceneManager.LoadScene("Stage4");
        }
        else if (curStage.Equals("Stage4"))
        {
            print("move");
            player.transform.position = new Vector3(2, 2, 0);

            player.GetComponentInChildren<PlayerInfo>().savePlayerInfo();
#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif
            SceneManager.LoadScene("BossStage4");
        }
    }
}
