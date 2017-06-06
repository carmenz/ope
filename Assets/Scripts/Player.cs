using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class Player: MonoBehaviour  {

	int currentPosition;
	int targetPosition;
	string info;
	[SerializeField]
	GameObject wheel;

	public int chance;


	public static Player instance;
		
	void Awake() {

	}

	// Use this fors initialization
	void Start () {
		Debug.Log("execute start in Player.cs");
		currentPosition = GameObject.Find("GameManager").GetComponent<GameManager>().currentPosition;
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void Move(int num) {
		Debug.Log(num);
		chance = num;
		UpdateTargetPosition();
		MoveTo();
	}


	void MoveTo() {
		if (currentPosition < targetPosition) {
			var nextSquare = GameObject.Find("Board").GetComponent<BoardManager>().Squares [currentPosition + 1];
			Debug.Log(nextSquare);
			transform.DOMove(new Vector3(nextSquare.transform.position.x, nextSquare.transform.position.y, 0f), 1).OnComplete(MoveTo);
			currentPosition++;
		} else {
			GameObject.Find("GameManager").GetComponent<GameManager>().currentPosition = currentPosition;
			OnLanding();
		}
	}

	void UpdateTargetPosition() {
		targetPosition = currentPosition + chance;
	}

	void OnLanding() {
		//TODO: Check current square's type
		//TODO: Load required mini-game / quiz scene
		SceneManager.LoadScene("TestQuiz");
	}

	void UpdateScore() {
		//TODO: Update & save score
		//Set spinning button to active
		wheel = GameObject.FindGameObjectWithTag("Wheel");
		wheel.GetComponent<SpinWheel>().setWheelRotatable(true);
	}
}
