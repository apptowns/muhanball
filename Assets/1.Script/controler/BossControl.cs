using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossControl : MonoBehaviour
{
    /*
    [SerializeField] private SpriteRenderer spriteRenderer = null;
    [SerializeField] private BoxCollider2D collider2D = null;
    [SerializeField] private Text numberText = null;
    [SerializeField] private LayerMask ballLayerMask = new LayerMask();
    [SerializeField] private LayerMask boostUpLayerMask = new LayerMask();

    [SerializeField] private LayerMask deadLayerMask = new LayerMask();

    public bool isArmor;
    public float armorValue;
    public float currentArmorValue;
    public GameObject armor;
    Vector3 v;

    public SpriteRenderer SpriteRenderer { get { return spriteRenderer; } }
    public BoxCollider2D Collider2D { get { return collider2D; } }

    public bool IsStillOverlap { private set; get; }

    private float scaleDownFactor = 0;
    private float fallingSpeed;
    private bool isVisible = false;

    /// <summary>
    /// Disable this ball and handle other actions.  
    /// </summary>
    public void DisableObject()
    {
        Debug.Log("die");
        EffectManager.Instance.PlayBallExplodeEffect(transform.position, spriteRenderer.color);
        gameObject.SetActive(false);
    }

    public int hp;
    public int c;
    /// <summary>
    /// Setup this ball.
    /// </summary>
    /// <param name="ballParameters"></param>
    public void OnSetup(int _count)
    {
        Debug.Log("boss");

        isArmor = true;

        currentArmorValue = 3.2f;
        armorValue = (float)(_count);
        Debug.Log(armorValue);
        v = this.transform.localScale;
        v.x = 3.2f;

        armor.transform.localScale = v;

        hp = _count;
        isVisible = false;
        spriteRenderer.enabled = false;
        Collider2D.enabled = false;
        IsStillOverlap = true;
        fallingSpeed = DataManager.Instance.fallingSpeed*2;

        //Random a number for this ball
        numberText.text = _count.ToString();
        spriteRenderer.color = new Color(0.84f, 0, 0.63f);

        float scaleFactor = (1 - DataManager.Instance.MinBallScale) / DataManager.Instance.maxBallNumber;
        float scale = (_count == 1) ? DataManager.Instance.MinBallScale : (scaleFactor * _count) + DataManager.Instance.MinBallScale;

        transform.localScale = new Vector3(scale, scale, 1);
        StartCoroutine(CRCheckingOverlap());
    }

    /// <summary>
    /// Checking overlap another ball.
    /// </summary>
    /// <returns></returns>
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
        Collider2D.enabled = true;
        IsStillOverlap = false;
    }



    /// <summary>
    /// Move this ball down after IsStillOverlap = false.
    /// </summary>
    public void MoveDown()
    {
        StartCoroutine(CRMovingDown());
        StartCoroutine(CRFalling());

    }
     
    IEnumerator CRFalling()
    {
        while (gameObject.activeInHierarchy)
        {
            yield return null;
            if (Physics2D.OverlapCircle(transform.position, spriteRenderer.bounds.extents.x, deadLayerMask) != null)
            {
                //내가 누구인지
                // ServicesManager.Instance.SoundManager.PlaySound(ServicesManager.Instance.SoundManager.collectCoin);
                EffectManager.Instance.PlayCollectCoinEffect(transform.position);
                DisableObject();
                BossManager.i.gameoverGame();
            }
        }
    } 

    /// <summary>
    /// Moving down.
    /// </summary>
    /// <returns></returns>
    IEnumerator CRMovingDown()
    {
        while (BossManager.i.bossState == BossManager.BOSSSTATE.GAME) // gameObject.activeInHierarchy)
        {
            Vector2 pos = transform.position;
            pos += Vector2.down * (fallingSpeed / DataManager.Instance.fallingSpeedSec) * Time.deltaTime;
            transform.position = pos;
            yield return null;


            //Checking overlap with a boost up.
            Collider2D boostUpCollider = Physics2D.OverlapCircle(transform.position, spriteRenderer.bounds.extents.x, boostUpLayerMask);
            if (boostUpCollider != null)
            {
                //ServicesManager.Instance.SoundManager.PlaySound(ServicesManager.Instance.SoundManager.ballExplode);
                DisableObject();
            }


            //Check vidible.
            if (!isVisible)
            {
                Vector2 currentPos = (Vector2)transform.position + Vector2.down * (spriteRenderer.bounds.extents.y);
                float y = Camera.main.WorldToViewportPoint(currentPos).y;
                if (y < 0.99f)
                {
                    isVisible = true;
                }
            }


            Vector2 checkPos = (Vector2)transform.position + Vector2.up * spriteRenderer.bounds.extents.y;
            if (checkPos.y < -20.0f)
            {
                if (Gamemanager.i.gameMode == Gamemanager.GAMEMODE.ORIGINAL)
                {
                    Gamemanager.i.hud.getHp(int.Parse(numberText.text.Trim()));
                    DisableObject();
                    yield break;
                }
                else
                {


                }
            }

        }
    }



    /// <summary>
    /// 볼 데미지 체크
    /// </summary>
    public void HandleOnHitByBullet()
    {
        if (isArmor) 
        {
            if ((currentArmorValue - ((3.2f / armorValue) * DataManager.Instance.attack)) > 0.0f)
            {
                v = this.transform.localScale;
                v.y = 0.7f;
                v.x = currentArmorValue - ((3.2f / armorValue) * DataManager.Instance.attack);
                currentArmorValue = v.x;
                Debug.Log(v.x);
                armor.transform.localScale = v;

                return;
            }
            else
            {
                isArmor = false;
                v = this.transform.localScale;
                v.y = 0.7f;
                v.x = 0.0f;
                currentArmorValue = v.x;
                armor.transform.localScale = v;
            }
            
        }


        if ((int.Parse(numberText.text) - DataManager.Instance.attack) > 0)
        {

            if ((int.Parse(numberText.text) - DataManager.Instance.attack) < DataManager.Instance.minBallNumber)
            {
                spriteRenderer.color = new Color(1, 1, 1);
                int newNumber = int.Parse(numberText.text) - DataManager.Instance.attack;

                numberText.text = newNumber.ToString();
                Invoke("setColor", 0.05f);

            }
            else
            {


                spriteRenderer.color = new Color(1, 1, 1);
                int newNumber = int.Parse(numberText.text) - DataManager.Instance.attack;

                float scaleFactor = (1 - DataManager.Instance.MinBallScale) / DataManager.Instance.maxBallNumber;
                float scale = (newNumber == 1) ? DataManager.Instance.MinBallScale : (scaleFactor * newNumber) + DataManager.Instance.MinBallScale;

                transform.localScale = new Vector3(scale, scale, 1);
                
                Vector3 v = this.transform.position;
                v.x = 0.0f;
                transform.position = v;

                numberText.text = newNumber.ToString();
                Invoke("setColor", 0.05f);

            }
        }
        else
        {
            DisableObject();
            //BossManager.i.CreateItem(transform.position);
            BossManager.i.gameoverGame();
        }
    }

    public void setColor()
    {
        spriteRenderer.color = new Color(0.84f, 0, 0.63f);
    } 

    /// <summary>
    /// Stop this ball from falling.
    /// </summary>
    public void StopFalling()
    {
        StopAllCoroutines();
        if (IsStillOverlap)
        {
            spriteRenderer.enabled = true;
            Collider2D.enabled = true;
            gameObject.SetActive(false);
        }
    }
    */
}
