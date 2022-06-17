using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MesManager : MonoBehaviour
{
    public static MesManager i;

    public GameObject m_button;
    public GameObject m_present;
    public GameObject m_achieve;

    public GameObject[] present;
    public Transform prePos;

    public GameObject[] achieve;
    public Transform achiPos;

    private void Start()
    {
        i = this;
        presentSet();
        achieveSet();
        OnMes(0);
    }

    public void OnMes(int _id)
    {
        m_button.SetActive(false);
        m_present.SetActive(false);
        m_achieve.SetActive(false);
        switch (_id)
        {
            case 0:
                m_button.SetActive(true);
                break;
            case 1:
                m_present.SetActive(true);
                break;
            case 2:
                m_achieve.SetActive(true);
                break;
        }
    }

    public void presentSet()
    {
        present = new GameObject[10];
        for (int i = 0; i < present.Length; i++)
        {
            present[i] = prePos.GetChild(i).gameObject;
            if(i<DataManager.Instance.etcPresentList.Count)
                present[i].GetComponent<ElementLetteer>().OnSet(i);
            else
                present[i].gameObject.SetActive(false);
        }
    }


    public void achieveSet()
    { 
       achieve = new GameObject[10];
        for (int i = 0; i < achieve.Length; i++)
        {
            achieve[i] = achiPos.GetChild(i).gameObject;
            if (i < DataManager.Instance.etcAchieveList.Count)
                achieve[i].GetComponent<ElementAchieve>().OnSet(i);
            else
                achieve[i].gameObject.SetActive(false);
        }
    }   
}
