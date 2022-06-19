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


    void Start()
    {
        var requestConfiguration = new RequestConfiguration
           .Builder()
           .SetTestDeviceIds(new List<string>() { "1DF7B7CC05014E8" }) // test Device ID
           .build();

        MobileAds.SetRequestConfiguration(requestConfiguration);

        LoadBannerAd();
        LoadFrontAd();
        LoadRewardAd();
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
        bannerAd = new BannerView(isTestMode ? bannerTestID : bannerID,
            AdSize.SmartBanner, AdPosition.Bottom);
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


    void LoadRewardAd()
    {
        rewardAd = new RewardedAd(isTestMode ? rewardTestID : rewardID);
        
        rewardAd.LoadAd(GetAdRequest());
        rewardAd.OnUserEarnedReward += (sender, e) =>
        {
            LogText.text = "리워드 광고 성공";
            DataManager.Instance.setDia(DataManager.Instance.getDia()+50);
            Time.timeScale = 0.0f;
        };
    }

    public void ShowRewardAd()
    {       
        rewardAd.Show();
        LoadRewardAd();
    }
    #endregion
}
