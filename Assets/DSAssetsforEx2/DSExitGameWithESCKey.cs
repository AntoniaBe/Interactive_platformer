using UnityEngine;
using System.Collections;

public class DSExitGameWithESCKey : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.Escape))
		{
			Debug.Log("ESC pressed");
			//from http://answers.unity3d.com/questions/161858/startstop-playmode-from-editor-script.html
			#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
			#elif UNITY_WEBPLAYER
			Application.OpenURL(webplayerQuitURL);
			#else
			Application.Quit();
			#endif
		}
	}
}
