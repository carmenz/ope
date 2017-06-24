using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Xml;


public class Option2 : MonoBehaviour {

	private QuizManager qm;
	private GameManager gm;
	// Use this for initialization
	void Start () {
		qm = GameObject.Find("QuizManager").GetComponent<QuizManager>();
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();

		Button option2 = GameObject.Find ("Option2").GetComponent<Button> ();
		option2.onClick.AddListener (qm.Task2OnClick);
	}

	// Update is called once per frame
	void Update () {

	}
}
