using System.Globalization;
using static DefaultNamespace.Constants;
using static DefaultNamespace.Timer;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public Button[] levels;
    public Sprite bronzeMedal;
    public Sprite silverMedal;
    public Sprite goldMedal;
    public Button[] skinButtons;
    public Text coinText;
    public Slider musicSlided, soundSlider;
    public Text musicText, soundText;
    public AudioSource musicSource;
    private Button activeSkinButton;
    private bool hasPink;
    private bool hasGreen;
    private bool hasYellow;
    private bool hasBeige;

    private void Start()
    {
        
        if (PlayerPrefs.HasKey(Level))
        {
            CheckCompletedLevel();
        }

        if (!PlayerPrefs.HasKey(SkinActive))
        {
            PlayerPrefs.SetString(SkinActive, SkinColorBlue);
            activeSkinButton = skinButtons[0];
            activeSkinButton.interactable = false;
        }
        else
        {
            SetActiveSkinButton();
        }

        InitializePlayerPrefKeys();
        CheckBoughtSkins();
        musicSlided.value = PlayerPrefs.GetInt(MusicVolume);
        musicSource.volume = PlayerPrefs.GetInt(MusicVolume) / 9f;
        soundSlider.value = PlayerPrefs.GetInt(SoundVolume);
    }

    private void CheckCompletedLevel()
    {
        for (var i = 0; i < levels.Length; i++)
        {
            if (i <= PlayerPrefs.GetInt(Level))
            {
                levels[i].interactable = true;
                SetBestTimeAndMedal(i);
            }
            else
            {
                levels[i].interactable = false;
            }
        }
    }

    private void InitializePlayerPrefKeys()
    {
        if (!PlayerPrefs.HasKey(HasSkinBlue))
        {
            PlayerPrefs.SetInt(HasSkinBlue, 1);
        }

        if (!PlayerPrefs.HasKey(HasSkinBeige))
        {
            PlayerPrefs.SetInt(HasSkinBeige, 0);
        }

        if (!PlayerPrefs.HasKey(HasSkinGreen))
        {
            PlayerPrefs.SetInt(HasSkinGreen, 0);
        }

        if (!PlayerPrefs.HasKey(HasSkinPink))
        {
            PlayerPrefs.SetInt(HasSkinPink, 0);
        }

        if (!PlayerPrefs.HasKey(HasSkinYellow))
        {
            PlayerPrefs.SetInt(HasSkinYellow, 0);
        }

        if (!PlayerPrefs.HasKey(MusicVolume))
        {
            PlayerPrefs.SetInt(MusicVolume, 6);
        }

        if (!PlayerPrefs.HasKey(SoundVolume))
        {
            PlayerPrefs.SetInt(SoundVolume, 6);
        }
    }

    private void SetActiveSkinButton()
    {
        switch (PlayerPrefs.GetString(SkinActive, SkinColorBlue))
        {
            case SkinColorBlue:
                activeSkinButton = skinButtons[0];
                break;
            case SkinColorPink:
                activeSkinButton = skinButtons[1];
                break;
            case SkinColorGreen:
                activeSkinButton = skinButtons[2];
                break;
            case SkinColorYellow:
                activeSkinButton = skinButtons[3];
                break;
            case SkinColorBeige:
                activeSkinButton = skinButtons[4];
                break;
        }

        activeSkinButton.interactable = false;
    }

    private void CheckBoughtSkins()
    {
        if (PlayerPrefs.GetInt(HasSkinPink) == 1)
        {
            hasPink = true;
            skinButtons[1].transform.GetChild(0).gameObject.SetActive(false);
        }

        if (PlayerPrefs.GetInt(HasSkinGreen) == 1)
        {
            hasGreen = true;
            skinButtons[2].transform.GetChild(0).gameObject.SetActive(false);
        }

        if (PlayerPrefs.GetInt(HasSkinYellow) == 1)
        {
            hasYellow = true;
            skinButtons[3].transform.GetChild(0).gameObject.SetActive(false);
        }

        if (PlayerPrefs.GetInt(HasSkinBeige) == 1)
        {
            hasBeige = true;
            skinButtons[4].transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    private void SetBestTimeAndMedal(int levelIndex)
    {
        var levelNumber = levelIndex + 1;
        if (PlayerPrefs.HasKey(Timer + levelNumber))
        {
            var bestTime = PlayerPrefs.GetFloat(Timer + levelNumber);
            levels[levelIndex].transform.GetChild(1).GetComponent<Text>().text = GetTimerString(bestTime);
            levels[levelIndex].transform.GetChild(2).gameObject.SetActive(true);
            SetMedal(levelIndex, bestTime);
            SetGems(levelIndex);
        }
    }

    private void SetMedal(int levelIndex, float bestTime)
    {
        var worstTimeForCurrentLevel = LevelsWorstTime[levelIndex + 1];
        var timeRatio = bestTime / worstTimeForCurrentLevel;
        levels[levelIndex].transform.GetChild(3).gameObject.SetActive(true);
        var medalImage = levels[levelIndex].transform.GetChild(3).GetComponent<Image>();
        if (timeRatio < 0.2f)
        {
            medalImage.sprite = goldMedal;
        }
        else if (timeRatio < 0.5f && timeRatio >= 0.2f)
        {
            medalImage.sprite = silverMedal;
        }
        else
        {
            medalImage.sprite = bronzeMedal;
        }
    }

    private void SetGems(int levelIndex)
    {
        var levelNumber = levelIndex + 1;
        if (PlayerPrefs.HasKey(GemBlue + levelNumber) && PlayerPrefs.GetInt(GemBlue + levelNumber) == 1)
        {
            levels[levelIndex].transform.GetChild(4).transform.GetChild(0).gameObject.SetActive(true);
        }
        if (PlayerPrefs.HasKey(GemGreen + levelNumber) && PlayerPrefs.GetInt(GemGreen + levelNumber) == 1)
        {
            levels[levelIndex].transform.GetChild(4).transform.GetChild(1).gameObject.SetActive(true);
        }
        if (PlayerPrefs.HasKey(GemYellow + levelNumber) && PlayerPrefs.GetInt(GemYellow + levelNumber) == 1)
        {
            levels[levelIndex].transform.GetChild(4).transform.GetChild(2).gameObject.SetActive(true);
        }
    }
    private void Update()
    {
        PlayerPrefs.SetInt(MusicVolume, (int) musicSlided.value);
        musicSource.volume = PlayerPrefs.GetInt(MusicVolume) / 9f;
        PlayerPrefs.SetInt(SoundVolume, (int) soundSlider.value);
        musicText.text = musicSlided.value.ToString(CultureInfo.InvariantCulture);
        soundText.text = soundSlider.value.ToString(CultureInfo.InvariantCulture);
        if (PlayerPrefs.HasKey(Coins))
        {
            coinText.text = PlayerPrefs.GetInt(Coins).ToString();
        }
        else
        {
            coinText.text = "0";
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PlayGamesScript.SignOut();
            Application.Quit();
        }
    }

    public void OpenScene(int index)
    {
        if (index == 1)
        {
            PlayGamesScript.UnlockAchievement(GPGSIds.achievement_welcome);
        }
        SceneManager.LoadScene(index);
    }

    public void DelKeys()
    {
        PlayerPrefs.DeleteAll();
    }

    public void BuyBlueSkin()
    {
        activeSkinButton.interactable = true;
        skinButtons[0].interactable = false;
        activeSkinButton = skinButtons[0];
        PlayerPrefs.SetString(SkinActive, SkinColorBlue);
    }

    public void BuyPinkSkin(int cost)
    {
        if (hasPink || PlayerPrefs.GetInt(Coins) >= cost)
        {
            activeSkinButton.interactable = true;
            skinButtons[1].interactable = false;
            activeSkinButton = skinButtons[1];
            activeSkinButton.transform.GetChild(0).gameObject.SetActive(false);
            PlayerPrefs.SetInt(HasSkinPink, 1);
            PlayerPrefs.SetString(SkinActive, SkinColorPink);
            if (!hasPink)
            {
                PlayGamesScript.UnlockAchievement(GPGSIds.achievement_new_look);
                PlayerPrefs.SetInt(Coins, PlayerPrefs.GetInt(Coins) - cost);
                hasPink = true;
            }
        }
    }

    public void BuyGreenSkin(int cost)
    {
        if (hasGreen || PlayerPrefs.GetInt(Coins) >= cost)
        {
            activeSkinButton.interactable = true;
            skinButtons[2].interactable = false;
            activeSkinButton = skinButtons[2];
            activeSkinButton.transform.GetChild(0).gameObject.SetActive(false);
            PlayerPrefs.SetInt(HasSkinGreen, 1);
            PlayerPrefs.SetString(SkinActive, SkinColorGreen);
            if (!hasGreen)
            {
                PlayGamesScript.UnlockAchievement(GPGSIds.achievement_new_look);
                PlayerPrefs.SetInt(Coins, PlayerPrefs.GetInt(Coins) - cost);
                hasGreen = true;
            }
        }
    }


    public void BuyYellowSkin(int cost)
    {
        if (hasYellow || PlayerPrefs.GetInt(Coins) >= cost)
        {
            activeSkinButton.interactable = true;
            skinButtons[3].interactable = false;
            activeSkinButton = skinButtons[3];
            activeSkinButton.transform.GetChild(0).gameObject.SetActive(false);
            PlayerPrefs.SetInt(HasSkinYellow, 1);
            PlayerPrefs.SetString(SkinActive, SkinColorYellow);
            if (!hasYellow)
            {
                PlayGamesScript.UnlockAchievement(GPGSIds.achievement_new_look);
                PlayerPrefs.SetInt(Coins, PlayerPrefs.GetInt(Coins) - cost);
                hasYellow = true;
            }
        }
    }

    public void BuyBeigeSkin(int cost)
    {
        if (hasBeige || PlayerPrefs.GetInt(Coins) >= cost)
        {
            activeSkinButton.interactable = true;
            skinButtons[4].interactable = false;
            activeSkinButton = skinButtons[4];
            activeSkinButton.transform.GetChild(0).gameObject.SetActive(false);
            PlayerPrefs.SetInt(HasSkinBeige, 1);
            PlayerPrefs.SetString(SkinActive, SkinColorBeige);
            if (!hasBeige)
            {
                PlayGamesScript.UnlockAchievement(GPGSIds.achievement_new_look);
                PlayerPrefs.SetInt(Coins, PlayerPrefs.GetInt(Coins) - cost);
                hasBeige = true;
            }
        }
    }

    public void ShowRewardedAd()
    {
        AdManager.PlayRewardedVideoAd();
    }

    public void ShowAchievements()
    {
        PlayGamesScript.ShowAchievements();
    }
    
    public void ShowLeaderboard()
    {
        PlayGamesScript.ShowLeaderboard();
    }
}