using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class BallController : MonoBehaviour
{  
    public string[] TYPE ={"NOMAL","HP","UP"};
    
    public bool         isboss;             // 보스인지 아닌지..
    public int          bossid;             // 아이디 => 몇번째 보스인지..  
    public bool         damageType;         // 데지미를 어떻게 줄것인가
    
    private Color       color;              // 칼라
    public int          dem;                // 데미지
    public Text         fxText;             // 크리티컬등.. 표기

    public bool         isArmor;
    public int          armorValue;
    public int          currentArmorValue;
    public GameObject   armor;
    Vector3             v;
   
    private float       baseScale = 0.30f;  //가장 작은 크기
    private float       maxScale = 2.0f;    //최대 크기    
    private float       baseUpscale;        //그사이의 단계

    public SpriteRenderer SpriteRenderer { get { return spriteRenderer; } } 
    public bool         IsStillOverlap { private set; get; }

    private float       scaleDownFactor = 0;
    private float       fallingSpeed;
    private bool        isVisible = false;

    public int          hp;                  //볼의 체력
    public int          currenthp;
    public int          c;                   //배열에서 칼라 위치 저장
    public bool         endCount;            //마지막 보스인지 체크

    [SerializeField] private SpriteRenderer     spriteRenderer = null;
    [SerializeField] public GameObject          collider2D = null;
    [SerializeField] private Text               numberText = null;
    [SerializeField] private Text               colorText = null;
    [SerializeField] private LayerMask          ballLayerMask = new LayerMask();
    [SerializeField] private LayerMask          boostUpLayerMask = new LayerMask();   
 
    ////////////////////////////////////////////////////////////////// 볼 생성 

    public void OnSetup(int _count)
    { 
        collider2D.GetComponent<CircleCollider2D>().enabled = false;
        fxText.gameObject.SetActive(false);
        hp = _count;
        currenthp = hp;

        isVisible = false;
        spriteRenderer.enabled = false;
        IsStillOverlap = true;
        fallingSpeed = DataManager.Instance.stageList[DataManager.Instance.getStageBest() / 10].speed;

        //Random a number for this ball
        numberText.text = _count.ToString();

        c = (!isboss) ? Random.Range(0, colorList.Length) : colorList.Length - 1;
        colorText.text = (DataManager.Instance.isColorText) ? colorText.text = colorList[c] : colorText.text = "";
        ColorUtility.TryParseHtmlString(colorList[c], out color);
        spriteRenderer.color = color;

        float scale = GetScale(_count);
        transform.localScale = new Vector3(scale, scale, 1);
        StartCoroutine(CRCheckingOverlap());
    }

    ////////////////////////////////////////////////////////////////// 볼 생성 후 위치 체크 

    private IEnumerator CRCheckingOverlap()
    {
        yield return null;
        IsStillOverlap = true;

        //Set position
        float borderLeft = Camera.main.ViewportToWorldPoint(new Vector2(0, 1)).x;
        float borderRight = Camera.main.ViewportToWorldPoint(new Vector2(1, 1)).x;
        float radius = spriteRenderer.bounds.extents.x;
        float leftPoint = borderLeft + radius;
        float rightPoint = borderRight - radius;
        float yPos = Camera.main.ViewportToWorldPoint(new Vector2(0, 1)).y + radius + 0.5f;
        transform.position = new Vector2(Random.Range(leftPoint, rightPoint), yPos);
        yield return null;

        //The collider is overlapping another     
        while (Physics2D.OverlapCircle(transform.position, radius + 0.1f, ballLayerMask) != null)
        {
            yield return null;
            //Reset position
            transform.position = new Vector2(Random.Range(leftPoint, rightPoint), yPos);
        }
        spriteRenderer.enabled = true;
        collider2D.GetComponent<CircleCollider2D>().enabled = true;
        IsStillOverlap = false;
    }

    ////////////////////////////////////////////////////////////////// 볼 이동 아래로 내려감
    public void MoveDown()
    {
        //StartCoroutine(CRFalling());
        StartCoroutine(CRMovingDown());
    }
      
    IEnumerator CRMovingDown()
    {
        while (PlayerController.Instance.isplay)
        {
            //이동
            Vector2 pos = transform.position;
            pos += Vector2.down * (fallingSpeed / DataManager.Instance.fallingSpeedSec) * Time.deltaTime;
            transform.position = pos;
            yield return null;

            // 이동중 부닫혔을때
            // 아이템이면.
            Collider2D boostUpCollider = Physics2D.OverlapCircle(transform.position, spriteRenderer.bounds.extents.x, boostUpLayerMask);
            if (boostUpCollider != null)
            {
                Debug.Log("아이템 데미지 : " + boostUpCollider.gameObject.GetComponent<BoostUpController>().CheckType());
                HandleOnHitByItem(boostUpCollider.gameObject.GetComponent<BoostUpController>().CheckType()); 
                
            }

            // 특정 위치면.. 없어진다..
            if (!isVisible)
            {
                Vector2 currentPos = (Vector2)transform.position + Vector2.down * (spriteRenderer.bounds.extents.y);
                float y = Camera.main.WorldToViewportPoint(currentPos).y;
                if (y < 0.99f)
                {
                    isVisible = true;
                }
            }

            // hp  위치보다 내려오면.. 남은 숫자 값을 뺀다.
            Vector2 checkPos = (Vector2)transform.position + Vector2.up * spriteRenderer.bounds.extents.y;
            if (checkPos.y < -20.0f)
            {
                if (isboss) // 그게 보스면 게임오바
                {
                    lineDisableObject(); //아무것도 주지 않는다.
                    Gamemanager.i.gameoverGame();
                }
                else // 그냥 볼이면 남은 숫자 만큼 체력을 깎는다. 
                {

                    Gamemanager.i.hud.getHp(int.Parse(numberText.text.Trim()));
                    lineDisableObject(); //같은 경우 
                    yield break;
                }
            }

        }
    }

    ////////////////////////////////////////////////////////////// 총알로 데미지를 입었다.
    public void HandleOnHitByBullet()
    {
        //기본 공격력 + 총알의 플러스 공격력
        dem = DataManager.Instance.getAttack() + DataManager.Instance.opBulList[DataManager.Instance.getWepon()].upat;
        
        //크리티컬 데미지 체크
        float ran = Random.Range(0, 10.0f);
        if (ran < DataManager.Instance.getCri())
        {
            dem = dem * 2; //2배 
            EffectManager.Instance.PlayBallExplodeEffect(transform.position, spriteRenderer.color);
            fxText.text = dem.ToString(); // "CRITICAL!!";
        }
               
        if (isboss)                 // 보스 일 경우 미리 정한 규칙에 따라 데미지를 입는다. 
        {
            BossType(bossid);       // 보스의 데미지가 먹는 방식
        }
        else                        // 일반
        {
            damege(true);
        } 
    }

    ////////////////////////////////////////////////////////////// 보스일때 데지지 수치는 어떻게 적용되는지  
    void BossType(int _id)
    {
        //데미지를 어떻게 입는가.. 데미지를 입을때 반응
        switch (DataManager.Instance.getstageID())
        {
            case 0: //50%              
                dem = dem / 2;
                damege(true);
                break;

            default: // 이동... 위치 이동
                dem = dem / 2;
                damege(true);
                break;

        }
    }

    ////////////////////////////////////////////////////////////// 데미지를 통해 볼이 커질지 줄어들지   
    void damege(bool _isup) 
    {
        int _minnumber = DataManager.Instance.stageList[DataManager.Instance.getstageID()].minnumber;
        int _maxnumber = DataManager.Instance.stageList[DataManager.Instance.getstageID()].maxnumber;
        int _upnumber = DataManager.Instance.stageList[DataManager.Instance.getstageID()].upnumber;

        if (_isup)
        {
            if ((int.Parse(numberText.text) - dem) > 0)              //데미지를 입었어도 살았을때.  
            {
                SoundManager.Instance.play(1);
                spriteRenderer.color = new Color(1, 1, 1);
                int newNumber = int.Parse(numberText.text) - dem;

                if ((int.Parse(numberText.text) - dem) < _minnumber) //남은 볼수치가 min 보다 작을때 크기를 줄이지 않는다.
                {
                    numberText.text = newNumber.ToString();
                    Invoke("setColor", 0.05f);
                }
                else                                                // 남은 볼수치가 min보다 크면 크기를 조절한다. 
                {
                    float scale = GetScale(newNumber); //  getScale(newNumber);
                    transform.localScale = new Vector3(scale, scale, 1);

                    numberText.text = newNumber.ToString();
                    Invoke("setColor", 0.05f);
                }
            }
            else                                                    //죽었을 경우
            {
                DisableObject(); 
            }
        }
        else // 특정 수치에 도달하면 죽는 방식
        {
            if ((int.Parse(numberText.text)+ dem)<(_upnumber*2))              //데미지를 입었어도 살았을때.  
            {
                SoundManager.Instance.play(1);
                spriteRenderer.color = new Color(1, 1, 1);
                int newNumber = int.Parse(numberText.text) + dem;

                if ((int.Parse(numberText.text) - dem) < _minnumber) //남은 볼수치가 min 보다 작을때 크기를 줄이지 않는다.
                {
                    numberText.text = newNumber.ToString();
                    Invoke("setColor", 0.05f);
                }
                else                                                // 남은 볼수치가 min보다 크면 크기를 조절한다. 
                {
                    float scale = GetScale(newNumber); //  getScale(newNumber);
                    transform.localScale = new Vector3(scale, scale, 1);

                    numberText.text = newNumber.ToString();
                    Invoke("setColor", 0.05f);
                }
            }
            else                                                    //죽었을 경우
            {
                DisableObject();
            }
        }
    }

    ////////////////////////////////////////////////////////////// 볼 데미지 체크 어텍 수치 만큼.. 정리    
    public void HandleOnHitByItem(int _damege)
    {
        dem = _damege;

        //크리티컬 먼저 적용
        float ran = Random.Range(0, 10.0f);
        if (ran < DataManager.Instance.getCri())
        {
            _damege = _damege * 2; //2배 
            //이펙트
            EffectManager.Instance.PlayBallExplodeEffect(transform.position, spriteRenderer.color);
            //텍스트 
            //fxText.text = "CRITICAL!!";
            //fxText.gameObject.SetActive(true);

        }

        // 보스는 아이템 데미지가 들어가지 않는다. 
        if (isboss)
            return;


        //이미 데미지를 입었을 경우 리턴
        if (isCall) 
            return;

        if ((int.Parse(numberText.text) - _damege) > 0)
        {
            SoundManager.Instance.play(1);
            isCall = true; //한번맞았어
            // 남은 데지지가 작은 볼수치보다 작을때
            if ((int.Parse(numberText.text) - _damege) < DataManager.Instance.stageList[DataManager.Instance.getstageID()].minnumber)
            {
                spriteRenderer.color = new Color(1, 1, 1);
                int newNumber = int.Parse(numberText.text) - _damege;
                numberText.text = newNumber.ToString();
                Invoke("setColor", 0.05f);

            }
            else //클때 스케일을 줄인다.
            {
                spriteRenderer.color = new Color(1, 1, 1);
                int newNumber = int.Parse(numberText.text) - _damege;
                float scale = GetScale(newNumber); //  getScale(newNumber);
                transform.localScale = new Vector3(scale, scale, 1);

                numberText.text = newNumber.ToString();
                Invoke("setColor", 0.05f);

            }
        }
        else //죽어야 해
        {
            itemDisableObject(); //            lineDisableObject(); // 아이템도 안줌.. 
        }

        //fxText.gameObject.SetActive(false);
    }

    public void DisableObject()
    {
        if (isboss) // 내가 보스면 
        {
            SoundManager.Instance.play(6);
            Gamemanager.i.color.Coloroff();         // 위험도 끝났어

            itemValue();                            // 아이템 결과 

            Gamemanager.i.isbossplay = false;       // 보스 플레이는 끝났어..
            Gamemanager.i.isballstop = false;   // 이제 볼이 내려와야 한다.
            
            //보스가 죽었으니.. 게임 뮤직으로 전환
            Gamemanager.i.musicOn(Gamemanager.i.GameMusic); 
        }
        else // 아니면 
        {
            if (Gamemanager.i.isbossplay)
            {
                
            }
            else 
            {
                Gamemanager.i.hud.getExp(hp);
            }

            SoundManager.Instance.play(0);
            Gamemanager.i.CreateItem(transform.position);
        }

        BallCrearAchieve();

        EffectManager.Instance.PlayBallExplodeEffect(transform.position, spriteRenderer.color);
        DataManager.Instance.Save();

        gameObject.SetActive(false);
    }

    public void itemDisableObject() 
    {
        Debug.Log("die"); 
        EffectManager.Instance.PlayBallExplodeEffect(transform.position, spriteRenderer.color);

        SoundManager.Instance.play(0);
        Gamemanager.i.CreateItem(transform.position);

        gameObject.SetActive(false);
    }

    public void lineDisableObject()
    {
        Debug.Log("die");
        SoundManager.Instance.play(0);
        EffectManager.Instance.PlayBallExplodeEffect(transform.position, spriteRenderer.color);
        gameObject.SetActive(false); 
    }
    
    void BallCrearAchieve() 
    {
        //공 클리어 저장
        DataManager.Instance.saveData.AchieveUp[5] += 1;
        DataManager.Instance.saveData.AchieveUp[6] += 1;
        DataManager.Instance.saveData.AchieveUp[7] += 1;
    }

    public  bool isCall = false;

    float GetScale(int _number)
    {
        baseScale = 0.30f;
        maxScale = 1.8f;

        int min, max, up;
        min = DataManager.Instance.stageList[DataManager.Instance.getstageID()].minnumber;
        max = DataManager.Instance.stageList[DataManager.Instance.getstageID()].maxnumber;
        up = DataManager.Instance.stageList[DataManager.Instance.getstageID()].upnumber;

        //baseUpscale = (maxScale - baseScale) / (max - min);

        baseUpscale = (maxScale - baseScale) / (up - min); //업 스케일 수치

        return baseScale + (baseUpscale * (_number - min));
    }

    public int getScore(int _no) // 숫자 표기
    {
        string a = _no.ToString();
        int aa = 0;
        switch (a.Length)
        {
            case 1:
                aa = int.Parse(a.Substring(0, 1));
                break;
            case 2:
                aa = int.Parse(a.Substring(0, 1));
                break;
            default:
                aa = int.Parse(a.Substring(0, 2));
                break;
        }

        Debug.Log("수치 : " + aa);

        return aa;
    }

    public void setColor()
    {
        ColorUtility.TryParseHtmlString(colorList[c], out color);
        spriteRenderer.color = color;

        return;
    }

    public float vC(int _x)
    {
       return _x / 255;
    }

    public void StopFalling()
    {
        StopAllCoroutines();
        if (IsStillOverlap)
        {
            spriteRenderer.enabled = true;

            if (isboss)
                collider2D.GetComponent<BoxCollider2D>().enabled = true;            
            else
                collider2D.GetComponent<CircleCollider2D>().enabled = true;            

            gameObject.SetActive(false);
        }
    }

    string[] colorList = { // 칼라리스트
        "#DC143C",
        "#B22222",
        "#8B4513",
        "#FF7F50",
        "#FF6347",
        "#FF8C00",
        "#DAA520",
        "#FFD700",
        "#ADFF2F",
        "#BDB76B",
        "#9ACD32",
        "#6B8E23",
        "#2E8B57",
        "#00FF7F",
        "#98FB98",
        "#20B2AA",
        "#00FFFF",
        "#87CEEB",
        "#00BFFF",
        "#1E90FF",
        "#483D8B",
        "#9932CC",
        "#DA70D6",
        "#EE82EE",
        "#7B68EE",
        "#FF1493",
        "#696969",
        "#2F4F4F"
    };

    void itemValue() 
    {
        Debug.LogError("boss end ------------------------");
        int count =0;

        //폭팔후 아이템은.. 
        if (!DataManager.Instance.getBossCrear(DataManager.Instance.getstageID()))
        {
            Debug.LogError("안 깼으면");
            count = DataManager.Instance.stageList[DataManager.Instance.getstageID()].gold;
            for (int i = 0; i < count; i++)
                Gamemanager.i.CreateMoveItem(0, transform.position);
            
            count = DataManager.Instance.stageList[DataManager.Instance.getstageID()].dia;
            for (int i = 0; i < count; i++)
                Gamemanager.i.CreateMoveItem(1, transform.position);
            
            count = DataManager.Instance.stageList[DataManager.Instance.getstageID()].m;
            for (int i = 0; i < count; i++)
                Gamemanager.i.CreateMoveItem(2, transform.position);
            
            count = DataManager.Instance.stageList[DataManager.Instance.getstageID()].b;
            for (int i = 0; i < count; i++)
                Gamemanager.i.CreateMoveItem(3, transform.position);
                        
            count = DataManager.Instance.stageList[DataManager.Instance.getstageID()].l;
            for (int i = 0; i < count; i++)
                Gamemanager.i.CreateMoveItem(4, transform.position);
            
            //하트는 주지 않는다.

            DataManager.Instance.setBossCrear(DataManager.Instance.getstageID(), true);
        }
        else // 클리어 한 보스를 다시 깰 꼉우 최소한의 아이템만 준다. 
        {
            Debug.Log("깼으면");
            count = DataManager.Instance.stageList[DataManager.Instance.getstageID()].gold/2;
            for (int i = 0; i < count; i++)
                Gamemanager.i.CreateMoveItem(0, transform.position);

            count = DataManager.Instance.stageList[DataManager.Instance.getstageID()].dia/2;
            for (int i = 0; i < count; i++)
                Gamemanager.i.CreateMoveItem(1, transform.position);
        }
    }
}
