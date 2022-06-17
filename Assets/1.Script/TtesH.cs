using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TtesH : MonoBehaviour
{
    public  int a;
    // Start is called before the first frame update
    public int getScore(int _no)
    {
        string a = _no.ToString();
        if (_no > 99)
        {
            Debug.Log(int.Parse(a.Substring(0, 2)));
            return int.Parse(a.Substring(0, 2));
        }
        else
        {
            return _no;
        }
    }

    public void Start()
    {
        int a = 23540;
        string aa = a.ToString();

        Debug.Log(aa.Substring(0, 1));
        Debug.Log(aa.Substring(0, 2));




    }



    float getScale(int _number)
    {
        //int cha = 0;
       // int min = DataManager.Instance.stageList[DataManager.Instance.getstageID()].minnumber;

        if (_number > 100)
        {
           // Debug.Log("결과 :" + (_number - 100).ToString() + " | " +getScore(_number - 100) + " | " + ((getScore(_number - 100) * 0.02f) + 0.3f));            
            return (getScore(_number - 100) * 0.02f) + 0.3f;
        }
        else
        {
         //  Debug.Log("결과 :" + (_number - 100).ToString() + " | " + getScore(_number - 100) + " | " + 0.3f);
            return 0.3f;
        }
    }
}
