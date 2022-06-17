using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance { private set; get; }

    [SerializeField] private ParticleSystem ballExplodeEffectPrefab = null;
    [SerializeField] private ParticleSystem bulletExplodeEffectPrefab = null;
    [SerializeField] private ParticleSystem collectCoinEffectPrefab = null;
    [SerializeField] private ParticleSystem collectHiddenGunsEffectPrefab = null;
    [SerializeField] private ParticleSystem collectMissileEffectPrefab = null;
    [SerializeField] private ParticleSystem collectBombEffectPrefab = null;
    [SerializeField] private ParticleSystem collectLaserEffectPrefab = null;


    private List<ParticleSystem> listBallExplodeEffect = new List<ParticleSystem>();
    private List<ParticleSystem> listBulletExplodeEffect = new List<ParticleSystem>();
    private List<ParticleSystem> listCollectCoinEffect = new List<ParticleSystem>();
    private List<ParticleSystem> listCollectHiddenGunsEffect = new List<ParticleSystem>();
    private List<ParticleSystem> listCollectMissileEffect = new List<ParticleSystem>();
    private List<ParticleSystem> listCollectBombEffect = new List<ParticleSystem>();
    private List<ParticleSystem> listCollectLaserEffect = new List<ParticleSystem>();
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


    /// <summary>
    /// Play the given particle then disable it 
    /// </summary>
    /// <param name="par"></param>
    /// <returns></returns>
    private IEnumerator CRPlayParticle(ParticleSystem par)
    {
        par.Play();
        yield return new WaitForSeconds(2f);
        par.gameObject.SetActive(false);
    }



    /// <summary>
    /// Play a bullet explode effect at given position.
    /// </summary>
    /// <param name="pos"></param>
    public void PlayBulletExplodeEffect(Vector2 pos, Vector2 lookDir)
    {
        //Find in the list
        ParticleSystem planeExplode = listBulletExplodeEffect.Where(a => !a.gameObject.activeInHierarchy).FirstOrDefault();

        if (planeExplode == null)
        {
            //Didn't find one -> create new one
            planeExplode = Instantiate(bulletExplodeEffectPrefab, pos, Quaternion.identity);
            planeExplode.gameObject.SetActive(false);
            listBulletExplodeEffect.Add(planeExplode);
        }

        planeExplode.transform.position = pos;
        planeExplode.transform.up = -lookDir;
        planeExplode.gameObject.SetActive(true);
        StartCoroutine(CRPlayParticle(planeExplode));
    }



    /// <summary>
    /// Play a ball explode effect at given position.
    /// </summary>
    /// <param name="pos"></param>
    public void PlayBallExplodeEffect(Vector2 pos, Color color)
    {
        //Find in the list
        ParticleSystem ballExplode = listBallExplodeEffect.Where(a => !a.gameObject.activeInHierarchy).FirstOrDefault();

        if (ballExplode == null)
        {
            //Didn't find one -> create new one
            ballExplode = Instantiate(ballExplodeEffectPrefab, pos, Quaternion.identity);
            ballExplode.gameObject.SetActive(false);
            listBallExplodeEffect.Add(ballExplode);
        }

        ballExplode.transform.position = pos;
        var main = ballExplode.main;
        main.startColor = color;
        ballExplode.gameObject.SetActive(true);
        StartCoroutine(CRPlayParticle(ballExplode));
    }



    /// <summary>
    /// Play a collect coin effect at given position.
    /// </summary>
    /// <param name="pos"></param>
    public void PlayCollectCoinEffect(Vector2 pos)
    {
        //Find in the list
        ParticleSystem collectCoinEffect = listCollectCoinEffect.Where(a => !a.gameObject.activeInHierarchy).FirstOrDefault();

        if (collectCoinEffect == null)
        {
            //Didn't find one -> create new one
            collectCoinEffect = Instantiate(collectCoinEffectPrefab, pos, Quaternion.identity);
            collectCoinEffect.gameObject.SetActive(false);
            listCollectCoinEffect.Add(collectCoinEffect);
        }

        collectCoinEffect.transform.position = pos;
        collectCoinEffect.gameObject.SetActive(true);
        StartCoroutine(CRPlayParticle(collectCoinEffect));
    }




    /// <summary>
    /// Play a collect hidden guns effect at given position.
    /// </summary>
    /// <param name="pos"></param>
    public void PlayCollectHiddenGunsEffect(Vector2 pos)
    {
        //Find in the list
        ParticleSystem collectHiddenGunsEffect = listCollectHiddenGunsEffect.Where(a => !a.gameObject.activeInHierarchy).FirstOrDefault();

        if (collectHiddenGunsEffect == null)
        {
            //Didn't find one -> create new one
            collectHiddenGunsEffect = Instantiate(collectHiddenGunsEffectPrefab, pos, Quaternion.identity);
            collectHiddenGunsEffect.gameObject.SetActive(false);
            listCollectHiddenGunsEffect.Add(collectHiddenGunsEffect);
        }

        collectHiddenGunsEffect.transform.position = pos;
        collectHiddenGunsEffect.gameObject.SetActive(true);
        StartCoroutine(CRPlayParticle(collectHiddenGunsEffect));
    }



    /// <summary>
    /// Play a collect missile effect at given position.
    /// </summary>
    /// <param name="pos"></param>
    public void PlayCollectMissileEffect(Vector2 pos)
    {
        //Find in the list
        ParticleSystem collectMissileEffect = listCollectMissileEffect.Where(a => !a.gameObject.activeInHierarchy).FirstOrDefault();

        if (collectMissileEffect == null)
        {
            //Didn't find one -> create new one
            collectMissileEffect = Instantiate(collectMissileEffectPrefab, pos, Quaternion.identity);
            collectMissileEffect.gameObject.SetActive(false);
            listCollectMissileEffect.Add(collectMissileEffect);
        }

        collectMissileEffect.transform.position = pos;
        collectMissileEffect.gameObject.SetActive(true);
        StartCoroutine(CRPlayParticle(collectMissileEffect));
    }


    /// <summary>
    /// Play a collect bomb effect at given position.
    /// </summary>
    /// <param name="pos"></param>
    public void PlayCollectBombEffect(Vector2 pos)
    {
        //Find in the list
        ParticleSystem collectBombEffect = listCollectBombEffect.Where(a => !a.gameObject.activeInHierarchy).FirstOrDefault();

        if (collectBombEffect == null)
        {
            //Didn't find one -> create new one
            collectBombEffect = Instantiate(collectBombEffectPrefab, pos, Quaternion.identity);
            collectBombEffect.gameObject.SetActive(false);
            listCollectBombEffect.Add(collectBombEffect);
        }

        collectBombEffect.transform.position = pos;
        collectBombEffect.gameObject.SetActive(true);
        StartCoroutine(CRPlayParticle(collectBombEffect));
    }


    /// <summary>
    /// Play a collect laser effect at given position.
    /// </summary>
    /// <param name="pos"></param>
    public void PlayCollectLaserEffect(Vector2 pos)
    {
        //Find in the list
        ParticleSystem collectLaserEffect = listCollectLaserEffect.Where(a => !a.gameObject.activeInHierarchy).FirstOrDefault();

        if (collectLaserEffect == null)
        {
            //Didn't find one -> create new one
            collectLaserEffect = Instantiate(collectLaserEffectPrefab, pos, Quaternion.identity);
            collectLaserEffect.gameObject.SetActive(false);
            listCollectLaserEffect.Add(collectLaserEffect);
        }

        collectLaserEffect.transform.position = pos;
        collectLaserEffect.gameObject.SetActive(true);
        StartCoroutine(CRPlayParticle(collectLaserEffect));
    }
}