using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;

public class ElementLetteer : MonoBehaviour
{
    public int id;
    public int valueID;
 
    public Text m_title;
    public Image m_vicon;
    public Text m_vcount;

    public Button button;
    public Text buttonMes;

    public void OnSet(int _id )
    {
        id = _id;
        m_title.text = DataManager.Instance.etcPresentList[id].content;

        int count =0;
        for (int i = 0; i < 5; i++)
        {
            if (DataManager.Instance.etcPresentList[id].value[i] > 0)
            {
                valueID = i;
                m_vcount.text = DataManager.Instance.etcPresentList[id].value[i].ToString();
                switch (i)
                {
                    case 0:
                        m_vicon.sprite = ButtonManager.i.m_Atals.GetSprite("Icon_ImageIcon_Glod01_s");
                        break;
                    case 1:
                        m_vicon.sprite = ButtonManager.i.m_Atals.GetSprite("Icon_ImageIcon_Gem01_m");
                        break;
                    case 2:
                        m_vicon.sprite = ButtonManager.i.m_Atals.GetSprite("missile");
                        break;
                    case 3:
                        m_vicon.sprite = ButtonManager.i.m_Atals.GetSprite("bomb");
                        break;
                    case 4:
                        m_vicon.sprite = ButtonManager.i.m_Atals.GetSprite("thunder");
                        break;
                }  
            }

            if (DataManager.Instance.getPresent(id))
            {
                button.interactable = false;
                buttonMes.text = "완료";
            }
        }
    }

    public void clickButton() 
    {
        button.interactable = false;
        buttonMes.text ="완료";

        DataManager.Instance.setPresent(id,true);

        switch (valueID)
        {
            case 0:
                DataManager.Instance.setCoin(DataManager.Instance.getCoin()+int.Parse(m_vcount.text));
                break;
            case 1:
                DataManager.Instance.setDia(DataManager.Instance.getDia() + int.Parse(m_vcount.text)); 
                break;
            case 2:
                DataManager.Instance.setMissle(DataManager.Instance.getMissale() + int.Parse(m_vcount.text)); 
                break;
            case 3:
                DataManager.Instance.setBomb(DataManager.Instance.getBomb() + int.Parse(m_vcount.text)); 
                break;
            case 4:
                DataManager.Instance.setLazer(DataManager.Instance.getLazer() + int.Parse(m_vcount.text)); 
                break;
        }

        DataManager.Instance.Save();
    }
}
