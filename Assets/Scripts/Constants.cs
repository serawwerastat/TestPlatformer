using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    /*
     * Gold amount:
     * level 1 - 34
     * level 2 - 37
     * level 3 - 47
     * level 4 - 48
     * level 5 - 65
     * level 6 - 47
     * level 7 - 65
     * level 8 - 68
     * level 9 - 160
     * level 10 - 153
     */
    public static class Constants
    {
        //current skin
        public const string SkinActive = "SkinActive";
        public const string SkinColorBlue = "Blue";
        public const string SkinColorGreen = "Green";
        public const string SkinColorYellow = "Yellow";
        public const string SkinColorPink = "Pink";

        public const string SkinColorBeige = "Beige";

        //buying skins
        public const string HasSkinBlue = "SkinBlue";
        public const string HasSkinGreen = "SkinGreen";
        public const string HasSkinYellow = "SkinYellow";
        public const string HasSkinPink = "SkinPink";
        public const string HasSkinBeige = "SkinBeige";

        //music config
        public const string MusicVolume = "MusicVolume";
        public const string SoundVolume = "SoundVolume";

        //game
        public const string Level = "Lvl";
        public const string Coins = "Coins";
        public const string Timer = "Timer";
        public const string HiddenPass = "HiddenPass";
        public const string Player = "Player";
        public const string Ground = "Ground";
        public const string KeyBox = "KeyBox";

        //collectibles
        public const string Coin = "Coin";
        public const string Heart = "Heart";
        public const string Mushroom = "Mushroom";
        public const string Ladder = "Ladder";
        public const string Door = "Door";

        public const string GemGreen = "GreenGem";
        public const string GemBlue = "BlueGem";
        public const string GemYellow = "YellowGem";
        public const string Key = "Key";

        public const string TimerButtonStart = "TimerButtonStart";
        public const string TimerButtonStop = "TimerButtonStop";

        public const string Icy = "Icy";
        public const string Lava = "Lava";
        public const string QuickSand = "QuickSand";
        public const string Trampoline = "Trampoline";

        //scene
        public const string MainMenu = "MainMenu";

        public static String[] levelLeaderboards =
        {
            "NO ZERO LEVEL",
            GPGSIds.leaderboard_level_1,
            GPGSIds.leaderboard_level_2,
            GPGSIds.leaderboard_level_3,
            GPGSIds.leaderboard_level_4,
            GPGSIds.leaderboard_level_5,
            GPGSIds.leaderboard_level_6,
            GPGSIds.leaderboard_level_7,
            GPGSIds.leaderboard_level_8,
            GPGSIds.leaderboard_level_9,
            GPGSIds.leaderboard_level_10
        };
    }

    public static class Timer
    {
        private const float LevelWorstTime1 = 100f;
        private const float LevelWorstTime2 = 125f;
        private const float LevelWorstTime3 = 300f;
        private const float LevelWorstTime4 = 350f;
        private const float LevelWorstTime5 = 425f;
        private const float LevelWorstTime6 = 440f;
        private const float LevelWorstTime7 = 750f;
        private const float LevelWorstTime8 = 225f;
        private const float LevelWorstTime9 = 90f;
        private const float LevelWorstTime10 = 525f;

        public static readonly Dictionary<int, float> LevelsWorstTime = new Dictionary<int, float>()
        {
            {1, LevelWorstTime1},
            {2, LevelWorstTime2},
            {3, LevelWorstTime3},
            {4, LevelWorstTime4},
            {5, LevelWorstTime5},
            {6, LevelWorstTime6},
            {7, LevelWorstTime7},
            {8, LevelWorstTime8},
            {9, LevelWorstTime9},
            {10, LevelWorstTime10},
        };

        public static string GetTimerString(float timer)
        {
            int sec = Convert.ToInt32(timer);
            int min = sec / 60;
            return $"{min:D2}:{(sec % 60):D2}";
        }
    }

    public static class AchievementHelper
    {
        public static long RecountScore()
        {
            long score = 0;
            int levelsCount = Timer.LevelsWorstTime.Count;
            int currentLevel = 1;
            while (currentLevel <= levelsCount)
            {
                int levelNumber = currentLevel;
                if (PlayerPrefs.HasKey(Constants.Timer + levelNumber))
                {
                    int multiplier = 1;
                    if (PlayerPrefs.HasKey(Constants.GemBlue + levelNumber) 
                        && PlayerPrefs.GetInt(Constants.GemBlue + levelNumber) == 1)
                    {
                        multiplier++;
                    }
                    if (PlayerPrefs.HasKey(Constants.GemGreen + levelNumber) 
                        && PlayerPrefs.GetInt(Constants.GemGreen + levelNumber) == 1)
                    {
                        multiplier++;
                    }
                    if (PlayerPrefs.HasKey(Constants.GemYellow + levelNumber) 
                        && PlayerPrefs.GetInt(Constants.GemYellow + levelNumber) == 1)
                    {
                        multiplier++;
                    }
                    var bestTime = PlayerPrefs.GetFloat(Constants.Timer + levelNumber);
                    var remainingTime = Timer.LevelsWorstTime[levelNumber] - bestTime;
                    score += (int) remainingTime * multiplier;
                }
                currentLevel++;
            }
            return score;
        }

        public static bool IsGoldenMedal(float timer, int levelNumber)
        {
            var worstTimeForCurrentLevel = Timer.LevelsWorstTime[levelNumber];
            var timeRatio = timer / worstTimeForCurrentLevel;
            if (timeRatio < 0.2f)
            {
                return true;
            }
            return false;
        }

        public static bool AreAllGoldenMedals()
        {
            long medalCount = 0;
            int levelsCount = Timer.LevelsWorstTime.Count;
            int currentLevel = 1;
            while (currentLevel <= levelsCount)
            {
                int levelNumber = currentLevel;
                if (!PlayerPrefs.HasKey(Constants.Timer + levelNumber))
                {
                    break;
                }
                var bestTime = PlayerPrefs.GetFloat(Constants.Timer + levelNumber);
                var worstTimeForCurrentLevel = Timer.LevelsWorstTime[levelNumber];
                var timeRatio = bestTime / worstTimeForCurrentLevel;
                if (!(timeRatio < 0.2f))
                {
                    break;
                }
                medalCount++;
                currentLevel++;
            }

            if (levelsCount == medalCount)
            {
                return true;
            }
            return false;
        }

        public static bool AreAllGemsCollected()
        {
            long gemsCount = 0;
            int levelsCount = Timer.LevelsWorstTime.Count;
            int currentLevel = 1;
            while (currentLevel <= levelsCount)
            {
                int levelNumber = currentLevel;
                if (PlayerPrefs.HasKey(Constants.GemBlue + levelNumber) 
                    && PlayerPrefs.GetInt(Constants.GemBlue + levelNumber) == 1)
                {
                    gemsCount++;
                }
                else
                {
                    break;
                }
                if (PlayerPrefs.HasKey(Constants.GemGreen + levelNumber) 
                    && PlayerPrefs.GetInt(Constants.GemGreen + levelNumber) == 1)
                {
                    gemsCount++;
                }
                else
                {
                    break;
                }
                if (PlayerPrefs.HasKey(Constants.GemYellow + levelNumber) 
                    && PlayerPrefs.GetInt(Constants.GemYellow + levelNumber) == 1)
                {
                    gemsCount++;
                }
                else
                {
                    break;
                }

                currentLevel++;
            }

            if (gemsCount == (levelsCount * 3))
            {
                return true;
            }

            return false;
        }
    }

    public static class SaveLoadManager
    {
        public static void SaveInt(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
        }
        
        public static int GetInt(string key)
        {
           return PlayerPrefs.GetInt(key);
        }
        
        public static void SaveFloat(string key, float value)
        {
            PlayerPrefs.SetFloat(key, value);
            
        }
        
        public static float GetFloat(string key)
        {
            return PlayerPrefs.GetFloat(key);
        }
        
        public static void SaveString(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
            
        }
        
        public static string GetString(string key)
        {
            return PlayerPrefs.GetString(key);
        }
    }
}