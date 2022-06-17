using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ErrorManager : MonoBehaviour
{
    public static ErrorManager i;
    public Text mes;


    private void Start()
    {
        i = this;
        offPanel();
    }

    public void onPanel(string _tex) 
    {
        mes.text = _tex;
        this.gameObject.SetActive(true);

        Invoke("offPanel",2.0f);
    }

    public void offPanel() 
    {
    this.gameObject.SetActive(false);
    }
}
