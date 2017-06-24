using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Xml;


public class Option3 : MonoBehaviour {

	private QuizManager qm;
	private GameManager gm;
	// Use this for initialization
	void Start () {
		qm = GameObject.Find("QuizManager").GetComponent<QuizManager>();
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();

		Button option3 = GameObject.Find ("Option3").GetComponent<Button> ();
		option3.onClick.AddListener (qm.Task3OnClick);
	}

	// Update is called once per frame
	void Update () {

	}
}
