using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostUpController : MonoBehaviour
{
    //폭탄은 연사 속도 
    //미사일은 데미지  
    //레이저도 데미지
    //체력은 회복 수치


    [SerializeField] private BoostUpType boostUpType = BoostUpType.BOMB;
    [SerializeField] private SpriteRenderer spriteRenderer = null;
    public BulletController bulletPrefab;

    public int CheckType() 
    {
        if (boostUpType == BoostUpType.MISSILE)                 //미사일이면
        {
            Debug.Log("나는 미사일이야! "+DataManager.Instance.getMissaleLevel());
            return DataManager.Instance.getMissaleLevel();
        }
        else if (boostUpType == BoostUpType.LASER)
        {
            Debug.Log("나는 레이저!"+ DataManager.Instance.getLazerLevel());
            return DataManager.Instance.getLazerLevel();        //레이저면
        }
        else 
        {
            return 0;
        }
    }
    /// <summary>
    /// Set up this boost up.
    /// </summary>
    public void OnSetup()
    {
        Debug.Log("item set");
        if (boostUpType == BoostUpType.BOMB)
        {
            Debug.Log("나는 폭탄이야!");
            List<Vector2> listDirection = new List<Vector2>();

            listDirection.Add(Vector2.up);
            listDirection.Add(Vector2.left);
            listDirection.Add(Vector2.right);
            listDirection.Add(Vector2.up + Vector2.left);
            listDirection.Add(Vector2.up + Vector2.right);
            listDirection.Add(Vector2.down + Vector2.left);
            listDirection.Add(Vector2.down + Vector2.right);

            StartCoroutine(CRFiring(listDirection));
            StartCoroutine(CRRotating());
        }
        else if (boostUpType == BoostUpType.LASER)
        {
            transform.localScale = new Vector3(10f, 1.5f, 1f);
        }

        StartCoroutine(CRMovingUp());
    }


    /// <summary>
    /// Moving this object up.
    /// Should be use for all boost up.
    /// </summary>
    /// <returns></returns>
    IEnumerator CRMovingUp()
    {
        float speed = 0;
        if (boostUpType == BoostUpType.BOMB)
            speed = DataManager.Instance.laserMovingUpSpeed/3;
        else if (boostUpType == BoostUpType.LASER)
            speed = DataManager.Instance.laserMovingUpSpeed;
        else if (boostUpType == BoostUpType.MISSILE)
            speed =DataManager.Instance.missileMovingUpSpeed;

        while (gameObject.activeInHierarchy)
        {
            Vector2 pos = transform.position;
            pos += Vector2.up * speed * Time.deltaTime;
            transform.position = pos;
            yield return null;

            //모드가 아니면... 
            if (Gamemanager.i.gameState != Gamemanager.GAMESTATE.GAME)
            {
                gameObject.SetActive(false);
                yield break;
            }

            //선을 넘으면.. 
            float topLimit = Camera.main.ViewportToWorldPoint(new Vector2(0.5f, 1f)).y;
            float checkPos = ((Vector2)transform.position + Vector2.down * spriteRenderer.bounds.extents.y).y;
            if (checkPos >= topLimit)
            {
                GameObject[] ball = GameObject.FindGameObjectsWithTag("Ball");
                for (int i=0; i<ball.Length; i++) 
                {
                    ball[i].GetComponent<BallController>().isCall = false;
                
                }
                gameObject.SetActive(false);
                yield break;
            }
        }
    }




    /// <summary>
    /// Firing bullet.
    /// This should be only for bomb.
    /// </summary>
    /// <returns></returns>
    private IEnumerator CRFiring(List<Vector2> directions)
    {
        while (gameObject.activeInHierarchy)
        {
            foreach (Vector2 o in directions)
            {
                BulletController bulletControl = Instantiate(bulletPrefab, Vector3.zero, Quaternion.identity);
                bulletControl.transform.position = transform.position;
                bulletControl.gameObject.SetActive(true);
                bulletControl.OnSetup(o);
            }

            //폭탄 연사 속도
            yield return new WaitForSeconds(DataManager.Instance.getBombLevel());
        }
    }


    /// <summary>
    /// Rotating.
    /// This should be only for bomb.
    /// </summary>
    /// <returns></returns>
    private IEnumerator CRRotating()
    {
        while (gameObject.activeInHierarchy)
        {
            transform.eulerAngles += new Vector3(0, 0, 150f * Time.deltaTime);
            yield return null;
        }
    }
}