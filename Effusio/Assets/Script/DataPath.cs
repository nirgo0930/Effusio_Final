using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPath : MonoBehaviour
{
    public static string dataPath;
    // Start is called before the first frame update
    void Start()
    {
#if UNITY_EDITOR
        dataPath = Application.dataPath + "/Data/";
#else
        dataPath = UnityEngine.Application.streamingAssetsPath + "/Data/";
#endif
        print(dataPath);
    }
}
