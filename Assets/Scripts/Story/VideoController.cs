using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoController : MonoBehaviour {

	public string mainSceneName = "Main";
	private VideoPlayer vPlayer;

	void Awake () {
		vPlayer = gameObject.GetComponent<VideoPlayer>();
	}
	
	// Use this for initialization
	void Start () {
		vPlayer.loopPointReached += EndReached;
        vPlayer.Play();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void EndReached(VideoPlayer vPlayer)
    {
		SceneManager.LoadScene(mainSceneName);
    } 
}
