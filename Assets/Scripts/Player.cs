using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	int currentPosition;
	int targetPosition;
	string info;

	int dice = 5;

	// Use this for initialization
	void Start () {
		currentPosition = this.currentPosition;
		Move(dice);
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void Move(int num) {
		StartCoroutine(MoveTo(num));
	}


	IEnumerator MoveTo(int num) {
		
		for (int i = 0; i < num; i++) {
			var nextSquare = BoardManager.Squares [currentPosition + 1];

			float currentx = this.transform.position.x;
			float currenty = this.transform.position.y;

			this.transform.Translate(new Vector2(nextSquare.transform.position.x - currentx + 1, nextSquare.transform.position.y - currenty));

	
			this.transform.position = transform.position;

			currentPosition++;
			yield return new WaitForSeconds (1);
		}


	}

	void UpdateTargetPosition() {
		targetPosition = currentPosition + dice;
	}

	void OnLanding() {

	}



	void UpdateScore() {

	}
}
