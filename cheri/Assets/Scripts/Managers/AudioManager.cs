using UnityEngine.Audio;
using System;
using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public GameObject buttonPrefab;
	public static AudioManager instance;

	public Sound[] sounds;

	public float currentPitch = 1.25f;
	public float incdif = 0.2f;

	void Awake()
	{
		if(!PlayerPrefs.HasKey("Music"))
		{
			PlayerPrefs.SetInt("Music", 1);
			PlayerPrefs.SetInt("Sound", 1);
		}

		if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}

		foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.loop = s.loop;
		}

		if(PlayerPrefs.GetInt("Music") == 1)
			Play("Theme");
	}

	public void Play(string sound)
	{
		if((sound == "Button" || sound == "Gem collect" || sound == "Collision" || sound == "Purchase" || sound == "Rotation") && PlayerPrefs.GetInt("Sound") == 0)
			return;
			
		if(sound == "Theme" && PlayerPrefs.GetInt("Music") == 0)
			return;
		
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));
		s.source.volume = 1;

		switch(sound)
		{
			case "Theme":
				s.source.volume = 0.2f;
				s.source.Play();
				break;
				
			case "Purchase":
				s.source.volume = 0.2f;
				s.source.Play();
				break;

			case "Rotation":
				s.source.volume = 0.2f;
				s.source.pitch = 1.25f;
				s.source.Play();
				break;

			case "Button":
				s.source.volume = 0.35f;
				s.source.Play();
				break;
				
			case "Collision":
				s.source.Play();
				break;

			case "Gem collect":
				StartCoroutine(GemSound());
				break;				
		}

	}

	public void Stop(string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);

		s.source.volume = 0;
	}

	private IEnumerator GemSound()
	{
		GameObject inst = Instantiate(buttonPrefab);

		inst.name = currentPitch.ToString();
		currentPitch += incdif;
		inst.GetComponent<AudioSource>().pitch = currentPitch;

		inst.GetComponent<AudioSource>().Play();

		yield return new WaitForSeconds(0.75f);

		Destroy(inst);
		currentPitch -= incdif;
	}

}

