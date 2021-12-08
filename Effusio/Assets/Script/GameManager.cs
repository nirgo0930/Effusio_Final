using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Text UIHealth;
    public GameObject playerObj;
    public PlayerController playerContorller;
    public PlayerInfo playerInfo;
    public Advent adventInfo;
    public Effusio effusioInfo;
    public GenerateMap map;
    public GameObject miniObj;
    public GameObject userImageObj;
    public GameObject skillObj;
    public GameObject effusioImageObj;
    public Image effusioImage;
    public GameObject effusioBaseObj;
    public Image effusioBaseImage;
    public GameObject adventImageObj;
    public Image adventImage;
    public GameObject adventBaseObj;
    public Image adventBaseImage;
    public GameObject gameOverUI;
    public GameObject gameClearUI;
    public GameObject potal;
    private bool isEnded = true;
    public int hp;
    public int hp_max = 5;
    public bool EffusioOn;
    public float EffusioCoomTime;
    public bool AdventOn;
    public float AdventCoomTime;
    public float EffusioLeftTime = 0.0f;
    public float AdventLeftTime = 0.0f;
    void Start()
    {
#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
        GameObject player = Resources.Load("Prefab/Player") as GameObject;

        Instantiate(player, new Vector3(2, 3, 0), transform.rotation);
        DontDestroyOnLoad(GameObject.Find("InGameUI"));
        playerObj = GameObject.FindWithTag("Player");

        playerContorller = playerObj.GetComponent<PlayerController>();
        playerInfo = GameObject.Find("PlayerInfo").GetComponent<PlayerInfo>();
        adventInfo = GameObject.Find("AdventInfo").GetComponent<Advent>();
        effusioInfo = GameObject.Find("EffusioInfo").GetComponent<Effusio>();

        userImageObj = GameObject.Find("userImage");
        miniObj = GameObject.Find("minimapIcon");
        effusioImageObj = GameObject.Find("effusio_Image");
        effusioBaseObj = GameObject.Find("effusio_BaseImage");
        adventImageObj = GameObject.Find("advent_Image");
        adventBaseObj = GameObject.Find("advent_BaseImage");

        effusioImage = effusioImageObj.GetComponent<Image>();
        effusioBaseImage = effusioBaseObj.GetComponent<Image>();
        adventImage = adventImageObj.GetComponent<Image>();
        adventBaseImage = adventBaseObj.GetComponent<Image>();

#if UNITY_EDITOR
        userImageObj.GetComponent<Image>().sprite = Resources.Load<Sprite>("Player/Sprites/new_face");
        miniObj.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Player/Sprites/new_face");
#else
        string path = UnityEngine.Application.streamingAssetsPath + "/FD/new_face.png";
        Texture2D texture2D = new Texture2D(0,0);
        byte[] byteTexture = System.IO.File.ReadAllBytes(path);
        texture2D.LoadImage(byteTexture);
        Sprite face = Sprite.Create(texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height),new Vector2(0.5f, 0.5f), 100.0f);
        userImageObj.GetComponent<Image>().sprite = face;
        miniObj.GetComponent<SpriteRenderer>().sprite = face;
