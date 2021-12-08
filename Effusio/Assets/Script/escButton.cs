using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class escButton : MonoBehaviour
{
    public GameObject escWindow;
    public GameObject mainMenuBtn;
    public GameObject soundMenuBtn;
    public GameObject BGM;
    public GameObject selectAdvent;
    public float offsetX = 0f;
    public float offsetY = 2;
    public float offsetZ = -5f;
    Vector3 escWindowPostion;
    private bool state;
    // Start is called before the first frame update
    void Start()
    {
        escWindow = GameObject.Find("escButton");
        mainMenuBtn = GameObject.Find("nomalButton");
        soundMenuBtn = GameObject.Find("soundOption");
        selectAdvent = GameObject.Find("SelectAdvent");

        BGM = GameObject.Find("BGM");
        soundMenuBtn.SetActive(false);
        escWindow.SetActive(false);
        state = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(state == false)
            {
                Time.timeScale = 0;
                escWindow.SetActive(true);
                state = true;
            }
            else
            {
                if(selectAdvent != null){
                    if(selectAdvent.active)
                    {
                        Time.timeScale = 0;
                    }
                    else
                    {
                        Time.timeScale = 1;
                    }
                }
                else
                {
                    Time.timeScale = 1;
                }

                soundMenuBtn.SetActive(false);
                mainMenuBtn.SetActive(true);
                escWindow.SetActive(false);
                state = false;
            }
        }
    }

    public void goMainMenu(){
        Time.timeScale = 1;
        SceneManager.LoadScene("StartScrene");
        BGM = GameObject.Find("BGM");
        Destroy(BGM);
        Destroy(GameObject.Find("InGameUI"));
        Destroy(GameObject.FindWithTag("Player"));
    }
    public void gameExit(){
        Time.timeScale = 1;
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    public void soundMange(){
        mainMenuBtn.SetActive(false);
        soundMenuBtn.SetActive(true);
    }
    public void goBack(){
        soundMenuBtn.SetActive(false);
        mainMenuBtn.SetActive(true);
    }
    public void closeEscWindow(){
        soundMenuBtn.SetActive(false);
        mainMenuBtn.SetActive(true);        
        Time.timeScale = 1;
        escWindow.SetActive(false);
        state = false;
    }
}
