using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
	public static ButtonManager i;
	public SpriteAtlas m_Atals = null;

	public Image m_musicIcon;
	public Image m_fxIcon;


	//ulletPrefab.GetComponent<SpriteRenderer>().sprite = m_Atals.GetSprite("bullet_" + DataManager.Instance.getWepon().ToString());


	private void Start()
	{
		i = this;

		if (DataManager.Instance.saveData.ismusic)
			m_musicIcon.sprite = m_Atals.GetSprite("Icon_PictoIcon_Music_on");
		else
			m_musicIcon.sprite = m_Atals.GetSprite("Icon_PictoIcon_Music_off");

		if (DataManager.Instance.saveData.isfx)
			m_fxIcon.sprite = m_Atals.GetSprite("Icon_PictoIcon_Sound_on");
		else
			m_fxIcon.sprite = m_Atals.GetSprite("Icon_PictoIcon_Sound_off");

		m_coin.text = DataManager.Instance.getCoin().ToString();
		m_dia.text = DataManager.Instance.getDia().ToString();
		m_stage.text = DataManager.Instance.getStagePlay().ToString();
	}

	public void goLogo()
	{
		SceneManager.LoadScene("LOGO");
	}


	//타이틀 씬 이동
	public void goTitle()
	{
		SceneManager.LoadScene("TITLE");
	}

	//게임 씬 이동
	public void goGame()
	{
		DataManager.Instance.Save();
		SceneManager.LoadScene("GAME");
	}

	public void Reset()
	{
		DataManager.Instance.dell();
		OkManager.i.onPanel(1);
		Debug.Log("dell");
	}


	public void nomalButtron(int _id)
	{
		SoundManager.Instance.play(2);

		switch (_id)
		{
			case 0: // sound
				if (DataManager.Instance.saveData.ismusic)
				{
					DataManager.Instance.saveData.ismusic = false;
					m_musicIcon.sprite = m_Atals.GetSprite("Icon_PictoIcon_Music_off");
					GameObject.Find("background").gameObject.GetComponent<AudioSource>().mute = true;
				}
				else
				{
					DataManager.Instance.saveData.ismusic = true;
					m_musicIcon.sprite = m_Atals.GetSprite("Icon_PictoIcon_Music_on");
					GameObject.Find("background").gameObject.GetComponent<AudioSource>().mute = false;
				}
				break;

			case 1: // fx
				if (DataManager.Instance.saveData.isfx)
				{
					DataManager.Instance.saveData.isfx = false;
					m_fxIcon.sprite = m_Atals.GetSprite("Icon_PictoIcon_Sound_off");
				}
				else
				{
					DataManager.Instance.saveData.isfx = true;
					m_fxIcon.sprite = m_Atals.GetSprite("Icon_PictoIcon_Sound_on");
				}
				break;

			case 2: // load
				break;

			case 3: // save
				OkManager.i.onPanel(3);
				break;

			case 4: // dell 
				OkManager.i.onPanel(2);
				break;

			case 5: // title 
				OkManager.i.onPanel(1);
				break;

			case 6: // exit 
				OkManager.i.onPanel(0);
				break;
			case 7: // color
				OkManager.i.onPanel(4);
				break;
		}
	}

	// coin 
	public Text m_coin;
	public void leftCoin()
	{
		DataManager.Instance.setCoin(DataManager.Instance.getCoin() -100);
		m_coin.text = DataManager.Instance.getCoin().ToString();
	}

	public void rightCoint()
	{
		DataManager.Instance.setCoin(DataManager.Instance.getCoin() + 100);
		m_coin.text = DataManager.Instance.getCoin().ToString();
	}

	public Text m_dia;
	public void leftDia()
	{
		DataManager.Instance.setDia(DataManager.Instance.getDia() - 100);
		m_dia.text = DataManager.Instance.getDia().ToString();
	}

	public void rightDia()
	{
		DataManager.Instance.setDia(DataManager.Instance.getDia() + 100);
		m_dia.text = DataManager.Instance.getDia().ToString();
	}

	public Text m_stage;
	public void leftStage()
	{
		int a = DataManager.Instance.getStagePlay();
		DataManager.Instance.setStagePlay(a - 10);

		m_stage.text = DataManager.Instance.getStagePlay().ToString();
	}

	public void rightStage()
	{
		int a = DataManager.Instance.getStagePlay();
		DataManager.Instance.setStagePlay(a + 10);

		m_stage.text = DataManager.Instance.getStagePlay().ToString();
	}

	public void geGame() 
	{
		nomalButtron(5);
	}
}
