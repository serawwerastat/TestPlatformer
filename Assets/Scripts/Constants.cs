using System;
using System.Collections.Generic;

namespace DefaultNamespace
{
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
        private const float LevelWorstTime1 = 10f;
        private const float LevelWorstTime2 = 10f;
        private const float LevelWorstTime3 = 10f;
        public static Dictionary<int, float> levelsWorstTime = new Dictionary<int, float>()
        {
            {1, LevelWorstTime1},
            {2, LevelWorstTime2},
            {3, LevelWorstTime3},
        };

        public static string GetTimerString(float timer)
        {
            int sec = Convert.ToInt32(timer);
            int min = sec / 60;
            return $"{min:D2}:{(sec % 60):D2}";
        }
    }
}