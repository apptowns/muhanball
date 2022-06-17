using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonInfo : MonoBehaviour
{
    public int id;

    public Text m_id;
    public Text m_titel;
    public Text m_day;

    public void setElement(int _id)
    {
        id = _id;
        m_id.text = (id + 1).ToString();
        m_titel.text = DataManager.Instance.etcInfoList[_id].title;
        m_day.text = DataManager.Instance.etcInfoList[_id].day;
    }

    public void clickInfo()
    {
        SoundManager.Instance.play(12);
        TitleManager.i.Etc.getInfo(id);
    }
}