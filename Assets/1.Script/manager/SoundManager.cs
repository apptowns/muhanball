using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
	//싱글톤
	private static SoundManager instance;
	public static SoundManager Instance 
	{
		get 
		{
			if (instance == null) 
			{
				GameObject newObject = new GameObject("_SoundManager"); 
				instance = newObject.AddComponent<SoundManager> ();
				if (instance == null)
					Debug.LogError ("SoundManager null");
			}
			return instance;
		}
	}

	public List<AudioClip> fxSound;

	public bool comprete()
	{	
		Debug.Log ("SoundManager is start");
		soundSet ();
		DontDestroyOnLoad (this);
		return true;
	}

	void soundSet()
	{
		this.gameObject.AddComponent<AudioSource>();

		fxSound = new List<AudioClip>();

		// 긍정, 부정AWP_Impact
		/*0*/	fxSound.Add(Resources.Load("Audio/ballExplode") as AudioClip); // 조각 폭파\
		/*1*/	fxSound.Add(Resources.Load("Audio/bulletExplode") as AudioClip); // 조각 폭파
		/*2*/	fxSound.Add(Resources.Load("Audio/button") as AudioClip); // 조각 폭파
		/*3*/	fxSound.Add(Resources.Load("Audio/collectBoostUp") as AudioClip); // 조각 폭파\
		/*4*/	fxSound.Add(Resources.Load("Audio/collectCoin") as AudioClip); // 조각 폭파
		/*5*/	fxSound.Add(Resources.Load("Audio/enableHiddenGuns") as AudioClip); // 조각 폭파
		/*6*/	fxSound.Add(Resources.Load("Audio/levelCompleted") as AudioClip); // 조각 폭파
		/*7*/	fxSound.Add(Resources.Load("Audio/levelFailed") as AudioClip); // 조각 폭파
		/*8*/	fxSound.Add(Resources.Load("Audio/missileExplode") as AudioClip); // 조각 폭파
		/*9*/	fxSound.Add(Resources.Load("Audio/missileExplode") as AudioClip); // 조각 폭파
		/*10*/	fxSound.Add(Resources.Load("Audio/playerDied") as AudioClip); // 조각 폭파
		/*11*/	fxSound.Add(Resources.Load("Audio/rewarded") as AudioClip); // 조각 폭파
		/*12*/	fxSound.Add(Resources.Load("Audio/tick") as AudioClip); // 조각 폭파
		/*13*/	fxSound.Add(Resources.Load("Audio/unlock") as AudioClip); // 조각 폭파 
	}

	public void play(int _no)
	{
		if (DataManager.Instance.saveData.isfx)
		{
			this.GetComponent<AudioSource>().PlayOneShot(fxSound[_no]);
			this.GetComponent<AudioSource>().Play();
		}
	}

}
