using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Gamemanager : MonoBehaviour
{
    public static Gamemanager i;
    public enum GAMESTATE
    {
        WAIT, GAME, PAUSE, WIN, GAMEOVER, BOSS
    }
    public GAMESTATE gameState;

    //프레펩
    public GameObject ballControllerPrefab;
    GameObject music;

    //볼에서 나오는 아이템
    public ItemController CoinItem;
    public ItemController missailItem;
    public ItemController Bomb_Item;
    public ItemController Laser_Itemm;
    public ItemController hp_Item;

    public ItemController HiddenGunsItem;
    public ItemController SpeedItem;

    //날아가는 아이템
    public ItemMoveControl m_Coin;
    public ItemMoveControl m_Dia;
    public ItemMoveControl m_missail;
    public ItemMoveControl m_Bomb;
    public ItemMoveControl m_Laser;
    public ItemMoveControl m_hp;

    //배경음악
    public AudioClip GameMusic;
    public AudioClip BossMusic;

    //인클루드
    public GameHud          hud;
    public BossValueControl value;
    public EtcManager       Etc;
    public OptionManger     Op;
    public PopupManager     Popup;

    //효과 
    public Shake            shake;
    public OpenControl      fade;
    public OpenControl      color;
    //public GameObject       popup; 

    //확률
    public float NoneItemFrequency;         // 아이템 안 나올 확률은 50프로
     
    public  float NormalItemFrequency;      // 나머지 30에서 일반 아이템 나올 확률은 30프로
    public  float BoostUpItemFrequency;     // 나머지 30에서 부스트업 아이템 나올 확률은 10프로
    
    public float coinFrequency;             // 동전 확률 10프로
    public float hiddenGunsFrequency;       // 멀티 총알... 두줄 세줄로 나가는 아이템 나올 확률
     
    public  float missileFrequency;         // 미사일 확률 40프로   
    public  float bombFrequency;            // 폭탄     
    public  float laserFrequency;           // 레이저
    public  float hpFrequency;              // 체력 회복 


    //private float NoneItemFrequency { get { return noneItemFrequency; } }

    public bool inbossplay; // 보스 플레이

    public void setPer()
    {
        NoneItemFrequency       = DataManager.Instance.perList[0];   // 아이템 안 나올 확률은 50프로

        NormalItemFrequency     = DataManager.Instance.perList[1];   // 나머지 30에서 일반 아이템 나올 확률은 30프로
        BoostUpItemFrequency    = DataManager.Instance.perList[2];   // 나머지 30에서 부스트업 아이템 나올 확률은 10프로

        coinFrequency           = DataManager.Instance.perList[3];   // 동전 확률 10프로
        hiddenGunsFrequency     = DataManager.Instance.perList[4];   // 멀티 총알... 두줄 세줄로 나가는 아이템 나올 확률

        missileFrequency        = DataManager.Instance.perList[5];   // 미사일 확률 40프로   
        bombFrequency           = DataManager.Instance.perList[6];   // 폭탄     
        laserFrequency          = DataManager.Instance.perList[7];   // 레이저
        hpFrequency             = DataManager.Instance.perList[8];   // 체력 회복 
    }

    private void Awake()
    {
        if (!DataManager.Instance.isFirst)
        {
            SceneManager.LoadScene("LOGO");
        }
        i = this;
        setPer();
    }

    private void Start()
    {
        music = GameObject.Find("background").gameObject; 
        setPrefab();
        value.gameObject.SetActive(false);
        waitGame();
    }

    public void waitGame()
    {    
        musicOn(Gamemanager.i.GameMusic);
        inbossplay = false; 

        Time.timeScale = 0;
        gameState = GAMESTATE.WAIT;

        hud.expBar.value = 0;
        hud.setExp(false);
        hud.setHp();

        startGame();
    }

    public void startGame()
    {
        Debug.Log("///////////////////// startgame");
        Time.timeScale = 1;
        fade.Fadeon();

        PlayerController.Instance.isfire = true;
        PlayerController.Instance.isplay = true;

        gameState = GAMESTATE.GAME;

        StartCoroutine(CRCreatingBalls());
        PlayerController.Instance.PlayerLiving();
    }

    public void gameoverGame()
    {
        PlayerController.Instance.isfire = false; //총쏘지마
        PlayerController.Instance.isplay = false; //움직일수도 없어

        GameObject.Find("background").gameObject.GetComponent<AudioSource>().Stop();

        gameState = GAMESTATE.GAMEOVER;
        //restartButton.gameObject.transform.localPosition = new Vector3(0, 148f, 0);

        GameObject[] ball = GameObject.FindGameObjectsWithTag("Ball");
        Debug.Log(" dieball " + ball.Length);

        for (int i = 0; i < ball.Length; i++)
        {
            ball[i].GetComponent<BallController>().lineDisableObject(); //아이템 없이 폭파
        }

        //판넬을 오픈 dnlapdl
        value.set(false);
        DataManager.Instance.Save();
    }

    public void musicOn(AudioClip _backMusic) //배경음악
    {
        Debug.Log("///////////////////// sound");
        music.GetComponent<AudioSource>().volume = 0.500f;
        music.GetComponent<AudioSource>().loop = true;
        music.GetComponent<AudioSource>().clip = _backMusic; // Gamemanager.i.GameMusic;
        music.GetComponent<AudioSource>().Play();

        if (DataManager.Instance.saveData.ismusic)
            music.GetComponent<AudioSource>().mute = true;
        else
            music.GetComponent<AudioSource>().mute = false;

        DontDestroyOnLoad(GameObject.Find("background"));
    }

    public void checkCount() // 새기록 점수 
    {
        if (int.Parse(hud.point.text) > DataManager.Instance.getBestScore())
        {
            DataManager.Instance.setBestScore(int.Parse(hud.point.text));
            SoundManager.Instance.play(6);
            //MessageManager.i.onPanel("새로운 기록을 달성 했어요.");
        }
    }

    public int getCount() // 볼의 수치가 어떻게 낭로 것인가?
    {
        int value = 0;
        value = DataManager.Instance.getstageID();
        //제한 스테이지까지 일정 이하의 수치만 나온다.
        //
        if ((DataManager.Instance.getStagePlay()) < DataManager.Instance.stageList[value].baseStage)
        {
            return Random.Range(DataManager.Instance.stageList[value].minnumber, DataManager.Instance.stageList[value].minsetnumber);
        }
        else
        {
            int a = Random.Range(0, 100);
            if (a < 5)
            {
                return Random.Range(DataManager.Instance.stageList[value].maxnumber, DataManager.Instance.stageList[value].upnumber);
            }
            else
            {
                return Random.Range(DataManager.Instance.stageList[value].minnumber, DataManager.Instance.stageList[value].maxnumber);
            }
        }
    }

    public int count;
    public bool isboss = false;         //보스가 나왔어
    public bool isballstop = false; //보스중 볼 나올지..
    public bool isbossplay = false;     //보스중 볼 나올지

    private IEnumerator CRCreatingBalls()
    {
        Debug.Log("make ball");
        while (gameState == GAMESTATE.GAME)
        {
            Vector2 tempPos;
            GameObject m;
            int number;

            //스테이지 체크 . 현재 보스 출현했는지 . 출현 상황인지
            if ((((DataManager.Instance.getStagePlay() + 1) % 10) == 0) && !isboss && hud.isBossCall()) // 보스 나올 타이밍 일때
            {
                Gamemanager.i.isboss = true;
                bossGame();                
            }
            else  // 아닐때
            {
                if (isballstop) // 볼 플레이 안한다면..
                {

                }
                else
                {
                    m = Instantiate(ballControllerPrefab, Vector3.zero, Quaternion.identity);
                    m.gameObject.SetActive(false);

                    tempPos = Camera.main.ViewportToWorldPoint(new Vector2(0.5f, 1)) + new Vector3(0, 5f, 0);
                    m.transform.position = tempPos;
                    m.gameObject.SetActive(true);

                    number = getCount();

                    m.GetComponent<BallController>().isArmor = false;
                    m.GetComponent<BallController>().isboss = false;
                    m.GetComponent<BallController>().OnSetup(number);

                    while (m.GetComponent<BallController>().IsStillOverlap)
                    {
                        yield return null;
                    }

                    m.GetComponent<BallController>().MoveDown();
                }
            }

            //딜레이
            yield return new WaitForSeconds(hud.ballDelay());// /* + DataManager.Instance.fallingSpeedSec*/);
            yield return null;
        }
    }

    public void bossGame() 
    {
        Gamemanager.i.isbossplay = true;
        color.Coloron();                    // 위험 효과...
        musicOn(Gamemanager.i.BossMusic);   // 보스 음악
        StartCoroutine(boss0());

        return;

        switch (DataManager.Instance.getstageID()) 
        {
            case 0: // 데지미가. 오십프로만 들어간다.
                StartCoroutine(boss0());
                break;
            case 1: // 데미지가 랜덤하게 30~50프로만 들어간다.
                StartCoroutine(boss0());
                break;
            case 2: // 데미지가 오히려 커진다. 최대수치까지 커지면 끝 (업넘버)
                StartCoroutine(boss0());
                break;
            case 3:
                StartCoroutine(boss0());
                break;
        }     
    }

    IEnumerator boss0()
    {
        Vector2 tempPos;
        GameObject m;
        int number;

        GameObject prefab = Resources.Load("ball/Ball1") as GameObject;
        if (!prefab)
            Debug.Log("-0-----------------------------");
        
        //Create boss           
        m = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        m.gameObject.SetActive(false);

        tempPos = Camera.main.ViewportToWorldPoint(new Vector2(0.5f, 1)) + new Vector3(0, 5f, 0);
        m.transform.position = tempPos;
        m.gameObject.SetActive(true);

        //보스급 에너미 출현
        int bosshp = int.Parse(DataManager.Instance.LocalList[3].local[0]);
        number = DataManager.Instance.stageList[DataManager.Instance.getstageID()].upnumber *  bosshp;

        m.GetComponent<BallController>().isArmor = false;
        m.GetComponent<BallController>().isboss = true; 
        m.GetComponent<BallController>().bossid = 0; // 기본 컨셉
        
        m.GetComponent<BallController>().OnSetup(number); //

        //MessageManager.i.onPanel("보스 출현");

        while (m.GetComponent<BallController>().IsStillOverlap)
        {
            yield return null;
        }

        m.GetComponent<BallController>().MoveDown();
    }

    ////////////////////////////////////////////////////////////////// 아이템 발생    
    public void CreateItem(Vector2 pos)
    {
        Debug.Log("item");

        Debug.Log("bullet");

        //토탈에 가중치를 더한다..
        // 아이템은 없는게 50 있는게 50
        // 그중 코인, 더블 아이템 / 미사일,폭탄, 레이저, 체력 

        int total = 0;
        for (int i = 0; i < DataManager.Instance.itemWeightList.Count; i++)
        {
            total += DataManager.Instance.itemWeightList[i]; //하위템일수록 가중치가 높다.
        }

        Debug.Log("total :" + total);
        int weight = 0;
        int selecnum = 0;
        int itemNumber = 0;

        selecnum = Mathf.RoundToInt(total * Random.Range(0.0f, 1.0f));

        for (int i = 0; i < DataManager.Instance.itemWeightList.Count; i++)
        {
            weight += DataManager.Instance.itemWeightList[i];
            if (selecnum <= weight)
            {
                Debug.Log(selecnum + " <= " + weight + "당첨 :" + i);
                itemNumber = i;
                break;                
            }
        }
         
        switch (itemNumber) 
        {
            case 0: // 없어
                break;

            case 1: // 코인
                ItemController itemController = Instantiate(CoinItem, Vector3.zero, Quaternion.identity);
                itemController.transform.position = pos;
                itemController.gameObject.SetActive(true);
                itemController.OnSetup();
                break;

            case 2: // 더블 총알
                if (Random.Range(0, 100) < 50) 
                {
                    itemController = Instantiate(HiddenGunsItem, Vector3.zero, Quaternion.identity);
                    itemController.transform.position = pos;
                    itemController.gameObject.SetActive(true);
                    itemController.OnSetup();
                }
                else 
                {
                    itemController = Instantiate(CoinItem, Vector3.zero, Quaternion.identity);
                    itemController.transform.position = pos;
                    itemController.gameObject.SetActive(true);
                    itemController.OnSetup();
                } 
                break;

            case 3: // 미사일
                itemController = Instantiate(missailItem, Vector3.zero, Quaternion.identity);
                itemController.transform.position = pos;
                itemController.gameObject.SetActive(true);
                itemController.OnSetup();
                break;

            case 4: //폭탄
                itemController = Instantiate(Bomb_Item, Vector3.zero, Quaternion.identity);
                itemController.transform.position = pos;
                itemController.gameObject.SetActive(true);
                itemController.OnSetup();
                break;

            case 5: // 레이저
                itemController = Instantiate(Laser_Itemm, Vector3.zero, Quaternion.identity);
                itemController.transform.position = pos;
                itemController.gameObject.SetActive(true);
                itemController.OnSetup();
                break;
            case 6:  //체력
                itemController = Instantiate(Laser_Itemm, Vector3.zero, Quaternion.identity);
                itemController.transform.position = pos;
                itemController.gameObject.SetActive(true);
                itemController.OnSetup();
                break;
        }
    }

    ////////////////////////////////////////////////////////////////// 닿음녀 날아가는 아이템 발생
    public void CreateMoveItem(int _id,Vector2 pos)
    {
        Debug.Log("moveitem");
        switch(_id)
        {
            case 0: //코인'GameObject m;
                GameObject m;
                m = Instantiate(m_Coin.gameObject, Vector3.zero, Quaternion.identity);
                m.transform.position = pos;
                m.gameObject.SetActive(true);
                m.GetComponent<ItemMoveControl>().OnSetup(0);
                break;
            case 1: //다이아
                m = Instantiate(m_Dia.gameObject, Vector3.zero, Quaternion.identity);
                m.transform.position = pos;
                m.gameObject.SetActive(true);
                m.GetComponent<ItemMoveControl>().OnSetup(1);
                break;
            case 2: //미사일
                m = Instantiate(m_missail.gameObject, Vector3.zero, Quaternion.identity);
                m.transform.position = pos;
                m.gameObject.SetActive(true);
                m.GetComponent<ItemMoveControl>().OnSetup(2);
                break;
            case 3: //폭탄
                m = Instantiate(m_Bomb.gameObject, Vector3.zero, Quaternion.identity);
                m.transform.position = pos;
                m.gameObject.SetActive(true);
                m.GetComponent<ItemMoveControl>().OnSetup(3);
                break;
            case 4: //레이저
                m = Instantiate(m_Laser.gameObject, Vector3.zero, Quaternion.identity);
                m.transform.position = pos;
                m.gameObject.SetActive(true);
                m.GetComponent<ItemMoveControl>().OnSetup(4);
                break;
            case 100: //코인
                StartCoroutine(coinmove(pos));
                break;
        }
    }

    ////////////////////////////////////////////////////////////////// 일반 스테이지에서 랜덤 코인..     
    IEnumerator coinmove(Vector2 _pos) 
    {
        Debug.Log("렌덤코인");

        GameObject m;
        int ran = Random.Range(1, DataManager.Instance.stageList[DataManager.Instance.getstageID()].goldValue + 1);
        for (int i = 0; i < ran; i++)
        {
            m = Instantiate(m_Coin.gameObject, Vector3.zero, Quaternion.identity);
            m.transform.position = _pos;
            m.gameObject.SetActive(true);
            m.GetComponent<ItemMoveControl>().OnSetup(0);
            yield return null;
            yield return null;
            yield return null;
            yield return null;
        }
    }

    public BoostUpController bombControllerPrefab;
    public BoostUpController missileControllerPrefab;
    public BoostUpController laserControllerPrefab; 

    public BoostUpController bombController;
    public BoostUpController missileController;
    public BoostUpController laserController; 

    public void setPrefab()
    {
        //Create a missile
        missileController = Instantiate(missileControllerPrefab, Vector3.zero, Quaternion.identity);
        missileController.gameObject.SetActive(false);

        //Create a bomb
        bombController = Instantiate(bombControllerPrefab, Vector3.zero, Quaternion.identity);
        bombController.gameObject.SetActive(false);

        //Create a laser
        laserController = Instantiate(laserControllerPrefab, Vector3.zero, Quaternion.identity);
        laserController.gameObject.SetActive(false);        
    }

    public void CreateBoostUp(BoostUpType boostUpType)
    {
        if (bombController.gameObject.activeInHierarchy || missileController.gameObject.activeInHierarchy || laserController.gameObject.activeInHierarchy)
            return;

        if (boostUpType == BoostUpType.BOMB && DataManager.Instance.getBomb() > 0)
        {
            DataManager.Instance.setBomb(DataManager.Instance.getBomb()-1); //            DataManager.Instance.Bombs -=1;

            bombController.transform.position = PlayerController.Instance.transform.position;
            bombController.gameObject.SetActive(true);
            bombController.OnSetup();
        }
        else if (boostUpType == BoostUpType.MISSILE && DataManager.Instance.getMissale() > 0)
        {
            DataManager.Instance.setMissle(DataManager.Instance.getMissale()-1); //            DataManager.Instance.Missiles -= 1;
            missileController.transform.position = PlayerController.Instance.transform.position;
            missileController.gameObject.SetActive(true);
            missileController.OnSetup();
        }
        else if (boostUpType == BoostUpType.LASER && DataManager.Instance.getLazer()/*ItemsManager.Instance.Lasers*/ > 0)
        {
            DataManager.Instance.setLazer(DataManager.Instance.getLazer()-1); //DataManager.Instance.Lasers -= 1;
            Vector3 pos = PlayerController.Instance.transform.position;
            pos.x = 0;
            laserController.transform.position = pos;
            laserController.gameObject.SetActive(true);
            laserController.OnSetup();
        }

        DataManager.Instance.Save();
    }

}
