using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoController : MonoBehaviour {

	public VideoPlayer vPlayer;

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
        Debug.Log("End reached!");
    } 
}
