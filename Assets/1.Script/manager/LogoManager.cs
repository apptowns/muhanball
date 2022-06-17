using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class LogoManager : MonoBehaviour
{
    public  static LogoManager i;	

	//loadingbar
    public Slider loadingBar;
    public OpenControl fade;
	public string[] url;        // 웹주소
	public string[] webData;    // 담아놓을 데이터 
	const int checkCount = 150;
	int currentCount;
	const int listCount = 6;

	public Text mes;

	void Start()
    {
        i = this;

        loadingBar.minValue = 0;
        loadingBar.maxValue= 1000;
        loadingBar.value = 0;

		DataManager.Instance.comprete();
		SoundManager.Instance.comprete();

        fade.Fadeon();
        StartCoroutine(SceneMove());
    } 

    IEnumerator SceneMove() 
    {
		currentCount = 0;

        while (loadingBar.value < loadingBar.maxValue) 
        {
			if (loadingBar.value < currentCount)
			{
				loadingBar.value += 1;
			}

            yield return null;
        }

        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene("TITLE");

    }  
}