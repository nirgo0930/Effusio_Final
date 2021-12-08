using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MoveProlog : MonoBehaviour
{
    int count = 0;
    // Start is called before the first frame update
     public Image Panel;
     public GameObject box;
    float time = 0f;
    float F_time=1f;
    void Start()
    {
    }

    void Update () 
    {
        if(count == 0)
        {
            StartCoroutine(FadeFlow());
        }
        if(count < 11300){
            box.transform.Translate(new Vector3(0,0.1f,0));
            count++;
        }
        if(count == 11300)
            SceneManager.LoadScene("CreatePlayer");
    }
    
    IEnumerator FadeFlow(){
        Panel.gameObject.SetActive(true);
        time=0f;
        Color alpha = Panel.color;
        
        yield return new WaitForSeconds(1f);

        while(alpha.a > 0f){
            time += Time.deltaTime/F_time;
            alpha.a = Mathf.Lerp(1,0,time);
            Panel.color = alpha;
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        Panel.gameObject.SetActive(false);
        yield return null;
    }
    public void Skip(){
        SceneManager.LoadScene("CreatePlayer");
    }
}
