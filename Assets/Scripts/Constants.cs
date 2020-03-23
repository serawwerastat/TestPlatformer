using System;
using System.Collections.Generic;

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
     * level 8 - ?
     * level 9 - ?
     * level 10 - ?
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
        private const float LevelWorstTime10 = 10f;
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
}