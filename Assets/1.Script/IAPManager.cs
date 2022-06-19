using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAPManager : MonoBehaviour
{ 
    public int id;
    private void Start()
    {
        //아이디 셋

    }

    public void Reward()
    {       
        DataManager.Instance.setDia(DataManager.Instance.getDia()+DataManager.Instance.etcShopList[id].valuePresent[0]);
        Debug.Log("인앱성공");
    }

    public void Failed()
    {        
       Debug.Log("인앱실패");
    }


}
