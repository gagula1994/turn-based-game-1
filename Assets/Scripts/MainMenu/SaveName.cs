using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveName : MonoBehaviour
{
    public string saveName = "";
    public static SaveName saveNameObj;

    void Awake()
    {
        if (saveNameObj == null) saveNameObj = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
}
