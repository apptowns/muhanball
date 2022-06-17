using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageManager : MonoBehaviour
{
    public static MessageManager i;
    public Text mes;


    private void Awake()
    {        
        i = this;
        offPanel();
    }

    public void onPanel(string _tex)
    {
       // SoundManager.Instance.play(0);
        mes.text = _tex;
        this.gameObject.SetActive(true);

        Invoke("offPanel", 1.0f);
    }

    public void offPanel()
    {
        this.gameObject.SetActive(false);        
    } 
}