#endif
        effusioImage.sprite = Resources.Load<Sprite>("Boss/" + playerContorller.selectEffusioId + "/Sprites/Icon");
        effusioBaseImage.sprite = Resources.Load<Sprite>("Boss/" + playerContorller.selectEffusioId + "/Sprites/Icon");
        adventImage.sprite = Resources.Load<Sprite>("Player/Advent/Sprites/" + playerContorller.selectAdventId);
        adventBaseImage.sprite = Resources.Load<Sprite>("Player/Advent/Sprites/" + playerContorller.selectAdventId);

        effusioImage.type = Image.Type.Filled;
        effusioImage.fillMethod = Image.FillMethod.Radial360;
        effusioImage.fillOrigin = (int)Image.Origin360.Top;
        effusioImage.fillClockwise = false;

        adventImage.type = Image.Type.Filled;
        adventImage.fillMethod = Image.FillMethod.Radial360;
        adventImage.fillOrigin = (int)Image.Origin360.Top;
        adventImage.fillClockwise = false;

        EffusioOn = playerContorller.isEffusioUse;
        AdventOn = playerContorller.isAdventUse;

        skillObj = GameObject.Find("SkillInfo");
        skillObj.SetActive(false);

        gameOverUI = GameObject.Find("GameOver");
        gameOverUI.SetActive(false);

        gameClearUI = GameObject.Find("GameClear");
        gameClearUI.SetActive(false);
    }
    private void Update()
    {
        hp = playerInfo.hp;
        hp_max = playerInfo.maxHp;
        EffusioCoomTime = effusioInfo.coolTime;
        AdventCoomTime = adventInfo.skillCoolTime;
        EffusioOn = playerContorller.isEffusioUse;
        AdventOn = playerContorller.isAdventUse;

        string heart = "";
        for (int i = 0; i < hp_max; i++)
        {
            if (i < hp) { heart += "♥ "; }
        }
        UIHealth.text = heart;


        if ((Input.GetKeyDown("1") || Input.GetKeyDown("2") || Input.GetKeyDown("3")) && EffusioOn)
        {
            effusioImage.sprite = Resources.Load<Sprite>("Boss/" + playerContorller.selectEffusioId + "/Sprites/Icon");
            effusioBaseImage.sprite = Resources.Load<Sprite>("Boss/" + playerContorller.selectEffusioId + "/Sprites/Icon");
        }


        if (Input.GetKeyDown(KeyCode.V) && EffusioOn)
        {
            EffusioLeftTime = EffusioCoomTime;
        }
        else if (!EffusioOn)
        {
            if (EffusioLeftTime > 0)
            {
                EffusioLeftTime -= Time.deltaTime;
                if (EffusioLeftTime < 0)
                {
                    EffusioLeftTime = 0;
                }
                float ratio = 1.0f - (EffusioLeftTime / EffusioCoomTime);
                if (effusioImage)
                    effusioImage.fillAmount = ratio;
            }
        }

        if (Input.GetKeyDown(KeyCode.Z) && AdventOn)
        {
            AdventLeftTime = AdventCoomTime;
        }
        else if (!AdventOn)
        {
            if (AdventLeftTime > 0)
            {
                AdventLeftTime -= Time.deltaTime;
                if (AdventLeftTime < 0)
                {
                    AdventLeftTime = 0;
                }
                float ratio = 1.0f - (AdventLeftTime / AdventCoomTime);
                if (adventImage)
                    adventImage.fillAmount = ratio;
            }
        }
    }

    public void Refresh()
    {

#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
        userImageObj = GameObject.Find("userImage");
        miniObj = GameObject.Find("minimapIcon");

#if UNITY_EDITOR
        userImageObj.GetComponent<Image>().sprite = Resources.Load<Sprite>("Player/Sprites/new_face");
        miniObj.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Player/Sprites/new_face");
#else
        string path = UnityEngine.Application.streamingAssetsPath + "/FD/new_face.png";
        Texture2D texture2D = new Texture2D(0,0);
        byte[] byteTexture = System.IO.File.ReadAllBytes(path);
        texture2D.LoadImage(byteTexture);
        Sprite face = Sprite.Create(texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height),new Vector2(0.5f, 0.5f), 100.0f);
        userImageObj.GetComponent<Image>().sprite = face;
        miniObj.GetComponent<SpriteRenderer>().sprite = face;
#endif

        effusioImageObj = GameObject.Find("effusio_Image");
        effusioImage = effusioImageObj.GetComponent<Image>();
        effusioImage.sprite = Resources.Load<Sprite>("Boss/" + playerContorller.selectEffusioId + "/Sprites/Icon");

        effusioBaseObj = GameObject.Find("effusio_BaseImage");
        effusioBaseImage = effusioBaseObj.GetComponent<Image>();
        effusioBaseImage.sprite = Resources.Load<Sprite>("Boss/" + playerContorller.selectEffusioId + "/Sprites/Icon");

        adventImageObj = GameObject.Find("advent_Image");
        adventImage = adventImageObj.GetComponent<Image>();
        adventImage.sprite = Resources.Load<Sprite>("Player/Advent/Sprites/" + playerContorller.selectAdventId);

        adventBaseObj = GameObject.Find("advent_BaseImage");
        adventBaseImage = adventBaseObj.GetComponent<Image>();
        adventBaseImage.sprite = Resources.Load<Sprite>("Player/Advent/Sprites/" + playerContorller.selectAdventId);
    }

    public void save()
    {
        print("save");
        playerInfo.savePlayerInfo();
    }

    public void die()
    {
        gameOverUI.SetActive(true);
    }
}