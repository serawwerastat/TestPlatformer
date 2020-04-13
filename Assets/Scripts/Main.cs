using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using GooglePlayGames;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using static DefaultNamespace.Constants;
using static DefaultNamespace.Timer;

public class Main : MonoBehaviour
{

    public Player player;
    public Text coinText;
    public Image[] hearts;
    public Sprite isLife, noLife;
    public GameObject pauseScreen;
    public GameObject winScreen;
    public GameObject loseScreen;
    public GameObject bonusPanel;
    public GameObject newRecordPanel;
    public Button pauseButton;
    private float timer;
    public Text timeText;
    public TimeWork timeWork;
    public float countDown;
    public SoundEffector soundEffector;
    public AudioSource musicSource, soundSource;
    private int bonusReward;
    private int levelNumber;
    public void ReloadLvl()
    {
        AdManager.ShowAddAfterDeath();
        Time.timeScale = 1f;
        player.enabled = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Start()
    {
        levelNumber = SceneManager.GetActiveScene().buildIndex;
        musicSource.volume = PlayerPrefs.GetInt(MusicVolume) / 9f;
        soundSource.volume = PlayerPrefs.GetInt(SoundVolume) / 9f;
        if (timeWork == TimeWork.Timer)
        {
            timer = countDown;
        }
    }

    public void Update()
    {
        coinText.text = player.GetCoins().ToString();

        for (int i = 0; i < hearts.Length; i++)
        {
            if (player.GetHp() > i)
            {
                hearts[i].sprite = isLife;
            }
            else
            {
                hearts[i].sprite = noLife;
            }
        }

        if (timeWork == TimeWork.StopWatch)
        {
            timer += Time.deltaTime;
            timeText.text = GetTimerString(timer);
        }
        else if(timeWork == TimeWork.Timer)
        {
            timer -= Time.deltaTime;
            timeText.text = GetTimerString(timer);
            if (timer <= 0)
            {
                Lose();
            }
        }
        else
        {
            timeText.gameObject.SetActive(false);
        }
    }
    
    public void PauseOn()
    {
        Time.timeScale = 0f;
        player.enabled = false;
        pauseScreen.SetActive(true);
    }

    public void PauseOff()
    {
        Time.timeScale = 1f;
        player.enabled = true;
        pauseScreen.SetActive(false);
    }

    public void Win()
    {
        pauseButton.interactable = false;
        soundEffector.PlayWinSound();
        Time.timeScale = 0f;
        player.enabled = false;
        winScreen.SetActive(true);
        
        if (!PlayerPrefs.HasKey(Level) || PlayerPrefs.GetInt(Level) < levelNumber)
        {
            if (levelNumber == 1)
            {
                PlayGamesScript.UnlockAchievement(GPGSIds.achievement_first_step);
            }
            if (levelNumber == 10)
            {
                PlayGamesScript.UnlockAchievement(GPGSIds.achievement_runner);
            }
            PlayerPrefs.SetInt(Level,  SceneManager.GetActiveScene().buildIndex);
        }

        if (GetComponent<Inventory>().AreAllGemsCollected())
        {
            bonusPanel.SetActive(true);
            bonusReward = 500;
            PlayGamesScript.UnlockAchievement(GPGSIds.achievement_collector);
        }
        if (PlayerPrefs.HasKey(Coins))
        {
            PlayerPrefs.SetInt(Coins, PlayerPrefs.GetInt(Coins) + player.GetCoins() + bonusReward);
        }
        else
        {
            PlayerPrefs.SetInt(Coins, player.GetCoins() + bonusReward);
        }
        GetComponent<Inventory>().RecountItems();

        if (AchievementHelper.AreAllGemsCollected())
        {
            PlayGamesScript.UnlockAchievement(GPGSIds.achievement_explorer);
        }

        if (PlayerPrefs.HasKey(Constants.Timer + levelNumber))
        {
            var bestTime = PlayerPrefs.GetFloat(Constants.Timer + levelNumber);
            if (bestTime > timer)
            {
                setRecordAndCheckAchievements();
            }
        }
        else
        {
            setRecordAndCheckAchievements();
        }
    }

    private void setRecordAndCheckAchievements()
    {
        PlayerPrefs.SetFloat(Constants.Timer + levelNumber, timer);
        PlayGamesScript.AddScoreToLeaderboard(levelLeaderboards[levelNumber], 
            (long) (timer * 1000));
        if (AchievementHelper.IsGoldenMedal(timer, levelNumber))
        {
            PlayGamesScript.UnlockAchievement(GPGSIds.achievement_champion);
        }
        if (AchievementHelper.AreAllGoldenMedals())
        {
            PlayGamesScript.UnlockAchievement(GPGSIds.achievement_speed_runner);
        }
        long score = AchievementHelper.RecountScore();
        PlayGamesScript.AddScoreToLeaderboard(GPGSIds.leaderboard_overall, score);
        newRecordPanel.SetActive(true);
    }
    
    public void Lose()
    {
        // musicSource.volume = 0;
        // soundSource.volume = 0;
        AdManager.ShowAddAfterDeath();
        // musicSource.volume = PlayerPrefs.GetInt(MusicVolume) / 9f;
        // soundSource.volume = PlayerPrefs.GetInt(SoundVolume) / 9f;
        
        pauseButton.interactable = false;
        soundEffector.PlayLoseSound();
        Time.timeScale = 0f;
        player.enabled = false;
        loseScreen.SetActive(true);
    }

    public void OpenMainMenu()
    {
        Time.timeScale = 1f;
        player.enabled = true;
        SceneManager.LoadScene(MainMenu);
    }

    public void NextLvl()
    {
        Time.timeScale = 1f;
        player.enabled = true;
        SceneManager.LoadScene(levelNumber + 1);
    }
}

public enum TimeWork{
None,
StopWatch,
Timer,
}