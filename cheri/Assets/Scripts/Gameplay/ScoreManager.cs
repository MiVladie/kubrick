using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour 
{
	public GameObject Morning;
	public GameObject Afternoon;
	public GameObject Evening;
	public GameObject Night;

	public GameObject Clouds;

	public Text score;
	public Text highscore;
	public Text gems;

	public Text PlusGem;
	private bool FadeOut;
	private float period = 0;

	private void Start()
	{
		if(!PlayerPrefs.HasKey("Highscore")) PlayerPrefs.SetInt("Highscore", 0);
		if(!PlayerPrefs.HasKey("Gems")) PlayerPrefs.SetInt("Gems", 0);

		score.text = "0";
		highscore.text = "BEST: " + PlayerPrefs.GetInt("Highscore").ToString();
		gems.text = PlayerPrefs.GetInt("Gems").ToString();
	}

	private void Update()
	{
		if(FadeOut)
		{
			period += Time.deltaTime;
			PlusGem.color = Color.Lerp(new Color32(255, 124, 128, 255), new Color32(255, 124, 128, 0), period * 1.5f);
			PlusGem.gameObject.transform.localPosition += new Vector3(0, 0.25f, 0);
		}
			
		if(PlusGem.color.a < 0.1f && FadeOut)
		{
			PlusGem.gameObject.transform.localPosition = new Vector3(0, -320, 0);
			PlusGem.color = new Color32(255, 124, 128, 0);
			FadeOut = false;
			period = 0;
			PlusGem.text = "+0";
		}
	}

	public void ScoreUpdate(int platforms)
	{
		score.text = (platforms / 3 - 4).ToString();

		if(Int32.Parse(score.text) > PlayerPrefs.GetInt("Highscore"))
		{
			PlayerPrefs.SetInt("Highscore", Int32.Parse(score.text));
		}

		highscore.text = "BEST: " + PlayerPrefs.GetInt("Highscore").ToString();

		Checkpoint();
	}

	public void GemUpdate()
	{		
		PlusGem.text = "+" + (Int32.Parse(PlusGem.text) + 1).ToString();
		FadeOut = true;
		period = 0;
		PlusGem.gameObject.transform.localPosition += new Vector3(0, 3f, 0);

		int amount = PlayerPrefs.GetInt("Gems");
		amount++;
		PlayerPrefs.SetInt("Gems", amount);
		gems.text = amount.ToString();
	}

	public void CheckGems()
	{
		int amount = PlayerPrefs.GetInt("Gems");
		gems.text = amount.ToString();

		FindObjectOfType<GameOver>().CheckGems();
	}

	private void Checkpoint()
	{
		float currentScore = Int32.Parse(score.text);

		float MorningStart = 3;
		float AfternoonStart = 25;
		float EveningStart = 75;
		float NightStart = 150;

		float period = 15f;

		if(currentScore <= MorningStart)
		{
			StartCoroutine(SunAnimation(35, 0));
		}
		if(currentScore == AfternoonStart)
		{
			StartCoroutine(BackgroundChanger(Morning, Afternoon, period));
			StartCoroutine(SunAnimation(60, 1));
		}
		else if(currentScore == EveningStart)
		{
			StartCoroutine(BackgroundChanger(Afternoon, Evening, period));	
			StartCoroutine(SunAnimation(60, 2));		
		}
		else if(currentScore == NightStart)
		{
			StartCoroutine(BackgroundChanger(Evening, Night, period));	
			StartCoroutine(SunAnimation(1000, 3));
		}
	}

	private IEnumerator BackgroundChanger(GameObject fromGM, GameObject toGM, float time)
	{
		toGM.SetActive(true);

		float journey = 0f;

		// Clouds
		Color32 from = new Color32(255, 255, 255, 0);
		Color32 to = new Color32(255, 255, 255, 128);

		// Sun & Rainbow
		Color32 fromSun = new Color32(255, 255, 255, 0);
		Color32 toSun = new Color32(255, 255, 255, 216);
		
		// Background
		Color32 toBG = new Color32(255, 255, 255, 255);
		
		// Afternoon
		Color32 toBGAN = new Color32(184, 220, 255, 255);

		while(journey < time)
		{
			journey += Time.deltaTime;

			if(toGM.name != "Afternoon")
				toGM.GetComponent<Image>().color = Color32.Lerp(from, toBG, journey / time);
			else
				toGM.GetComponent<Image>().color = Color32.Lerp(from, toBGAN, journey / time);

			
			if(toGM.name == "Afternoon")
			{
				Clouds.transform.GetChild(0).gameObject.GetComponent<Image>().color = Color32.Lerp(to, from, journey / time);
				Clouds.transform.GetChild(1).gameObject.GetComponent<Image>().color = Color32.Lerp(to, from, journey / time);
				Morning.transform.GetChild(0).gameObject.GetComponent<Image>().color = Color32.Lerp(to, from, journey / time);

				Afternoon.transform.GetChild(0).gameObject.GetComponent<Image>().color = Color32.Lerp(fromSun, toSun, journey / time);
				Afternoon.transform.GetChild(1).gameObject.GetComponent<Image>().color = Color32.Lerp(fromSun, toSun, journey / time);
			}
			
			if(toGM.name == "Evening")
			{
				Clouds.transform.GetChild(0).gameObject.GetComponent<Image>().color = Color32.Lerp(to, from, journey / time);
				Clouds.transform.GetChild(1).gameObject.GetComponent<Image>().color = Color32.Lerp(from, to, journey / time);		

				Evening.transform.GetChild(0).gameObject.GetComponent<Image>().color = Color32.Lerp(fromSun, toSun, journey / time);
				Evening.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>().color = Color32.Lerp(fromSun, toSun, journey / time);
			}
			
			if(toGM.name == "Night")
			{
				Clouds.transform.GetChild(1).gameObject.GetComponent<Image>().color = Color32.Lerp(new Color32(255, 255, 255, 128), from, journey / time);
				Clouds.transform.GetChild(2).gameObject.GetComponent<Image>().color = Color32.Lerp(new Color32(255, 255, 255, 144), from, journey / time);
				Clouds.transform.GetChild(3).gameObject.GetComponent<Image>().color = Color32.Lerp(new Color32(255, 255, 255, 144), from, journey / time);
				Clouds.transform.GetChild(4).gameObject.GetComponent<Image>().color = Color32.Lerp(new Color32(255, 255, 255, 144), from, journey / time);

				Night.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>().color = Color32.Lerp(from, to, journey / time);
				Night.transform.GetChild(0).GetChild(1).gameObject.GetComponent<Image>().color = Color32.Lerp(from, to, journey / time);
				Night.transform.GetChild(0).GetChild(2).gameObject.GetComponent<Image>().color = Color32.Lerp(from, to, journey / time);
				Night.transform.GetChild(0).GetChild(3).gameObject.GetComponent<Image>().color = Color32.Lerp(from, to, journey / time);
				Night.transform.GetChild(0).GetChild(4).gameObject.GetComponent<Image>().color = Color32.Lerp(from, to, journey / time);				
				Night.transform.GetChild(0).GetChild(5).gameObject.GetComponent<Image>().color = Color32.Lerp(from, to, journey / time);

				Night.transform.GetChild(0).gameObject.GetComponent<Image>().color = Color32.Lerp(fromSun, toSun, journey / time);
			}

			yield return null;
		}

		if(toGM.name == "Night")
			Clouds.SetActive(false);

		fromGM.SetActive(false);
	}

	private IEnumerator SunAnimation(float time, int index)
	{
		// index [0] - Morning
		// index [1] - Afternoon
		// index [2] - Evening
		// index [3] - Night

		float journey = 0f;

		while(journey < time)
		{
			journey += Time.deltaTime;

			switch(index)
			{
				case 0:
					Morning.transform.GetChild(0).localPosition = Vector3.Lerp(new Vector3(-350, 825, 0), new Vector3(550, 825, 0), journey / time);
					break;
					
				case 1:
					Afternoon.transform.GetChild(0).localPosition = Vector3.Lerp(new Vector3(-520, 745, 0), new Vector3(575, 745, 0), journey / time);
					break;

				case 2:
					Evening.transform.GetChild(0).localPosition = Vector3.Lerp(new Vector3(-450, 745, 0), new Vector3(575, 745, 0), journey / time);
					break;
					
				case 3:
					Night.transform.GetChild(0).localPosition = Vector3.Lerp(new Vector3(-420, 745, 0), new Vector3(575, 745, 0), journey / time);
					break;
			}

			yield return null;
		}
	}

}

