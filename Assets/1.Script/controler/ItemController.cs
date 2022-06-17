using System.Collections;
using UnityEngine;


public class ItemController : MonoBehaviour
{

    [SerializeField] private ItemType itemType = ItemType.COIN;
    [SerializeField] private LayerMask playerLayerMask = new LayerMask();
    [SerializeField] private SpriteRenderer spriteRenderer = null;

    public ItemType ItemType { get { return itemType; } }

    private float fallingSpeed = 0; 

    public void OnSetup()
    {
        fallingSpeed = DataManager.Instance.fallingSpeed; //.BallFallingSpeed;

        StartCoroutine(CRFalling());
        
        if (itemType == ItemType.COIN)
        {
            StartCoroutine(CRRotating()); //회전 
        }
        else
        {
            StartCoroutine(CRFading());//깜박.. 
        }
    }


    /// <summary>
    /// Increase the falling speed of this item.
    /// </summary>
    public void IncreaseFallingSpeed()
    {
        fallingSpeed = 10f;
    }



    /// <summary>
    /// Falling this item down.
    /// </summary>
    /// <param name="speed"></param>
    /// <returns></returns>
    IEnumerator CRFalling()
    {
        while (gameObject.activeInHierarchy)
        {  
            Vector2 pos = transform.position;
            pos += Vector2.down * fallingSpeed * Time.deltaTime;
            transform.position = pos;
            yield return null;
           
            //Check and disable this item
            Vector2 checkPos = (Vector2)transform.position + Vector2.up * spriteRenderer.bounds.extents.y;
            if (checkPos.y < PlayerController.Instance.limitBottom)
            {
                gameObject.SetActive(false);
                yield break;
            }

            //Check hitting the player
            if (Physics2D.OverlapCircle(transform.position, spriteRenderer.bounds.extents.x, playerLayerMask) != null)
            {
                if (itemType == ItemType.COIN)
                {
                    EffectManager.Instance.PlayCollectCoinEffect(transform.position);
                    SoundManager.Instance.play(4);
                    Gamemanager.i.CreateMoveItem(100,transform.position);
                    //DataManager.Instance.setCoin(DataManager.Instance.getCoin()+(DataManager.Instance.getstageID() + 1));
                }

                else if (itemType == ItemType.HIDDEN_GUNS)
                {                    
                    EffectManager.Instance.PlayCollectHiddenGunsEffect(transform.position);
                    SoundManager.Instance.play(5);
                    PlayerController.Instance.HandleCollectHiddenGunsItem();
                }

                else
                {
                    SoundManager.Instance.play(3);

                    if (itemType == ItemType.DIA)
                    {
                        EffectManager.Instance.PlayCollectMissileEffect(transform.position);
                        Gamemanager.i.CreateMoveItem(1, transform.position);
                        //DataManager.Instance.setMissle(DataManager.Instance.//getMissale()+1);
                    }
                    else if (itemType == ItemType.MISSILE)
                    {
                        EffectManager.Instance.PlayCollectMissileEffect(transform.position);
                        Gamemanager.i.CreateMoveItem(2, transform.position);
                        //DataManager.Instance.setMissle(DataManager.Instance.//getMissale()+1);
                    }
                    else if (itemType == ItemType.BOMB)
                    {
                        EffectManager.Instance.PlayCollectBombEffect(transform.position);
                        Gamemanager.i.CreateMoveItem(3, transform.position);
                        //DataManager.Instance.setBomb(DataManager.Instance.getBomb()+1);
                    }

                    else if (itemType == ItemType.LASER)
                    {
                        EffectManager.Instance.PlayCollectLaserEffect(transform.position);
                        Gamemanager.i.CreateMoveItem(4, transform.position);
                        //DataManager.Instance.setLazer(DataManager.Instance.getLazer()+ 1);
                    }

                    else if (itemType == ItemType.HP)
                    {
                        EffectManager.Instance.PlayCollectLaserEffect(transform.position);
                        Gamemanager.i.CreateMoveItem(5, transform.position);
                        //DataManager.Instance.setLazer(DataManager.Instance.getLazer()+ 1);
                    }
                }


                gameObject.SetActive(false);
            }
        }
    }


    /// <summary>
    /// Rotating this item. 
    /// </summary>
    /// <returns></returns>
    private IEnumerator CRRotating()
    {
        while (gameObject.activeInHierarchy)
        {
            transform.localEulerAngles += Vector3.forward * 100f * Time.deltaTime;
            yield return null;
        }
    }

    /// <summary>
    /// Fading in and out of the SpriteRenderer component.
    /// </summary>
    /// <returns></returns>
    private IEnumerator CRFading()
    {
        float t = 0;
        float fadingTime = 0.5f;
        //Color startColor = spriteRenderer.color;
        //startColor.a = 1;
        //spriteRenderer.color = startColor;
       // Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0);
        while (gameObject.activeInHierarchy)
        {
            t = 0;
            while (t < fadingTime)
            {
                t += Time.deltaTime;
                float factor = t / fadingTime;
                //spriteRenderer.color = Color.Lerp(startColor, endColor, factor);
                yield return null;
            }

            t = 0;
            while (t < fadingTime)
            {
                t += Time.deltaTime;
                float factor = t / fadingTime;
                //spriteRenderer.color = Color.Lerp(endColor, startColor, factor);
                yield return null;
            }
        }
    }
}
