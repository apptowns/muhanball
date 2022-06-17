using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Atelement : MonoBehaviour
{
    public int id;

    public Image rock;
    public Text message;

    public GameObject set;

    public Image icon;
    public Text level;

    public Text title;
    public Text content;
    public Text maxlevel;
    
    public Text valuecoin;
    public int coin0, coin1;
    public void setElement(int _id)
    {       
        id = _id;
        Debug.Log("어택 리스트 : " + DataManager.Instance.getStageBest() + " / " + DataManager.Instance.opAttackList[id].onLevel);

        //오픈 조건 -> 조건이 안되면.. 막는다.
        if (DataManager.Instance.getStageBest() < DataManager.Instance.opAttackList[id].onLevel) 
        {
            set.SetActive(false);
         
            rock.enabled = true;
            message.enabled = true;

            message.text = DataManager.Instance.opAttackList[_id].onLevel.ToString() + " 레벨 클리어 하면 개방";            
            return;
        }

        //정상 세팅
        rock.enabled = false;
        message.enabled = false;               

        level.text = DataManager.Instance.saveData.attackLevel[id].ToString(); //`
        maxlevel.text = DataManager.Instance.opAttackList[id].max.ToString();
        title.text = DataManager.Instance.opAttackList[id].titel;
        content.text = DataManager.Instance.setAttackContent(id);

        int _level = DataManager.Instance.saveData.attackLevel[id];
        coin0 = DataManager.Instance.opAttackList[id].coin;
        coin1 = (((_level * (_level + 1) / 2) - (_level - 1)) * coin0);
        valuecoin.text = coin1.ToString();

        setCheck(id);
    }

    public void clickButton()
    {
        Debug.Log("pay :" + coin1.ToString());

        if (DataManager.Instance.GetAttack(id) < DataManager.Instance.opAttackList[id].max) 
        {
            if ((DataManager.Instance.getCoin() - coin1) >= 0)
            {
                DataManager.Instance.setCoin(DataManager.Instance.getCoin() - coin1);
                //어택 레벨을 올려야 함.. 
                DataManager.Instance.SetAttack(id,DataManager.Instance.GetAttack(id)+1); //
                DataManager.Instance.Save();

                setElement(id);
            }
            else
            {
                Debug.Log("돈이 없어요.");
            }      
        }
        else 
        {
            Debug.Log("레벨이 맥스에요.");
        }
    }
   
    public void setCheck(int _id) // 완료된 것을 확인
    {
        Debug.Log("확인");

        Gamemanager.i.hud.setHp();
        DataManager.Instance.Save();
        
        
        //DataManager.Instance.attack = 1 * DataManager.Instance.attackLevel[0];
        //DataManager.Instance.speed = (-0.01f * DataManager.Instance.attackLevel[1]) + (DataManager.Instance.opAttackList[1].basePoint * 0.1f);
        //DataManager.Instance.hp = 10 * DataManager.Instance.attackLevel[2];
    }
}
