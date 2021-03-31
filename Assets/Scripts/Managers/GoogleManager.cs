using UnityEngine;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class GoogleManager : MonoBehaviour 
{	
	public static GoogleManager instance;

	private void Awake()
	{
		if (instance != null) Destroy(gameObject);
		else { instance = this; DontDestroyOnLoad(gameObject); }
	}

	private void Start()
	{
		GooglePlayActivate();
	}

	public void UnlockAchievement(int achievementID)
	{
		string scoreID = "";

		switch(achievementID)
		{
			case 25: scoreID = KGPS.achievement_score_25; break;
			case 50: scoreID = KGPS.achievement_score_50; break;
			case 100: scoreID = KGPS.achievement_score_100; break;
			case 200: scoreID = KGPS.achievement_score_200; break;
			case 500: scoreID = KGPS.achievement_score_500; break;
			case 750: scoreID = KGPS.achievement_score_750; break;
			case 1000: scoreID = KGPS.achievement_score_1000; break;

			case 0: scoreID = KGPS.achievement_join_the_kubrick; break;
		}

		Social.ReportProgress(scoreID, 100.0f, (bool success) =>
		{
			Debug.Log("Progress has been successfully reported");
		});
	}

	public void DeclareScore(int score)
	{
		Social.ReportScore(score, KGPS.leaderboard_leaderboard, (bool success) =>
		{
			Debug.Log("Score has been successfully reported");
		});
	}

	public void LeaderBoard()
	{
		if(Social.localUser.authenticated)
		{
			DeclareScore(PlayerPrefs.GetInt("Highscore"));
			Social.ShowLeaderboardUI();
		}
		else
		{
			GooglePlayActivate();
			GooglePlayConnect();

			Social.ShowLeaderboardUI();
		}
	}

	public void Achievement()
	{
		if(Social.localUser.authenticated)
		{
			Social.ShowAchievementsUI();
		}
		else
		{
			GooglePlayActivate();
			GooglePlayConnect();

			Social.ShowAchievementsUI();
		}
	}

	public void GooglePlayActivate()
	{
		PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();         
		PlayGamesPlatform.InitializeInstance(config);
     	PlayGamesPlatform.DebugLogEnabled = true;
		PlayGamesPlatform.Activate();
		
		GooglePlayConnect();
	}

	public void GooglePlayConnect()
	{
		Social.localUser.Authenticate((bool success) => {
			if (success) {
				Debug.Log("You've successfully logged in");
				UnlockAchievement(0);
			} else {
				Debug.Log("Login failed for some reason");
			}
		});
		
		UnlockAchievement(0);
	}

}

