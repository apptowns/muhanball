using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleMobileAds.Api;

public class AdmobManager : MonoBehaviour
{
    public bool isTestMode;
    public Text LogText;
    public Button FrontAdsBtn, RewardAdsBtn;

    public Image reon;
    public Image reoff;
    public Text time;
    public int timeRemit = 300;

    public int isValue;

    void Start()
    {
        timeRemit = 300;
        time.text = (timeRemit / 60).ToString() + ":" + (timeRemit % 60).ToString();
        reon.enabled = true;
        reoff.enabled = false;

        LoadBannerAd();
        LoadFrontAd();
        LoadRewardAd();

        ToggleBannerAd(true);
    }

    void Update()
    {
        FrontAdsBtn.interactable = frontAd.IsLoaded();
        RewardAdsBtn.interactable = rewardAd.IsLoaded();
    }

    AdRequest GetAdRequest()   
    {
        return new AdRequest.Builder().Build();
    }


    #region 배너 광고
    const string bannerTestID = "ca-app-pub-3940256099942544/6300978111"; //ca-app-pub-3940256099942544/6300978111
    const string bannerID = "ca-app-pub-4642965268273458/7780510760";
    BannerView bannerAd;


    void LoadBannerAd()
    {
        bannerAd = new BannerView(isTestMode ? bannerTestID : bannerID,AdSize.SmartBanner, AdPosition.Bottom);
        bannerAd.LoadAd(GetAdRequest());
        ToggleBannerAd(false);
    }

    public void ToggleBannerAd(bool b)
    {
        if (b) bannerAd.Show();
        else bannerAd.Hide();
    }
    #endregion



    #region 전면 광고
    const string frontTestID = "ca-app-pub-3940256099942544/8691691433"; //ca-app-pub-3940256099942544/1033173712
    const string frontID = "ca-app-pub-4642965268273458/8116650165";
    InterstitialAd frontAd;


    void LoadFrontAd()
    {
        frontAd = new InterstitialAd(isTestMode ? frontTestID : frontID);
        frontAd.LoadAd(GetAdRequest());
        frontAd.OnAdClosed += (sender, e) =>
        {
            LogText.text = "전면광고 성공";
        };
    }

    public void ShowFrontAd()
    {
        frontAd.Show();
        LoadFrontAd();
    }
    #endregion



    #region 리워드 광고
    const string rewardTestID = "ca-app-pub-3940256099942544/5224354917"; //ca-app-pub-3940256099942544/5224354917
    const string rewardID = "ca-app-pub-4642965268273458/2672751795";
    RewardedAd rewardAd;

    IEnumerator timecheck() 
    {
        RewardAdsBtn.interactable = false;
        reon.enabled = false;
        reoff.enabled = true;

        while (timeRemit > 0)
        {
            yield return new WaitForSeconds(1.0f);
            timeRemit -= 1;
            time.text = (timeRemit/60).ToString() + ":" +(timeRemit%60).ToString();
        }

        RewardAdsBtn.interactable = true;
        reon.enabled = true;
        reoff.enabled = false;
    }

    void LoadRewardAd()
    {
        rewardAd = new RewardedAd(isTestMode ? rewardTestID : rewardID);

        rewardAd.LoadAd(GetAdRequest());
        rewardAd.OnUserEarnedReward += (sender, e) =>
        {
            LogText.text = "리워드 광고 성공";

            switch (isValue)
            {
                case 0:
                    break;
                    Debug.Log("다이아 보상");
                    DataManager.Instance.setDia(DataManager.Instance.getDia() + 50);
                    StartCoroutine(timecheck());

                case 1:
                    Debug.Log("보너스 씬 이동");
                    DataManager.Instance.setStagePlay(DataManager.Instance.getStagePlay());
                    TitleManager.i.goGame();
                    break;
            }

        };
    }

    public void ShowRewardAd(int b)
    {
        isValue = b;

        rewardAd.Show();
        LoadRewardAd();
    }
    #endregion
}
