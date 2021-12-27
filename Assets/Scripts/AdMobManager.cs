using System.Collections;
using System;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdMobManager
{
    private readonly bool _personalizationAdsUser;//Персонализация рекламы

    //Объекты с рекламой
    private AppOpenAd _appOpenAd;
    private BannerView _bannerAd;
    private InterstitialAd _interstitialAd;
    private RewardedInterstitialAd _rewardInterstitialAd;
    private RewardedAd _rewardedAd;

    //Флаги состояния рекламы для внешнего взаимодействия (bool загружен и готов к показу/выгружен или незагружен, к показу не готов):
    //1 - AppOpened реклама (2 поля: флаг состояния рекламы загружена или нет,
    //время загрузки для проверки действительности объявления), 2 - состояние баннера, 3 - состояние межстраничной рекламы,
    //4 - состояние вознаграждаемой межстраничной рекламы, 5 - состояние вознаграждаемой рекламы (3 поля: флаг состояния рекламы
    //загружено или нет, делегат отвечающий за вручение пользователю вознаграждения, делегат отвечающий за трансляцию состояния рекламы
    //загружена или нет - этот делегат я использую для установки состояния кнопок показа рекламы, устанавливая их прозрачность, тем самым, невзрачно,
    //намекая пользователю что он может посмотреть рекламу и получить награду или же нажатие на кнопку не к чему не приведет)
    public bool AppOnenedAdLoad;
    private DateTime _AppOpenedLoadTime;

    public bool InterstitialAdsLoad;
    public bool RewardInterstitialAdsLoad;

    public event Action RewardEvent;
    public event Action<bool> RewardeStatus;

    private bool _isResumeGameOnShowingAd;//Показывает что приложение свернуто изза перехода по рекламе

    private int _maxFailedLoadBanner = 50;
    private int _maxFailedLoadReward = 20;

    //Тестовые идентификаторы Рекламных блоков: ApOpenAd, баннера, интерститиальной, вознаграждаемой интерститиальной и вознагрждаемой рекламы
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
        if (_appOpenedId != null) RequestAndLoadAppOpenAd();//Загружаем AppOpenedAds рекламу
        if (_interstitionId != null) RequestInterstitial();//Загружаем Интерститиальную рекламу
        if (_rewardInterstitionId != null) RequestRewardInterstitial();//Загружаем Вознаграждаемую интерститиальную рекламу
        if (_rewardId != null) RequestRewardedAd();//Загружаем Вознаграждаемую рекламу 
    }


    //********** AP OPPENED AD *************//
    //Активация рекламы, показываемой перед запуском приложения или после возвращения в него после сварачивания
    public void RequestAndLoadAppOpenAd()
    {
        //Если ранее было загружено объявление уничтожаем и обнуляем его объект
        if (_appOpenAd != null)
        {
            _appOpenAd.Destroy();
            _appOpenAd = null;
        }

        //Посылаем запрос на рекламу
        AppOpenAd.LoadAd(_appOpenedId, ScreenOrientation.AutoRotation, GetAdRequest(), AdLoadAppOpenedCallback);
    }

    private void AdLoadAppOpenedCallback(AppOpenAd ad, AdFailedToLoadEventArgs error)
    {
        if (error != null)
        {
            //Отмечаем НЕ удачаную загрузку рекламы, обнуляем объект
            AppOnenedAdLoad = false;
            _appOpenAd = null;
            return;
        }


        //Отмечаем удачаную загрузку
        AppOnenedAdLoad = true;

        //Запоминаем время загрузки объявления
        _AppOpenedLoadTime = DateTime.UtcNow;

        //Присваиваем объекту _appOpenAd загруженную порцию рекламы
        _appOpenAd = ad;
    }
    public void ShowAppOpenAd()
    {
        //Если при вызове показа объявления оно уже показывается или небыло загружено - прерываем попытку нового показа
        if (_isResumeGameOnShowingAd)
        {
            _isResumeGameOnShowingAd = false;
            return;
        }

        //Если объект небыл создан, прерываем попытку показа
        if (_appOpenAd == null)
        {
            //Отмечаем выгрузку рекламы
            AppOnenedAdLoad = false;
            return;
        }

        //Если при попытке показа объявления его время загрузки превышает 4 часа - прерываем попытку показа и перезагружаем объявление
        //Внимание, 4 часа это время срока хранения объявления, после которого его показ уже не принесет владельцу приложения
        //денежное вознаграждение, на 27.09.2021 года это время дейтвительно, согласно документации "Быстрый старт с Unity", однако
        //это времяможет быть увеличено или уменьшено по инициативе AdMob в любое время. См. документацию
        if ((DateTime.UtcNow - _AppOpenedLoadTime).TotalHours > 4)
        {
            RequestAndLoadAppOpenAd();
            return;
        }

        //Регистрация слушателей: Реакция на закрытие объявления, Реакция на неудачный показ, Реакция на удачный показ
        _appOpenAd.OnAdDidDismissFullScreenContent += HandleAdDidDismissFullScreenContent_AppOpened;
        _appOpenAd.OnAdFailedToPresentFullScreenContent += HandleAdFailedToPresentFullScreenContent_AppOpened;
        _appOpenAd.OnAdDidPresentFullScreenContent += HandleAdDidPresentFullScreenContent_AppOpened;

        //Вызываем показ объявления
        _appOpenAd.Show();
    }

    //Вызывается когда игрок закрывает объявление
    private void HandleAdDidDismissFullScreenContent_AppOpened(object sender, EventArgs args)
    {
        //Отмечаем что объявление было выгружено
        AppOnenedAdLoad = false;

        //Загружаем новое объявление
        RequestAndLoadAppOpenAd();
    }

    //Вызывается при неудачной попытке показа объявления
    private void HandleAdFailedToPresentFullScreenContent_AppOpened(object sender, AdErrorEventArgs args)
    {
        //Отмечаем что объявление было выгружено
        AppOnenedAdLoad = false;
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
        _bannerAd.Hide();
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
    //Метод загружающий интерстициональную рекламу
    public void RequestInterstitial()
    {
        //Если объект рекламы уже был загружен - удаляем его
        if (_interstitialAd != null)
            _interstitialAd.Destroy();

        //Создаем объект интерстиционной рекламы
        _interstitialAd = new InterstitialAd(_interstitionId);

        //добавляем слушателей: Удачная загрузка рекламы, Неудачная загрузка рекламы, Открытие объявления, Закрытие объявления
        _interstitialAd.OnAdLoaded += HandleOnAdLoaded_Interstitial;
        _interstitialAd.OnAdFailedToLoad += HandleOnAdFailedToLoad_Interstitial;
        _interstitialAd.OnAdOpening += HandleAdOpened_Interstitial;
        _interstitialAd.OnAdClosed += HandleOnAdClosed_Interstitial;

        //Загружаем рекламу для показа
        _interstitialAd.LoadAd(GetAdRequest());
    }

    //Вызывается после завершения загрузки объявления.
    private void HandleOnAdLoaded_Interstitial(object sender, EventArgs args)
    {
        //Отмечаем удачную загрузку рекламы
        InterstitialAdsLoad = true;
    }

    //Вызывается когда объявление не загружается. Этот Message параметр описывает тип возникшего сбоя.
    private void HandleOnAdFailedToLoad_Interstitial(object sender, AdFailedToLoadEventArgs args)
    {
        //Отмечаем неудачнуюзагрузку рекламы
        InterstitialAdsLoad = false;
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
        //В момент закрытия объявления загружаем новую порцию рекламы для предстоящего показа
        RequestInterstitial();
    }

    //Метод показа интерсстициальной рекламы
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

    //******** REWAR INTERSTITIAL ************//
    //Метод загружающий вознаграждаемую интерстициональную рекламу
    public void RequestRewardInterstitial()
    {
        if (_rewardInterstitialAd != null)
            _rewardInterstitialAd.Destroy();

        //Посылаем запрос на загрузку рекламы
        RewardedInterstitialAd.LoadAd(_rewardInterstitionId, GetAdRequest(), AdLoadRewardInterstitialCallback);
    }

    //Метод вызываемый после запроса
    private void AdLoadRewardInterstitialCallback(RewardedInterstitialAd ad, AdFailedToLoadEventArgs error)
    {
        if (error != null)
        {
            //Отмечаем неудачнуюзагрузку рекламы
            RewardInterstitialAdsLoad = false;
            return;
        }

        //Отмечаем удачную загрузку рекламы
        RewardInterstitialAdsLoad = true;

        _rewardInterstitialAd = ad;

        //Регистрация слушателей: Неудачный показ, Объявление открыто и показывается, Объявление закрыто пользователем
        _rewardInterstitialAd.OnAdDidPresentFullScreenContent += HandleAdDidPresent;
        _rewardInterstitialAd.OnAdFailedToPresentFullScreenContent += HandleAdFailedToPresent;
        _rewardInterstitialAd.OnAdDidDismissFullScreenContent += HandleAdDidDismiss;
    }

    //Вызывается при неудачной попытке показа объявления
    private void HandleAdFailedToPresent(object sender, AdErrorEventArgs args)
    {
        //Отмечаем выгрузку рекламы
        RewardInterstitialAdsLoad = false;
    }

    //Вызывается когда игрок закрывает объявление
    private void HandleAdDidDismiss(object sender, EventArgs args)
    {
        //Перезагружаем рекламный блок для следующего показа
        RequestRewardInterstitial();
    }

    //Вызывается когда объявление открыто для показа и показывается
    private void HandleAdDidPresent(object sender, EventArgs args)
    {
        //Отмечаем реклама показывается
        _isResumeGameOnShowingAd = true;
    }

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


    //*********** REWARD  *************//
    //Метод показывающий ВОЗНАГРАЖДЕННУЮ РЕКЛАМУ
    public void ShowRewardAd(Action EnterDelegateReward)
    {
        if (_rewardedAd != null && _rewardedAd.IsLoaded())
        {
            //Назначаем делегат вознаграждающий пользователя
            RewardEvent = EnterDelegateReward;

            //Инициируем показ рекламы
            _rewardedAd.Show();
        }
    }

    public void AddRewardedAdListener(Action<bool> RewardedAdListener)
    {
        RewardedAdListener.Invoke(_rewardedAd.IsLoaded());
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
    }


    //Вызывается  когда по каким то причинам объявление не загружается - передает и перечисляет ошибки
    private void HandleRewardedAdFailedToLoad(object sender, EventArgs args)
    {
        //Вызываем Делегаты подписанные на событие неудачной загрузки рекламы, она позволяет кнопкам узначть, что реклама НЕ была загружена, НЕ готова к показу и изменить свое состояние
        RewardeStatus?.Invoke(false);
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
        if (_personalizationAdsUser)
            return new AdRequest.Builder().Build();

        //Если пользователь отказался от персонализации рекламы - создаем запрос на неперсонифицированную рекламу (npa - Non-personalized ads (NPA) и 1 активна)
        return new AdRequest.Builder().AddExtra("npa", "1").Build();
    }
}
