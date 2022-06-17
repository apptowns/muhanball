using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButtonControl : MonoBehaviour
{
    GameObject obj;
    Vector2 v;

    private void Start()
    {
        obj = this.gameObject;
        v = obj.transform.position;
    }


    public void clickButton() 
    {
        this.transform.localPosition = new Vector3(1000.0f,0,0);
        //Gamemanager.i.startGame();
    }
}
