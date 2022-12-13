using System;
using System.Collections;
using UnityEngine;

public class PlatformSpawn : MonoBehaviour 
{
	public GameObject ObstaclePrefab;
	public GameObject GemPrefab;

	private GameObject Player;

	private int ScoreOffset = 200;

	private Vector3[] ObstaclePosition = new Vector3[4] { 
		new Vector3(0, 1, 0), 
		new Vector3(0, 0, -1), 
		new Vector3(0, -1, 0), 
		new Vector3(0, 0, 1) 
	};
	
	private Vector3[] GemPosition = new Vector3[4] { 
		new Vector3(0, 0.5f, 0.375f), 
		new Vector3(0, -0.25f, -0.625f), 
		new Vector3(0, -1.5f, 0.375f), 
		new Vector3(0, -0.25f, 1.375f) 
	};

	private void Start()
	{
		Player = FindObjectOfType<PlayerMoving>().gameObject;
		ObstacleManager();
	}

	private void FixedUpdate()
	{
		// Third Type animation
		if(transform.localPosition.x - Player.transform.localPosition.x < 30f + PlayerMoving.MovingSpeed)
			for(int i = 0; i < transform.childCount; i++)
				if(transform.GetChild(i).name != "None" && transform.GetChild(i).name[0] != '2')
					if(transform.GetChild(0).position.y == ObstaclePosition[0].y && 
						transform.GetChild(1).position.z == ObstaclePosition[1].z &&
							transform.GetChild(2).position.y == ObstaclePosition[2].y &&
								transform.GetChild(3).position.z == ObstaclePosition[3].z)
									{
										int index = UnityEngine.Random.Range(0, 4);
										transform.GetChild(index).gameObject.GetComponent<Animation>().Play(transform.GetChild(index).name);
										return;
									}
	}

	private void ObstacleManager()
	{
		// Cleaning start
		if(Int32.Parse(name) < 10)
			return;

		// Defining type
		int score = Int32.Parse(FindObjectOfType<ScoreManager>().score.text);

		if(score > 200)			
			ScoreOffset = 100 * (int)(Int32.Parse(FindObjectOfType<ScoreManager>().score.text) / 100);

		if(score >= 25 && score < 35)
		{
			FirstType();
		}
		else if(score >= 50 && score < 75)
		{
			SecondType();
		}
		else if(score >= 100 && score < 135)
		{
			ThirdType();
		}
		else if(score >= ScoreOffset && score < ScoreOffset + 35)
		{ 
			int num = ((int)(Int32.Parse(FindObjectOfType<ScoreManager>().score.text) / 100) % 3);

			if(num == 0) FirstType();
			else if(num == 1) SecondType();
			else if (num == 2) ThirdType();
		}
		else
		{
			NoneType();
		}
	}

	private void NoneType()
	{
		int index = UnityEngine.Random.Range(1, 101);		

		if(index > 10 && index < 65)
		{	
			SpawnCube();	
		}
		else if(index <= 10)
		{
			SpawnGem();
		}
	}

	private void FirstType()
	{
		if(Int32.Parse(name) % 2 == 0)
			return;

		GameObject obstacle = SpawnCube();

		string[] animations = new string[4] { "1_level_01", "1_level_02", "1_level_03", "1_level_04" };
		int index = UnityEngine.Random.Range(0, 4); 

		StartCoroutine(AnimationDelay(obstacle, animations[index]));
	}
	
	private void SecondType()
	{
		if(Int32.Parse(name) % 2 == 0)
			return;

		GameObject obstacle = SpawnCube();
			
		string[] animations = new string[2] { "2_level_01", "2_level_02" };
		int index = UnityEngine.Random.Range(0, 3); 

		if(index != 2)
			obstacle.GetComponent<Animation>().Play(animations[index]);
		else
			obstacle.transform.localPosition = new Vector3(obstacle.transform.localPosition.x, 1, 0);
	}

	private void ThirdType()
	{
		if(Int32.Parse(name) % 5 != 0)
			return;
			
		GameObject[] obstacles = new GameObject[4];			
		string[] animations = new string[4] { "3_level_01", "3_level_02", "3_level_03", "3_level_04" };
		
		for(int i = 0; i < 4; i++)
		{			
			obstacles[i] = SpawnCube();
			obstacles[i].transform.position = new Vector3(obstacles[i].transform.position.x, ObstaclePosition[i].y, ObstaclePosition[i].z);
			obstacles[i].name = animations[i];
		}

	}

	private GameObject SpawnCube()
	{
		int indexPosition = UnityEngine.Random.Range(0, 4); 
		Vector3 spawnPosition = new Vector3(transform.position.x, ObstaclePosition[indexPosition].y, ObstaclePosition[indexPosition].z);

		GameObject obstacle = Instantiate(ObstaclePrefab, spawnPosition, Quaternion.Euler(0, 0, 0), transform);
		obstacle.transform.localScale = new Vector3(0.2f, 1, 1);

		obstacle.GetComponent<BoxCollider>().size = new Vector3(0.9f, 0.95f, 0.95f);
		obstacle.name = "None";

		return obstacle;
	}

	private void SpawnGem()
	{
		if(Int32.Parse(name) < 7)
			return;

		int index = UnityEngine.Random.Range(0, 4);
		Vector3 spawnPosition = new Vector3(transform.position.x, GemPosition[index].y, GemPosition[index].z);

		int amount = UnityEngine.Random.Range(3, 5);
		for (int i = 0; i < amount; i++)
		{
			GameObject gem = Instantiate(GemPrefab, new Vector3(spawnPosition.x + i, spawnPosition.y, spawnPosition.z), Quaternion.Euler(0, 0, 0), transform);
			gem.transform.rotation = Quaternion.Euler(-90, 0, 0);
		}
			
	}

	private IEnumerator AnimationDelay(GameObject obstacle, string animation)
	{
		yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, 0.5f));

		obstacle.GetComponent<Animation>().Play(animation);
	}

}

