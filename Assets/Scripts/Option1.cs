using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Xml;


public class Option1 : MonoBehaviour {

	private QuizManager qm;
	private GameManager gm;
	// Use this for initialization
	void Start () {
		qm = GameObject.Find("QuizManager").GetComponent<QuizManager>();
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();

		Button option1 = GameObject.Find ("Option1").GetComponent<Button> ();
		option1.onClick.AddListener (qm.Task1OnClick);
	}
	
	// Update is called once per frame
	void Update () {

	}
}
