using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Xml;
using System;

public class VideoController : MonoBehaviour {

	public GameObject panel;
	private GameManager gm;
	private VideoPlayer vPlayer;
	private int currentScore = 30;
	private static string userpath = string.Empty;

	private VideoClip videoToPlay;
	
	public AudioSource audioA;
	public AudioSource audioB;

	GameObject slider;
	bool _reachEnd = false;

	void Awake () {
		vPlayer = gameObject.GetComponent<VideoPlayer>();
		slider = GameObject.Find("Slider");
	}
	
	// Use this for initialization
	void Start () {
		vPlayer.clip = GetVideo();
        vPlayer.Play();

		gm.updateDBTotalScore(currentScore);

	}

	void Update() {
		Text clipLength = GameObject.Find("ClipLength").GetComponent<Text>();

		TimeSpan totalTime = TimeSpan.FromSeconds((float)vPlayer.clip.length);
		clipLength.text = string.Format("{0:D2}:{1:D2}", totalTime.Minutes, totalTime.Seconds);

		Text playedLength = GameObject.Find("PlayedLength").GetComponent<Text>();
		TimeSpan playTime = TimeSpan.FromSeconds((float)vPlayer.time);
		playedLength.text = string.Format("{0:D2}:{1:D2}", playTime.Minutes, playTime.Seconds);
		float result = (float)(vPlayer.time / vPlayer.clip.length);
		if (float.IsPositiveInfinity(result))
		{
			result = float.MaxValue;
		} else if (float.IsNegativeInfinity(result))
		{
			result = float.MinValue;
		}
		slider.GetComponent<Slider>().value = result;

		// For windows bug
		if (clipLength.text != playedLength.text) {
			_reachEnd = false;
		}
		if (clipLength.text == playedLength.text && !_reachEnd) {
			_reachEnd = true;
			AddScore();
		}
	}

	void AddScore() {
		GameObject.Find("AudioComplete").GetComponent<AudioSource>().Play();
		panel.SetActive(true);
    } 

	public void Replay() {
		panel.SetActive(false);
		vPlayer.clip = GetVideo();
		vPlayer.frame = 0;
		vPlayer.Play();
		GameObject.Find("PlayedLength").GetComponent<Text>().text = "";
	}

	VideoClip GetVideo() {
		// store and update score in user.xml
		userpath = System.IO.Path.Combine (Application.dataPath, "Resources/users.xml");
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
	
		XmlDocument xmlUserDoc = new XmlDocument ();
		xmlUserDoc.Load (userpath);

		XmlNode usernameNode = xmlUserDoc.SelectSingleNode ("//Username");
		while (usernameNode.InnerText != gm.Username) {
			usernameNode = usernameNode.ParentNode.NextSibling.FirstChild;
		} 

		String island = usernameNode.ParentNode.SelectSingleNode (".//CurrentIsland").InnerText;
		if (island == "A") {
			vPlayer.SetTargetAudioSource(0, audioA);
			return Resources.Load("Videos/video1") as VideoClip;
		} else if (island == "B") {
			vPlayer.SetTargetAudioSource(0, audioB);
			return Resources.Load ("Videos/video2") as VideoClip;
		}

		return null;
	}


}
