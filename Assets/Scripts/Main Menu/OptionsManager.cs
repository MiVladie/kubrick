using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour 
{
	public GameObject Logo;
	public GameObject Start;
	public GameObject Options;
	public GameObject MainMusic;

	public GameObject GameOverPanel;
	public GameObject PausePanel;
	
	public GameObject SettingsMenu;

	private int index;

	public void SettingsOpen()
	{
		if(GameOverPanel != null && PausePanel != null)
			if(GameOverPanel.activeSelf) index = 0;
			else if(PausePanel.activeSelf) index = 1;

		StartCoroutine(Settings(true));
	}

	public void SettingsClose()
	{
		StartCoroutine(Settings(false));
	}

	private IEnumerator Settings(bool toOpen)
	{
		if(toOpen)
		{
			SettingsMenu.SetActive(true);

			Animation anim = SettingsMenu.GetComponent<Animation>();
			anim["Settings"].speed = 1f;
			anim.Play();

			if(GameOverPanel == null)
			{
				yield return new WaitForSeconds(0.2f);

				Logo.SetActive(false);
				Start.SetActive(false);
				Options.SetActive(false);
			}
			else
			{
				if(index == 0)
					GameOverPanel.SetActive(false);
				else if(index == 1)
					PausePanel.SetActive(false);
			}
		}
		else
		{
			Animation anim = SettingsMenu.GetComponent<Animation>();
			anim["Settings"].speed = -1f;
			anim["Settings"].time = 0.3f;
			anim.Play();

			if(GameOverPanel == null)
			{
				yield return new WaitForSeconds(0.2f);

				Logo.SetActive(true);
				Start.SetActive(true);
				Options.SetActive(true);

				Logo.transform.GetChild(0).GetComponent<Animation>().Play();
				Start.transform.GetChild(0).GetComponent<Animation>().Play();
				
				SettingsMenu.SetActive(false);				
			}
			else
			{
				if(index == 0)
					GameOverPanel.SetActive(true);
				else if(index == 1)
					PausePanel.SetActive(true);
									
				yield return new WaitForSeconds(0.35f);
				SettingsMenu.SetActive(false);	
			}
		}
	}

	#region Settings

	public void MusicMain()
	{
		if(SettingsMenu.transform.GetChild(0).GetChild(2).GetChild(0).gameObject.activeSelf)
		{
			SettingsMenu.transform.GetChild(0).GetChild(2).GetChild(0).gameObject.SetActive(false);
			SettingsMenu.transform.GetChild(0).GetChild(2).GetChild(1).gameObject.SetActive(true);
			
			MainMusic.transform.GetChild(0).gameObject.SetActive(false);
			MainMusic.transform.GetChild(1).gameObject.SetActive(true);

			PlayerPrefs.SetInt("Music", 0);
			FindObjectOfType<AudioManager>().Stop("Theme");
		}
		else
		{
			SettingsMenu.transform.GetChild(0).GetChild(2).GetChild(0).gameObject.SetActive(true);
			SettingsMenu.transform.GetChild(0).GetChild(2).GetChild(1).gameObject.SetActive(false);		
			
			MainMusic.transform.GetChild(0).gameObject.SetActive(true);
			MainMusic.transform.GetChild(1).gameObject.SetActive(false);

			PlayerPrefs.SetInt("Music", 1);	
			
			FindObjectOfType<AudioManager>().Play("Theme");
		}
	}

	public void Sound()
	{
		if(SettingsMenu.transform.GetChild(0).GetChild(1).GetChild(0).gameObject.activeSelf)
		{
			SettingsMenu.transform.GetChild(0).GetChild(1).GetChild(0).gameObject.SetActive(false);
			SettingsMenu.transform.GetChild(0).GetChild(1).GetChild(1).gameObject.SetActive(true);

			PlayerPrefs.SetInt("Sound", 0);
		}
		else
		{
			SettingsMenu.transform.GetChild(0).GetChild(1).GetChild(0).gameObject.SetActive(true);
			SettingsMenu.transform.GetChild(0).GetChild(1).GetChild(1).gameObject.SetActive(false);			
			
			PlayerPrefs.SetInt("Sound", 1);
		}
	}

	public void Music()
	{
		if(SettingsMenu.transform.GetChild(0).GetChild(2).GetChild(0).gameObject.activeSelf)
		{
			SettingsMenu.transform.GetChild(0).GetChild(2).GetChild(0).gameObject.SetActive(false);
			SettingsMenu.transform.GetChild(0).GetChild(2).GetChild(1).gameObject.SetActive(true);

			if(Logo != null)
			{
				MainMusic.transform.GetChild(0).gameObject.SetActive(false);
				MainMusic.transform.GetChild(1).gameObject.SetActive(true);
			}

			PlayerPrefs.SetInt("Music", 0);
			FindObjectOfType<AudioManager>().Stop("Theme");
		}
		else
		{
			SettingsMenu.transform.GetChild(0).GetChild(2).GetChild(0).gameObject.SetActive(true);
			SettingsMenu.transform.GetChild(0).GetChild(2).GetChild(1).gameObject.SetActive(false);		
			
			if(Logo != null)
			{
				MainMusic.transform.GetChild(0).gameObject.SetActive(true);
				MainMusic.transform.GetChild(1).gameObject.SetActive(false);				
			}

			PlayerPrefs.SetInt("Music", 1);	
			FindObjectOfType<AudioManager>().Play("Theme");
		}
	}
	
	public void Vibrations()
	{
		if(SettingsMenu.transform.GetChild(0).GetChild(3).GetChild(0).gameObject.activeSelf)
		{
			SettingsMenu.transform.GetChild(0).GetChild(3).GetChild(0).gameObject.SetActive(false);
			SettingsMenu.transform.GetChild(0).GetChild(3).GetChild(1).gameObject.SetActive(true);

			PlayerPrefs.SetInt("Vibration", 0);
		}
		else
		{
			SettingsMenu.transform.GetChild(0).GetChild(3).GetChild(0).gameObject.SetActive(true);
			SettingsMenu.transform.GetChild(0).GetChild(3).GetChild(1).gameObject.SetActive(false);	
			
			PlayerPrefs.SetInt("Vibration", 1);		
		}
	}

	public void Facebook()
	{
		Application.OpenURL("https://www.facebook.com/Voelapp");
	}
	
	public void Twitter()
	{
		Application.OpenURL("https://www.twitter.com/Voelapp");
	}
	
	public void Youtube()
	{
		Application.OpenURL("https://www.youtube.com/VoelappStudio");
	}
	
	public void MoreGames()
	{
		Application.OpenURL("https://play.google.com/store/apps/developer?id=Voelapp");
	}

	public void UpdateSettings()
	{
		if(Logo != null)
		{
			if(PlayerPrefs.GetInt("Music") == 0)
			{
				SettingsMenu.transform.GetChild(0).GetChild(2).GetChild(0).gameObject.SetActive(false);
				SettingsMenu.transform.GetChild(0).GetChild(2).GetChild(1).gameObject.SetActive(true);

				MainMusic.transform.GetChild(0).gameObject.SetActive(false);
				MainMusic.transform.GetChild(1).gameObject.SetActive(true);
			}
			else
			{
				SettingsMenu.transform.GetChild(0).GetChild(2).GetChild(0).gameObject.SetActive(true);
				SettingsMenu.transform.GetChild(0).GetChild(2).GetChild(1).gameObject.SetActive(false);
				
				MainMusic.transform.GetChild(0).gameObject.SetActive(true);
				MainMusic.transform.GetChild(1).gameObject.SetActive(false);
			}
		}

		if(PlayerPrefs.GetInt("Sound") == 0)
		{
			SettingsMenu.transform.GetChild(0).GetChild(1).GetChild(0).gameObject.SetActive(false);
			SettingsMenu.transform.GetChild(0).GetChild(1).GetChild(1).gameObject.SetActive(true);
		}
		else
		{
			SettingsMenu.transform.GetChild(0).GetChild(1).GetChild(0).gameObject.SetActive(true);
			SettingsMenu.transform.GetChild(0).GetChild(1).GetChild(1).gameObject.SetActive(false);
		}
		
		if(PlayerPrefs.GetInt("Music") == 0)
		{
			SettingsMenu.transform.GetChild(0).GetChild(2).GetChild(0).gameObject.SetActive(false);
			SettingsMenu.transform.GetChild(0).GetChild(2).GetChild(1).gameObject.SetActive(true);
		}
		else
		{
			SettingsMenu.transform.GetChild(0).GetChild(2).GetChild(0).gameObject.SetActive(true);
			SettingsMenu.transform.GetChild(0).GetChild(2).GetChild(1).gameObject.SetActive(false);
		}
		
		if(PlayerPrefs.GetInt("Vibration") == 0)
		{
			SettingsMenu.transform.GetChild(0).GetChild(3).GetChild(0).gameObject.SetActive(false);
			SettingsMenu.transform.GetChild(0).GetChild(3).GetChild(1).gameObject.SetActive(true);
		}
		else
		{
			SettingsMenu.transform.GetChild(0).GetChild(3).GetChild(0).gameObject.SetActive(true);
			SettingsMenu.transform.GetChild(0).GetChild(3).GetChild(1).gameObject.SetActive(false);
		}
	}

	#endregion

	public void Achievement()
	{
		FindObjectOfType<GoogleManager>().Achievement();
	}

	public void Leaderboard()
	{
		FindObjectOfType<GoogleManager>().LeaderBoard();
	}


}

