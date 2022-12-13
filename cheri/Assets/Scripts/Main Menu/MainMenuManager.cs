using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour 
{
	private bool AllowPress = true;

	public GameObject BackgroundMorning;

	public GameObject MainMenu;

	public GameObject Logo;
	public GameObject StartGame;
	public GameObject Options;

	private void Start()
	{
		Initialisation();

		Application.targetFrameRate = 80;
	}

	private void Initialisation()
	{
		Logo.transform.GetChild(0).gameObject.GetComponent<Animation>().Play();
		StartGame.transform.GetChild(0).gameObject.GetComponent<Animation>().Play();

		// Resetting
		if(!PlayerPrefs.HasKey("Gems"))
		{
			PlayerPrefs.SetInt("Gems", 0);
			PlayerPrefs.SetInt("Highscore", 0);
			PlayerPrefs.SetInt("Sound", 1);
			PlayerPrefs.SetInt("Music", 1);
			PlayerPrefs.SetInt("Ads", 1);
			
			PlayerPrefs.SetString("Cubes", "2000000");
		}

		FindObjectOfType<OptionsManager>().UpdateSettings();
	}

	public void StartTheGame()
	{
		BackgroundMorning.SetActive(true);

		StartCoroutine(LoadGameplay());
	}

	public void OptionsOpen()
	{
		if(!AllowPress)
			return;

		AllowPress = false;

		Animation optionsFade = Options.transform.GetChild(0).gameObject.GetComponent<Animation>();

		if(!Options.transform.GetChild(0).GetChild(0).gameObject.activeSelf)
		{
			optionsFade["Options"].speed = 1f;	
		}
		else
		{	
			optionsFade["Options"].speed = -1f;
			optionsFade["Options"].time = 0.5f;	
		}

		optionsFade.Play();

		AllowPress = true;
	}

	private IEnumerator LoadGameplay()
	{
		Animation anim = MainMenu.GetComponent<Animation>();
		anim["Main Menu"].speed = -1f;
		anim["Main Menu"].time = 0.5f;
		anim.Play();

		float journey = 0f;
		float time = 1f;

		while(journey < time)
		{
			journey += Time.deltaTime;

			BackgroundMorning.GetComponent<CanvasGroup>().alpha = journey / time;

			yield return null;
		}

		SceneManager.LoadScene("Gameplay");	
	}

}

