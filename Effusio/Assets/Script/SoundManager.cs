using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public GameObject BGM;
    public AudioSource normalBGM;
    public AudioMixer bgmMixer;
    public AudioClip[] audioClips;
    public Slider audioSlider;

    public bool status;
    void Start()
    {
        BGM = GameObject.Find("BGM");
        normalBGM = GameObject.Find("BGM").GetComponent<AudioSource>();
        status = true;
    }

    void Update()
    {
        if(GameObject.Find("Slider") != null){
            float soundVal;
            bgmMixer.GetFloat("BGM", out soundVal);
            audioSlider.value = soundVal;
        }

        Scene scene = SceneManager.GetActiveScene();
        if(scene.name.Equals("StartScrene") && !BGM.GetComponent<AudioSource>().isPlaying){
            BGM.GetComponent<AudioSource>().Stop();
            BGM.GetComponent<AudioSource>().clip = audioClips[0];
            BGM.GetComponent<AudioSource>().Play();
        }
        else if(scene.name.Equals("Town") && status){
            BGM.GetComponent<AudioSource>().Stop();
            BGM.GetComponent<AudioSource>().clip = audioClips[1];
            BGM.GetComponent<AudioSource>().Play();
            status = false;
        }
        else if(scene.name.Equals("Stage1") && !status){
            BGM.GetComponent<AudioSource>().Stop();
            BGM.GetComponent<AudioSource>().clip = audioClips[0];
            BGM.GetComponent<AudioSource>().Play();
            status = true;
        }
        else if(scene.name.Equals("BossStage1") && status){
            BGM.GetComponent<AudioSource>().Stop();
            BGM.GetComponent<AudioSource>().clip = audioClips[2];
            BGM.GetComponent<AudioSource>().Play();
            status = false;
        }
        else if(scene.name.Equals("Stage2") && !status){
            BGM.GetComponent<AudioSource>().Stop();
            BGM.GetComponent<AudioSource>().clip = audioClips[0];
            BGM.GetComponent<AudioSource>().Play();
            status = true;
        }
        else if(scene.name.Equals("BossStage2") && status){
            BGM.GetComponent<AudioSource>().Stop();
            BGM.GetComponent<AudioSource>().clip = audioClips[2];
            BGM.GetComponent<AudioSource>().Play();
            status = false;
        }
        else if(scene.name.Equals("Stage3") && !status){
            BGM.GetComponent<AudioSource>().Stop();
            BGM.GetComponent<AudioSource>().clip = audioClips[0];
            BGM.GetComponent<AudioSource>().Play();
            status = true;
        }
        else if(scene.name.Equals("BossStage3") && status){
            BGM.GetComponent<AudioSource>().Stop();
            BGM.GetComponent<AudioSource>().clip = audioClips[2];
            BGM.GetComponent<AudioSource>().Play();
            status = false;
        }
        else if(scene.name.Equals("Stage4") && !status){
            BGM.GetComponent<AudioSource>().Stop();
            BGM.GetComponent<AudioSource>().clip = audioClips[0];
            BGM.GetComponent<AudioSource>().Play();
            status = true;
        }
        else if(scene.name.Equals("BossStage4") && status){
            BGM.GetComponent<AudioSource>().Stop();
            BGM.GetComponent<AudioSource>().clip = audioClips[3];
            BGM.GetComponent<AudioSource>().Play();
            status = false;
        }
    }
    private void Awake() {
        BGM = GameObject.Find("BGM");
        normalBGM= BGM.GetComponent<AudioSource>(); //???????????? ????????????
        if (normalBGM.isPlaying) 
        {
            DontDestroyOnLoad(BGM);
            return; //??????????????? ???????????? ????????? ??????
        }
        else
        {
            normalBGM.Play();
            DontDestroyOnLoad(BGM); //???????????? ?????? ????????????(?????? ????????????????????? ??????)
        }
    }

    public void AudioControl(){
        float sound = audioSlider.value;

        if(sound == -40f) bgmMixer.SetFloat("BGM", -80);
        else bgmMixer.SetFloat("BGM", sound);
    }
}
