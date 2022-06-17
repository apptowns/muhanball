using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBonus : MonoBehaviour
{
    public int id;
    public int day;

    public Text m_day;
    public Text m_value;

    public bool isCheck;
    public Image m_check;
    

    public void setBonus(int _id)
    {
        id = _id;     
        day = id + 1;        

        if (id < DataManager.Instance.saveData.dbonus)
                        this.GetComponent<Button>().interactable = true;
        else 
            this.GetComponent<Button>().interactable = false;

        m_day.text = day.ToString() + "일";        
        m_value.text = (day * 10).ToString();

        if ( id < DataManager.Instance.saveData.dbonus) 
        {
            isCheck = true;
            m_check.enabled = true;
        }
        else 
        {
            isCheck = false;
            m_check.enabled = false;
        }
    }

    bool isclick;
    public void clickBonus() 
    {
        if(id < DataManager.Instance.saveData.dbonus)
        {
            SoundManager.Instance.play(13);
            DataManager.Instance.saveData.dia += int.Parse(m_value.text);
        }
        else 
        {
            ErrorManager.i.onPanel("이미 받았어");
        } 
    }
}
