using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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

    // Use this for initialization
    void Start () {
        currentPosition = 0;
        Move();
        
    }
    
    // Update is called once per frame
    void Update () {
        
    }


    void Move() {
        targetPosition = currentPosition + dice;
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
        targetPosition = currentPosition + dice;
    }

    void OnLanding() {

    }

    void UpdateScore() {

    }
}
