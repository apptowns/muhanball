using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.U2D;

public enum PlayerState
{
    Prepare,
    Living,
    Die,
    CompleteLevel,
}
public enum BoostUpType
{
    MISSILE = 0,
    BOMB = 1,
    LASER = 2,
}

public enum ItemType
{
    COIN = 0,
    HIDDEN_GUNS = 1,
    DIA = 2,
    MISSILE = 3,
    BOMB = 4,
    LASER = 5,
    HP=6
}
public enum IngameState
{
    Prepare,
    Playing,
    Revive,
    CompleteLevel,
    GameOver,
}

public class Player 
{
    public int attack;
    public int hp;
}

public class PlayerController : MonoBehaviour
{
    public GameObject coin;
    public GameObject m;
    public GameObject b;
    public GameObject l;

    public SpriteAtlas m_Atals = null;
    public static PlayerController Instance { private set; get; }
    public static event System.Action<PlayerState> PlayerStateChanged = delegate { };
    
    public PlayerState PlayerState
    {
        get
        {
            return playerState;
        }

        private set
        {
            if (value != playerState)
            {
                value = playerState;
                PlayerStateChanged(playerState);
            }
        }
    }

    public BulletController bulletPrefab;
    public GameObject[] pos;
    public bool isfire;
    public bool isplay;
    private PlayerState playerState = PlayerState.Die;


    [Header("Player Config")]
    public float limitBottom = -20.0f;
    float limitTop = 0.0f;

    [SerializeField] private float doubleGunsTime = 10f;
    [SerializeField] private float tripleGunsTime = 5f;
    [SerializeField] [Range(0f, 1f)] private float doubleGunsFrequency = 0.8f;
    [SerializeField] [Range(0f, 1f)] private float tripleGunsFrequency = 0.2f;

    [Header("Player References")]
    [SerializeField] private SpriteRenderer characterSpRender = null;
    [SerializeField] private BoxCollider2D characterPolygonCollider2D = null;


