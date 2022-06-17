using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossValueControl : MonoBehaviour
{
    public bool iswin;
    public Text title;
    public Text value;

    // 표기
    public void set(bool _iswin)
    {
        iswin = _iswin;
        this.gameObject.SetActive(true);
        SoundManager.Instance.play(7);
        // 게임 정보 표기 

        if (iswin)
        {            
            title.text = DataManager.Instance.stageList[DataManager.Instance.getstageID()].name.ToString();

            value.text = "" +
                DataManager.Instance.stageList[DataManager.Instance.getstageID()].gold.ToString() + "\n" +
                DataManager.Instance.stageList[DataManager.Instance.getstageID()].dia.ToString() + "\n" +
                DataManager.Instance.stageList[DataManager.Instance.getstageID()].m.ToString() + "\n" +
                DataManager.Instance.stageList[DataManager.Instance.getstageID()].b.ToString() + "\n" +
                DataManager.Instance.stageList[DataManager.Instance.getstageID()].l.ToString();
        }
        else 
        {
        
        
        }

    }

    // 보스 클리어 후.. 보상 받기
    public void ok() 
    {
        Debug.Log("ok");


        //게임 정보...

        if (iswin)
        {
            int _id = DataManager.Instance.getstageID();

            DataManager.Instance.setCoin(DataManager.Instance.getCoin() + DataManager.Instance.stageList[_id].gold);
            DataManager.Instance.setDia(DataManager.Instance.getDia() + DataManager.Instance.stageList[_id].dia);
            DataManager.Instance.setMissle(DataManager.Instance.getMissale() + DataManager.Instance.stageList[_id].m);
            DataManager.Instance.setBomb(DataManager.Instance.getBomb()+DataManager.Instance.stageList[_id].b);
            DataManager.Instance.setLazer(DataManager.Instance.getLazer() + DataManager.Instance.stageList[_id].l);

            DataManager.Instance.setStagePlay(DataManager.Instance.getStagePlay()+1);
            DataManager.Instance.Save();
            this.gameObject.SetActive(false);
            Gamemanager.i.waitGame();
        }
        else 
        {
            ButtonManager.i.goTitle();
        }       
    }
}
