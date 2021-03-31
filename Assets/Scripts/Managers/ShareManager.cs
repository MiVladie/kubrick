using System.Collections;
using UnityEngine;
using System.IO; 

public class ShareManager : MonoBehaviour 
{ 
	public void ShareText()
	{
		var shareSubject = "";
		var shareMessage = "";

		StartCoroutine(ShareTextMG(shareSubject, shareMessage));
	}

	public void ShareScreenshot(string score)
	{
		string subject = "";
		string message = "";

		subject = "Can you beat it?";
		message = "Can you beat my score: " + score + "?";

		StartCoroutine(ShareScreenshotMG(subject, message));
	}

	private IEnumerator ShareTextMG(string shareSubject, string shareMessage)
	{
		yield return new WaitForSeconds(0.5f);
		
		// Create intent for action send

		AndroidJavaClass intentClass =  new AndroidJavaClass ("android.content.Intent");
		AndroidJavaObject intentObject =  new AndroidJavaObject ("android.content.Intent");
		intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string> ("ACTION_SEND"));

		// Put text and subject extra

		intentObject.Call<AndroidJavaObject> ("setType", "text/plain");
		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string> ("EXTRA_SUBJECT"), shareSubject);
		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string> ("EXTRA_TEXT"), shareMessage);

		// Call createChooser method of activity class

		AndroidJavaClass unity = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
		AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject> ("currentActivity");
		AndroidJavaObject chooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intentObject, "Share your high score");
		currentActivity.Call("startActivity", chooser);
	}
	
	private IEnumerator ShareScreenshotMG(string subject, string message)
	{
		yield return new WaitForEndOfFrame();
		
		Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
		ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
		ss.Apply();

		string filePath = Path.Combine(Application.temporaryCachePath, "shared img.png");
		File.WriteAllBytes(filePath, ss.EncodeToPNG());
		
		Destroy(ss);

		new NativeShare().AddFile(filePath).SetSubject(subject).SetText(message).Share();
	}

}

