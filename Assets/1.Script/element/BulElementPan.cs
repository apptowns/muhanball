
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;

public class BulElementPan : MonoBehaviour
{
    public int id;

    [SerializeField]
    public SpriteAtlas m_Atals = null;

    public Text clas;
    public Image icon;
    public Text m_title;
    public Text m_level;
    public Text m_itemcount;
    public Slider m_getitemcountbar;

    public Text m_content;
    public Button m_leftButton;
    public Button m_rightButton;

    public Image check;

    public Image rock;

    private void Start()
    {
        //this.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    public void onSet(int _id)
    {
        //this.gameObject.SetActive(true);
        id = _id;
        clas.text = DataManager.Instance.opBulList[id].clas.ToString();
        icon.sprite = m_Atals.GetSprite("bullet_" + id.ToString());
        m_title.text = id.ToString() + ": " + DataManager.Instance.opBulList[id].title.ToString();

        string[] con = new string[4];
        con[0] = "공격력 플러스 " + DataManager.Instance.opBulList[id].upat.ToString();
        con[1] = (DataManager.Instance.opBulList[id].upcriat ==0) ? "": "크리티컬 확률 " + DataManager.Instance.opBulList[id].upcriat.ToString();
        con[2] = (DataManager.Instance.opBulList[id].uphp == 0) ? "" : "추가 체력 " + DataManager.Instance.opBulList[id].uphp.ToString();
        con[3] = (DataManager.Instance.opBulList[id].bestat  == "0") ? "" : ""+DataManager.Instance.opBulList[id].bestat;

        if (id == DataManager.Instance.getWepon())
            check.gameObject.SetActive(true);
        else
            check.gameObject.SetActive(false);


        m_content.text = ""+
            con[0] + "\n"+ con[1] + "\n"+ con[2] + "\n"+ con[3];

        m_level.text = DataManager.Instance.opBulList[id].level.ToString();
        if (DataManager.Instance.getStagePlay() < DataManager.Instance.opBulList[id].level)
        {
            rock.gameObject.SetActive(true);
        }
        else
        {
            rock.gameObject.SetActive(false);
        }

        m_itemcount.text = DataManager.Instance.getWeponUp(id).ToString() + "/" + DataManager.Instance.getWeponDown(id).ToString();

        m_getitemcountbar.minValue = 0;
        m_getitemcountbar.maxValue = 5;
        m_getitemcountbar.value = DataManager.Instance.opBulList[id].getcount;

        m_leftButton.gameObject.SetActive(true);
        m_rightButton. gameObject.SetActive(true);

    }

    public void offPanel()
    {
        this.gameObject.SetActive(false);
    }

    public void leftButton()
    {
        if ((id - 1) >= 0)
        {
            onSet(id - 1);
        } 
        else
        {
            onSet(id);
        }

    }

    public void rightButton()
    {
        if ((id + 1) <= 23)
        {
            onSet(id + 1);
        }
        else 
        {
            onSet(id);
        }
    }

    public void hapsung()
    {
        //자동으로 상위로 갈것인가?

        if ((DataManager.Instance.getWeponUp(id) -5) >= 0)
        {
            DataManager.Instance.setWeponUp(id, DataManager.Instance.getWeponUp(id) - 5);  // 업에서  5빼고
            DataManager.Instance.setWeponDown(id, DataManager.Instance.getWeponDown(id) + 1);  // 업에서  5빼고

            if ((DataManager.Instance.getWeponDown(id) - 5) >= 0)
            {
                DataManager.Instance.setWeponDown(id, DataManager.Instance.getWeponDown(id) -5);  // 업에서  5빼고
                DataManager.Instance.setWeponUp(id + 1, DataManager.Instance.getWeponUp(id + 1) + 1);  // 업에서  5빼고
            }

            onSet(id);
            PopupManager.i.bulletSet();
            SoundManager.Instance.play(12);
            DataManager.Instance.Save();

        }
        else
        {
            ErrorManager.i.onPanel("합성할 총알이 부족해요.");
            onSet(id);
            return;
        }
    }

    public void jangchak()
    {
        // 아무것도 없으면 장착 된것은 제거 되어야 하는가  ?
        if (DataManager.Instance.getWeponUp(id) > 0 || DataManager.Instance.getWeponDown(id) > 0)
        {
            DataManager.Instance.setWepon(id);
            SoundManager.Instance.play(12);
            DataManager.Instance.Save();
            //MessageManager.i.onPanel("새로운 총알이 장착 되었습니다."); //팝업위에서는 시간이 흐르지 않으니 조심
            onSet(id);
            ErrorManager.i.onPanel("새로운 총알이 장착 되었습니다.");
        }
        else
        {
            ErrorManager.i.onPanel("무기가 없어");
        }

        PopupManager.i.bulletSet();
    }
}