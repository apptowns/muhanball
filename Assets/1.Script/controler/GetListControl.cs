using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GetListControl : MonoBehaviour
{
	private static GetListControl instance;

	public static GetListControl Instance
	{
		get
		{
			if (instance == null)
			{
				GameObject newObject = new GameObject("_GetListControl");
				instance = newObject.AddComponent<GetListControl>();
				if (instance == null)
					Debug.LogError("GetListControl null");
			}
			return instance;
		}
	}

	public string[] url;		// 웹주소
	public string[] webData;    // 담아놓을 데이터 
	const int checkCount = 150;
	const int listCount = 8;
	public void	NetData()
	{		
		if(Application.internetReachability == NetworkReachability.NotReachable) 
		{
			LocalDataSet();
			MessageManager.i.onPanel("인터넷이 연결 없이 게임을 시작합니다.");
		}
		else if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork) 
		{
			StartCoroutine(Version());
			MessageManager.i.onPanel("데이터로 연결 합니다.");
		}
        else if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
		{
			StartCoroutine(Version());
			MessageManager.i.onPanel("와이파이로 연결 합니다.");

		} 
	} 

	public IEnumerator Version()
	{
		yield return null;
		const string _url = "https://docs.google.com/spreadsheets/d/1inBv3CSQAG1Q9oj9OYVbRz-PIJHYgwX-PccafHKtVp4/export?format=csv&gid=0";
		
		UnityWebRequest www;
		www = UnityWebRequest.Get(_url);		
		yield return www.SendWebRequest();
		string _wdata = www.downloadHandler.text;

		DataManager.Instance.LocalList = new List<Local>();
		
		string[] _data = _wdata.Split("\n"[0]);
		for (int i = 2; i < _data.Length; i++)
		{			
			string[] dd = _data[i].Split(","[0]);
			Local d = new Local();
			d.id = i - 2;
			d.local = new string[2];
            for (int j=0; j<d.local.Length; j++) 
			{
				d.local[j] = dd[j+1].Trim();
			}
			DataManager.Instance.LocalList.Add(d);
		}

		if (Application.version.ToString() == DataManager.Instance.LocalList[0].local[0])
		{ 
			//같으면 
			Debug.Log(Application.version.ToString() +"////////////////"+ DataManager.Instance.LocalList[0].local[0]);
			StartCoroutine(webDataSet());
		}
        else 
		{ 
			//틀리면.. 스토어로 이동
			Debug.Log(Application.version.ToString() + "////////////////" + DataManager.Instance.LocalList[0].local[0]);
			Debug.Log("https://play.google.com/apps/internaltest/4700422805330452343" + "////////////////" + DataManager.Instance.LocalList[0].local[0]);

			Application.OpenURL(DataManager.Instance.LocalList[1].local[0]);
		}
	}

	public void LocalDataSet()
	{
		//로컬데이터를 배열에 일단 옮김
		string[] resourcesPath = new string[listCount];
		resourcesPath[0] = "data/shooter - info";
		resourcesPath[1] = "data/shooter - day";
		resourcesPath[2] = "data/shooter - stage";
		resourcesPath[3] = "data/shooter - attack";
		resourcesPath[4] = "data/shooter - bul";
		resourcesPath[5] = "data/shooter - present";

		webData = new string[listCount];
		for (int i = 0; i < webData.Length; i++)
		{
			TextAsset aa = Resources.Load(resourcesPath[i]) as TextAsset;
			webData[i] = aa.text;
		}

		dataSet();
	}

	public IEnumerator webDataSet()
	{
		//주소
		url = new string[listCount];

		url[0] = "https://docs.google.com/spreadsheets/d/1inBv3CSQAG1Q9oj9OYVbRz-PIJHYgwX-PccafHKtVp4/export?format=csv&gid=9302501";       // 공지사항
		url[1] = "https://docs.google.com/spreadsheets/d/1inBv3CSQAG1Q9oj9OYVbRz-PIJHYgwX-PccafHKtVp4/export?format=csv&gid=1536291960";    // 데일리 리스트
		url[2] = "https://docs.google.com/spreadsheets/d/1inBv3CSQAG1Q9oj9OYVbRz-PIJHYgwX-PccafHKtVp4/export?format=csv&gid=1431677134";    // 스테이지 리스트

		url[3] = "https://docs.google.com/spreadsheets/d/1inBv3CSQAG1Q9oj9OYVbRz-PIJHYgwX-PccafHKtVp4/export?format=csv&gid=2057496427";    // 강화리스트
		url[4] = "https://docs.google.com/spreadsheets/d/1inBv3CSQAG1Q9oj9OYVbRz-PIJHYgwX-PccafHKtVp4/export?format=csv&gid=410320183";     // 총알 리스트 
	
		url[5] = "https://docs.google.com/spreadsheets/d/1inBv3CSQAG1Q9oj9OYVbRz-PIJHYgwX-PccafHKtVp4/export?format=csv&gid=1516259447";    // 선물 리스트
		url[6] = "https://docs.google.com/spreadsheets/d/1inBv3CSQAG1Q9oj9OYVbRz-PIJHYgwX-PccafHKtVp4/export?format=csv&gid=920679080";     // 업적 리스트

		url[7] = "https://docs.google.com/spreadsheets/d/1inBv3CSQAG1Q9oj9OYVbRz-PIJHYgwX-PccafHKtVp4/export?format=csv&gid=1352402895";    // 확률 리스트 


		yield return null;
		UnityWebRequest www;

		webData = new string[listCount];
		for (int i = 0; i < webData.Length; i++)
		{
			www = UnityWebRequest.Get(url[i]);
			yield return www.SendWebRequest();
			webData[i] = www.downloadHandler.text;

			yield return null;
		}

		dataSet();
	}

	void dataSet() 
	{		
		string[] data; 

		DataManager.Instance.etcInfoList = new List<Info>();
		DataManager.Instance.etcDayList = new List<Day>();

		DataManager.Instance.etcPresentList = new List<Present>();
		DataManager.Instance.etcAchieveList = new List<Achieve>();

		DataManager.Instance.stageList = new List<Stage>();
		DataManager.Instance.opAttackList = new List<Attack>();
		DataManager.Instance.opBulList = new List<Bul>();
		DataManager.Instance.perList = new List<float>();

		//배열 구성 	
		/////////////////////////////////////////////////////// 0		
		data = webData[0].Split("\n"[0]);
		for (int i = 2; i < data.Length; i++)
		{
			string[] dd = data[i].Split(","[0]);
			Info d = new Info();
			d.id = int.Parse(dd[0].Trim());
			d.title = dd[1];
			d.content = dd[2];
			d.day = dd[3];

			DataManager.Instance.etcInfoList.Add(d); 
		}

		LogoManager.i.loadingBar.value += checkCount;
		//TitleManager.i.loadingbarvalue += checkCount;
		//TitleManager.i.Etc.infoSet();

		/////////////////////////////////////////////////////// 1
		data = webData[1].Split("\n"[0]);
		for (int i = 2; i < data.Length; i++)
		{
			string[] dd = data[i].Split(","[0]);
			Day d = new Day();
			d.id = int.Parse(dd[0].Trim());
			d.dayLevel = int.Parse(dd[0].Trim());
			d.iconName = dd[0].Trim();
			d.iconValue = int.Parse(dd[0].Trim());
			d.check = (int.Parse(dd[0].Trim()) > 0) ? true : false;

			DataManager.Instance.etcDayList.Add(d);			
		}

		LogoManager.i.loadingBar.value += checkCount;
		//TitleManager.i.loadingbarvalue += checkCount;
		//TitleManager.i.Etc.bonusSet();
		
		////////////////////////////////////////////////////// 2
		
		data = webData[2].Split("\n"[0]);
		for (int i = 2; i < data.Length; i++)	
		{
			string[] dd = data[i].Split(","[0]);
			Stage d = new Stage();
			d.id			= int.Parse(dd[0].Trim());
			d.name			= dd[1].Trim();
			d.setStage		= int.Parse(dd[2].Trim());
			d.baseStage		= int.Parse(dd[3].Trim());
			d.hudleStage	= int.Parse(dd[4].Trim());
			d.speed			= int.Parse(dd[5].Trim()) * 0.1f;
			d.delay			= int.Parse(dd[6].Trim()) * 0.1f;
			d.delayMax		= int.Parse(dd[7].Trim()) * 0.1f;
			d.hp			= int.Parse(dd[8].Trim());
			d.time			= int.Parse(dd[9].Trim());
			d.minnumber		= int.Parse(dd[10].Trim());
			d.minsetnumber	= int.Parse(dd[11].Trim());
			d.maxnumber		= int.Parse(dd[12].Trim());
			d.upnumber		= int.Parse(dd[13].Trim());
			d.goldValue		= int.Parse(dd[14].Trim());
			d.gold			= int.Parse(dd[15].Trim());
			d.dia			= int.Parse(dd[16].Trim());
			d.m				= int.Parse(dd[17].Trim());
			d.b				= int.Parse(dd[18].Trim());
			d.l				= int.Parse(dd[19].Trim());

			DataManager.Instance.stageList.Add(d);
		}

		LogoManager.i.loadingBar.value += checkCount;
		//TitleManager.i.loadingbarvalue += checkCount;
		//TitleManager.i.Etc.stageSet();

		/////////////////////////////////////////////////// 3
		data = webData[3].Split("\n"[0]);
		for (int i = 2; i < data.Length; i++)
		{
			string[] dd = data[i].Split(","[0]);
			Attack d = new Attack();
			d.id		= int.Parse(dd[0].Trim());
			d.titel		= dd[1];
			d.basePoint = int.Parse(dd[2].Trim());
			d.level		= int.Parse(dd[3].Trim());
			d.setPoint	= int.Parse(dd[4].Trim());	//
			d.max		= int.Parse(dd[5].Trim());
			d.onLevel	= int.Parse(dd[6].Trim());	
			d.coin		= int.Parse(dd[7].Trim());

			DataManager.Instance.opAttackList.Add(d);
		}

		LogoManager.i.mes.text ="강화리스트 체크입니다.";
		LogoManager.i.loadingBar.value += checkCount;
		//TitleManager.i.loadingbarvalue += checkCount;

		////////////////////////////////////////////////////// 4
		data = webData[4].Split("\n"[0]);
		for (int i = 2; i < data.Length; i++)
		{
			string[] dd = data[i].Split(","[0]);
			Bul d = new Bul();
			d.id = int.Parse(dd[0].Trim());
			d.title = dd[1];
			d.clas = dd[2];
			d.level = int.Parse(dd[3].Trim());

			d.getcount = int.Parse(dd[4].Trim());           //합성 전
			d.getset = int.Parse(dd[5].Trim());             //합성 후

			d.upat = int.Parse(dd[6].Trim());
			d.upcriat = int.Parse(dd[7].Trim()) * 0.1f;
			d.uphp = int.Parse(dd[8].Trim()) * 0.1f;

			d.bestat = dd[9].Trim();
			d.weight = int.Parse(dd[10].Trim());

			DataManager.Instance.opBulList.Add(d);

		}

		LogoManager.i.mes.text = "총알 데이터 체크";
		LogoManager.i.loadingBar.value += checkCount;
		//TitleManager.i.loadingbarvalue += checkCount;

		/////////////////////////////////////////////////////// 5		
		data = webData[5].Split("\n"[0]);
		for (int i = 2; i < data.Length; i++)
		{
			string[] dd = data[i].Split(","[0]);
			Present d = new Present();
			d.id = int.Parse(dd[0].Trim());
			d.title = dd[1];
			d.content = dd[2];
			d.value = new int[5];
            for (int j=0; j<d.value.Length; j++) 
			{
				d.value[j] = int.Parse(dd[j + 3]);
			}

			DataManager.Instance.etcPresentList.Add(d);
		}

		LogoManager.i.mes.text = "선물 리스트입니다.";
		LogoManager.i.loadingBar.value =checkCount; 

		/////////////////////////////////////////////////////// 6		
		data = webData[6].Split("\n"[0]);
		for (int i = 2; i < data.Length; i++)
		{
			string[] dd = data[i].Split(","[0]);
			Achieve d = new Achieve();
			d.id			= int.Parse(dd[0].Trim());
			d.title			= dd[1];
			d.content		= dd[2];
			d.set			= int.Parse(dd[3]);
			d.valueKind		=int.Parse(dd[4]);
			d.valueCount	= int.Parse(dd[5]);

			DataManager.Instance.etcAchieveList.Add(d);
		} 
		LogoManager.i.mes.text = "업적 리스트입니다.";
		LogoManager.i.loadingBar.value = 900;


		/////////////////////////////////////////////////////// 7		
		data = webData[7].Split("\n"[0]);
		for (int i = 2; i < data.Length; i++)
		{
			string[] dd = data[i].Split(","[0]);
			DataManager.Instance.perList.Add(float.Parse(dd[2])); 
		}
		LogoManager.i.mes.text = "확률 리스트입니다.";
		LogoManager.i.loadingBar.value = 1000;

		DataManager.Instance.itemWeightList = new List<int>();
		data = webData[7].Split("\n"[0]);
		for (int i = 2; i < data.Length; i++)
		{
			string[] dd = data[i].Split(","[0]);
			if(int.Parse(dd[3]) !=0)
				DataManager.Instance.itemWeightList.Add(int.Parse(dd[3]));
		}


		//업적 관련 추가.. 함수 실행
		AchieveSet();
	}



	public void AchieveSet() 
	{ 
	}
}

[System.Serializable]
public class Achieve 
{
	public int id;
	public string title;
	public string content;
	public int set;
	public int valueKind;	//뭐가
	public int valueCount;	//얼마?
}


