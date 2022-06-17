using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
    public int id;

    //경험치
    public Slider expBar;
    public Text expPoint;
    public Text point;
    public Text bestPoint;

    //타임바
    public Slider timeBar;
    public Text timePoint;

    //체력
    public Slider hpBar;
    public Text hpPoint;

    //스테이지 관련
    public Text stage;          // 현재 스테이지
    public Text maxstage;       // 최고 도달 스테이지    

    public Text coin;
    public Text coin1;

    public Text dia;
    public Text attack;

    public Text maxstage1;

    //아이템 수량 
    public Text m_missile;
    public Text m_bomb;
    public Text m_laser;

    public Vector3 ballguide;

    public virtual void init()
    {
        id              = DataManager.Instance.getStagePlay();
        ballguide       = hpBar.gameObject.transform.localPosition;

        maxstage.text   = DataManager.Instance.getStageBest().ToString();
        maxstage1.text  = "스테지이 레벨 : " + DataManager.Instance.getStageBest().ToString();

        //기본 정보        
        attack.text     = (DataManager.Instance.getAttack() + DataManager.Instance.opBulList[DataManager.Instance.getWepon()].upat).ToString();
        coin.text       = DataManager.Instance.getCoin().ToString();    //k
        coin1.text      = DataManager.Instance.getCoin().ToString();
        dia.text        = DataManager.Instance.getDia().ToString();

        //아이템 정보
        m_missile.text  = DataManager.Instance.getMissale().ToString();
        m_bomb.text     = DataManager.Instance.getBomb().ToString();
        m_laser.text    = DataManager.Instance.getLazer().ToString();
    }



    public virtual IEnumerator timeOn()
    {
        yield return null;
        while (Gamemanager.i.gameState == Gamemanager.GAMESTATE.BOSS)
        {
            if (timeBar.value > 0)
            {
                yield return new WaitForSeconds(1.0f);
                timeBar.value -= 1;
                timePoint.text = timeBar.value.ToString();
            }
            else
            {
                timePoint.text = "0";
                //Gamemanager.i.gameoverBossGame();
            }
        }
    }

    public virtual void MissileBtn()
    {        
        if (DataManager.Instance.getMissale() > 0)
        {
            Gamemanager.i.CreateBoostUp(BoostUpType.MISSILE);
        }
    }

    public virtual void BombBtn()
    {        
        if (DataManager.Instance.getBomb() > 0)
        {
            Gamemanager.i.CreateBoostUp(BoostUpType.BOMB);
        }
    }

    public virtual void LaserBtn()
    { 
        if (DataManager.Instance.getLazer() > 0)
        {
            Gamemanager.i.CreateBoostUp(BoostUpType.LASER);
        }
    }

}
