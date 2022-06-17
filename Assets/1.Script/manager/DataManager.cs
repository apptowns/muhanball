using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

[System.Serializable]
public class Local 
{
	public int id;
	public string[] local;
}

public class DataManager : MonoBehaviour
{
	private static DataManager instance;
	public static DataManager Instance
	{
		get
		{
			if (instance == null)
			{
				GameObject newObject = new GameObject("_DataManager");
				instance = newObject.AddComponent<DataManager>();
				if (instance == null)
					Debug.LogError("DataManager null");
			}
			return instance;
		}
	}

	[Header("Game Configuration")]
	public SaveData saveData;                   //유저 정보 -> 저장 파일

	public bool isMusic;
	public bool isFx;
	public bool isColorText;
	public int level;                           //현재 스테이지 (level + 1)


	public List<Local> LocalList;

	//
	public List<Attack> opAttackList;
	public List<Bul> opBulList;
	public List<Stage> stageList;

	//etc
	public List<Info> etcInfoList;
	public List<Day> etcDayList;
	public List<Present> etcPresentList;
	public List<Achieve> etcAchieveList;

	public List<float> perList;
	public List<int> itemWeightList;

	public bool isFirst = false;
	public bool isLogo = false;

	public float delay = 10.0f;                 // 볼 출현 딜레이 
	public float fallingSpeed = 6f;             // 볼 내려오는 속도
	public float fallingSpeedSec = 3.0f;
	public float bulletSpeed = 40.0f;           // 총알 스피드
	public float bullSpeed = 0.01f;

	public float missileMovingUpSpeed = 30f;    // 미사일 스피드
	public float bombMovingUpSpeed = 8f;        // 폭탄 스피드
	public float laserMovingUpSpeed = 15f;      // 레이저 스피드


	public void comprete()
	{
		GetListControl.Instance.NetData();
		Load();
		DontDestroyOnLoad(this);
	}

	public void Load()
	{
		DataStart();
		time();
	}

	bool islogo;

	public void DataStart()
	{
		if (SaveData.LoadData().lastSaveDate == default(DateTime).ToString()) // 첫 실행시
		{
			islogo = true; //
			//TitleManager.i.logo.SetActive(true);
		}
		saveData = SaveData.LoadData();
	}

	public void Save()
	{
		saveData.Save();		
	}

	public void dell() 
	{
		saveData = SaveData.DeleteSaveData();
	}
		 
	public void time() { StartCoroutine("timeOn",1.0f); }
	IEnumerator timeOn(float _time) 
	{
		yield return new WaitForSeconds(3.0f);
		while (true)
		{ 
			yield return new WaitForSeconds(_time);
			saveData.AchieveUp[1] += 1;
		} 
	}

	/// <summary>
	/// 아이템
	/// </summary>
	public int getstageID() { return DataManager.Instance.getStagePlay()/ 10; }

	public void setCoin(int _count){ saveData.coin = _count;}
	public int getCoin() { return saveData.coin; }

	public void setDia(int _count){	saveData.dia = _count;}
	public int getDia() { return saveData.dia; }

	public void setMissle(int _count)
	{
		saveData.misale =_count;
        if (saveData.misale > 9) 
		{
			saveData.misale = 10;
		}
	}

	public int getMissale() { return saveData.misale;}

	public void setBomb(int _count)
	{
		saveData.bomb = _count;
		if (saveData.bomb > 9)
		{
			saveData.bomb = 10;
		}
	}

	public int getBomb() { return saveData.bomb; }

	public void setLazer(int _count)
	{
		saveData.lazer = _count;
		if (saveData.lazer > 9)
		{
			saveData.lazer = 10;
		}
	}

	public int getLazer() { return saveData.lazer; }

	/// <summary>
	/// 총알
	/// </summary>
	public void setWepon(int _count) { saveData.wepon = _count; }
	public int getWepon() { return saveData.wepon;}

	public int setWeponUp(int _no,int _count) { return saveData.weponUp[_no] = _count; }	
	public int getWeponUp(int _no) { return saveData.weponUp[_no]; }

	public int setWeponDown(int _no, int _count) { return saveData.weponDown[_no] = _count; }
	public int getWeponDown(int _no) { return saveData.weponDown[_no]; }


	/// <summary>
	/// 컨피그
	/// </summary>
	public void setStageBest(int _count) { saveData.stageMax = _count; }
	public int getStageBest() { return saveData.stageMax; }
	
	public void setStagePlay(int _count) { saveData.stagePlay = _count; }
	public int getStagePlay() { return saveData.stagePlay; }

	public int getAchieveUp(int _no) { return saveData.AchieveUp[_no]; }
	public int getAchieveDown(int _no) { return saveData.AchieveDown[_no]; }

	public int setAchieveUp(int _no,int _value) { return saveData.AchieveUp[_no] = _value; }
	public int setAchieveDown(int _no, int _value) { return saveData.AchieveDown[_no] = _value; }


	//현재 씬이름 확인
	public string getScene(){Scene scene = SceneManager.GetActiveScene();	return scene.name;}

	public void setBestScore(int _count){ saveData.bestPoint = _count;}

