using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewPlayer : MonoBehaviour {

	[SerializeField]
	private float movingSpeed = 25f;

	private Vector2 moveTowardPosition = Vector2.zero;
    private Vector2 moveStartPosition = Vector2.zero;
    private float totalTime = 0.0f;
    private float costTime = 0.0f;
    private float timePrecent = 0.0f;

	private static Quiz quiz = new Quiz();
	private static Story story = new Story ();
	private static WordGame wordGame = new WordGame();



	bool _isRuning = false;
	bool _canBeTriggered = true;

	GameManager gm;
	Text missionContainer;

	public bool IsRuning
    {
        get { return _isRuning; }
        set { _isRuning = value; }
    }

	private void MoveByTimeline() {
        Vector2 curenPosition = new Vector2(this.transform.position.x, this.transform.position.y);
        if (Input.GetButton("Fire1"))
        {   
            moveStartPosition = curenPosition;
            moveTowardPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            var subVector3 = moveTowardPosition - curenPosition;
            transform.GetComponent<Rigidbody2D>().velocity = subVector3;
            _isRuning = true;
        }
        if (_isRuning)
        {
            var d = Vector2.Distance(curenPosition, moveTowardPosition);
//            Debug.Log(d);
//            Debug.Log(curenPosition);
//            Debug.Log(moveTowardPosition);
            if (d < 1) {
                //Debug.Log("daolele");
                transform.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                transform.position = moveTowardPosition;
                _isRuning = false;
            }

        }
    }

	// Use this for initialization
	void Start () {
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        transform.position = gm.Coordinate;
        _canBeTriggered = false;
		missionContainer = GameObject.Find("MissionList").GetComponent<Text>();
		if (gm.missions.quizRequired.Count > 0) {
			missionContainer.text = gm.missions.quizInfo;
		}
	}
	
	// Update is called once per frame
	void Update () {
		MoveByTimeline();		
	}

    void OnTriggerEnter2D(Collider2D collider) {
        if (IsRuning)
            _canBeTriggered = true;
    }

	void OnTriggerStay2D(Collider2D collider) {
		// Trigger should only be triggerd after the player stop moving
		if (IsRuning || !_canBeTriggered)
			return;
		// Only can be triggerd once in the slot
		_canBeTriggered = false;
		// Get slot type
		// WORDPLAY, SLIDESTORY, MADLIBS, SPINNINGWHEEL, SHOP, PORT
		// var type = collider.name.Replace(" Slot", "");
		var type = collider.GetComponent<SquareController>().type;
		gm.typeCode = collider.GetComponent<SquareController>().index;
		gm.Index = collider.GetComponent<SquareController>().index;
        gm.Coordinate = new Vector2(transform.position.x, transform.position.y);

		if (type == "Quiz") {
			quiz.GetData ();
		} else if (type == "WordGame") {
			wordGame.GetData ();
		} else if (type == "Story") {
			story.GetData ();
		}

		SceneManager.LoadScene(type);

		// Debug.Log(type);
	}

	void OnTriggerExit2D(Collider2D collider) {
		_canBeTriggered = true;
		// TODO: 
	}
}
