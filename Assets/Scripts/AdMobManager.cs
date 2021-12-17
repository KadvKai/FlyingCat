using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdMobManager 
{

    private bool _personalizationAdsUser;
    private BannerView _bannerAd;

    private string _banner_Id = "ca-app-pub-8405086734887888/1283778070";

    public AdMobManager(bool personalization)
    {
        _personalizationAdsUser = personalization;

        //Инициализация SDK Рекламы AdMob
        MobileAds.Initialize(initStatus =>
        {
            LoadAds();
        });
    }

    private void LoadAds()
    {
            RequestBanner();
    }

    //********** BANNER *************//
    //Метод загружающий и показывающий баннер
    public void RequestBanner()
    {
        //Если баннер ранее использовался очищаем его
        if (_bannerAd != null)
            _bannerAd.Destroy();


        //Cоздаем объект баннера
        _bannerAd = new BannerView(_banner_Id, AdSize.Banner, AdPosition.Bottom);

        //Добавляем слушателей: Удачная загрузка, Неудачная загрузка, Отработка клика по баннеру, Возвращение игрока в приложение после перехода (клика)
        _bannerAd.OnAdLoaded += HandleOnAdLoadedBanner;
        _bannerAd.OnAdFailedToLoad += HandleOnAdFailedToLoadBanner;
        _bannerAd.OnAdOpening += HandleOnAdOpenedBanner;
        _bannerAd.OnAdClosed += HandleOnAdClosedBanner;

        //Загружаем баннер для показа (показ начинается автоматически)
        _bannerAd.LoadAd(GetAdRequest());
    }

    //Вызывается когда баннер загружен
    private void HandleOnAdLoadedBanner(object sender, EventArgs args)
    {
        //Отмечаем удачную загрузку рекламы
        BanerAdLoad = true;
    }

    //Вызывается если произошла ошибка загрузки баннера
    private void HandleOnAdFailedToLoadBanner(object sender, AdFailedToLoadEventArgs args)
    {
        //Отмечаем НЕ удачную загрузку рекламы
        BanerAdLoad = false;
    }

    //Вызывается когда игрок кликает по баннеру
    private void HandleOnAdOpenedBanner(object sender, EventArgs args)
    {
        //Отмечаем что мы перешли из баннера в браузер и - идет показ по переходу с баннера
        _isResumeGameOnShowingAd = true;
    }

    //Вызывается когда игрок возвращается в игру после клика по баннеру
    private void HandleOnAdClosedBanner(object sender, EventArgs args)
    {
        //.. Действия связанные с возвращением игрока в игру
        //Загружаем новый баннер
        //Так как баннер обновляется автоматичекси (это можно изменить в настройках баннера на Сайте АдМоб),
        //через определенное время в его перезагрузке нет необходимости, однако, мы можем его перезагрузить когда хотим - обновить принудительно
        //RequestBanner();
    }

}
