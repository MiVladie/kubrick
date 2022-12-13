using System;
using System.Collections;
using UnityEngine;

public class PlayerMoving : MonoBehaviour 
{	
	public static float MovingSpeed = 2f;

	private int RotatingSpeed = 8;

	public GameObject PlatformPrefab;
	public GameObject Platforms;

	private GameObject Right;
	private GameObject Left;
	private GameObject RightTop;
	private GameObject LeftTop;

	public bool AllowRotation = true;
	public bool GameOver = false;

	[HideInInspector]
	public int platforms;

	private void Start()
	{
		Initialisation();
	}

	private void FixedUpdate()
	{
		if(GameOver)
			return;

		// Moving Cube
		Moving();

		// Rotating Cube
		Rotation();

		// Respawning Platforms
		Respawning();
	}

	private void Initialisation()
	{
		// Assigning values

		Right = transform.GetChild(0).gameObject;
		Left = transform.GetChild(1).gameObject;
		RightTop = transform.GetChild(2).gameObject;
		LeftTop = transform.GetChild(3).gameObject;

		// Resetting values

		platforms = Platforms.transform.childCount + 1;
		MovingSpeed = 2f;
	}

	private void OnCollisionEnter(Collision col)
	{
		if(col.gameObject.tag == "Obstacle")
		{
			FindObjectOfType<GameOver>().GameOverAppear();
			FindObjectOfType<CameraFollow>().ResetCamera();
			FindObjectOfType<AudioManager>().Play("Collision");

			Destroy(col.gameObject);

			GameOver = true;
			
		}
		
		if(col.gameObject.tag == "Gem")
		{
			FindObjectOfType<ScoreManager>().GemUpdate();
			FindObjectOfType<AudioManager>().Play("Gem collect");
			Destroy(col.gameObject);
		}
	}

	#region Rotation

	public void TurnLeft()
	{
		if(AllowRotation)
		{
			AllowRotation = false;
			StartCoroutine(RotateLeft());
		}
	}
	
	public void TurnRight()
	{
		if(AllowRotation)
		{
			AllowRotation = false;
			StartCoroutine(RotateRight());
		}
	}

