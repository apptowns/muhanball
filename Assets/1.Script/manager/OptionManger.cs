using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionManger : MonoBehaviour
{
    public static OptionManger i;

    public Button[] buttonSet;

    public int curret;
    //강화
    public GameObject m_attack;
    public Transform attack;
    public GameObject[] at;

    //총알
    public GameObject m_bullet;
    public Transform bullet;
    public GameObject[] bul;

    public BulElementPan panel;

    //총알
    public GameObject m_bulletset;
    public Transform bulletset;
    public GameObject[] bulset;

    //sellect
    public GameObject[] sellect;
     int[] coinvalue;

    //info
    public GameObject m_menuinfo;
    public Text menuinfo;
    public Text menuinfoValue;

    //shop
    public GameObject m_shop;
    public Transform shop;
    public GameObject[] sh;


    private void Awake()
    {
        i = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        coinvalue = new int[4];
        coinvalue[0]= 0;
        coinvalue[1] = 5;
        coinvalue[2] = 50;
        coinvalue[3] = 100;

        curret = 0;
        attackSet();
        bulletSet();

        onOption(curret);
    }

    public void onOption(int _id)
    {
      
    }

    /// </summary>
    /// <param name="_curret"></param>

    
    public void attackSet()
    {
        at = new GameObject[7];
        for (int i = 0; i < at.Length; i++)
        {
           // at[i] = attack.transform.GetChild(i).gameObject;
            //at[i].GetComponent<Atelement>().setElement(i);
        }
    }

    public void bulletSet()
    {
    }

    public void bullreset() //총알 판넬 리셋
    {
    }

    public List<int> selectValueList;
   
    // 뽑기 선택
    public void getSellect(int _id)
    {
        //돈 지불하고 
        if ((DataManager.Instance.getDia()-coinvalue[_id])>=0)
        {
            DataManager.Instance.setDia(DataManager.Instance.getDia() - coinvalue[_id]);
            
        }
        else
        {
            ErrorManager.i.onPanel("다이아 없어");
            return;
        }

        //뽑기
        //판넬 다 끄고
        for (int i = 0; i < sellect.Length; i++)
        {
            sellect[i].SetActive(false);
        }

        //필요 판넬 오픈 
        sellect[_id].SetActive(true);                       //내꺼.. 
        selectValueList = new List<int>();                  //담을 모델
        sellect[_id].GetComponent<SelectControl>().init();  //판넬 세팅
    }

    public void setSelect()
    {
        //원래로 돌아간다.
        //정산하고..
        
        for (int i = 0; i < OptionManger.i.selectValueList.Count; i++)
        {
            int a = OptionManger.i.selectValueList[i];
            DataManager.Instance.setWeponUp(a,DataManager.Instance.getWeponUp(a) + 1); 
            DataManager.Instance.opBulList[a].getcount += 1;
        }

        OptionManger.i.selectValueList = new List<int>();
        //무기 장착 화면 리셋
        bullreset();
        onOption(2);
    }
 

    public void openPanel(int _id)
    {
        panel.gameObject.SetActive(true);
        panel.onSet(_id);

    }

    public void menuinfoSet() 
    {
        menuinfo.text = "" +
            "공격력\n" + 
            "총알\n" +
            "총알 연사 속도\n" +
            "총알 이름(공격력 플러스)\n" +
            "총알 이름(공격력 플러스)\n" +
            "총알 이름(공격력 플러스)\n" +
            "총알 이름(공격력 플러스)\n" +
            "";
        menuinfoValue.text = "" +
            (1*DataManager.Instance.saveData.attackLevel[0]) + "\n";
    }
}
