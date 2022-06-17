using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public static TitleManager i;
    public int loadingbarvalue;

    public enum TITLEMODE
    {
        FIRST,
        dellFIRST,
        reLOAD
    }

    public TITLEMODE titleMode;

    public SpriteAtlas m_Atals = null;

    public GameObject stagepos;
    public GameObject[] stage;
    public Slider loadingbar;

    public Image music;
    public Text ver;

    // float[] pos = { 100, 200, 50, 150, -55, 200, 320, 155, -122, -446, -338, -234, -155, 102, -4, -258, 109, -102, 04, 158 };

    public GameObject logo;
    public GameObject questButton;
    public GameObject loginButton;
    public ScrollRect scroll;

    public EtcManager Etc;
    public OpenControl fade;

    public AudioClip TitleMusic;

    private void Awake()
    {
        i = this;
        fade.Fadeon();
        titleMode = TITLEMODE.FIRST;
    }


    // Start is called before the first frame update
    void Start()
    {
        setTitle();
    }
    
    public int getEtc() 
    {
        //중요한 새글이 있다면..

        //데일리 보너스 안했다면

        //스테이지 화면 -> 조선이 없으면 항상 이화면 위주로 뜬다.    

        return 2;
    
    }

    public void setTitle()
    {
        //스타트 버튼
        ver.text =Application.version.ToString();

        Debug.Log("처음 들어 왔어"); 

        //그냥 다시 오면... 로고 화면 없어진다.
        Etc.onEtc(getEtc());

        onLost();
    }

    public void login()
    {
        DataManager.Instance.isFirst = true; //타이틀 들렸어... 
        onLost();
    }

    public void quest() //게스트 버튼
    {
        DataManager.Instance.isFirst = true; //타이틀 들렸어... 
        onLost();
    }
    void onLost() 
    {
        SoundManager.Instance.play(2);
        if (!DataManager.Instance.isFirst) 
        {
            loginButton.GetComponent<Button>().interactable = false;
            questButton.SetActive(true);
            logo.SetActive(true);


        }
        else
        {
            loginButton.GetComponent<Button>().interactable = false;
            questButton.SetActive(false);
            logo.SetActive(false);

        }
    }
     

    public void goGame()
    {
        SceneManager.LoadScene("GAME");
    }
}
