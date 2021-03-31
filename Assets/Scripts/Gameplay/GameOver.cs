using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour 
{
	public GameObject BackgroundMain;
	public GameObject BackgroundMorning;

	public GameObject GameOverPanel;

	public GameObject Pause;
	public GameObject PausePanel;

	public Text ScoreInGameOverPanel;
	public Text HighscoreInGameOverPanel;
	public Text Price;
	
	public GameObject Player;
	public GameObject Platforms;
	public GameObject ScoreInPlay;

	private int ContinuedGame = 0;
	private bool AllowPress = true;

	public void GameOverAppear()
	{
		Application.targetFrameRate = 80;
		
		// Displaying scores
		ScoreInGameOverPanel.text = ScoreInPlay.GetComponent<Text>().text;
		HighscoreInGameOverPanel.text = "BEST: " + (PlayerPrefs.GetInt("Highscore")).ToString();

		// Checking whether we hit a highscore 
		if(Int32.Parse(ScoreInGameOverPanel.text) >= PlayerPrefs.GetInt("Highscore"))
		{
			GameOverPanel.transform.GetChild(0).gameObject.SetActive(false);
			GameOverPanel.transform.GetChild(1).gameObject.SetActive(true);
		}
		else
		{
			GameOverPanel.transform.GetChild(0).gameObject.SetActive(true);
			GameOverPanel.transform.GetChild(1).gameObject.SetActive(false);			
		}

		StartCoroutine(GameOverAnimations(true));	

		CheckGems();
	}
	
	public void GameOverDisappear()
	{
		StartCoroutine(GameOverAnimations(false));
	}

	public void ContinuePlaying(bool ad)
	{
		if(ad)
		{		
			if(PlayerPrefs.GetInt("Ads") == 1)	
				FindObjectOfType<AdsManager>().ShowRewardBasedVideo();
			else
				RewardedPlay();	
		}
		else
		{
			// Billing -100 gems
			int gems = PlayerPrefs.GetInt("Gems"); gems -= Int32.Parse(Price.text) + 1;
			PlayerPrefs.SetInt("Gems", gems);	
			FindObjectOfType<ScoreManager>().GemUpdate();

	 		ContinuedGame++;

			RewardedPlay();		
		}
		
	}	

	public void RewardedPlay()
	{
		Price.text = (Int32.Parse(Price.text) + 50).ToString();

		//FindObjectOfType<AudioManager>().Play("Purchase");	
		GameOverDisappear();		
	}

	public void Restart()
	{
		StartCoroutine(Exiting("Gameplay"));
	}

	private IEnumerator Exiting(string scene)
	{
		AllowPress = false;

		if(GameOverPanel.activeSelf)
		{
			Animation anim = GameOverPanel.GetComponent<Animation>();
			anim["Game Over"].speed = -1f;
			anim["Game Over"].time = 0.35f;
			anim.Play();
		}
		else
		{
			Animation anim = PausePanel.GetComponent<Animation>();
			anim["Pause"].speed = -1f;
			anim["Pause"].time = 0.35f;
			anim.Play();
		}
		
		float journey = 0;
		float time = 1f;

		Color32 from = new Color32(255, 255, 255, 128);
		Color32 to = new Color32(255, 255, 255, 0);

		BackgroundMorning.SetActive(true);
		BackgroundMain.SetActive(true);

		while (journey < time)
		{
			journey += Time.deltaTime;

			GameOverPanel.transform.parent.gameObject.GetComponent<Image>().color = Color32.Lerp(from, to, journey / time);

			if(scene == "Gameplay")
				BackgroundMorning.GetComponent<CanvasGroup>().alpha = journey / time;
			else if(scene == "Main Menu")
				BackgroundMain.GetComponent<CanvasGroup>().alpha = journey / time;

			yield return null;
		}
		
		yield return new WaitForSeconds(0.5f);

		SceneManager.LoadScene(scene);

	}

	private IEnumerator ContinuePlayingTimer()
	{
		// Countdown
		float journey = 0f;
		float time = 5f;

		while(journey < time)
		{
			journey += Time.deltaTime;

			GameOverPanel.transform.GetChild(4).GetChild(0).GetChild(0).gameObject.GetComponent<Image>().fillAmount = 1 - journey/time;

			yield return null;
		}

		// Displaying proper menu
		GameOverPanel.transform.GetChild(4).gameObject.SetActive(false);
		GameOverPanel.transform.GetChild(5).gameObject.SetActive(false);
		GameOverPanel.transform.GetChild(6).gameObject.SetActive(true);
	}

	private IEnumerator GameOverAnimations(bool toOpen)
	{
		if(toOpen)
		{
			// Enabling Non-Play Menus
			GameOverPanel.transform.parent.gameObject.SetActive(true);

			// Hiding gameplay UI
			ScoreInPlay.SetActive(false);
			Player.GetComponent<MeshRenderer>().enabled = false;
			for(int i = 0; i < Platforms.transform.childCount; i++)
			{
				Platforms.transform.GetChild(i).gameObject.GetComponent<MeshRenderer>().enabled = false;
				for(int y = 0; y < Platforms.transform.GetChild(i).childCount; y++)
					Platforms.transform.GetChild(i).GetChild(y).gameObject.GetComponent<MeshRenderer>().enabled = false;
			}
			Pause.SetActive(false);

			GameOverPanel.SetActive(true);

			Animation anim = GameOverPanel.GetComponent<Animation>();
			anim["Game Over"].speed = 1f;
			anim.Play();

			float journey = 0;
			float time = 0.5f;

			Color32 from = new Color32(255, 255, 255, 0);
			Color32 to = new Color32(255, 255, 255, 128);

			while (journey < time)
			{
				journey += Time.deltaTime;

				GameOverPanel.transform.parent.gameObject.GetComponent<Image>().color = Color32.Lerp(from, to, journey / time);

				yield return null;
			}
			
			FindObjectOfType<AdsManager>().ShowInterstitial();
			FindObjectOfType<AdsManager>().RequestInterstitial();
		}
		else
		{
			Animation anim = GameOverPanel.GetComponent<Animation>();
			anim["Game Over"].speed = -1f;
			anim["Game Over"].time = 0.35f;
			anim.Play();
			
			float journey = 0;
			float time = 0.5f;

			Color32 from = new Color32(255, 255, 255, 128);
			Color32 to = new Color32(255, 255, 255, 0);

			while (journey < time)
			{
				journey += Time.deltaTime;

				GameOverPanel.transform.parent.gameObject.GetComponent<Image>().color = Color32.Lerp(from, to, journey / time);

				yield return null;
			}

			GameOverPanel.SetActive(false);

			// Disabling Non-Play Menus
			GameOverPanel.transform.parent.gameObject.SetActive(false);

			ScoreInPlay.SetActive(true);
			Player.GetComponent<MeshRenderer>().enabled = true;
			for(int i = 0; i < Platforms.transform.childCount; i++)
			{
				Platforms.transform.GetChild(i).gameObject.GetComponent<MeshRenderer>().enabled = true;
				for(int y = 0; y < Platforms.transform.GetChild(i).childCount; y++)
					Platforms.transform.GetChild(i).GetChild(y).gameObject.GetComponent<MeshRenderer>().enabled = true;
			}	
			Pause.SetActive(true);	

			// Continuing the game
			FindObjectOfType<PlayerMoving>().ContinuePlaying();		
		}

		yield return null;
	}

	public void PauseGame(bool pause)
	{
		if(!AllowPress)
			return;

		if(Player.GetComponent<MeshRenderer>().enabled)
			if(!FindObjectOfType<PlayerMoving>().AllowRotation)
				return;

		StartCoroutine(PauseTheGame(pause));
	}

	private IEnumerator PauseTheGame(bool pause)
	{
		AllowPress = false;

		if(pause)
		{
			FindObjectOfType<PlayerMoving>().GameOver = true;

			// Hiding gameplay UI
			ScoreInPlay.SetActive(false);
			Player.GetComponent<MeshRenderer>().enabled = false;
			for(int i = 0; i < Platforms.transform.childCount; i++)
			{
				Platforms.transform.GetChild(i).gameObject.GetComponent<MeshRenderer>().enabled = false;
				for(int y = 0; y < Platforms.transform.GetChild(i).childCount; y++)
					Platforms.transform.GetChild(i).GetChild(y).gameObject.GetComponent<MeshRenderer>().enabled = false;
			}	
			Pause.SetActive(false);

			if(FindObjectOfType<ScoreManager>().Afternoon.activeSelf && FindObjectOfType<ScoreManager>().Afternoon.GetComponent<Image>().color.a >= 0.5f)
			{
				PausePanel.transform.GetChild(1).gameObject.GetComponent<Image>().color = new Color32(97, 255, 199, 255); // Resume
				PausePanel.transform.GetChild(2).gameObject.GetComponent<Image>().color = new Color32(89, 255, 158, 255); // Home
				PausePanel.transform.GetChild(3).gameObject.GetComponent<Image>().color = new Color32(115, 255, 125, 255); // Shop
				PausePanel.transform.GetChild(4).gameObject.GetComponent<Image>().color = new Color32(107, 255, 79, 255); // Settings
				PausePanel.transform.GetChild(5).gameObject.GetComponent<Image>().color = new Color32(127, 255, 102, 255); // Exit Game
			}
			else if(FindObjectOfType<ScoreManager>().Evening.activeSelf && FindObjectOfType<ScoreManager>().Evening.GetComponent<Image>().color.a >= 0.5f)
			{
				PausePanel.transform.GetChild(1).gameObject.GetComponent<Image>().color = new Color32(175, 118, 237, 255); // Resume
				PausePanel.transform.GetChild(2).gameObject.GetComponent<Image>().color = new Color32(138, 103, 244, 255); // Home
				PausePanel.transform.GetChild(3).gameObject.GetComponent<Image>().color = new Color32(119, 132, 254, 255); // Shop
				PausePanel.transform.GetChild(4).gameObject.GetComponent<Image>().color = new Color32(91, 142, 246, 255); // Settings
				PausePanel.transform.GetChild(5).gameObject.GetComponent<Image>().color = new Color32(113, 157, 247, 255); // Exit Game
			}
			else if(FindObjectOfType<ScoreManager>().Night.activeSelf && FindObjectOfType<ScoreManager>().Night.GetComponent<Image>().color.a >= 0.5f)
			{
				PausePanel.transform.GetChild(1).gameObject.GetComponent<Image>().color = new Color32(178, 97, 255, 255); // Resume
				PausePanel.transform.GetChild(2).gameObject.GetComponent<Image>().color = new Color32(136, 89, 255, 255); // Home
				PausePanel.transform.GetChild(3).gameObject.GetComponent<Image>().color = new Color32(115, 124, 255, 255); // Shop
				PausePanel.transform.GetChild(4).gameObject.GetComponent<Image>().color = new Color32(102, 147, 255, 255); // Settings
				PausePanel.transform.GetChild(5).gameObject.GetComponent<Image>().color = new Color32(79, 131, 255, 255); // Exit Game
			}

			// Enabling Non-Play Menus
			GameOverPanel.transform.parent.gameObject.SetActive(true);
			PausePanel.SetActive(true);

			Animation anim = PausePanel.GetComponent<Animation>();
			anim["Pause"].speed = 1;
			anim.Play();

			float journey = 0;
			float time = 0.5f;

			Color32 from = new Color32(255, 255, 255, 0);
			Color32 to = new Color32(255, 255, 255, 128);

			while (journey < time)
			{
				journey += Time.deltaTime;
				GameOverPanel.transform.parent.gameObject.GetComponent<Image>().color = Color32.Lerp(from, to, journey / time);
				yield return null;
			}					
		}
		else
		{
			Animation anim = PausePanel.GetComponent<Animation>();
			anim["Pause"].speed = -1;
			anim["Pause"].time = 0.5f;
			anim.Play();

			yield return new WaitForSeconds(0.35f);

			ScoreInPlay.SetActive(true);
			Player.GetComponent<MeshRenderer>().enabled = true;
			for(int i = 0; i < Platforms.transform.childCount; i++)
			{
				Platforms.transform.GetChild(i).gameObject.GetComponent<MeshRenderer>().enabled = true;
				for(int y = 0; y < Platforms.transform.GetChild(i).childCount; y++)
					Platforms.transform.GetChild(i).GetChild(y).gameObject.GetComponent<MeshRenderer>().enabled = true;
			}

			FindObjectOfType<PlayerMoving>().GameOver = false;

			float journey = 0; float time = 0.5f;
			Color32 from = new Color32(255, 255, 255, 128); Color32 to = new Color32(255, 255, 255, 0);
			while (journey < time)
			{
				journey += Time.deltaTime;
				GameOverPanel.transform.parent.gameObject.GetComponent<Image>().color = Color32.Lerp(from, to, journey / time);
				yield return null;
			}

			// Disabling Non-Play Menus
			if(!FindObjectOfType<PlayerMoving>().GameOver)
			{
				GameOverPanel.transform.parent.gameObject.SetActive(false);			
				Pause.SetActive(true);	
			}

			PausePanel.SetActive(false);			
		}

		AllowPress = true;
	}

	public void CheckGems()
	{
		bool adReady = true;

		if(FindObjectOfType<AdsManager>().isRewardBasedVideoLoaded() != null)
			adReady = FindObjectOfType<AdsManager>().isRewardBasedVideoLoaded();

		// Displaying proper options
		if(PlayerPrefs.GetInt("Gems") >= Int32.Parse(Price.text))
		{
			GameOverPanel.transform.GetChild(4).gameObject.SetActive(false);
			GameOverPanel.transform.GetChild(5).gameObject.SetActive(true);	
			GameOverPanel.transform.GetChild(6).gameObject.SetActive(false);
		}
		else if(ContinuedGame == 0 && adReady)
		{
			GameOverPanel.transform.GetChild(4).gameObject.SetActive(true);
			GameOverPanel.transform.GetChild(5).gameObject.SetActive(false);	
			StartCoroutine(ContinuePlayingTimer());
		}
		else
		{
			GameOverPanel.transform.GetChild(4).gameObject.SetActive(false);
			GameOverPanel.transform.GetChild(5).gameObject.SetActive(false);
			GameOverPanel.transform.GetChild(6).gameObject.SetActive(true);		
			StartCoroutine(ContinuePlayingTimer());
		}
	}

	#region Options

	public void Home()
	{
		if(!AllowPress)
			return;

		FindObjectOfType<AdsManager>().HideBanner();

		StartCoroutine(Exiting("Main Menu"));
	}

	public void Share()
	{
		if(!AllowPress)
			return;
			
		FindObjectOfType<ShareManager>().ShareScreenshot(ScoreInGameOverPanel.text);
	}

	public void ExitGame()
	{
		Application.Quit();
	}

	#endregion

	public void PlaySound(string sound)
	{
		FindObjectOfType<AudioManager>().Play(sound);
	}

}

