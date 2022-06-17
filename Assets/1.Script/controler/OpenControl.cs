using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class OpenControl : MonoBehaviour
{
    public Image fade;
    public SpriteRenderer color;

    public void Fadeon() 
    {
        this.gameObject.SetActive(true);
        StartCoroutine(setFade());

        if(DataManager.Instance.getScene()=="GAME")
            Gamemanager.i.shake.shakeCall();
    }

    IEnumerator setFade()
    {
        float a = 1.0f;
        fade.color = new Color(0, 0, 0, a);
        while (fade.color.a > 0.0f)
        {
            yield return null;
            a -= 0.01f;
            fade.color = new Color(0, 0, 0, a);
        }

        this.gameObject.SetActive(false);
        
    }

    public void Coloron()
    {
        this.gameObject.SetActive(true);
        StartCoroutine(setColor());

        if (DataManager.Instance.getScene() == "GAME")
            Gamemanager.i.shake.shakeCall();
    }

    public void Coloroff() 
    {
        this.gameObject.SetActive(false);
        StopCoroutine(setColor());
    }

    IEnumerator setColor() 
    {
        float a = 1.0f;
        color.color = new Color(1, 0, 0, a);
        while (Gamemanager.i.isboss)
        {
            yield return null;

            if (color.color.a > 0.0f)
            { 
                a -= 0.01f;
            }
            else
            {
                a = 1.0f;
            }

            color.color = new Color(1, 0, 0, a);
        }

        this.gameObject.SetActive(false);

    }
    
}
