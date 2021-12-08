using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using System.Windows.Forms;
using Ookii.Dialogs;
using System.Diagnostics;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class CreatePlayer : MonoBehaviour
{
    public GameObject imageLoadButton;
    public GameObject changFaceButton;
    public GameObject oringinFaceObj;
    public GameObject changeFaceObj;
    public GameObject gameManager;
    public Image originImg;
    public Image changeImg;

    public void FaceImage()
    {
        VistaOpenFileDialog ofd = new VistaOpenFileDialog();
        ofd.Title = "FaceImage";
        ofd.FileName = "test";
        ofd.Filter = "(*.jpg, *.gif, *.bmp, *png) | *.jpg; *.gif; *.bmp; *.png; | (*.*) | *.*";

        DialogResult dr = ofd.ShowDialog();

        if (dr == DialogResult.OK)
        {
            string fileFullName = ofd.FileName;
            print(fileFullName);
            string source = @fileFullName;
            print(source);
            fileFullName = fileFullName.Replace("\\", "/");
#if UNITY_EDITOR
            // 유니티 에디터일 경우
            string face = @"Assets\StreamingAssets\FD\original_face.jpg";
            File.Copy(fileFullName, face, true);
#else
	            // 런타임일 경우
	            string face = UnityEngine.Application.streamingAssetsPath + "/FD/original_face.jpg";
                File.Copy(fileFullName, face, true);
#endif

        }
        //취소버튼 클릭시 또는 ESC키로 파일창을 종료 했을경우
        else if (dr == DialogResult.Cancel) { }

        imageLoadButton.SetActive(false);
        changFaceButton.SetActive(true);
        Excu();
    }

    public void Excu()
    {

        ProcessStartInfo startInfo = new ProcessStartInfo();
        startInfo.FileName = "face_Detection.exe";
        startInfo.Arguments = "shape_predictor_68_face_landmarks.dat";
#if UNITY_EDITOR
        // 유니티 에디터일 경우
        startInfo.WorkingDirectory = @"Assets\Resources\FD";
#else
	        // 런타임일 경우
	        startInfo.WorkingDirectory = UnityEngine.Application.streamingAssetsPath + "/FD";
#endif
        startInfo.WindowStyle = ProcessWindowStyle.Maximized;
        startInfo.ErrorDialog = true;

        Process process;
        try
        {
            process = Process.Start(startInfo);
            Console.WriteLine("Waiting 30 seconds for process to finish.");
            if (process.WaitForExit(30000))
            {
                Console.WriteLine("Process terminated.");
            }
            else
            {
                Console.WriteLine("Timed out waiting for process to end.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Could not start process.");
            Console.WriteLine(ex);
        }
#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif

        oringinFaceObj = GameObject.Find("OriginFace");
        originImg = oringinFaceObj.GetComponent<Image>();

        changeFaceObj = GameObject.Find("ArtFace");
        changeImg = changeFaceObj.GetComponent<Image>();

#if UNITY_EDITOR
        // 유니티 에디터일 경
        originImg.sprite = Resources.Load<Sprite>("FD/original_face");
        changeImg.sprite = Resources.Load<Sprite>("FD/new_face");
#else
	        // 런타임일 경우
            string path = UnityEngine.Application.streamingAssetsPath + "/FD/original_face.jpg";
            Texture2D texture2D = new Texture2D(0,0);
            byte[] byteTexture = System.IO.File.ReadAllBytes(path);
            texture2D.LoadImage(byteTexture);
            originImg.sprite = Sprite.Create(texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height),new Vector2(0.5f, 0.5f), 100.0f);

            string path2 = UnityEngine.Application.streamingAssetsPath + "/FD/new_face.png";
            Texture2D texture2D2 = new Texture2D(0,0);
            byte[] byteTexture2 = System.IO.File.ReadAllBytes(path2);
            texture2D2.LoadImage(byteTexture2);
            changeImg.sprite = Sprite.Create(texture2D2, new Rect(0f, 0f, texture2D2.width, texture2D2.height),new Vector2(0.5f, 0.5f), 100.0f);
#endif

#if UNITY_EDITOR
        File.Copy(@"Assets\Resources\FD\new_face.png", @"Assets\Resources\Player\Sprites\new_face.png", true);
        File.Copy(@"Assets\Resources\FD\HeroKnight.png", @"Assets\Resources\Player\Sprites\HeroKnight.png", true);
#endif
    }

    public void confirm()
    {
#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
        SceneManager.LoadScene("Town");
        // gameManager = GameObject.Find("InGameUI");
        // DontDestroyOnLoad(gameManager);
    }

//     public void makePlayer()
//     {
//         Texture2D texture2D = new Texture2D(0,0);
//         Sprite tts;
//         Sprite[] sprites;
// #if UNITY_EDITOR
//         {
//             sprites = Resources.LoadAll<Sprite>("Player/Sprites/HeroKnight"); // load all sprites in "assets/Resources/sprite" folder
//         }
// #endif
//         {
//             string path2 = UnityEngine.Application.streamingAssetsPath + "/FD/HeroKnight.png";
//             texture2D = new Texture2D(0,0);
//             byte[] byteTexture2 = System.IO.File.ReadAllBytes(path2);
//             texture2D.LoadImage(byteTexture2);
//             tts = Sprite.Create(texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height),new Vector2(0.5f, 0.5f), 100.0f);
//         }

//         AnimationClip animClip = new AnimationClip();
//         animClip.frameRate = 25;   // FPS
//         EditorCurveBinding spriteBinding = new EditorCurveBinding();
//         spriteBinding.type = typeof(SpriteRenderer);
//         spriteBinding.path = "";
//         spriteBinding.propertyName = "m_Sprite";
//         ObjectReferenceKeyframe[] spriteKeyFrames = new ObjectReferenceKeyframe[sprites.Length];
//         for (int i = 0; i < (sprites.Length); i++)
//         {
//             spriteKeyFrames[i] = new ObjectReferenceKeyframe();
//             spriteKeyFrames[i].time = i;
//             spriteKeyFrames[i].value = sprites[i];
//         }
//         AnimationUtility.SetObjectReferenceCurve(animClip, spriteBinding, spriteKeyFrames);


//     }
}