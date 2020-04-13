using System;
using UnityEngine;
using UnityEngine.Advertisements;
using static DefaultNamespace.Constants;

public class AdManager : MonoBehaviour, IUnityAdsListener
{
    public static AdManager Instance { set; get; }
    private string playStoreID = "3550331";
    private string appleStoreID = "3550330";

    private static string interstitialAd = "video";
    private static string rewardedVideoAd = "rewardedVideo";

    public bool isTargetPlayStore;
    public bool isTestAd;

    private static int _dieCount;
    private static int _restartCount;
    private static int _musicVolume = 6;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Advertisement.AddListener(this);
            InitializeAdvertisement();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeAdvertisement()
    {
        if (isTargetPlayStore)
        {
            Advertisement.Initialize(playStoreID, isTestAd);
            return;
        }

        Advertisement.Initialize(appleStoreID, isTestAd);
    }

    public static void PlayInterstitialAd()
    {
        if (!Advertisement.IsReady(interstitialAd))
        {
            return;
        }

        Advertisement.Show(interstitialAd);
    }

    public static void PlayRewardedVideoAd()
    {
        // _musicVolume = volume;
        if (!Advertisement.IsReady(rewardedVideoAd))
        {
            return;
        }

        Advertisement.Show(rewardedVideoAd);
    }

    public void OnUnityAdsReady(string placementId)
    {
        // throw new NotImplementedException();
    }

    public void OnUnityAdsDidError(string message)
    {
        // throw new NotImplementedException();
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        //TODO mute audio
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        switch (showResult)
        {
            case ShowResult.Failed:
                //throw smth
                break;
            case ShowResult.Skipped:
                //if user skipped ad
                PlayerPrefs.SetInt(MusicVolume, _musicVolume);
                break;
            case ShowResult.Finished:
                PlayerPrefs.SetInt(MusicVolume, _musicVolume);
                if (placementId.Equals(rewardedVideoAd))
                {
                    PlayerPrefs.SetInt(Coins, PlayerPrefs.GetInt(Coins) + 200);
                }

                break;
        }
    }

    public static void ShowAddAfterDeath()
    {
        _dieCount++;
        // if (dieCount % 6 == 0)
        // {
        //     PlayRewardedVideoAd();
        //     return;
        // }
        if (_dieCount % 3 == 0)
        {
            PlayInterstitialAd();
        }
    }
    
    public static void ShowAddAfterRestart()
    {
        _restartCount++;
        // if (dieCount % 10 == 0)
        // {
        //     PlayRewardedVideoAd();
        //     return;
        // }
        if (_restartCount % 5 == 0)
        {
            PlayInterstitialAd();
        }
    }
}