	public int getBestScore() {return saveData.bestPoint;}
	
	public void setPresent(int _count,bool _check) { saveData.present[_count] = _check; }

	public bool getPresent(int _count) { return saveData.present[_count]; }

	public void setBossCrear(int _count, bool _check) { saveData.bossCrear[_count] = _check; }

	public bool getBossCrear(int _count) { return saveData.bossCrear[_count]; }

	/// <summary>
	/// 강화 리스트
	/// </summary>
	/// 

	//1 연사
	public float getSpeed() { return (opAttackList[1].basePoint * 0.1f) - ((opAttackList[1].setPoint * 0.01f) *saveData.attackLevel[1]); }

	//2. 체력
	public int getHp() { return opAttackList[2].setPoint * saveData.attackLevel[2]; }

	//3. 크리티컬 
	public float getCri() { return (opAttackList[3].basePoint * 0.1f) +((opAttackList[3].setPoint * 0.1f) * saveData.attackLevel[3]); }

	//4. 미사일 데미지 증가
	public int getMissaleLevel() 
	{ 
		return opAttackList[4].basePoint + (opAttackList[4].setPoint * saveData.attackLevel[4]); 
	}// 미사일 데미지

	//5. 폭탄 연사 증가
	public float getBombLevel() 
	{ 
		return (opAttackList[5].basePoint * 0.1f) - ((opAttackList[5].setPoint * 0.01f) * saveData.attackLevel[5]); 
	}      //폭타 진헹 속도	

	//6. 레이저 데미지 증가
	public int getLazerLevel() 
	{
		Debug.Log("레이저 --- "+ opAttackList[6].basePoint + (opAttackList[6].setPoint * saveData.attackLevel[6]));
		return opAttackList[6].basePoint + (opAttackList[6].setPoint * saveData.attackLevel[6]); 
	}//레이저 데미지 ->가지고 있는 공격력의 10배
																																  //7.체력
	public int getHpLevel() 
	{ 
		return opAttackList[7].basePoint + (opAttackList[7].setPoint * saveData.attackLevel[7]); 
	}

	public void SetAttack(int _id,int _count){saveData.attackLevel[_id] = _count;}
	public int GetAttack(int _id){ return saveData.attackLevel[_id]; }					// 강화 리스트 전체 비교	 
	public int getAttack() { return (opAttackList[0].setPoint * saveData.attackLevel[0]);}

	
	//아이템
	

	public string setAttackContent(int _id)
	{
		string tex = "";
		switch (_id)
		{
			case 0://공겨력
				tex = getAttack().ToString() + " >> " + (getAttack()+ opAttackList[0].setPoint).ToString();
				break;
			case 1://연사
				tex = getSpeed().ToString() + "sec >> " + (getSpeed() - (opAttackList[1].setPoint * 0.01f)).ToString()+"sec";
				break;
			case 2://체력
				tex = getHp().ToString() + " >> " + (getHp()+ opAttackList[2].setPoint).ToString();
				break;
			case 3://크리티컬
				tex = getCri().ToString() + "% >> " + (getCri() + (opAttackList[3].setPoint*0.1f)).ToString()+"%";
				break;
			case 4://미사일 데미지
				tex = getMissaleLevel().ToString() + " >> " + (getMissaleLevel() + (opAttackList[4].setPoint * saveData.attackLevel[4])).ToString();
				break;
			case 5://폭탄 연사속도
				tex =  getBombLevel().ToString() + "sec >> " + (getBombLevel() - (opAttackList[5].setPoint*0.01f)).ToString() + "sec";
				break;
			case 6:// 레이저 데미지
				tex = getLazerLevel().ToString() + " >> " + (getLazerLevel() + (opAttackList[6].setPoint * saveData.attackLevel[6])).ToString();
				break;
		}

		return tex;
	}
}

 public class LocalKeys 
{
	//나중에 로컬 배열로 바꿈
	public const string ok		= "확인";
	public const string yes		= "예";
	public const string no		= "아니오";
}


public class PlayerPrefsKeys
{
	public const string PPK_SAVED_LEVEL = "OFGAMES_SAVED_LEVEL";
	public const string PPK_SAVED_USER_NAME = "OFGAMES_SAVED_USER_NAME";
	public const string PPK_BOMB = "BOMB";
	public const string PPK_LASER = "LASER";
	public const string PPK_MISSILE = "MISSILE";
	public const string PPK_SOUND = "OFGAMES_SOUND_KEY";
	public const string PPK_MUSIC = "OFGAMES_MUSIC_KEY";
	public const string PPK_TOTAL_COINS = "OFGAMES_TOTAL_COINS";
	public const string PPK_BEST_SCORE = "OFGAMES_BEST_SCORE";
	public const string PPK_SELECTED_CHARACTER = "OFGAMES_SELECTED_CHARACTER";
	public const string PPK_ShOOTING_SPEED_LEVEL = "OFGAMES_SHOOTING_SPEED_LEVEL_KEY";
	public const string PPK_BULLET_SPEED_LEVEL = "OFGAMES_BULLET_SPEED_LEVEL_KEY";
}