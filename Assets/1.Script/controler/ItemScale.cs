using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemScale : MonoBehaviour
{
    public int id;

    float a;
    bool ismove = false;
    Vector2 pos;

    public Image ran;

    private void Start()
    {
        ran = this.GetComponent<Image>();
    }


    public void scaleStart()
    {
        setValue();
        this.transform.localScale = new Vector3(1.5f,1.5f,1f);
        this.transform.localScale = new Vector3(1f, 1f, 1f);


        StartCoroutine(move());
    }

    IEnumerator move()
    {
        //Debug.Log("맞았어!"); 
        this.transform.localScale = new Vector3(1.5f, 1.5f, 1f);
        yield return new WaitForSeconds(0.1f);
        this.transform.localScale = new Vector3(1f, 1f, 1f);        

    }

    void setValue()
    {
        
        switch (this.gameObject.name)
        {
            case "ButtonC":
                //int ran = Random.Range(1, DataManager.Instance.stageList[DataManager.Instance.getstageID()].goldValue);
                DataManager.Instance.setCoin(DataManager.Instance.getCoin() +1);
                break;
            case "ButtonD":
                DataManager.Instance.setDia(DataManager.Instance.getDia() + 1);
                break;
            case "ButtonM":
                DataManager.Instance.setMissle(DataManager.Instance.getMissale() + 1);
                break;

            case "ButtonB":
                DataManager.Instance.setBomb(DataManager.Instance.getBomb() + 1);
                break;

            case "ButtonL":
                DataManager.Instance.setLazer(DataManager.Instance.getLazer() + 1);
                break;
        }
        SoundManager.Instance.play(12);
        EffectManager.Instance.PlayBallExplodeEffect(transform.position, ran.color);

    }
}
