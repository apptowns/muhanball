using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Security.Cryptography; 

[System.Serializable]
public class SaveData
{
	public int id;              // 위치를 알려주는 몇페이지에 있는지
	public string uid;          // 기계 아이디

	public int currentPoint;	//현재포인트
	public int lastPoint;		//직전		
	public int bestPoint;		//최고

	public string createDate;
	public string lastSaveDate;
	public string lastPlayDate;

	public bool isfx;
	public bool ismusic;

	//얼마에요?
	public int coin;            // 코인
	public int dia;             // 다이아
	public int misale;          // 미사일
	public int bomb;            // 폭탄
	public int lazer;           // 레이저
	 

	public int[] attackLevel;   // 공격레벨 0.공격력 1.연사 2.체력 3.크리티컬 4.미사일 능력 5.폭탄 능력 6.레이저 능력

	public int dbonus;          // 연속 언제 받았냐? 데일리 보너스

	public int stagePlay;       // 직전 플레이 하던 스테이지
	public int stageMax;        // 최대 진행 스테이지
	public int endBoss;

	public int wepon;           //무기	
	public int[] weponUp;       //수량 
	public int[] weponDown;     //세트 

	public int[] AchieveUp;     //업적 수량  
	public int[] AchieveDown;   //업적 세트
	
	public bool[] bossCrear;    //보스 클리어 10~200
	public bool[] present;      //택배함 -> 선물리스트 

	public const string LOCAL_SAVE = "saveData";
	public const string LOCAL_SAVE_HASH = "hashKey";

	public const string dateFormat = "dd/MM/yyyy hh:mm:ss";

	static public SaveData LoadData()
	{
		SaveData saveData;

		string jsonSaveData = PlayerPrefs.GetString(LOCAL_SAVE);
		//Debug.Log(jsonSaveData);
		if (jsonSaveData.Equals(String.Empty))
		{
			saveData = CreateSaveData();

			jsonSaveData = JsonUtility.ToJson(saveData);
			PlayerPrefs.SetString("saveData", jsonSaveData);
		}
		else
		{
			saveData = JsonUtility.FromJson<SaveData>(jsonSaveData);
		}
		return saveData;
	}

	static public SaveData DeleteSaveData()
	{
		PlayerPrefs.DeleteKey(LOCAL_SAVE);
		return LoadData();
	}

	public void Save()
	{
		PlayerPrefs.SetString(LOCAL_SAVE, JsonUtility.ToJson(this));
	}

	static public SaveData CreateSaveData()
	{
		SaveData saveData;

		saveData = new SaveData();
		saveData.id = 0;												// 위치를 알려주는 몇페이지에 있는지
		saveData.uid = SystemInfo.deviceUniqueIdentifier.ToString();    //  기계 아이디

		// 날짜 정보 관련 (21.10.20 추가)
		saveData.createDate = DateTime.UtcNow.ToString(dateFormat);
		saveData.lastSaveDate = default(DateTime).ToString();                               // default(DateTime): 01/01/0001 12:00:00		

		saveData.isfx		= true;
		saveData.ismusic	= true;

		saveData.coin		= 0;			// 코인
		saveData.dia		= 5;            // 다이아
		saveData.misale		= 1;            // 미사일
		saveData.bomb		= 1;            // 폭탄
		saveData.lazer		= 1;            // 레이저

		saveData.attackLevel = new int[7];
		for (int i = 0; i < saveData.attackLevel.Length; i++)
		{
			saveData.attackLevel[i] = 1;
		}

		saveData.dbonus		= 1;			// 연속 언제 받았냐? 데일리 보너스

		saveData.stagePlay	= 1;			// 직전 플레이 하던 스테이지
		saveData.stageMax	= 1;			// 최대 진행 스테이지
		saveData.endBoss	= 0;
		saveData.wepon		= 0;			//무기	
		saveData.weponUp	= new int[24];  //수량 
		saveData.weponDown	= new int[24];  //세트
		for (int i = 0; i < saveData.weponUp.Length; i++)
		{
			saveData.weponUp[i] = 0;
			saveData.weponDown[i] = 0;
		}

		saveData.weponUp[0] = 1;

		saveData.AchieveUp = new int[10];     //업적 수량
		saveData.AchieveDown = new int[10];   //업적 세트
		for (int i = 0; i < saveData.AchieveUp.Length; i++)
		{
			saveData.AchieveUp[i] = 0;
			saveData.AchieveDown[i] = 0;
		}

		saveData.bossCrear = new bool[20];
		for (int i = 0; i< saveData.bossCrear.Length; i++)
		{
			saveData.bossCrear[i] = false;
		}

		saveData.present = new bool[5];
		for (int i = 0; i < saveData.present.Length; i++)
		{
			saveData.present[i] = false;
		}

		return saveData;
	}  
}