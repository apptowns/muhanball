using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
    public static PopupManager i;

    public GameObject[] option;
    public int call;

    public GameObject m_pause;
    public GameObject m_option;
    public GameObject m_optionButton;
    public GameObject m_letter;
    public GameObject[] m_letterList;
    public Transform m_letterpos;

    // 업적
    public GameObject m_achieve;
    public GameObject[] m_acheveList;
    public Transform m_achievepos;

    // 강화
    public GameObject m_attack;
    public GameObject[] m_attackList;
    public Transform m_attackpos;

    // 총알
    public GameObject m_bullet;
    public GameObject[] m_bulletList;
    public Transform m_bulletpos;
    public BulElementPan panel;
    
    // 뽑기
    public GameObject[] sellect;
    int[] coinvalue;
    public GameObject m_bulletsellect;
    
    // 상점
    public GameObject m_shop;
    public GameObject[] m_shopList;
    public Transform m_shoppos;


    // Start is called before the first frame update
    void Start()
    {
        i = this;

        coinvalue = new int[4];
        coinvalue[0] = 0;
        coinvalue[1] = 5;
        coinvalue[2] = 50;
        coinvalue[3] = 100;

        presentSet();
        achieveSet();
        attackSet();
        bulletSet();
        bulletselectSet();
        shopSet();

        offpanel();    
    }

    public void Pause()
    {
        offpanel();

        m_pause.SetActive(true);
        Time.timeScale = 0;        
    }

    public void Game()                      // 다시 게임으로
    {
        offpanel();       
        Time.timeScale = 1;
    }

    public void Otion()                     // 옵션 메니저
    {
        offpanel();
        Time.timeScale = 0;
        m_optionButton.SetActive(true);
    }

    public void off()                       // 판넬꺼
    {
        Pause();
    }

    public void Letter()                    // 선물.. 우편함
    {
        offpanel();
        Time.timeScale = 0;     
        m_letter.SetActive(true);
    }

    public void Achieve()                   // 업적
    {
        offpanel();
        Time.timeScale = 0;
        m_achieve.SetActive(true);
    }

    public void Attack()                    // 강화
    {
        offpanel();

        for (int i = 0; i < m_acheveList.Length; i++)
        {
            m_acheveList[i].GetComponent<Atelement>().setElement(i);
        }

        Time.timeScale = 0; 
        m_attack.SetActive(true);
    }

    public void Bullet()                    // 총알
    {
        offpanel();
        for (int i = 0; i < m_bulletList.Length; i++)
        {
            m_bulletList[i].GetComponent<BulElement>().onSet(i);
        }
        Time.timeScale = 0;
        m_bullet.SetActive(true);
    }

    public void BulletSet()                 // 총알 뽑기
    {
        offpanel();
        //판넬 다 끄고
        for (int i = 0; i < sellect.Length; i++)
        {
            sellect[i].SetActive(false);
        }
        sellect[0].SetActive(true);

        Time.timeScale = 0;
        m_bulletsellect.SetActive(true);
    }

    public  void BullectSetOff()
    {
        bulletSet();
        BulletSet();
    }

    public void shop() 
    {
        offpanel();
        Time.timeScale = 0;
        m_shop.SetActive(true);
    }

    public void offpanel() 
    {
        m_pause.SetActive(false);

        m_optionButton.SetActive(false);
        m_letter.SetActive(false);
        m_achieve.SetActive(false);
        m_option.SetActive(false);

        m_attack.SetActive(false);
        m_bullet.SetActive(false);
        m_bulletsellect.SetActive(false);

        m_shop.SetActive(false);


        option[0].SetActive(false);
      // option[1].SetActive(false);

        DataManager.Instance.Save();
        
        Time.timeScale = 1;
    }

    public void presentSet()
    {
        m_letterList = new GameObject[10];
        for (int i = 0; i < m_letterList.Length; i++)
        {
            m_letterList[i] = m_letterpos.GetChild(i).gameObject;
            if (i < DataManager.Instance.etcPresentList.Count)
                m_letterList[i].GetComponent<ElementLetteer>().OnSet(i);
            else
                m_letterList[i].gameObject.SetActive(false);
        }
    }

    public void achieveSet()
    {
        m_acheveList = new GameObject[10];
        for (int i = 0; i < m_acheveList.Length; i++)
        {
            m_acheveList[i] =m_achievepos.GetChild(i).gameObject;
            if (i < DataManager.Instance.etcAchieveList.Count)
                m_acheveList[i].GetComponent<ElementAchieve>().OnSet(i);
            else
                m_acheveList[i].gameObject.SetActive(false);
        }
    }

    public void attackSet()
    {
        m_acheveList = new GameObject[7];
        for (int i = 0; i < m_acheveList.Length; i++)
        {
            m_acheveList[i] = m_attackpos.transform.GetChild(i).gameObject;
            m_acheveList[i].GetComponent<Atelement>().setElement(i);
        }
    }

    public void bulletSet()
    {
        m_bulletList = new GameObject[24];
        for (int i = 0; i < m_bulletList.Length; i++)
        {
            m_bulletList[i] = m_bulletpos.transform.GetChild(i).gameObject;
            m_bulletList[i].GetComponent<BulElement>().onSet(i);
        }
    }

    public void bullreset() //총알 판넬 리셋
    {
        for (int i = 0; i < m_bulletList.Length; i++)
        { 
            m_bulletList[i].GetComponent<BulElement>().onSet(i);
        }
    }

    public void openPanel(int _id)
    {
        panel.gameObject.SetActive(true);
        panel.onSet(_id);
    }

    public void bulletselectSet()
    {
        for (int i = 0; i < sellect.Length; i++)
        {
            sellect[i].SetActive(false);
        }
        sellect[0].SetActive(true);
    }


    public List<int> selectValueList;
    // 뽑기 선택
    public void getSellect(int _id)
    {
        //돈 지불하고 
        if ((DataManager.Instance.getDia() - coinvalue[_id]) >= 0)
        {
            DataManager.Instance.setDia(DataManager.Instance.getDia() - coinvalue[_id]);

        }
        else
        {
            ErrorManager.i.onPanel("필요한 다이아는 "+coinvalue[_id].ToString()+"입니다");
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

    public void getDiaonCoin(int _dia) 
    {
        //돈 지불하고 
        if ((DataManager.Instance.getDia() - _dia) >= 0)
        {
            //다이아 빼고
            DataManager.Instance.setDia(DataManager.Instance.getDia() - _dia);
            switch (_dia) 
            {
                case 10000:
                    DataManager.Instance.setCoin(DataManager.Instance.getCoin()+100);
                    break;
                case 20000:
                    DataManager.Instance.setCoin(DataManager.Instance.getCoin() + 200);
                    break;
                case 30000:
                    DataManager.Instance.setCoin(DataManager.Instance.getCoin() + 300);
                    break;
            }
        }
        else
        {
            ErrorManager.i.onPanel("필요한 다이아는 " + _dia.ToString() + "입니다");
            return;
        }
    }


    public void setSelect()
    {
        //원래로 돌아간다.
        //정산하고..

        for (int i = 0; i < OptionManger.i.selectValueList.Count; i++)
        {
            int a = OptionManger.i.selectValueList[i];
            DataManager.Instance.setWeponUp(a, DataManager.Instance.getWeponUp(a) + 1);
            DataManager.Instance.opBulList[a].getcount += 1;
        }

        OptionManger.i.selectValueList = new List<int>();

        //무기 장착 화면 리셋
        bullreset();
    }

    public void shopSet()
    {
        m_shopList = new GameObject[5];
        for (int i = 0; i < m_shopList.Length; i++)
        {
            m_shopList[i] = m_shoppos.transform.GetChild(i).gameObject;
            //m_shopList[i].GetComponent<Atelement>().setElement(i);
        }
    }

}
