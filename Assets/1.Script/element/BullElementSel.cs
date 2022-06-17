using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;

public class BullElementSel : MonoBehaviour
{
    public SpriteAtlas m_Atals = null;
    public GameObject icon;

    public int id;
    public int bullID;

    //public Text m_title;
    public Text m_grade;
    public Text m_level;

    public OpenControl fade;     

    public void OnSet(int _id)
    {
        fade.Fadeon();
        bullID = _id;
        icon.GetComponent<Image>().sprite = m_Atals.GetSprite("bullet_" + bullID.ToString());

        m_grade.text = DataManager.Instance.opBulList[bullID].clas;
        m_level.text = DataManager.Instance.opBulList[bullID].level.ToString();
        //m_title.text = DataManager.Instance.opBulList[bullID].title;
    }
}