    private Vector2 firstPos = Vector2.zero;
    private Vector2 originalPos = Vector2.zero;
    private float limitLeft = 0;
    private float hiddenGunsTime = 0;
    private float limitRight = 0;
    int gunCount = 2;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            DestroyImmediate(Instance.gameObject);
            Instance = this;
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }


    private void Start()
    {
        //Fire event
        PlayerState = PlayerState.Prepare;
        playerState = PlayerState.Prepare;

        //Setup parameters

        float horizontalBorder = Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x - 3f;
       
        limitLeft = horizontalBorder + characterSpRender.bounds.size.x / 2;
        limitRight = Mathf.Abs(horizontalBorder) - characterSpRender.bounds.size.x / 2;
        
        originalPos = transform.position;

    }

    private void Update()
    {
        bulletPrefab.GetComponent<SpriteRenderer>().sprite = m_Atals.GetSprite("bullet_" + DataManager.Instance.getWepon().ToString());
        
        if (!isplay) //움직이지 못한다.
        {
            return;
        }        

        if (Input.GetMouseButtonDown(0))
        {
            firstPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        else if (Input.GetMouseButton(0))
        {
            Vector2 currentPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            float distanceX = Mathf.Abs(Mathf.Abs(firstPos.x) - Mathf.Abs(currentPos.x));
            float distanceY = Mathf.Abs(Mathf.Abs(firstPos.y) - Mathf.Abs(currentPos.y));

            if (currentPos.x < firstPos.x) //Draging left
            {
                if (transform.position.x > limitLeft)
                {
                    transform.position -= new Vector3(distanceX, 0, 0);
                }
            }
            else //Draging right
            {
                if (transform.position.x < limitRight)
                {
                    transform.position += new Vector3(distanceX, 0, 0);
                }
            }

            if (currentPos.y < firstPos.y) //Draging down
            {
                if (transform.position.y > limitBottom)
                {
                    transform.position -= new Vector3(0, distanceY, 0);
                }
            }
            else //Draging up
            {
                if (transform.position.y < limitTop)
                {
                    transform.position += new Vector3(0, distanceY, 0);
                }
            }
            firstPos = currentPos;
        }

        if (Input.GetMouseButtonUp(0))
        {
            firstPos = Vector2.zero;
        }

    }



    /// <summary>
    /// Call PlayerState.Living event and handle other actions.
    /// </summary>
    public void PlayerLiving()
    {
        Debug.Log("shoot");
        //Fire event
        PlayerState = PlayerState.Living;
        playerState = PlayerState.Living;

        //Add another actions here
        gunCount = 1;


        if (IsRevived)
        {
            transform.position = originalPos;
        }

        isfire = true;
        StartCoroutine(CRFiringBullets());
    }

    public bool IsRevived { private set; get; }

    /// <summary>
    /// Call PlayerState.Die event and handle other actions.
    /// </summary>
    private void PlayerDie()
    {
        //Fire event
        PlayerState = PlayerState.Die;
        playerState = PlayerState.Die;

    }


    /// <summary>
    /// Call PlayerState.CompleteLevel event and handle other actions.
    /// </summary>
    private void PlayerCompleteLevel()
    {
        //Fire event
        PlayerState = PlayerState.CompleteLevel;
        playerState = PlayerState.CompleteLevel;

        //Add another actions here
        StartCoroutine(CRMovingUp());
    }



    /// <summary>
    /// Moving this character up.
    /// </summary>
    /// <returns></returns>
    private IEnumerator CRMovingUp()
    {
        float movingTime = 1f;
        Vector3 startPos = transform.position;
        Vector3 endPos = Camera.main.ViewportToWorldPoint(new Vector2(0, 1.1f));
        endPos.x = startPos.x;
        float t = 0;
        while (t < movingTime)
        {
            t += Time.deltaTime;
            float factor = t / movingTime;
            transform.position = Vector3.Lerp(startPos, endPos, factor);
            yield return null;
        }
    }





    /// <summary>
    /// Firing bullets
    /// </summary>
    /// <returns></returns>
    private IEnumerator CRFiringBullets()
    {       
        while (isfire) //총쏘지 못한다.
        {
            if (gunCount == 1) //Fire center gun
            {
                Vector2 centerDir = pos[0].transform.TransformDirection(Vector2.up);                
                BulletController bullet = Instantiate(bulletPrefab, Vector3.zero, Quaternion.identity);
                bullet.gameObject.transform.position = pos[0].transform.position;
                bullet.OnSetup(centerDir);

            }
            else if (gunCount == 2) //Fire left and right guns
            {
                Vector2 leftDir = pos[1].transform.TransformDirection(Vector2.up);
                BulletController bullet = Instantiate(bulletPrefab, Vector3.zero, Quaternion.identity);
                bullet.gameObject.transform.position = pos[1].transform.position;
                bullet.OnSetup(leftDir);

                Vector2 rightDir = pos[2].transform.TransformDirection(Vector2.up);
                bullet = Instantiate(bulletPrefab, Vector3.zero, Quaternion.identity);
                bullet.gameObject.transform.position = pos[2].transform.position;
                bullet.OnSetup(rightDir);
            }
            else //Fire three guns
            {
                Vector2 leftDir = pos[1].transform.TransformDirection(Vector2.up);
                BulletController bullet = Instantiate(bulletPrefab, Vector3.zero, Quaternion.identity);
                bullet.gameObject.transform.position = pos[1].transform.position;
                bullet.OnSetup(leftDir);

                Vector2 rightDir = pos[2].transform.TransformDirection(Vector2.up);
                bullet = Instantiate(bulletPrefab, Vector3.zero, Quaternion.identity);
                bullet.gameObject.transform.position = pos[2].transform.position;
                bullet.OnSetup(rightDir);

                Vector2 centerDir = pos[0].transform.TransformDirection(Vector2.up);
                bullet = Instantiate(bulletPrefab, Vector3.zero, Quaternion.identity);
                bullet.gameObject.transform.position = pos[0].transform.position;
                bullet.OnSetup(centerDir);
            }
            yield return new WaitForSeconds(DataManager.Instance.getSpeed());
        }

    }

    /// <summary>
    /// Couting down hidden guns time.
    /// </summary>
    /// <returns></returns>
    private IEnumerator CRCountingHiddenGunsTime()
    {
        while (hiddenGunsTime > 0)
        {
            hiddenGunsTime -= Time.deltaTime;
            yield return null;
        }
        gunCount = 1;
        hiddenGunsTime = 0;
    }



    ///////////////////////////////////////////////////Publish functions


    /// <summary>
    /// handle actions when player collect a hidden guns item.
    /// </summary>
    public void HandleCollectHiddenGunsItem()
    {
        // 한발일때 2,3개로 
        //2개일때 시간 추가

        if (gunCount == 1)
        {
            while (true)
            {
                if (Random.value <= doubleGunsFrequency)
                {
                    //Enable double guns
                    gunCount = 2;
                    hiddenGunsTime = doubleGunsTime;
                    StartCoroutine(CRCountingHiddenGunsTime());
                    break;
                }

                if (Random.value <= tripleGunsFrequency)
                {
                    //Enable triple guns
                    gunCount = 3;
                    hiddenGunsTime = tripleGunsTime;
                    StartCoroutine(CRCountingHiddenGunsTime());
                    break;
                }
            }

        }
        else if (gunCount == 2)
        {
            //Player already enable double guns -> add more time
            hiddenGunsTime += doubleGunsTime;
            StartCoroutine(CRCountingHiddenGunsTime());
        }
        else if (gunCount == 3)
        {
            //Player already enable triple guns -> add more time
            hiddenGunsTime += tripleGunsTime;
            StartCoroutine(CRCountingHiddenGunsTime());
        }
    }
}
