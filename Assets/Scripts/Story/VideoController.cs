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

	GameObject slider;

	void Awake () {
		vPlayer = gameObject.GetComponent<VideoPlayer>();
		slider = GameObject.Find("Slider");
	}
	
	// Use this for initialization
	void Start () {
		vPlayer.loopPointReached += EndReached;
        vPlayer.Play();

		

		// store and update score in user.xml
		userpath = System.IO.Path.Combine (Application.dataPath, "Resources/users.xml");
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();

		XmlDocument xmlUserDoc = new XmlDocument ();
		xmlUserDoc.Load (userpath);
		// Find user and update <TotalScore>
		XmlNode usernameNode = xmlUserDoc.SelectSingleNode ("//Username");
		while (usernameNode.InnerText != gm.Username) {
			usernameNode = usernameNode.ParentNode.NextSibling.FirstChild;
		} 
		usernameNode.ParentNode.SelectSingleNode (".//TotalScore").InnerText = 
			(int.Parse(usernameNode.ParentNode.SelectSingleNode ("TotalScore").InnerText) + currentScore).ToString();

		xmlUserDoc.Save (userpath);

	}

	void Update() {
		float result = (float)(vPlayer.time / vPlayer.clip.length);
		if (float.IsPositiveInfinity(result))
		{
			result = float.MaxValue;
		} else if (float.IsNegativeInfinity(result))
		{
			result = float.MinValue;
		}
		slider.GetComponent<Slider>().value = result;
	}

	void EndReached(VideoPlayer vPlayer) {
		// SceneManager.LoadScene(mainSceneName);
		panel.SetActive(true);
    } 

	public void Replay() {
		panel.SetActive(false);
		vPlayer.Play();
	}
}
