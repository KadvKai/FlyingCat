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

    private bool _isResumeGameOnShowingAd;//Показывает что приложение свернуто изза перехода по рекламе
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
        if (_appOpenedId != null) RequestAndLoadAppOpenAd();//Загружаем AppOpenedAds рекламу
        if (_interstitionId != null) RequestInterstitial();//Загружаем Интерститиальную рекламу
        if (_rewardInterstitionId != null) RequestRewardInterstitial();//Загружаем Вознаграждаемую интерститиальную рекламу
        if (_rewardId != null) RequestRewardedAd();//Загружаем Вознаграждаемую рекламу 
    }


    //********** AP OPPENED AD *************//
    //Метод запуска рекламы при следующем возвращении в приложение из свернутого состояния
    public void ShowAppOpenAd()
    {
        //Только что показывалась реклама, вирнулись после показа рекламы
        if (_isResumeGameOnShowingAd)
        {
            _isResumeGameOnShowingAd = false;
            return;
        }

        if (_appOpenAd == null)
        {
            return;
        }
        
        //Реклама устарела
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

    //Загрузка рекламы, показываемой перед запуском приложения или после возвращения в него после сварачивания
    private void RequestAndLoadAppOpenAd()
    {
        if (_appOpenAd != null)
        {
            _appOpenAd.Destroy();
            _appOpenAd = null;
        }

        AppOpenAd.LoadAd(_appOpenedId, ScreenOrientation.AutoRotation, GetAdRequest(), AdLoadAppOpenedCallback);
    }
    //Реакция на удачную или не удачную загрузку рекламы
    private void AdLoadAppOpenedCallback(AppOpenAd ad, AdFailedToLoadEventArgs error)
    {
            
        if (error != null) //НЕ удачаная загрузку рекламы
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
    //Вызывается когда игрок закрывает объявление
    private void HandleAdDidDismissFullScreenContent_AppOpened(object sender, EventArgs args)
    {
        RequestAndLoadAppOpenAd();
    }

    //Вызывается при неудачной попытке показа объявления
    private void HandleAdFailedToPresentFullScreenContent_AppOpened(object sender, AdErrorEventArgs args)
    {
        if (_maxFailedLoadAppOpened > 0)
        {
            _maxFailedLoadAppOpened--;
            RequestAndLoadAppOpenAd();
        }
        else _appOpenAd = null;
    }

    //Вызывается когда объявление показывается
    private void HandleAdDidPresentFullScreenContent_AppOpened(object sender, EventArgs args)
    {
        //Отмечаем что идет показ объявления
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
    //Метод загружающий и показывающий баннер
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

            //Загружаем баннер для показа (показ начинается автоматически)
            _bannerAd.LoadAd(GetAdRequest());
        }
    }

    //Вызывается когда баннер загружен
    private void HandleOnAdLoadedBanner(object sender, EventArgs args)
    {
    }

    //Вызывается если произошла ошибка загрузки баннера
    private void HandleOnAdFailedToLoadBanner(object sender, AdFailedToLoadEventArgs args)
    {
        if (_maxFailedLoadBanner > 0)
        {
            _maxFailedLoadBanner--;
            RequestBanner();
        }
    }

    //Вызывается когда игрок кликает по баннеру
    private void HandleOnAdOpenedBanner(object sender, EventArgs args)
    {
        //Отмечаем что мы перешли из баннера в браузер
        _isResumeGameOnShowingAd = true;
    }

    //Вызывается когда игрок возвращается в игру после клика по баннеру
    private void HandleOnAdClosedBanner(object sender, EventArgs args)
    {
        RequestBanner();
    }


    //******** INTERSTITIAL ************//
    //Метод показа межстраничной рекламы
    public void ShowInterstitialAd()
    {
        if (_interstitialAd != null && InterstitialAdsLoad)
        {
            if (_interstitialAd.IsLoaded())
            {
                //Отмечаем выгрузку рекламы
                InterstitialAdsLoad = false;

                _interstitialAd.Show();
            }
        }
    }
    //Метод загружающий интерстициональную рекламу
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

    //Вызывается после завершения загрузки объявления.
    private void HandleOnAdLoaded_Interstitial(object sender, EventArgs args)
    {
        InterstitialAdsLoad = true;
    }

    //Вызывается когда объявление не загружается. Этот Message параметр описывает тип возникшего сбоя.
    private void HandleOnAdFailedToLoad_Interstitial(object sender, AdFailedToLoadEventArgs args)
    {
        //Отмечаем неудачнуюзагрузку рекламы
        InterstitialAdsLoad = false;
        if (_maxFailedLoadInterstition > 0)
        {
            _maxFailedLoadInterstition--;
            RequestInterstitial();
        }
    }

    //Вызывается когда объявление открывается
    private void HandleAdOpened_Interstitial(object sender, EventArgs args)
    {
        //Отмечаем что начат показ рекламы
        _isResumeGameOnShowingAd = true;
    }

    //Вызывается когда интерстициальное объявление закрывается из - за того, что пользователь нажимает на значок закрытия или кнопку Назад. 
    private void HandleOnAdClosed_Interstitial(object sender, EventArgs args)
    {
        RequestInterstitial();
    }


    //******** REWAR INTERSTITIAL ************//
    //Метод пока интерсстициальной рекламы
    public void ShowRewarInterstitialAd(Action EnterDelegateRewar)
    {
        if (_rewardInterstitialAd != null && RewardInterstitialAdsLoad)
        {
            //Назначаем делегат вознаграждающий пользователя
            RewardEvent = EnterDelegateRewar;

            //Отмечаем что реклама выгружена и новая порция не загружена
            RewardInterstitialAdsLoad = false;

            //Показываем рекламу
            _rewardInterstitialAd.Show((Reward) =>
            {
                //Если показ был удачным, вызываем делегат
                RewardEvent?.Invoke();
            });
        }
    }
    //Метод загружающий вознаграждаемую межстраничную рекламу
    private void RequestRewardInterstitial()
    {
        if (_rewardInterstitialAd != null)
            _rewardInterstitialAd.Destroy();

        //Посылаем запрос на загрузку рекламы
        RewardedInterstitialAd.LoadAd(_rewardInterstitionId, GetAdRequest(), AdLoadRewardInterstitialCallback);
    }

    //Реакция на удачную или не удачную загрузку рекламы
    private void AdLoadRewardInterstitialCallback(RewardedInterstitialAd ad, AdFailedToLoadEventArgs error)
    {
        if (error != null) //Неудачная загрузка рекламы
        {
            
            if (_maxFailedLoadRewardInterstition > 0)
            {
                _maxFailedLoadRewardInterstition--;
                RequestRewardInterstitial();
            }
            RewardInterstitialAdsLoad = false;
            return;
        }

        //Отмечаем удачную загрузку рекламы
        RewardInterstitialAdsLoad = true;

        _rewardInterstitialAd = ad;

        _rewardInterstitialAd.OnAdDidPresentFullScreenContent += HandleAdDidPresent;
        _rewardInterstitialAd.OnAdFailedToPresentFullScreenContent += HandleAdFailedToPresent;
        _rewardInterstitialAd.OnAdDidDismissFullScreenContent += HandleAdDidDismiss;
    }

    //Вызывается при неудачной попытке показа объявления
    private void HandleAdFailedToPresent(object sender, AdErrorEventArgs args)
    {
        if (_maxFailedLoadRewardInterstition > 0)
        {
            _maxFailedLoadRewardInterstition--;
            RequestRewardInterstitial();
        }
        RewardInterstitialAdsLoad = false;
    }

    //Вызывается когда игрок закрывает объявление
    private void HandleAdDidDismiss(object sender, EventArgs args)
    {
        RequestRewardInterstitial();
    }

    //Вызывается когда объявление открыто для показа и показывается
    private void HandleAdDidPresent(object sender, EventArgs args)
    {
        _isResumeGameOnShowingAd = true;
    }

    //*********** REWARD  *************//
    //Метод показывающий ВОЗНАГРАЖДЕННУЮ РЕКЛАМУ
    public void ShowRewardAd(Action EnterDelegateReward)
    {
        if (_rewardedAd != null && _rewardAdsLoad)
        {
            if (_rewardedAd.IsLoaded())
            {
                //Назначаем делегат вознаграждающий пользователя
                RewardEvent = EnterDelegateReward;

                //Инициируем показ рекламы
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

    //Метод загружающий вознаграждаемую рекламу
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

    //Вызывается когда обявление загружено
    private void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        //Вызываем Делегаты подписанные на событие загрузки рекламы, она позволяет кнопкам узначть, что реклама загружена, готова к показу и изменить свое состояние
        RewardeStatus?.Invoke(true);
        _rewardAdsLoad = true;

    }


    //Вызывается  когда по каким то причинам объявление не загружается - передает и перечисляет ошибки
    private void HandleRewardedAdFailedToLoad(object sender, EventArgs args)
    {
        //Вызываем Делегаты подписанные на событие неудачной загрузки рекламы, она позволяет кнопкам узначть, что реклама НЕ была загружена, НЕ готова к показу и изменить свое состояние
        RewardeStatus?.Invoke(false);
        _rewardAdsLoad = false;
        if (_maxFailedLoadReward > 0)
        {
            _maxFailedLoadReward--;
            RequestRewardedAd();
        }
    }

    //Вызывается  когда пользователь должен быть вознагражден за просмотр видео.
    private void HandleUserEarnedReward(object sender, Reward args)
    {
        //Вызываем Делегаты отвечающие за Выполнение действий, связанных с вознаграждением, обнуляем слушателя
        RewardEvent?.Invoke();

        //Вызываем событие выгрузки рекламы
        _rewardAdsLoad = false;
        RewardeStatus?.Invoke(false);
    }

    //Вызывается когда объявление открывается
    private void HandleRewaedeAdOpened(object sender, EventArgs args)
    {
        //Отмечаем что начат показ рекламы
        _isResumeGameOnShowingAd = true;
    }

    //Вызывается  когда видеореклама с вознаграждением закрывается из-за того, что пользователь нажимает на значок закрытия или кнопку Назад.
    private void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        //Если пользователь закрыл объявление раньше времени - Загружаем новое объявление
        RequestRewardedAd();
    }


    //Метод формирования запроса на загрузку рекламы, включая неперсонализированную рекламу
    private AdRequest GetAdRequest()
    {
        //Если пользователь дал согласие на Персонализацию рекламы - создаем простой универсальный запрос по умолчанию, он предоставляет персонализированные объявления
        if (_personalizationAds)
            return new AdRequest.Builder().Build();

        //Если пользователь отказался от персонализации рекламы - создаем запрос на неперсонифицированную рекламу (npa - Non-personalized ads (NPA) и 1 активна)
        return new AdRequest.Builder().AddExtra("npa", "1").Build();
    }
}
