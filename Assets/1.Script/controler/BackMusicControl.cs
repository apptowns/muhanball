using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackMusicControl : MonoBehaviour
{
    public static BackMusicControl i;

    private void Awake()
    {
        i = this;
        DontDestroyOnLoad(this);
    }

    public void Play()
    {

    }

}
