using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EtcManager : MonoBehaviour
{
    public Button[] buttonSet;

    [Header("공지사항")]
    public int infocall;
    public GameObject m_info;
    public Transform info;
    public GameObject[] infoset;
    public Text infoTitle;
    public Text infoContent;

    [Header("데일리 보너스")]
    public GameObject m_bonus;
    public Transform bonus;
    public GameObject[] bonusset;
    
    [Header("스테이지")]
    public GameObject m_Stage;
    public Transform stage;
    public GameObject[] stageset;

    public Text stageBest;

    bool isInfo;
    bool isBonus;
    bool isStage;

    public void Awake()
    {
       isInfo =false;
       isBonus = false;
       isStage =false;
    }


    public void onEtc(int _id)
    {
        //SoundManager.Instance.play(2);

        m_info.SetActive(false);
        m_bonus.SetActive(false);
        m_Stage.SetActive(false);

        switch (_id)
        {
            case 0:
                infocall = 0;
                infoSet();
                m_info.SetActive(true);
                break;

            case 1:
                infocall = 1;
                bonusSet();
                m_bonus.SetActive(true);
                break;

            case 2:
                infocall = 1;
                stagesizeset();
                m_Stage.SetActive(true);
                break;
        }
    }

    public void infoSet()
    {
        if (isInfo) 
        {
            infoReset();
            return;
        }

        isInfo = true;

        infoset = new GameObject[10];
        for (int i = 0; i < infoset.Length; i++)
        {
            infoset[i] = info.transform.GetChild(i).gameObject;
            infoset[i].GetComponent<ButtonInfo>().setElement(infoset.Length -(1+i));            
        } 
        getInfo(infoset.Length-1);        
    }

    public void infoReset()
    { 
        for (int i = 0; i < infoset.Length; i++)
        {     
            infoset[i].GetComponent<ButtonInfo>().setElement(infoset.Length - (1 + i));
        }

        getInfo(infoset.Length - 1);
    }

    public void getInfo(int _id) 
    {
        infoTitle.text = DataManager.Instance.etcInfoList[_id].id.ToString() +"    "+ DataManager.Instance.etcInfoList[_id].title;
        infoContent.text = DataManager.Instance.etcInfoList[_id].content + "\n" +
            DataManager.Instance.etcInfoList[_id].day;       ;
    }

    public void bonusSet() 
    {
        if (isBonus)
        {
            bonusReset();
            return;
        }

        isBonus = true;

        bonusset = new GameObject[16];
           for (int i=0; i<bonusset.Length; i++) 
        {
            bonusset[i] = bonus.transform.GetChild(i).gameObject;
            bonusset[i].GetComponent<ButtonBonus>().setBonus(i);
        }
    }

    public void bonusReset()
    {
        for (int i = 0; i < bonusset.Length; i++)
        {
            bonusset[i].GetComponent<ButtonBonus>().setBonus(i);
        }
    }

    public RectTransform contentsize;
    public GameObject ElementStageObj;
    public void stagesizeset() 
    {        
        //현재 최고 스테이지를 기준으로 셋
        int checkstage = DataManager.Instance.getStageBest()/10;    // 현재 스테이지 사이즈 
        Debug.Log("스테이지 세팅 "+ checkstage);       

        //컨텐츠 사이즈 정리
        if (checkstage>0)
            contentsize.sizeDelta = new Vector2(890.0f,180.0f*(1+checkstage));       
        else
            contentsize.sizeDelta = new Vector2(890.0f, 180.0f);

        stageset = new GameObject[checkstage + 1];
        for (int i = 0; i < stageset.Length; i++)
        {
            GameObject m = Instantiate(ElementStageObj, Vector3.zero, Quaternion.identity);
            m.transform.localScale = new Vector3(1, 1, 1);
            m.transform.parent = stage.transform;            
            m.transform.localScale = new Vector3(1,1,1);

            stageset[i] = stage.transform.GetChild(i).gameObject;
            stageset[i].GetComponent<ElementStage>().set(i);
        }

        for (int i = 0; i < stageset.Length; i++)
        {
            Vector3 v= stageset[i].GetComponent<RectTransform>().localPosition;
            v.z = 0.0f;            
            stageset[i].GetComponent<RectTransform>().localPosition = v;
        }
    }

    public void stageReset() 
    {
        stageBest.text = DataManager.Instance.getStageBest().ToString();

        for (int i = 0; i < stageset.Length; i++)
        {
            stageset[i].GetComponent<ElementStage>().set(i);
        }
    }
}
