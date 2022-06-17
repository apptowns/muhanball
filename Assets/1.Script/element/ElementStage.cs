using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElementStage : MonoBehaviour
{
    public int id;
    
    public Text m_min;
    public Text m_max;

    public Text m_id;
    public Text m_title;
    public Slider exp;

    public Image m_upImg;
    public Image m_checker;

    public Button thisButton;

    public void set(int _id)
    {
        id = _id;
        m_id.text = (id + 1).ToString();
        m_title.text = DataManager.Instance.stageList[id].name;

        m_checker.gameObject.SetActive(false);
        m_upImg.gameObject.SetActive(true);
        this.thisButton.interactable = false;

        setExp();

        if (exp.minValue <= DataManager.Instance.getStageBest())
        {
            m_upImg.gameObject.SetActive(false);
            this.thisButton.interactable = true;
        }

        if (DataManager.Instance.getStagePlay() >= exp.minValue && DataManager.Instance.getStagePlay() <= exp.maxValue)
        {       
            m_checker.gameObject.SetActive(true);
        }
    }

    void setExp() 
    {
        //범위 세팅
        exp.minValue = DataManager.Instance.stageList[id].setStage - 9;
        exp.maxValue = DataManager.Instance.stageList[id].setStage;
     
        m_min.text = exp.minValue.ToString();
        m_max.text = exp.maxValue.ToString();

        //현재 진행 중인 스테이지 표시
        if(exp.maxValue  < DataManager.Instance.getStageBest()) 
        {
            exp.value = exp.maxValue;
        }
        else
        {
            exp.value = DataManager.Instance.getStageBest();
        }
    }

    public void getScene() 
    {
        Debug.Log("씬 이동");
        SoundManager.Instance.play(2);
        DataManager.Instance.setStagePlay(id *10);
        TitleManager.i.goGame();
    }
}
