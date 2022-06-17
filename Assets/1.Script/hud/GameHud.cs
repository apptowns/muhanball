using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHud : MonoBehaviour
{
    public int id;
    public bool isboss;
    //경험치
    public Slider expBar;
    public Text expPoint;
    public Text point;
    public Text bestPoint;

    public float currentPoint;


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
    public Text dia1;
    public Text attack;

    public Text maxstage1;

    //아이템 수량 
    public Text m_missile;
    public Text m_bomb;
    public Text m_laser;

    public Vector3 ballguide;
     

    public void init()
    {
        id = DataManager.Instance.getStagePlay();
        isboss = false;
        ballguide = hpBar.gameObject.transform.localPosition;

        //스테이지 정보 출력
        stage.text      = (((DataManager.Instance.getStagePlay() + 1) % 10) == 0) ? (DataManager.Instance.getStagePlay() + 1).ToString(): (DataManager.Instance.getStagePlay() + 1).ToString();
                        
        maxstage.text   = DataManager.Instance.getStageBest().ToString();
        maxstage1.text  = "스테지이 레벨 : " + DataManager.Instance.getStageBest().ToString();

        //기본 정보        
        attack.text     = (DataManager.Instance.getAttack() + DataManager.Instance.opBulList[DataManager.Instance.getWepon()].upat).ToString();               
        coin.text       = DataManager.Instance.getCoin().ToString();    //k
        coin1.text      = DataManager.Instance.getCoin().ToString();
        dia.text        = DataManager.Instance.getDia().ToString();
        dia1.text       = DataManager.Instance.getDia().ToString();

        //아이템 정보
        m_missile.text  = (DataManager.Instance.getMissale() > 9)?"max": DataManager.Instance.getMissale().ToString();
        m_bomb.text     = (DataManager.Instance.getBomb() > 9) ? "max" : DataManager.Instance.getBomb().ToString();
        m_laser.text    = (DataManager.Instance.getLazer() > 9) ? "max" : DataManager.Instance.getLazer().ToString();
 
    }
    
    // Update is called once per frame
    public void Update()
    {
         init();
    }     

    public void bossUI() 
    {
        currentPoint = expBar.value;

        expBar.gameObject.SetActive(false);
        timeBar.gameObject.SetActive(true);

        //겅혐치 세팅
        timeBar.minValue = 0;
        timeBar.maxValue = DataManager.Instance.stageList[DataManager.Instance.getstageID()].time;
        timeBar.value = timeBar.maxValue;

        StartCoroutine(timeOn());
    }

    public void nomalUI()
    {
        expBar.gameObject.SetActive(true);
        timeBar.gameObject.SetActive(false);

        //겅혐치 세팅
        expBar.minValue = setExpPoint(DataManager.Instance.getStagePlay());
        expBar.maxValue = setExpPoint(DataManager.Instance.getStagePlay() + 1);
        expBar.value = currentPoint;
        expPoint.text = expBar.value.ToString() + " / " + expBar.maxValue.ToString();

        //스테이지 저장은 일반 스테이지에서만.. 됨
        if (DataManager.Instance.getStagePlay() > DataManager.Instance.getStageBest())
        {
            DataManager.Instance.setStageBest(DataManager.Instance.getStagePlay() + 1);
        }
    }

    public void setExp(bool _isboss)
    {
        Gamemanager.i.isboss = false;               // 보스는 아직 안 나왔어
        Gamemanager.i.isbossplay = false;           // 보스 플레이는 시작되지 않았어..
        Gamemanager.i.isballstop = false;           // 이제 볼이 내려와야 한다.
 

        if (_isboss)     //보스판 시작        
            bossUI(); 
        else            // 일반판 시작
            nomalUI(); 

        DataManager.Instance.setStagePlay(DataManager.Instance.getStagePlay());

        DataManager.Instance.getHp();
        OptionManger.i.bullreset();
        DataManager.Instance.Save();
    }

    public int setExpPoint(int _level)
    {
        int setpoint = int.Parse(DataManager.Instance.LocalList[2].local[0]);
        setpoint += DataManager.Instance.getstageID();
        return (_level == 0) ? 0 : (((_level * (_level + 1) / 2) - (_level - 1)) * 10) * setpoint;
    }


    public void getExp(int _point)
    {      
        if ((expBar.value + _point) < expBar.maxValue)
        {
            expBar.value += _point;
            expPoint.text = expBar.value.ToString() + " / " + expBar.maxValue.ToString();

            //점수
            point.text = expBar.value.ToString();
            if(expBar.value > DataManager.Instance.getBestScore()) 
            {
                bestPoint.text = expBar.value.ToString();
                DataManager.Instance.setBestScore(int.Parse(point.text));
            }
            else{
                bestPoint.text = DataManager.Instance.getBestScore().ToString();
            }
            
        }
        else
        {
            //경험치 오바 
            expBar.value += _point;             //마지막으로 계산하고 
            DataManager.Instance.setStagePlay(DataManager.Instance.getStagePlay()+1);

            DataManager.Instance.saveData.AchieveUp[2] += 1;
            DataManager.Instance.saveData.AchieveUp[3] += 1;
            DataManager.Instance.saveData.AchieveUp[4] += 1;
                        
            setExp(false);                          
        }
    } 

    public bool isBossCall() 
    {
        int a = (int)((expBar.maxValue - expBar.minValue) / 2);
        return expBar.value > (expBar.minValue + a);            
    }

    public float ballDelay()
    {
        int a = (int)((expBar.maxValue - expBar.minValue) /3);
        int _id = DataManager.Instance.getStageBest() / 10;

        DataManager.Instance.delay = (expBar.value > (expBar.minValue + (a * 2)))? DataManager.Instance.stageList[_id].delayMax : DataManager.Instance.delay = DataManager.Instance.stageList[_id].delay;

        return DataManager.Instance.delay;
    }

    public void setHp()
    {
        hpBar.minValue  = 0;
        hpBar.maxValue  = DataManager.Instance.getHp(); 
        hpBar.value     = hpBar.maxValue;
        hpPoint.text    = hpBar.value.ToString();
    }

    public void getHp(int _hp)
    {
        if ((hpBar.value - _hp) > 0)
        {
            Gamemanager.i.shake.shakeCall();
            hpBar.value -= _hp;
            hpPoint.text = hpBar.value.ToString();
        }
        else
        {
            hpBar.value = 0;
            hpPoint.text = hpBar.value.ToString();
            Gamemanager.i.gameoverGame();
        }
    } 
    IEnumerator timeOn() 
    { 
        yield return null;
        while (Gamemanager.i.isboss)
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


    public void MissileBtn()
    {
        //ViewManager.Instance.PlayClickButtonSound();
        if (DataManager.Instance.getMissale() > 0)
        {
            SoundManager.Instance.play(8);
            Gamemanager.i.CreateBoostUp(BoostUpType.MISSILE);
            DataManager.Instance.saveData.AchieveUp[7] += 1;
        }
    }

    public void BombBtn()
    {
        //ViewManager.Instance.PlayClickButtonSound();
        if (DataManager.Instance.getBomb() > 0)
        {
            Gamemanager.i.CreateBoostUp(BoostUpType.BOMB);
            DataManager.Instance.saveData.AchieveUp[8] += 1;
        }
    }

    public void LaserBtn()
    {
        //ViewManager.Instance.PlayClickButtonSound();
        if (DataManager.Instance.getLazer() > 0)
        {
            Gamemanager.i.CreateBoostUp(BoostUpType.LASER);
            DataManager.Instance.saveData.AchieveUp[9] += 1;
        }
    } 
}