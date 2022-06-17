using System.Collections;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer = null;
    [SerializeField] private LayerMask ballLayerMask = new LayerMask();


    /// <summary>
    /// Setup this bullet.
    /// </summary>
    /// <param name="movingDir"></param>
    /// <param name="speed"></param>
    public void OnSetup(Vector2 movingDir)
    {
        StartCoroutine(CRMoving(movingDir));
    }


    /// <summary>
    /// Falling this ball down.
    /// </summary>
    /// <param name="dir"></param>
    /// <param name="speed"></param>
    /// <returns></returns>
    IEnumerator CRMoving(Vector2 dir)
    {
        //Debug.Log("bul");
        float speed = DataManager.Instance.bulletSpeed *0.8f; 
        while (gameObject.activeInHierarchy)
        {
            Vector2 pos = transform.position;
            pos += dir * speed * Time.deltaTime;
            transform.position = pos;
            yield return null;

            //총알 적용 범위
            Vector2 viewportPos = Camera.main.WorldToViewportPoint(transform.position);
            if (viewportPos.x > 1.05f || viewportPos.x < -0.05f || viewportPos.y > 0.99f || viewportPos.y < -0.05f)
            {
                gameObject.SetActive(false);
                yield break;
            }

            //볼에 맞을 경우 사라진다.
            Collider2D collider2D = Physics2D.OverlapCircle(transform.position, spriteRenderer.bounds.extents.x, ballLayerMask);
            if (collider2D != null)
            {
                //Hit a ball
                //ServicesManager.Instance.SoundManager.PlaySound(ServicesManager.Instance.SoundManager.bulletExplode);
                Vector3 lookDirection = (collider2D.transform.position - transform.position).normalized;
                Vector2 contactPos = transform.position + lookDirection * spriteRenderer.bounds.extents.x;

                //이펙트
                EffectManager.Instance.PlayBulletExplodeEffect(contactPos, lookDirection);
                collider2D.GetComponent<BallController>().HandleOnHitByBullet();

                /*
                switch (DataManager.Instance.getScene()) 
                {
                    case "GAME":
                        collider2D.GetComponent<BallController>().HandleOnHitByBullet();
                        break;
                }
               */
                gameObject.SetActive(false);
                yield break;
            }
        }
    }
}

