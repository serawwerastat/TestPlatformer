using System.Collections;
using System.Collections.Generic;
using Unity.UNetWeaver;
using static DefaultNamespace.Constants;
using static DefaultNamespace.Timer;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
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

    private Button activeSkinButton;
    private bool hasPink;
    private bool hasGreen;
    private bool hasYellow;
    private bool hasBeige;

    private void Start()
    {
        //TODO Remove
        // PlayerPrefs.SetInt(Coins, 100);

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

    private void Update()
    {
        PlayerPrefs.SetInt(MusicVolume, (int) musicSlided.value);
        PlayerPrefs.SetInt(SoundVolume, (int) soundSlider.value);
        musicText.text = musicSlided.value.ToString();
        soundText.text = soundSlider.value.ToString();
        if (PlayerPrefs.HasKey(Coins))
        {
            coinText.text = PlayerPrefs.GetInt(Coins).ToString();
        }
        else
        {
            coinText.text = "0";
        }
    }

    public void OpenScene(int index)
    {
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
                PlayerPrefs.SetInt(Coins, PlayerPrefs.GetInt(Coins) - cost);
                hasBeige = true;
            }
        }
    }
}