using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Bul
{
	public int id;
	public string title;
	public string clas;		// 일반, 레어등..

	public int level;		// 제한 레벨
	public int getcount;
	public int getset;

	public int upat;		//공격력 플러스
	public float upcriat;	//크리티컬 플러스
	public float uphp;		//체력 플러스

	public string bestat;	//특수 공격 -> 방향, 미사일 장착등
	public int weight;		//뽑기 확률을 위한 가중치
}
