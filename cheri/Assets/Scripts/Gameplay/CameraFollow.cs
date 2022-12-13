using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour 
{
	public GameObject BackgroundMorning;

	public GameObject Player;

	public Vector3 PositionOffset; 

    public float smoothTime = 1f;
    private Vector3 velocity = Vector3.zero;

	private void Start()
	{
		ResetCamera();

		StartCoroutine(LoadGameplay());
	}

	private void FixedUpdate()
	{
		if(!Player.GetComponent<MeshRenderer>().enabled)
			return;

		Vector3 desiredPosition = Player.transform.position + PositionOffset;
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);
		transform.position = new Vector3(Player.transform.position.x + PositionOffset.x, transform.position.y, transform.position.z);

		// Smooth time decrease by score
		smoothTime = Mathf.Lerp(1, 0.4f, PlayerMoving.MovingSpeed / 5);

		// Camera distance increase by score
		PositionOffset = Vector3.Slerp(new Vector3(-6, 5, -5), new Vector3(-11, 9, -10), PlayerMoving.MovingSpeed / 8);
	}

	private IEnumerator LoadGameplay()
	{
		float journey = 0f;
		float time = 3f;
		
		BackgroundMorning.GetComponent<CanvasGroup>().alpha = 1;

		while(journey < time)
		{
			journey += Time.deltaTime;

			BackgroundMorning.GetComponent<CanvasGroup>().alpha = 1 - journey / time;

			yield return null;
		}

		BackgroundMorning.SetActive(false);
	} 

	public void ResetCamera()
	{
		transform.localPosition = new Vector3(0, 0, 5);
	}

}

