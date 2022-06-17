using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;

public class BulElement : MonoBehaviour
{
    public int id;

    [SerializeField]
    public SpriteAtlas m_Atals = null;

    public Image icon; 
    public Text m_level;
    public Text m_itemcount;
    public Slider m_getitemcountbar;
    public Image check;

    public Image rock;
    
    // Start is called before the first frame update
    public void onSet(int _id)
    {
        id = _id;
         
        //icon.sprite =m_Atals.GetSprite("bullet_" + id.ToString());

        if (DataManager.Instance.getStageBest() <= DataManager.Instance.opBulList[id].level)
        {
            rock.gameObject.SetActive(false);
            this.GetComponent<Button>().interactable = false;
            icon.sprite = m_Atals.GetSprite("rock");

        }
        else
        {
            //수량 확인
            rock.gameObject.SetActive(false);
            this.GetComponent<Button>().interactable = true;
            icon.sprite = m_Atals.GetSprite("bullet_" + id.ToString());

        } 

        m_level.text = DataManager.Instance.opBulList[id].level.ToString();
        m_itemcount.text = DataManager.Instance.getWeponUp(id).ToString() + "/" + DataManager.Instance.getWeponDown(id).ToString();

        m_getitemcountbar.minValue = 0;
        m_getitemcountbar.maxValue = 5;
        m_getitemcountbar.value = DataManager.Instance.getWeponUp(id);

        if (id == DataManager.Instance.getWepon())
            check.gameObject.SetActive(true);
        else
            check.gameObject.SetActive(false);

    }

    public void buttonClick()
    {
        PopupManager.i.openPanel(id); 
    } 
}
