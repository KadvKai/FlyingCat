using System.Collections;
using System;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdMobManager
{
    private readonly bool _personalizationAds;

    private AppOpenAd _appOpenAd;
    private BannerView _bannerAd;
    private InterstitialAd _interstitialAd;
    private RewardedInterstitialAd _rewardInterstitialAd;
    private RewardedAd _rewardedAd;
    
    private DateTime _AppOpenedLoadTime;

    private bool _rewardAdsLoad; 
    private bool InterstitialAdsLoad;
    private bool RewardInterstitialAdsLoad;

    public event Action RewardEvent;
    public event Action<bool> RewardeStatus;

    private bool _isResumeGameOnShowingAd;//���������� ��� ���������� �������� ���� �������� �� �������
    private int _maxFailedLoadAppOpened = 20;
    private int _maxFailedLoadBanner = 50;
    private int _maxFailedLoadInterstition = 20;
    private int _maxFailedLoadRewardInterstition = 20;
    private int _maxFailedLoadReward = 20;

    private readonly string _appOpenedId;
    private readonly string _bannerId;
    private readonly string _interstitionId;
    private readonly string _rewardInterstitionId;
    private readonly string _rewardId;

    public AdMobManager(bool personalization = false, bool adForChild = false, string appOpenedId = null, string bannerId = null, string interstitionId = null, string rewardInterstitionId = null, string rewardId = null)
    {
        _personalizationAds = personalization;
        if (adForChild)
        {
            RequestConfiguration requestConfiguration = new RequestConfiguration.Builder().SetTagForChildDirectedTreatment(TagForChildDirectedTreatment.True).build();
            MobileAds.SetRequestConfiguration(requestConfiguration);
        }
        _appOpenedId = appOpenedId;
        _bannerId = bannerId;
        _interstitionId = interstitionId;
        _rewardInterstitionId = rewardInterstitionId;
        _rewardId = rewardId;
       MobileAds.Initialize(initStatus =>
        {
            PreLoadAds();
        });
    }

    private void PreLoadAds()
    {
        if (_appOpenedId != null) RequestAndLoadAppOpenAd();//��������� AppOpenedAds �������
        if (_interstitionId != null) RequestInterstitial();//��������� ���������������� �������
        if (_rewardInterstitionId != null) RequestRewardInterstitial();//��������� ��������������� ���������������� �������
        if (_rewardId != null) RequestRewardedAd();//��������� ��������������� ������� 
    }


    //********** AP OPPENED AD *************//
    //����� ������� ������� ��� ��������� ����������� � ���������� �� ���������� ���������
    public void ShowAppOpenAd()
    {
        //������ ��� ������������ �������, ��������� ����� ������ �������
        if (_isResumeGameOnShowingAd)
        {
            _isResumeGameOnShowingAd = false;
            return;
        }

        if (_appOpenAd == null)
        {
            return;
        }
        
        //������� ��������
        if ((DateTime.UtcNow - _AppOpenedLoadTime).TotalHours > 4)
        {
            RequestAndLoadAppOpenAd();
            return;
        }
        _appOpenAd.OnAdDidDismissFullScreenContent += HandleAdDidDismissFullScreenContent_AppOpened;
        _appOpenAd.OnAdFailedToPresentFullScreenContent += HandleAdFailedToPresentFullScreenContent_AppOpened;
        _appOpenAd.OnAdDidPresentFullScreenContent += HandleAdDidPresentFullScreenContent_AppOpened;

        _appOpenAd.Show();
    }

    //�������� �������, ������������ ����� �������� ���������� ��� ����� ����������� � ���� ����� ������������
    private void RequestAndLoadAppOpenAd()
    {
        if (_appOpenAd != null)
        {
            _appOpenAd.Destroy();
            _appOpenAd = null;
        }

        AppOpenAd.LoadAd(_appOpenedId, ScreenOrientation.AutoRotation, GetAdRequest(), AdLoadAppOpenedCallback);
    }
    //������� �� ������� ��� �� ������� �������� �������
    private void AdLoadAppOpenedCallback(AppOpenAd ad, AdFailedToLoadEventArgs error)
    {
            
        if (error != null) //�� �������� �������� �������
        {
            if (_maxFailedLoadAppOpened > 0)
            {
                _maxFailedLoadAppOpened--;
                RequestAndLoadAppOpenAd();
            }
            else _appOpenAd = null;
            return;
        }
        
        _AppOpenedLoadTime = DateTime.UtcNow;
        
        _appOpenAd = ad;
    }
    //���������� ����� ����� ��������� ����������
    private void HandleAdDidDismissFullScreenContent_AppOpened(object sender, EventArgs args)
    {
        RequestAndLoadAppOpenAd();
    }

    //���������� ��� ��������� ������� ������ ����������
    private void HandleAdFailedToPresentFullScreenContent_AppOpened(object sender, AdErrorEventArgs args)
    {
        if (_maxFailedLoadAppOpened > 0)
        {
            _maxFailedLoadAppOpened--;
            RequestAndLoadAppOpenAd();
        }
        else _appOpenAd = null;
    }

    //���������� ����� ���������� ������������
    private void HandleAdDidPresentFullScreenContent_AppOpened(object sender, EventArgs args)
    {
        //�������� ��� ���� ����� ����������
        _isResumeGameOnShowingAd = true;

    }


    //********** BANNER *************//
    public void ShowBanner()
    {
        if (_bannerAd != null)
            _bannerAd.Show();
        else
            RequestBanner();
    }
    public void HideBanner()
    {
        if (_bannerAd != null) _bannerAd.Hide();
    }
    //����� ����������� � ������������ ������
    public void RequestBanner()
    {
        if (_bannerId != null)
        {
            if (_bannerAd != null)
                _bannerAd.Destroy();
            _bannerAd = new BannerView(_bannerId, AdSize.SmartBanner, AdPosition.Top);

            _bannerAd.OnAdLoaded += HandleOnAdLoadedBanner;
            _bannerAd.OnAdFailedToLoad += HandleOnAdFailedToLoadBanner;
            _bannerAd.OnAdOpening += HandleOnAdOpenedBanner;
            _bannerAd.OnAdClosed += HandleOnAdClosedBanner;

            //��������� ������ ��� ������ (����� ���������� �������������)
            _bannerAd.LoadAd(GetAdRequest());
        }
    }

    //���������� ����� ������ ��������
    private void HandleOnAdLoadedBanner(object sender, EventArgs args)
    {
    }

    //���������� ���� ��������� ������ �������� �������
    private void HandleOnAdFailedToLoadBanner(object sender, AdFailedToLoadEventArgs args)
    {
        if (_maxFailedLoadBanner > 0)
        {
            _maxFailedLoadBanner--;
            RequestBanner();
        }
    }

    //���������� ����� ����� ������� �� �������
    private void HandleOnAdOpenedBanner(object sender, EventArgs args)
    {
        //�������� ��� �� ������� �� ������� � �������
        _isResumeGameOnShowingAd = true;
    }

    //���������� ����� ����� ������������ � ���� ����� ����� �� �������
    private void HandleOnAdClosedBanner(object sender, EventArgs args)
    {
        RequestBanner();
    }


    //******** INTERSTITIAL ************//
    //����� ������ ������������� �������
    public void ShowInterstitialAd()
    {
        if (_interstitialAd != null && InterstitialAdsLoad)
        {
            if (_interstitialAd.IsLoaded())
            {
                //�������� �������� �������
                InterstitialAdsLoad = false;

                _interstitialAd.Show();
            }
        }
    }
    //����� ����������� ������������������ �������
    private void RequestInterstitial()
    {
        if (_interstitialAd != null)
            _interstitialAd.Destroy();

        _interstitialAd = new InterstitialAd(_interstitionId);

        _interstitialAd.OnAdLoaded += HandleOnAdLoaded_Interstitial;
        _interstitialAd.OnAdFailedToLoad += HandleOnAdFailedToLoad_Interstitial;
        _interstitialAd.OnAdOpening += HandleAdOpened_Interstitial;
        _interstitialAd.OnAdClosed += HandleOnAdClosed_Interstitial;

        _interstitialAd.LoadAd(GetAdRequest());
    }

    //���������� ����� ���������� �������� ����������.
    private void HandleOnAdLoaded_Interstitial(object sender, EventArgs args)
    {
        InterstitialAdsLoad = true;
    }

    //���������� ����� ���������� �� �����������. ���� Message �������� ��������� ��� ���������� ����.
    private void HandleOnAdFailedToLoad_Interstitial(object sender, AdFailedToLoadEventArgs args)
    {
        //�������� ����������������� �������
        InterstitialAdsLoad = false;
        if (_maxFailedLoadInterstition > 0)
        {
            _maxFailedLoadInterstition--;
            RequestInterstitial();
        }
    }

    //���������� ����� ���������� �����������
    private void HandleAdOpened_Interstitial(object sender, EventArgs args)
    {
        //�������� ��� ����� ����� �������
        _isResumeGameOnShowingAd = true;
    }

    //���������� ����� ���������������� ���������� ����������� �� - �� ����, ��� ������������ �������� �� ������ �������� ��� ������ �����. 
    private void HandleOnAdClosed_Interstitial(object sender, EventArgs args)
    {
        RequestInterstitial();
    }


    //******** REWAR INTERSTITIAL ************//
    //����� ���� ����������������� �������
    public void ShowRewarInterstitialAd(Action EnterDelegateRewar)
    {
        if (_rewardInterstitialAd != null && RewardInterstitialAdsLoad)
        {
            //��������� ������� ��������������� ������������
            RewardEvent = EnterDelegateRewar;

            //�������� ��� ������� ��������� � ����� ������ �� ���������
            RewardInterstitialAdsLoad = false;

            //���������� �������
            _rewardInterstitialAd.Show((Reward) =>
            {
                //���� ����� ��� �������, �������� �������
                RewardEvent?.Invoke();
            });
        }
    }
    //����� ����������� ��������������� ������������� �������
    private void RequestRewardInterstitial()
    {
        if (_rewardInterstitialAd != null)
            _rewardInterstitialAd.Destroy();

        //�������� ������ �� �������� �������
        RewardedInterstitialAd.LoadAd(_rewardInterstitionId, GetAdRequest(), AdLoadRewardInterstitialCallback);
    }

    //������� �� ������� ��� �� ������� �������� �������
    private void AdLoadRewardInterstitialCallback(RewardedInterstitialAd ad, AdFailedToLoadEventArgs error)
    {
        if (error != null) //��������� �������� �������
        {
            
            if (_maxFailedLoadRewardInterstition > 0)
            {
                _maxFailedLoadRewardInterstition--;
                RequestRewardInterstitial();
            }
            RewardInterstitialAdsLoad = false;
            return;
        }

        //�������� ������� �������� �������
        RewardInterstitialAdsLoad = true;

        _rewardInterstitialAd = ad;

        _rewardInterstitialAd.OnAdDidPresentFullScreenContent += HandleAdDidPresent;
        _rewardInterstitialAd.OnAdFailedToPresentFullScreenContent += HandleAdFailedToPresent;
        _rewardInterstitialAd.OnAdDidDismissFullScreenContent += HandleAdDidDismiss;
    }

    //���������� ��� ��������� ������� ������ ����������
    private void HandleAdFailedToPresent(object sender, AdErrorEventArgs args)
    {
        if (_maxFailedLoadRewardInterstition > 0)
        {
            _maxFailedLoadRewardInterstition--;
            RequestRewardInterstitial();
        }
        RewardInterstitialAdsLoad = false;
    }

    //���������� ����� ����� ��������� ����������
    private void HandleAdDidDismiss(object sender, EventArgs args)
    {
        RequestRewardInterstitial();
    }

    //���������� ����� ���������� ������� ��� ������ � ������������
    private void HandleAdDidPresent(object sender, EventArgs args)
    {
        _isResumeGameOnShowingAd = true;
    }

    //*********** REWARD  *************//
    //����� ������������ ��������������� �������
    public void ShowRewardAd(Action EnterDelegateReward)
    {
        if (_rewardedAd != null && _rewardAdsLoad)
        {
            if (_rewardedAd.IsLoaded())
            {
                //��������� ������� ��������������� ������������
                RewardEvent = EnterDelegateReward;

                //���������� ����� �������
                _rewardedAd.Show(); 
            }
        }
    }

    public void AddRewardedAdListener(Action<bool> RewardedAdListener)
    {
        //RewardedAdListener.Invoke(_rewardedAd.IsLoaded());
        RewardedAdListener.Invoke(_rewardAdsLoad);
        RewardeStatus += RewardedAdListener;
    }

    //����� ����������� ��������������� �������
    private void RequestRewardedAd()
    {
        _rewardedAd = new RewardedAd(_rewardId);

        _rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        _rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        _rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        _rewardedAd.OnAdOpening += HandleRewaedeAdOpened;
        _rewardedAd.OnAdClosed += HandleRewardedAdClosed;

        _rewardedAd.LoadAd(GetAdRequest());
    }

    //���������� ����� ��������� ���������
    private void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        //�������� �������� ����������� �� ������� �������� �������, ��� ��������� ������� �������, ��� ������� ���������, ������ � ������ � �������� ���� ���������
        RewardeStatus?.Invoke(true);
        _rewardAdsLoad = true;

    }


    //����������  ����� �� ����� �� �������� ���������� �� ����������� - �������� � ����������� ������
    private void HandleRewardedAdFailedToLoad(object sender, EventArgs args)
    {
        //�������� �������� ����������� �� ������� ��������� �������� �������, ��� ��������� ������� �������, ��� ������� �� ���� ���������, �� ������ � ������ � �������� ���� ���������
        RewardeStatus?.Invoke(false);
        _rewardAdsLoad = false;
        if (_maxFailedLoadReward > 0)
        {
            _maxFailedLoadReward--;
            RequestRewardedAd();
        }
    }

    //����������  ����� ������������ ������ ���� ������������ �� �������� �����.
    private void HandleUserEarnedReward(object sender, Reward args)
    {
        //�������� �������� ���������� �� ���������� ��������, ��������� � ���������������, �������� ���������
        RewardEvent?.Invoke();

        //�������� ������� �������� �������
        _rewardAdsLoad = false;
        RewardeStatus?.Invoke(false);
    }

    //���������� ����� ���������� �����������
    private void HandleRewaedeAdOpened(object sender, EventArgs args)
    {
        //�������� ��� ����� ����� �������
        _isResumeGameOnShowingAd = true;
    }

    //����������  ����� ������������ � ��������������� ����������� ��-�� ����, ��� ������������ �������� �� ������ �������� ��� ������ �����.
    private void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        //���� ������������ ������ ���������� ������ ������� - ��������� ����� ����������
        RequestRewardedAd();
    }


    //����� ������������ ������� �� �������� �������, ������� ��������������������� �������
    private AdRequest GetAdRequest()
    {
        //���� ������������ ��� �������� �� �������������� ������� - ������� ������� ������������� ������ �� ���������, �� ������������� ������������������� ����������
        if (_personalizationAds)
            return new AdRequest.Builder().Build();

        //���� ������������ ��������� �� �������������� ������� - ������� ������ �� ��������������������� ������� (npa - Non-personalized ads (NPA) � 1 �������)
        return new AdRequest.Builder().AddExtra("npa", "1").Build();
    }
}
