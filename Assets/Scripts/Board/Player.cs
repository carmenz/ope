using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class Player: MonoBehaviour  {

	GameManager gm;
	List<GameObject> squares;
	int currentPosition;
	int targetPosition;
	string info;

	public static Player instance;

	private static Quiz quiz = new Quiz();

	private static WordGame wordGame = new WordGame();

	private static User user = new User();

		
	void Awake() {
		// Reset current position and reset token's visual position on the board
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		squares = GameObject.Find("Board").GetComponent<BoardManager>().Squares;
		currentPosition = gm.CurrentPosition;
		print ("currentpos" + currentPosition);
		SetTokenPosition();
		ChangeToken(gm.TokenName);
		print ("awake");


	}

	// Use this fors initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void Move(int chance) {
		Debug.Log("You got " + chance + "!!!!");
		targetPosition = currentPosition + chance;
		MoveTo();
	}


	void MoveTo() {
		if (currentPosition < targetPosition) {
			if (HasAccident(currentPosition)) {
				// Now means to the end;
				//TODO: add more functions, save un-used steps
				Debug.Log("Congrats!!!");
				return;
			}
			var nextSquare = squares[currentPosition + 1];
			// Debug.Log("Next Landing Square is" + nextSquare);
			transform.DOMove(new Vector3(nextSquare.transform.position.x, nextSquare.transform.position.y, 0f), 1).OnComplete(MoveTo);
			currentPosition++;
		} else {

			gm.CurrentPosition = currentPosition;
			user.currentPos = currentPosition;


			SaveData.Update (System.IO.Path.Combine (Application.persistentDataPath, "Resources/users.xml"), user);
			OnLanding();
		}
	}

	// void UpdateTargetPosition() {
		
	// }

	bool HasAccident(int curPos) {
		var curSquare = squares[curPos];
		if (curSquare.GetComponent<SquareController>().type == "Ending")
			return true;
		else
			return false;
	}

	void OnLanding() {
		//TODO: Check current square's type
		//TODO: Load required mini-game / quiz scene

//		quiz.GetData ();
//
//		SceneManager.LoadScene("TestQuiz"+ gm.Index);

		wordGame.GetData ();
		SceneManager.LoadScene ("WordGame1");
	}

	void UpdateScore() {
		//TODO: Update & save score
		//Set spinning button to active, doesn't need now, because board scene will reload after mini-challenge
		// wheel = GameObject.FindGameObjectWithTag("Wheel");
		// wheel.GetComponent<SpinWheel>().setWheelRotatable(true);
	}

	void SetTokenPosition() {
		//Moving the token into current position
		var curSquare = squares[currentPosition];
		gameObject.transform.position = new Vector3(curSquare.transform.position.x, curSquare.transform.position.y, 0);
	}
		

	public void OnClickToken(string tokenName) {
		// Hide the panel
		GameObject.Find("Panel").SetActive(false);
		gm.TokenName = tokenName;
		ChangeToken(tokenName);
	}

	void ChangeToken(string tokenName) {
		// Disable all the token
		var childCount = gameObject.transform.childCount;
		for (var i=0;i<childCount;i++) {
			gameObject.transform.GetChild(i).gameObject.SetActive(false);
		}
		// Set the token image
		gameObject.transform.Find(tokenName).gameObject.SetActive(true);
	}


	public void LoadPlayerStatusOnSignIn(int currentPos) {
		print ("hehehheehehehhe" + currentPos);

		var curSquare = squares[currentPos];
		gameObject.transform.position = new Vector3(curSquare.transform.position.x, curSquare.transform.position.y, 0);

	}
}
