using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OkManager : MonoBehaviour
{
    public static OkManager i;
    public int id;
    public Text mes;

    string[] mesList = { "게임을 종료 하시겠습니까?", "홈화면으로 이동합니다.","저장된 데이터를 초기화 합니다.","데이터를 저장하시겠습니까?", "칼라를 표시하겠습니까?" };

    private void Start()
    {
        i = this;
        offPanel();
    }

    public void onPanel(int _id)
    {
        SoundManager.Instance.play(12);
        id = _id;
        mes.text =mesList[id];
        this.gameObject.SetActive(true);
    }

    public void clickOk()
    {
        SoundManager.Instance.play(2);

        switch (id) 
        {
            case 0:
                Application.Quit();
                break;
            case 1:
                ButtonManager.i.goTitle();
                break;
            case 2:
                PlayerPrefs.DeleteAll();
                Application.Quit();
                break;
            case 3:
                DataManager.Instance.Save();
                offPanel();
                break;
            case 4:
                DataManager.Instance.Save();

                if (DataManager.Instance.isColorText)
                    DataManager.Instance.isColorText = false;
                else
                    DataManager.Instance.isColorText = true;

                offPanel();
                break;
        }    
    }


    public void offPanel()
    {
        this.gameObject.SetActive(false);
    }
}
