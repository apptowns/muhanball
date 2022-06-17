using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stage
{
	public int id;
	public string name;

	public int setStage;		//스테이지 범위
	public int baseStage;		//기본 적용 스테이지
	public int hudleStage;		//허들스테이지

	public float speed;			//스테이지 성능 옵션
	public float delay;
	public float delayMax;
	public int hp;				//보스 체력
	public float time;			//보스 클리어 시간
	
	public int minnumber;		// 출현 볼 수치
	public int minsetnumber;	// 제한 스테이지에 나오는 볼 수치
	public int maxnumber;		// 최대 볼수치
	public int upnumber;		// 랜덤으로 나오는 최대 볼수치
	
	public int goldValue;		// 승리시 골드 보너스 맥스 수치
	
	public int gold;			//승리시 나오는 골드 수량
	public int dia;
	
	public int m;
	public int b;
	public int l;


	public float[] value;
}
