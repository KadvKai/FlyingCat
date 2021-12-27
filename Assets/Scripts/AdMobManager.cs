using System.Collections;
using System;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdMobManager
{
    private readonly bool _personalizationAdsUser;//�������������� �������

    //������� � ��������
    private AppOpenAd _appOpenAd;
    private BannerView _bannerAd;
    private InterstitialAd _interstitialAd;
    private RewardedInterstitialAd _rewardInterstitialAd;
    private RewardedAd _rewardedAd;

    //����� ��������� ������� ��� �������� �������������� (bool �������� � ����� � ������/�������� ��� ����������, � ������ �� �����):
    //1 - AppOpened ������� (2 ����: ���� ��������� ������� ��������� ��� ���,
    //����� �������� ��� �������� ���������������� ����������), 2 - ��������� �������, 3 - ��������� ������������� �������,
    //4 - ��������� ��������������� ������������� �������, 5 - ��������� ��������������� ������� (3 ����: ���� ��������� �������
    //��������� ��� ���, ������� ���������� �� �������� ������������ ��������������, ������� ���������� �� ���������� ��������� �������
    //��������� ��� ��� - ���� ������� � ��������� ��� ��������� ��������� ������ ������ �������, ������������ �� ������������, ��� �����, ���������,
    //������� ������������ ��� �� ����� ���������� ������� � �������� ������� ��� �� ������� �� ������ �� � ���� �� ��������)
    public bool AppOnenedAdLoad;
    private DateTime _AppOpenedLoadTime;

    public bool InterstitialAdsLoad;
    public bool RewardInterstitialAdsLoad;

    public event Action RewardEvent;
    public event Action<bool> RewardeStatus;

    private bool _isResumeGameOnShowingAd;//���������� ��� ���������� �������� ���� �������� �� �������

    private int _maxFailedLoadBanner = 50;
    private int _maxFailedLoadReward = 20;

    //�������� �������������� ��������� ������: ApOpenAd, �������, ����������������, ��������������� ���������������� � �������������� �������
    private readonly string _appOpenedId;
    private readonly string _bannerId;
    private readonly string _interstitionId;
    private readonly string _rewardInterstitionId;
    private readonly string _rewardId;

    public AdMobManager(bool personalization = false, bool adForChild = true, string appOpenedId = null, string bannerId = null, string interstitionId = null, string rewardInterstitionId = null, string rewardId = null)
    {
        _personalizationAdsUser = personalization;
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
    //��������� �������, ������������ ����� �������� ���������� ��� ����� ����������� � ���� ����� ������������
    public void RequestAndLoadAppOpenAd()
    {
        //���� ����� ���� ��������� ���������� ���������� � �������� ��� ������
        if (_appOpenAd != null)
        {
            _appOpenAd.Destroy();
            _appOpenAd = null;
        }

        //�������� ������ �� �������
        AppOpenAd.LoadAd(_appOpenedId, ScreenOrientation.AutoRotation, GetAdRequest(), AdLoadAppOpenedCallback);
    }

    private void AdLoadAppOpenedCallback(AppOpenAd ad, AdFailedToLoadEventArgs error)
    {
        if (error != null)
        {
            //�������� �� �������� �������� �������, �������� ������
            AppOnenedAdLoad = false;
            _appOpenAd = null;
            return;
        }


        //�������� �������� ��������
        AppOnenedAdLoad = true;

        //���������� ����� �������� ����������
        _AppOpenedLoadTime = DateTime.UtcNow;

        //����������� ������� _appOpenAd ����������� ������ �������
        _appOpenAd = ad;
    }
    public void ShowAppOpenAd()
    {
        //���� ��� ������ ������ ���������� ��� ��� ������������ ��� ������ ��������� - ��������� ������� ������ ������
        if (_isResumeGameOnShowingAd)
        {
            _isResumeGameOnShowingAd = false;
            return;
        }

        //���� ������ ����� ������, ��������� ������� ������
        if (_appOpenAd == null)
        {
            //�������� �������� �������
            AppOnenedAdLoad = false;
            return;
        }

        //���� ��� ������� ������ ���������� ��� ����� �������� ��������� 4 ���� - ��������� ������� ������ � ������������� ����������
        //��������, 4 ���� ��� ����� ����� �������� ����������, ����� �������� ��� ����� ��� �� �������� ��������� ����������
        //�������� ��������������, �� 27.09.2021 ���� ��� ����� ������������, �������� ������������ "������� ����� � Unity", ������
        //��� ���������� ���� ��������� ��� ��������� �� ���������� AdMob � ����� �����. ��. ������������
        if ((DateTime.UtcNow - _AppOpenedLoadTime).TotalHours > 4)
        {
            RequestAndLoadAppOpenAd();
            return;
        }

        //����������� ����������: ������� �� �������� ����������, ������� �� ��������� �����, ������� �� ������� �����
        _appOpenAd.OnAdDidDismissFullScreenContent += HandleAdDidDismissFullScreenContent_AppOpened;
        _appOpenAd.OnAdFailedToPresentFullScreenContent += HandleAdFailedToPresentFullScreenContent_AppOpened;
        _appOpenAd.OnAdDidPresentFullScreenContent += HandleAdDidPresentFullScreenContent_AppOpened;

        //�������� ����� ����������
        _appOpenAd.Show();
    }

    //���������� ����� ����� ��������� ����������
    private void HandleAdDidDismissFullScreenContent_AppOpened(object sender, EventArgs args)
    {
        //�������� ��� ���������� ���� ���������
        AppOnenedAdLoad = false;

        //��������� ����� ����������
        RequestAndLoadAppOpenAd();
    }

    //���������� ��� ��������� ������� ������ ����������
    private void HandleAdFailedToPresentFullScreenContent_AppOpened(object sender, AdErrorEventArgs args)
    {
        //�������� ��� ���������� ���� ���������
        AppOnenedAdLoad = false;
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
        _bannerAd.Hide();
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
    //����� ����������� ������������������ �������
    public void RequestInterstitial()
    {
        //���� ������ ������� ��� ��� �������� - ������� ���
        if (_interstitialAd != null)
            _interstitialAd.Destroy();

        //������� ������ ��������������� �������
        _interstitialAd = new InterstitialAd(_interstitionId);

        //��������� ����������: ������� �������� �������, ��������� �������� �������, �������� ����������, �������� ����������
        _interstitialAd.OnAdLoaded += HandleOnAdLoaded_Interstitial;
        _interstitialAd.OnAdFailedToLoad += HandleOnAdFailedToLoad_Interstitial;
        _interstitialAd.OnAdOpening += HandleAdOpened_Interstitial;
        _interstitialAd.OnAdClosed += HandleOnAdClosed_Interstitial;

        //��������� ������� ��� ������
        _interstitialAd.LoadAd(GetAdRequest());
    }

    //���������� ����� ���������� �������� ����������.
    private void HandleOnAdLoaded_Interstitial(object sender, EventArgs args)
    {
        //�������� ������� �������� �������
        InterstitialAdsLoad = true;
    }

    //���������� ����� ���������� �� �����������. ���� Message �������� ��������� ��� ���������� ����.
    private void HandleOnAdFailedToLoad_Interstitial(object sender, AdFailedToLoadEventArgs args)
    {
        //�������� ����������������� �������
        InterstitialAdsLoad = false;
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
        //� ������ �������� ���������� ��������� ����� ������ ������� ��� ������������ ������
        RequestInterstitial();
    }

    //����� ������ ����������������� �������
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

    //******** REWAR INTERSTITIAL ************//
    //����� ����������� ��������������� ������������������ �������
    public void RequestRewardInterstitial()
    {
        if (_rewardInterstitialAd != null)
            _rewardInterstitialAd.Destroy();

        //�������� ������ �� �������� �������
        RewardedInterstitialAd.LoadAd(_rewardInterstitionId, GetAdRequest(), AdLoadRewardInterstitialCallback);
    }

    //����� ���������� ����� �������
    private void AdLoadRewardInterstitialCallback(RewardedInterstitialAd ad, AdFailedToLoadEventArgs error)
    {
        if (error != null)
        {
            //�������� ����������������� �������
            RewardInterstitialAdsLoad = false;
            return;
        }

        //�������� ������� �������� �������
        RewardInterstitialAdsLoad = true;

        _rewardInterstitialAd = ad;

        //����������� ����������: ��������� �����, ���������� ������� � ������������, ���������� ������� �������������
        _rewardInterstitialAd.OnAdDidPresentFullScreenContent += HandleAdDidPresent;
        _rewardInterstitialAd.OnAdFailedToPresentFullScreenContent += HandleAdFailedToPresent;
        _rewardInterstitialAd.OnAdDidDismissFullScreenContent += HandleAdDidDismiss;
    }

    //���������� ��� ��������� ������� ������ ����������
    private void HandleAdFailedToPresent(object sender, AdErrorEventArgs args)
    {
        //�������� �������� �������
        RewardInterstitialAdsLoad = false;
    }

    //���������� ����� ����� ��������� ����������
    private void HandleAdDidDismiss(object sender, EventArgs args)
    {
        //������������� ��������� ���� ��� ���������� ������
        RequestRewardInterstitial();
    }

    //���������� ����� ���������� ������� ��� ������ � ������������
    private void HandleAdDidPresent(object sender, EventArgs args)
    {
        //�������� ������� ������������
        _isResumeGameOnShowingAd = true;
    }

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


    //*********** REWARD  *************//
    //����� ������������ ��������������� �������
    public void ShowRewardAd(Action EnterDelegateReward)
    {
        if (_rewardedAd != null && _rewardedAd.IsLoaded())
        {
            //��������� ������� ��������������� ������������
            RewardEvent = EnterDelegateReward;

            //���������� ����� �������
            _rewardedAd.Show();
        }
    }

    public void AddRewardedAdListener(Action<bool> RewardedAdListener)
    {
        RewardedAdListener.Invoke(_rewardedAd.IsLoaded());
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
    }


    //����������  ����� �� ����� �� �������� ���������� �� ����������� - �������� � ����������� ������
    private void HandleRewardedAdFailedToLoad(object sender, EventArgs args)
    {
        //�������� �������� ����������� �� ������� ��������� �������� �������, ��� ��������� ������� �������, ��� ������� �� ���� ���������, �� ������ � ������ � �������� ���� ���������
        RewardeStatus?.Invoke(false);
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
        if (_personalizationAdsUser)
            return new AdRequest.Builder().Build();

        //���� ������������ ��������� �� �������������� ������� - ������� ������ �� ��������������������� ������� (npa - Non-personalized ads (NPA) � 1 �������)
        return new AdRequest.Builder().AddExtra("npa", "1").Build();
    }
}
