using UnityEngine;

public class ItemsManager : MonoBehaviour
{
    

    public static ItemsManager Instance { private set; get; }

    public int Missiles
    {
        get { return DataManager.Instance.getMissale(); }
    }

    public int Bombs
    {
        get { return DataManager.Instance.getBomb(); }
    }

    public int Lasers
    {
        get { return DataManager.Instance.getLazer(); }
    }






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

}