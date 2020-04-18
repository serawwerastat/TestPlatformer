using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;

public class PlayGamesScript : MonoBehaviour
{
    public static PlayGamesScript Instance { set; get; }

    private bool isLogedIn;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            isLogedIn = false;
            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.Activate();
            SignIn();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void SignIn()
    {
        if (!isLogedIn)
        {
            Social.localUser.Authenticate(success => { isLogedIn = success; });
        }
    }

    public static void SignOut()
    {
        PlayGamesPlatform.Instance.SignOut();
    }

    #region Achivment

    public static void UnlockAchievement(string id)
    {
        Social.ReportProgress(id, 100, success => { });
    }

    public static void IncrementAchievement(string id, int stepsToIncrement)
    {
        PlayGamesPlatform.Instance.IncrementAchievement(id, stepsToIncrement, success => { });
    }

    public static void ShowAchievements()
    {
        Social.ShowAchievementsUI();
    }

    #endregion /Achievement

    #region Leaderboards

    public static void AddScoreToLeaderboard(string leaderboardId, long score)
    {
        Social.ReportScore(score, leaderboardId, success => { });
    }

    public static void ShowLeaderboard()
    {
        Social.ShowLeaderboardUI();
    }

    #endregion
}