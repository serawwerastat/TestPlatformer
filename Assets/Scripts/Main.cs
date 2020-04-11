using System;
using System.Collections;
using System.Collections.Generic;
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
            PlayerPrefs.SetInt(Level,  SceneManager.GetActiveScene().buildIndex);
        }

        if (GetComponent<Inventory>().AreAllGemsCollected())
        {
            bonusPanel.SetActive(true);
            bonusReward = 500;
        }
        if (PlayerPrefs.HasKey(Coins))
        {
            PlayerPrefs.SetInt(Coins, PlayerPrefs.GetInt(Coins) + player.GetCoins() + bonusReward);
        }
        else
        {
            PlayerPrefs.SetInt(Coins, player.GetCoins() + bonusReward);
        }

        if (PlayerPrefs.HasKey(Timer + levelNumber))
        {
            var bestTime = PlayerPrefs.GetFloat(Timer + levelNumber);
            if (bestTime > timer)
            {
                PlayerPrefs.SetFloat(Timer + levelNumber, timer);
                newRecordPanel.SetActive(true);
            }
        }
        else
        {
            PlayerPrefs.SetFloat(Timer + levelNumber, timer);
            newRecordPanel.SetActive(true);
        }

        
        
        GetComponent<Inventory>().RecountItems();
    }
    
    public void Lose()
    {
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