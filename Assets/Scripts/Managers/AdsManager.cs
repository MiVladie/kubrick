using System;
using UnityEngine;
using GoogleMobileAds.Api;
//using GoogleMobileAds.Api.Mediation.AppLovin;

public class AdsManager : MonoBehaviour
{
	public static AdsManager instance;

    private BannerView bannerView;
    private InterstitialAd interstitial;
    private RewardBasedVideoAd rewardBasedVideo;

    private float deltaTime = 0f;

    System.DateTime DateValueStart;
    System.DateTime DateValueFinish;

    public void Awake()
    {
		if (instance != null) Destroy(gameObject);
		else { instance = this; DontDestroyOnLoad(gameObject); }

        string appId = "";

        MobileAds.SetiOSAppPauseOnBackground(true);

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(appId);

        // AppLovin initialisation
        //AppLovin.Initialize();

        // Get singleton reward based video ad reference.
        this.rewardBasedVideo = RewardBasedVideoAd.Instance;

        // RewardBasedVideoAd is a singleton, so handlers should only be registered once.
        this.rewardBasedVideo.OnAdLoaded += this.HandleRewardBasedVideoLoaded;
        this.rewardBasedVideo.OnAdFailedToLoad += this.HandleRewardBasedVideoFailedToLoad;
        this.rewardBasedVideo.OnAdOpening += this.HandleRewardBasedVideoOpened;
        this.rewardBasedVideo.OnAdStarted += this.HandleRewardBasedVideoStarted;
        this.rewardBasedVideo.OnAdRewarded += this.HandleRewardBasedVideoRewarded;
        this.rewardBasedVideo.OnAdClosed += this.HandleRewardBasedVideoClosed;
        this.rewardBasedVideo.OnAdLeavingApplication += this.HandleRewardBasedVideoLeftApplication;
        
		RequestInterstitial();
		RequestRewardBasedVideo();
    }

    public void Update()
    {
        // Calculate simple moving average for time to render screen. 0.1 factor used as smoothing value.
        this.deltaTime += (Time.deltaTime - this.deltaTime) * 0.1f;
    }

    private AdRequest CreateAdRequest()
    {
        return new AdRequest.Builder()
            .AddKeyword("game")
            .SetGender(Gender.Male)
            .SetBirthday(new DateTime(1985, 1, 1))
            .TagForChildDirectedTreatment(false)
            .AddExtra("color_bg", "9B30FF")      
            .AddExtra("npa", "1")
            .Build();
    }

    #region BannerAd

    public void RequestBanner()
    {
		if(PlayerPrefs.GetInt("Ads") == 0)
			return;
            
        string adUnitId = "";

        // Clean up banner ad before creating a new one.
        if (this.bannerView != null)
        {
            this.bannerView.Destroy();
        }

        // Create a 320x50 banner at the top of the screen.
        this.bannerView = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.Bottom);

        // Load a banner ad.
        this.bannerView.LoadAd(this.CreateAdRequest());
    }

    public void HideBanner()
    {
        if(this.bannerView != null)
        {
            this.bannerView.Destroy();
        }
    }

    #endregion

    #region InterstitialAd
  
    public void RequestInterstitial()
    {
        string adUnitId = "";

 	    if (this.interstitial == null || !this.interstitial.IsLoaded())
        {	
            // Clean up interstitial ad before creating a new one.
            if (this.interstitial != null) this.interstitial.Destroy();

            // Create an interstitial.
            this.interstitial = new InterstitialAd(adUnitId);

            // Load an interstitial ad.
            this.interstitial.LoadAd(this.CreateAdRequest());
        }
    }
    
    public void ShowInterstitial()
    {
		if(PlayerPrefs.GetInt("Ads") == 0)
			return;
			
        if (this.interstitial.IsLoaded())
            this.interstitial.Show();
    }

    #endregion

    #region RewardedVideoAd

    public void RequestRewardBasedVideo()
    {
        string adUnitId = "";

        AdRequest request = new AdRequest.Builder().Build();

        this.rewardBasedVideo.LoadAd(this.CreateAdRequest(), adUnitId);
    }

    public void ShowRewardBasedVideo()
    {
        if(PlayerPrefs.GetInt("Ads") == 0)
            return;

        if (this.rewardBasedVideo.IsLoaded())
            this.rewardBasedVideo.Show();
    }

    public bool isRewardBasedVideoLoaded()
    {
        return this.rewardBasedVideo.IsLoaded();		
    }

    public void HandleRewardBasedVideoLoaded(object sender, EventArgs args)
    {
        //
    }
 
    public void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        //
    }

    public void HandleRewardBasedVideoOpened(object sender, EventArgs args)
    {
        //
    }

    public void HandleRewardBasedVideoStarted(object sender, EventArgs args)
    {
        //
    }

    public void HandleRewardBasedVideoClosed(object sender, EventArgs args)
    {
        RequestRewardBasedVideo();    
    }

    public void HandleRewardBasedVideoRewarded(object sender, Reward args)
    {
        FindObjectOfType<GameOver>().RewardedPlay();
    }

    public void HandleRewardBasedVideoLeftApplication(object sender, EventArgs args)
    {
        //
    }

    #endregion

}

