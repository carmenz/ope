using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Xml;


public class Option4 : MonoBehaviour {

	private QuizManager qm;
	private GameManager gm;

	// Use this for initialization
	void Start () {
		qm = GameObject.Find("QuizManager").GetComponent<QuizManager>();
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();

		Button option4 = GameObject.Find ("Option4").GetComponent<Button> ();
		option4.onClick.AddListener (qm.Task4OnClick);
	}

	// Update is called once per frame
	void Update () {

	}
}
