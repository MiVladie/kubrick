using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour 
{
	public GameObject Shop;
	public GameObject Skins;
	
	public GameObject Logo;
	public GameObject Start;
	public GameObject Options;

	public GameObject Score;
	public GameObject Pause;

	public GameObject PauseMenu;
	public GameObject GameOverPanel;

	public GameObject Player;
	public GameObject Platforms;

	public Text Gems;

	private int index;
	private int indexSkin;

	private bool AllowPress = true;

	private void Awake()
	{
		UpdateSkins();
	}

	// index[0] - Main Menu 
	// index[1] - Gameplay '+'
	// index[2] - Pause Menu
	// index[3] - Game Over

	// indexSkin[0] - Main Menu
	// indexSkin[1] - Game Over

	#region Shop

	public void ShopOpen(int ind)
	{
		if(/*!AllowPress || */Shop.activeSelf || Skins.activeSelf || (GameOverPanel != null && GameOverPanel.activeSelf && ind == 1))
			return;

		index = ind;

		if(index == 1 && PauseMenu.activeSelf)
			return;	

		if(index == 1 && GameOverPanel.activeSelf && !GameOverPanel.transform.GetChild(6).gameObject.activeSelf)
			index = 3;			

		StartCoroutine(Open(true));
	}	

	public void ShopClose()
	{
		if(!AllowPress)
			return;
			
		if(FindObjectOfType<GameOver>() != null) {
			FindObjectOfType<ScoreManager>().CheckGems();
		}

		StartCoroutine(Open(false));
	}

	private IEnumerator Open(bool toOpen)
	{
		AllowPress = false;

		if(toOpen)
		{
			if(Shop.activeSelf || Skins.activeSelf)
				yield break;

			switch(index)
			{
				case 0:
					Shop.SetActive(true);

					Animation anim0 = Shop.GetComponent<Animation>();
					anim0["Shop Menu"].speed = 1f;
					anim0.Play();

					yield return new WaitForSeconds(0.2f);

					Logo.SetActive(false);
					Start.SetActive(false);
					Options.SetActive(false);

					break;

				case 1:
					FindObjectOfType<PlayerMoving>().GameOver = true;

					Score.SetActive(false);
					Pause.SetActive(false);

					Player.GetComponent<MeshRenderer>().enabled = false;
					for(int i = 0; i < Platforms.transform.childCount; i++)
					{
						Platforms.transform.GetChild(i).gameObject.GetComponent<MeshRenderer>().enabled = false;
						for(int y = 0; y < Platforms.transform.GetChild(i).childCount; y++)
							Platforms.transform.GetChild(i).GetChild(y).gameObject.GetComponent<MeshRenderer>().enabled = false;
					}

					// Enabling Non-Play Menus
					GameOverPanel.transform.parent.gameObject.SetActive(true);
					Shop.SetActive(true);

					Animation anim1 = Shop.GetComponent<Animation>();
					anim1["Shop Menu"].speed = 1f;
					anim1.Play();
					
					float journey = 0; float time = 0.5f;
					Color32 from = new Color32(255, 255, 255, 0); Color32 to = new Color32(255, 255, 255, 128);
					while (journey < time)
					{
						journey += Time.deltaTime;
						GameOverPanel.transform.parent.gameObject.GetComponent<Image>().color = Color32.Lerp(from, to, journey / time);
						yield return null;
					}

					break;
					
				case 2:
					Shop.SetActive(true);

					Animation anim2 = Shop.GetComponent<Animation>();
					anim2["Shop Menu"].speed = 1f;
					anim2.Play();

					yield return new WaitForSeconds(0.25f);
					PauseMenu.SetActive(false);

					break;
					
				case 3:
					Shop.SetActive(true);
					GameOverPanel.SetActive(false);

					Animation anim3 = Shop.GetComponent<Animation>();
					anim3["Shop Menu"].speed = 1f;
					anim3.Play();

					break;
			}
				
		}
		else
		{
			switch(index)
			{
				case 0:
					Animation anim0 = Shop.GetComponent<Animation>();
					anim0["Shop Menu"].speed = -1f;
					anim0["Shop Menu"].time = 0.5f;
					anim0.Play();

					yield return new WaitForSeconds(0.2f);

					Logo.SetActive(true);
					Start.SetActive(true);
					Options.SetActive(true);

					Logo.transform.GetChild(0).GetComponent<Animation>().Play();
					Start.transform.GetChild(0).GetComponent<Animation>().Play();

					yield return new WaitForSeconds(0.55f);
					Shop.SetActive(false);				
			
					break;

				case 1:
					Animation anim1 = Shop.GetComponent<Animation>();
					anim1["Shop Menu"].speed = -1;
					anim1["Shop Menu"].time = 0.5f;
					anim1.Play();

					yield return new WaitForSeconds(0.25f);
					
					Score.SetActive(true);
					Pause.SetActive(true);

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
						GameOverPanel.transform.parent.gameObject.SetActive(false);

					Shop.SetActive(false);

					break;
					
				case 2:
					Animation anim2 = Shop.GetComponent<Animation>();
					anim2["Shop Menu"].speed = -1f;
					anim2["Shop Menu"].time = 0.5f;
					anim2.Play();

					yield return new WaitForSeconds(0.25f);
					PauseMenu.SetActive(true);

					yield return new WaitForSeconds(0.5f);
					Shop.SetActive(false);

					break;
					
				case 3:
					Animation anim3 = Shop.GetComponent<Animation>();
					anim3["Shop Menu"].speed = -1f;
					anim3["Shop Menu"].time = 0.5f;
					anim3.Play();

					yield return new WaitForSeconds(0.25f);
					GameOverPanel.SetActive(true);

					yield return new WaitForSeconds(0.75f);
					Shop.SetActive(false);

					FindObjectOfType<GameOver>().CheckGems();

					break;
			}
						
		}

		AllowPress = true;
	}

	#endregion

	#region Skins

	public void SkinsOpen(int ind)
	{
		if(/*!AllowPress || */Skins.activeSelf || Shop.activeSelf)
			return;

		indexSkin = ind;

		StartCoroutine(SkinsAnim(true));
	}

	public void SkinsClose()
	{
		if(!AllowPress)
			return;

		StartCoroutine(SkinsAnim(false));
	}

	private IEnumerator SkinsAnim(bool toOpen)
	{
		AllowPress = false;

		if(toOpen)
		{
			UpdateSkins();

			switch(indexSkin)
			{
				case 0:
					Skins.SetActive(true);

					Animation anim0 = Skins.GetComponent<Animation>();
					anim0["Shop Menu"].speed = 1f;
					anim0.Play();

					yield return new WaitForSeconds(0.2f);

					Logo.SetActive(false);
					Start.SetActive(false);
					Options.SetActive(false);
					break;
					
				case 1:
					Skins.SetActive(true);
					GameOverPanel.SetActive(false);

					Animation anim3 = Skins.GetComponent<Animation>();
					anim3["Shop Menu"].speed = 1f;
					anim3.Play();
					break;					
			}
		}
		else
		{
			switch(indexSkin)
			{
				case 0:
					Animation anim0 = Skins.GetComponent<Animation>();
					anim0["Shop Menu"].speed = -1f;
					anim0["Shop Menu"].time = 0.5f;
					anim0.Play();

					yield return new WaitForSeconds(0.2f);

					Logo.SetActive(true);
					Start.SetActive(true);
					Options.SetActive(true);

					Logo.transform.GetChild(0).GetComponent<Animation>().Play();
					Start.transform.GetChild(0).GetComponent<Animation>().Play();

					yield return new WaitForSeconds(0.55f);
					Skins.SetActive(false);

					break;
					
				case 1:
					Animation anim3 = Skins.GetComponent<Animation>();
					anim3["Shop Menu"].speed = -1f;
					anim3["Shop Menu"].time = 0.5f;
					anim3.Play();

					yield return new WaitForSeconds(0.25f);
					GameOverPanel.SetActive(true);

					yield return new WaitForSeconds(0.75f);
					Skins.SetActive(false);
					break;
			}
		}

		AllowPress = true;
	}

	public void BuySkin(int index)
	{
		int gems = PlayerPrefs.GetInt("Gems");

		char[] array = PlayerPrefs.GetString("Cubes").ToCharArray();

		if(array[index] == '0' && gems < 500)
			return;

		if(array[index] == '0')
		{
			gems -= 500;
			array[index] = '2';

			FindObjectOfType<AudioManager>().Play("Purchase");
		}

		for(int i = 0; i < 7; i++) array[i] = array[i] == '2' ? '1' : array[i];

		array[index] = '2';

		PlayerPrefs.SetString("Cubes", new string(array));

		UpdateSkins();

		PlayerPrefs.SetInt("Gems", gems);
		
		if(Gems != null) 
			Gems.text = gems.ToString();
	}

	private void UpdateSkins()
	{
		int cube = 0;

		char[] array = PlayerPrefs.GetString("Cubes").ToCharArray();

		for(int i = 0; i < 7; i++)
		{
			if(array[i] == '0')
			{
				Skins.transform.GetChild(0).GetChild(i + 1).GetChild(0).gameObject.SetActive(false); // Selected
				Skins.transform.GetChild(0).GetChild(i + 1).GetChild(1).gameObject.SetActive(false); // Unlocked
				Skins.transform.GetChild(0).GetChild(i + 1).GetChild(2).gameObject.SetActive(false); // Lock Opened
				Skins.transform.GetChild(0).GetChild(i + 1).GetChild(3).gameObject.SetActive(true); // Lock Closed
				Skins.transform.GetChild(0).GetChild(i + 1).GetChild(4).gameObject.SetActive(true); // Price
			}
			else if(array[i] == '1')
			{
				Skins.transform.GetChild(0).GetChild(i + 1).GetChild(0).gameObject.SetActive(false); // Selected
				Skins.transform.GetChild(0).GetChild(i + 1).GetChild(1).gameObject.SetActive(true); // Unlocked
				Skins.transform.GetChild(0).GetChild(i + 1).GetChild(2).gameObject.SetActive(true); // Lock Opened
				Skins.transform.GetChild(0).GetChild(i + 1).GetChild(3).gameObject.SetActive(false); // Lock Closed
				Skins.transform.GetChild(0).GetChild(i + 1).GetChild(4).gameObject.SetActive(false); // Price
			}
			else if(array[i] == '2')
			{
				Skins.transform.GetChild(0).GetChild(i + 1).GetChild(0).gameObject.SetActive(true); // Selected
				Skins.transform.GetChild(0).GetChild(i + 1).GetChild(1).gameObject.SetActive(false); // Unlocked
				Skins.transform.GetChild(0).GetChild(i + 1).GetChild(2).gameObject.SetActive(true); // Lock Opened
				Skins.transform.GetChild(0).GetChild(i + 1).GetChild(3).gameObject.SetActive(false); // Lock Closed
				Skins.transform.GetChild(0).GetChild(i + 1).GetChild(4).gameObject.SetActive(false); // Price

				cube = i;
			}
		}

		if(Player == null)
			return;

		switch(cube)
		{
			case 0:
				Player.GetComponent<Renderer>().material.color = new Color32(255, 79, 81, 255);
				break;

			case 1:
				Player.GetComponent<Renderer>().material.color = new Color32(137, 79, 252, 255);
				break;
				
			case 2:
				Player.GetComponent<Renderer>().material.color = new Color32(62, 168, 228, 255);
				break;
				
			case 3:
				Player.GetComponent<Renderer>().material.color = new Color32(61, 229, 176, 255);
				break;
				
			case 4:
				Player.GetComponent<Renderer>().material.color = new Color32(61, 214, 46, 255);
				break;
				
			case 5:
				Player.GetComponent<Renderer>().material.color = new Color32(226, 213, 47, 255);
				break;
				
			case 6:
				Player.GetComponent<Renderer>().material.color = new Color32(228, 107, 54, 255);
				break;
		}

	}

	#endregion

}

