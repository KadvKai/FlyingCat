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

        //������������� SDK ������� AdMob
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
    //����� ����������� � ������������ ������
    public void RequestBanner()
    {
        //���� ������ ����� ������������� ������� ���
        if (_bannerAd != null)
            _bannerAd.Destroy();


        //C������ ������ �������
        _bannerAd = new BannerView(_banner_Id, AdSize.Banner, AdPosition.Bottom);

        //��������� ����������: ������� ��������, ��������� ��������, ��������� ����� �� �������, ����������� ������ � ���������� ����� �������� (�����)
        _bannerAd.OnAdLoaded += HandleOnAdLoadedBanner;
        _bannerAd.OnAdFailedToLoad += HandleOnAdFailedToLoadBanner;
        _bannerAd.OnAdOpening += HandleOnAdOpenedBanner;
        _bannerAd.OnAdClosed += HandleOnAdClosedBanner;

        //��������� ������ ��� ������ (����� ���������� �������������)
        _bannerAd.LoadAd(GetAdRequest());
    }

    //���������� ����� ������ ��������
    private void HandleOnAdLoadedBanner(object sender, EventArgs args)
    {
        //�������� ������� �������� �������
        BanerAdLoad = true;
    }

    //���������� ���� ��������� ������ �������� �������
    private void HandleOnAdFailedToLoadBanner(object sender, AdFailedToLoadEventArgs args)
    {
        //�������� �� ������� �������� �������
        BanerAdLoad = false;
    }

    //���������� ����� ����� ������� �� �������
    private void HandleOnAdOpenedBanner(object sender, EventArgs args)
    {
        //�������� ��� �� ������� �� ������� � ������� � - ���� ����� �� �������� � �������
        _isResumeGameOnShowingAd = true;
    }

    //���������� ����� ����� ������������ � ���� ����� ����� �� �������
    private void HandleOnAdClosedBanner(object sender, EventArgs args)
    {
        //.. �������� ��������� � ������������ ������ � ����
        //��������� ����� ������
        //��� ��� ������ ����������� ������������� (��� ����� �������� � ���������� ������� �� ����� �����),
        //����� ������������ ����� � ��� ������������ ��� �������������, ������, �� ����� ��� ������������� ����� ����� - �������� �������������
        //RequestBanner();
    }

}
