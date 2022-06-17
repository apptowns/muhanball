using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElementAchieve : MonoBehaviour
{
    public int id;
    bool isAchieve;

    public GameObject onacheieve;

    int up;
    int down;
    int set;
    public Text m_titel;
    public Text m_content;
    
    public Text m_count;		// 수집 / 세트수
    public Slider bar;

	public Image m_valueKind;
	public Text m_valueCount;



    public void OnSet(int _id)
    {
        id = _id;
        isAchieve = true;
        isclick  = true;
        //info
        up      = DataManager.Instance.saveData.AchieveUp[id];      //수집 수치
        down    = DataManager.Instance.saveData.AchieveDown[id];    //세트 수치
        set     = DataManager.Instance.etcAchieveList[id].set;      //몇개로 할건지...

        //내용
        m_titel.text    = DataManager.Instance.etcAchieveList[id].title;        // 업적 내용
        m_content.text  = DataManager.Instance.etcAchieveList[id].content;      // 내용

        //
        m_count.text = up.ToString() + " / " + down.ToString();
        m_valueCount.text =DataManager.Instance.etcAchieveList[id].valueCount.ToString();

        //set   완료 됐는지..
        if (bar.value < bar.maxValue)
            onacheieve.SetActive(false);
        else
            onacheieve.SetActive(true);
         
        bar.minValue = 0;
        bar.maxValue =set;

        bar.value = DataManager.Instance.saveData.AchieveUp[id];
        m_valueKind.sprite = ButtonManager.i.m_Atals.GetSprite(GetIconName(DataManager.Instance.etcAchieveList[id].valueKind));
    } 

    public void SetClick()  //한번에 받기
    {
        if (isclick)
        {
            isclick = false;

            SoundManager.Instance.play(2);
            int count = 0;
            int _up = DataManager.Instance.saveData.AchieveUp[id];

            while (up > set)
            {
                up -= set;
                count += 1;
            }

            DataManager.Instance.saveData.AchieveUp[id] = up;
            DataManager.Instance.saveData.AchieveDown[id] += count;

            m_count.text = DataManager.Instance.getAchieveUp(id).ToString() + " / " + DataManager.Instance.getAchieveDown(id).ToString();
            bar.value = DataManager.Instance.getAchieveUp(id);

            DataManager.Instance.Save();
            isclick = true;
        }
    }

    bool isclick;
    public void valueClick() //한번 받기
    {
        isclick = false;

        if ((DataManager.Instance.saveData.AchieveUp[id] - set) >= 0)
        {
            SoundManager.Instance.play(9);
            DataManager.Instance.saveData.AchieveUp[id]     -= set;
            DataManager.Instance.saveData.AchieveDown[id]   += 1;
            
            m_count.text = DataManager.Instance.saveData.AchieveUp[id].ToString() + " / " + DataManager.Instance.getAchieveDown(id).ToString();
            switch (DataManager.Instance.etcAchieveList[id].valueKind) 
            {
                case 0: //골드
                    DataManager.Instance.setCoin(DataManager.Instance.getCoin() + DataManager.Instance.etcAchieveList[id].valueCount);
                    break;
                case 1: //다이아.
                    DataManager.Instance.setDia(DataManager.Instance.getDia() + DataManager.Instance.etcAchieveList[id].valueCount);
                    break;                    
            }
            DataManager.Instance.Save();
        }
        else 
        {
            SoundManager.Instance.play(9); 
        }
           
        isclick = true;
    }

    string GetIconName(int _no) 
    {
        string icon ="";

        switch (_no)
        {
            case 0:
                icon ="Icon_ImageIcon_Glod01_s";
                break;
            case 1:
                icon = "Icon_ImageIcon_Gem01_m";
                break;
            case 2:
                icon = "missile";
                break;
            case 3:
                icon ="bomb";
                break;
            case 4:
                icon ="thunder";
                break;
        }

        return icon;
    }
     
}
 
