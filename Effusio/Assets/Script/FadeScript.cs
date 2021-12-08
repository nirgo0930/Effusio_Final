using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
public class FadeScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Image Panel;
    float time = 0f;
    float F_time = 1f;

    public GameObject mainMenuBtn;
    public GameObject soundManageBtn;

    private void Start()
    {
        GameObject dataPath = GameObject.Find("DataPath");
        DontDestroyOnLoad(dataPath);
        GameObject so =  GameObject.Find("SoundOption");
        if(so != null)
            so.SetActive(false);
    }
    public void Fade()
    {
        GameObject newPlayerInfo = Resources.Load("Prefab/PlayerInfo") as GameObject;
        Instantiate(newPlayerInfo, new Vector3(0, 0, 0), Quaternion.identity);
        newPlayerInfo.GetComponent<PlayerInfo>().savePlayerInfo();

        //new game
        StartCoroutine(FadeFlow());
    }
    IEnumerator FadeFlow()
    {
        Panel.gameObject.SetActive(true);
        time = 0f;
        Color alpha = Panel.color;
        while (alpha.a < 1f)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(0, 1, time);
            Panel.color = alpha;
            yield return null;
        }
        time = 0f;

        SceneManager.LoadScene("Prolog");
    }
    IEnumerator loadGameFadeFlow()
    {
        Panel.gameObject.SetActive(true);
        time = 0f;
        Color alpha = Panel.color;
        while (alpha.a < 1f)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(0, 1, time);
            Panel.color = alpha;
            yield return null;
        }
        time = 0f;
        
        SceneManager.LoadScene("Town");
    }

    public void loadGameStart()
    {
        StartCoroutine(loadGameFadeFlow());
    }

    public void optionBtn()
    {
        mainMenuBtn.SetActive(false);
        soundManageBtn.SetActive(true);
    }

    public void goBackBtn()
    {
        mainMenuBtn.SetActive(true);
        soundManageBtn.SetActive(false);
    }

    public void GameQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

}