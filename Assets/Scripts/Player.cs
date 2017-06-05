using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour {

	int currentPosition;
	int targetPosition;
	string info;

	public int chance;



	// Use this for initialization
	void Start () {
		currentPosition = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void Move(int num) {
		chance = num;
		UpdateTargetPosition();
		MoveTo();
	}


	void MoveTo() {
		if (currentPosition < targetPosition) {
			var nextSquare = BoardManager.Squares [currentPosition + 1];
			transform.DOMove(new Vector3(nextSquare.transform.position.x, nextSquare.transform.position.y, 0f), 1).OnComplete(MoveTo);
			currentPosition++;
		}
	}

	void UpdateTargetPosition() {
		targetPosition = currentPosition + chance;
	}

	void OnLanding() {
		//TODO: Check current square's type
		//TODO: Load required mini-game / quiz scene
	}

	void UpdateScore() {
		//TODO: Update & save score
		//TODO: Set spinning button to active
	}
}