	private void Rotation()
	{
		if(AllowRotation)
		{
			if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D) || 
				(Input.GetButton("Fire1") && Input.mousePosition.x > Screen.width / 2 && Input.mousePosition.y < Screen.height * 0.8f && Input.mousePosition.y > Screen.height * 0.075f) ||
				(Input.touchCount > 0 && Input.GetTouch(0).position.x > Screen.width / 2 && Input.GetTouch(0).position.y < Screen.height * 0.8f && Input.GetTouch(0).position.y > Screen.height * 0.075f))
			{
				AllowRotation = false;
				StartCoroutine(RotateRight());

				FindObjectOfType<AudioManager>().Play("Rotation");
			}
			else if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A) || 
				(Input.GetButton("Fire1") && Input.mousePosition.x < Screen.width / 2 && Input.mousePosition.y < Screen.height * 0.8f && Input.mousePosition.y > Screen.height * 0.05f) ||
				(Input.touchCount > 0 && Input.GetTouch(0).position.x < Screen.width / 2 && Input.GetTouch(0).position.y < Screen.height * 0.8f && Input.GetTouch(0).position.y > Screen.height * 0.05f))
			{
				AllowRotation = false;
				StartCoroutine(RotateLeft());

				FindObjectOfType<AudioManager>().Play("Rotation");
			}			
		}
	}

	private IEnumerator RotateRight()
	{
		int index = 0;
		
		if(Mathf.Round(transform.localPosition.y) == 1 && Mathf.Round(transform.localPosition.z) == 0) index = 0;		
		if(Mathf.Round(transform.localPosition.y) == 0 && Mathf.Round(transform.localPosition.z) == -1) index = 1;		
		if(Mathf.Round(transform.localPosition.y) == -1 && Mathf.Round(transform.localPosition.z) == 0) index = 2;			
		if(Mathf.Round(transform.localPosition.y) == 0 && Mathf.Round(transform.localPosition.z) == 1) index = 3;

		float val = 40f;	
		float delta = 0f;

		while(true)
		{
			if(index == 0) transform.RotateAround(Right.transform.position, Vector3.left, RotatingSpeed * Time.fixedDeltaTime * val);
			if(index == 1) transform.RotateAround(RightTop.transform.position, Vector3.left, RotatingSpeed * Time.fixedDeltaTime * val);
			if(index == 2) transform.RotateAround(LeftTop.transform.position, Vector3.left, RotatingSpeed * Time.fixedDeltaTime * val);
			if(index == 3) transform.RotateAround(Left.transform.position, Vector3.left, RotatingSpeed * Time.fixedDeltaTime * val);

			delta += RotatingSpeed * Time.fixedDeltaTime * val;

			if((int)delta >= 180)
				break;

			yield return new WaitForSeconds(0.001f);
		}

		if(index == 0)
		{
			transform.position = new Vector3(transform.position.x, 0, -1);
			transform.rotation = Quaternion.Euler(-180, 0, 0);
		}

		if(index == 1) 
		{
			transform.position = new Vector3(transform.position.x, -1, 0);
			transform.rotation = Quaternion.Euler(0, 0, 0);
		}

		if(index == 2) 
		{
			transform.position = new Vector3(transform.position.x, 0, 1);
			transform.rotation = Quaternion.Euler(-180, 0, 0);
		}

		if(index == 3) 
		{
			transform.position = new Vector3(transform.position.x, 1, 0);
			transform.rotation = Quaternion.Euler(0, 0, 0);
		}


		AllowRotation = true;
	}

	private IEnumerator RotateLeft()
	{
		int index = 0;

		if(Mathf.Round(transform.localPosition.y) == 1 && Mathf.Round(transform.localPosition.z) == 0) index = 0;		
		if(Mathf.Round(transform.localPosition.y) == 0 && Mathf.Round(transform.localPosition.z) == 1) index = 1;		
		if(Mathf.Round(transform.localPosition.y) == -1 && Mathf.Round(transform.localPosition.z) == 0) index = 2;			
		if(Mathf.Round(transform.localPosition.y) == 0 && Mathf.Round(transform.localPosition.z) == -1) index = 3;	

		float val = 40f;	
		float delta = 0f;
		
		while(true)
		{
			if(index == 0) transform.RotateAround(Left.transform.position, Vector3.right, RotatingSpeed * Time.fixedDeltaTime * val);
			if(index == 1) transform.RotateAround(LeftTop.transform.position, Vector3.right, RotatingSpeed * Time.fixedDeltaTime * val);
			if(index == 2) transform.RotateAround(RightTop.transform.position, Vector3.right, RotatingSpeed * Time.fixedDeltaTime * val);
			if(index == 3) transform.RotateAround(Right.transform.position, Vector3.right, RotatingSpeed * Time.fixedDeltaTime * val);

			delta += RotatingSpeed * Time.fixedDeltaTime * val;

			if((int)delta >= 180)
				break;
				
			yield return new WaitForSeconds(0.001f);
		}

		if(index == 0) 
		{
			transform.position = new Vector3(transform.position.x, 0, 1);
			transform.rotation = Quaternion.Euler(-180, 0, 0);
		}

		if(index == 1) 
		{
			transform.position = new Vector3(transform.position.x, -1, 0);
			transform.rotation = Quaternion.Euler(0, 0, 0);
		}

		if(index == 2) 
		{
			transform.position = new Vector3(transform.position.x, 0, -1);
			transform.rotation = Quaternion.Euler(-180, 0, 0);
		}

		if(index == 3) 
		{
			transform.position = new Vector3(transform.position.x, 1, 0);
			transform.rotation = Quaternion.Euler(0, 0, 0);
		}

		AllowRotation = true;
	}

	#endregion

	private void Respawning()
	{
		if(transform.position.x - Platforms.transform.GetChild(0).position.x >= 15)
		{			
			Vector3 spawnPosition = Platforms.transform.GetChild(Platforms.transform.childCount - 1).transform.position + new Vector3(5, 0, 0);
			GameObject platform = Instantiate(PlatformPrefab, spawnPosition, Quaternion.Euler(0, 0, 0), Platforms.transform);
			Destroy(Platforms.transform.GetChild(0).gameObject);

			platform.name = platforms.ToString();
			platforms++;

			Accelerating();

			FindObjectOfType<ScoreManager>().ScoreUpdate(platforms);
		}
	}

	private void Moving()
	{
		// Moving cube
		transform.position += new Vector3(4f, 0, 0) * MovingSpeed * Time.fixedDeltaTime;
		transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -1.5f, 1.5f), Mathf.Clamp(transform.position.z, -1.5f, 1.5f));		
	}

	public void ContinuePlaying()
	{
		transform.position = new Vector3(transform.position.x, 1, 0);
		transform.rotation = Quaternion.Euler(0, 0, 0);

		for(int i = 0; i < Platforms.transform.childCount; i++)
			for(int y = 0; y < Platforms.transform.GetChild(i).childCount; y++)
				Destroy(Platforms.transform.GetChild(i).GetChild(y).gameObject);
		
		GameOver = false;
		AllowRotation = true;
	}

	private void Accelerating()
	{
		if(MovingSpeed < 8)
		{			
			int score = (int)(Int32.Parse(FindObjectOfType<ScoreManager>().score.text));		
			
			float acSpeed = 0; 

			if(score < 10) {
				acSpeed = 0.05f;
				MovingSpeed += acSpeed;
				MovingSpeed = Mathf.Clamp(MovingSpeed, 2, 3);
			}
			else if(score >= 10 && score < 25) {
				acSpeed = 0.04f;
				MovingSpeed += acSpeed;
				MovingSpeed = Mathf.Clamp(MovingSpeed, 3, 4);
			}
			else if(score >= 25 && score < 50) {
				acSpeed = 0.03f;
				MovingSpeed += acSpeed;
				MovingSpeed = Mathf.Clamp(MovingSpeed, 4, 5);
			}
			else if(score >= 50 && score < 100) {
				acSpeed = 0.02f;
				MovingSpeed += acSpeed;
				MovingSpeed = Mathf.Clamp(MovingSpeed, 5, 6);
			}
			else if(score >= 100 && score < 250) {
				acSpeed = 0.015f;
				MovingSpeed += acSpeed;
				MovingSpeed = Mathf.Clamp(MovingSpeed, 6, 7);
			}
			else if(score >= 250 && score < 500) {
				acSpeed = 0.01f;
				MovingSpeed += acSpeed;
				MovingSpeed = Mathf.Clamp(MovingSpeed, 7, 8);
			}
			
			RotatingSpeed = (int)(MovingSpeed * 2 + 6);
		}
	}

}

