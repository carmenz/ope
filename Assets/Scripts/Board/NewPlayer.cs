using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Xml;
using System.IO;
using System;

public class NewPlayer : MonoBehaviour {

	[SerializeField]
	private float movingSpeed = 25f;

	private Vector2 moveTowardPosition = Vector2.zero;
    private Vector2 moveStartPosition = Vector2.zero;
    private float totalTime = 0.0f;
    private float costTime = 0.0f;
    private float timePrecent = 0.0f;

	private FillInTheBlank fillInTheBlank = new FillInTheBlank();
	private Story story = new Story ();
	private WordGame wordGame = new WordGame();
	private Quiz quiz = new Quiz();



	bool _isRuning = false;
	bool _canBeTriggered = true;

	GameManager gm;
	GameObject missionsPanel;
	GameObject MissionToggle;

	public bool IsRuning
    {
        get { return _isRuning; }
        set { _isRuning = value; }
    }

	void MoveByTimeline() {
        Vector2 curenPosition = new Vector2(this.transform.position.x, this.transform.position.y);
        if (Input.GetButton("Fire1") && !EventSystem.current.IsPointerOverGameObject())
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

            if (d < 1) {
                transform.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                transform.position = moveTowardPosition;
                _isRuning = false;
				gm.Coordinate = new Vector2(transform.position.x, transform.position.y);
				SaveCoordinate();
            }

        }
    }

	// Use this for initialization
	void Start () {
		// init game manager
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		// init location
        transform.position = gm.Coordinate;
		// init missions panel and refresh mission list
		missionsPanel = GameObject.Find("MissionsPanel");
		MissionToggle = (GameObject)Resources.Load("MissionToggle", typeof(GameObject));
		
		var i = 1;
		foreach(var mission in gm.missionList) {
			var mtoggle = Instantiate(MissionToggle);
			mtoggle.transform.SetParent(missionsPanel.transform);
			mtoggle.GetComponent<RectTransform>().anchoredPosition = new Vector2(636f, -61f - i * 111f);
			// mtoggle.GetComponent<RectTransform>().position = new Vector3(185f, -9f - i * 112f, 0f);
			mtoggle.transform.Find("Label").GetComponent<Text>().text = mission.missionDesc;
			// toggle the checkmark
			var cm = mtoggle.transform.Find("Background").Find("Checkmark").gameObject;
			cm.SetActive(IsCompleted(mission.missionType) ? true : false);
			i++;
		}
		// calculate coins manually
		var coinsCm = missionsPanel.transform.Find("Coins").Find("Background").Find("Checkmark").gameObject;
		var tts = GetTotalScore();
		if (tts >= 200) {
			coinsCm.SetActive(true);
			missionsPanel.transform.Find("Footer").GetComponent<Text>().text = "0 Left";
		} else {
			coinsCm.SetActive(false);
			missionsPanel.transform.Find("Footer").GetComponent<Text>().text = (200-tts).ToString() + " Left";
		}

		// show or hide the mission panel
		missionsPanel.SetActive(tts > 0 ? false : true);

		// in case players stay on a position before quit
        _canBeTriggered = false;
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
		SaveCoordinate();

		if (type == "FillInTheBlank") {
			fillInTheBlank.GetData ();
		} else if (type == "WordGame") {
			wordGame.GetData ();
		} else if (type == "Story") {
			story.GetData ();
		} else if (type == "Quiz") {
			quiz.GetData ();
		}

		SceneManager.LoadScene(type);

	}

	void OnTriggerExit2D(Collider2D collider) {
		_canBeTriggered = true;
		// TODO: 
	}

	bool IsCompleted(string type) {
		var userpath = System.IO.Path.Combine (Application.dataPath, "Resources/users.xml");
		FileStream userStream = new FileStream (userpath, FileMode.Open);
		XmlTextReader xmlUserReader = new XmlTextReader (userStream);


		while (xmlUserReader.Read ()) {

			if (xmlUserReader.Name == "Username") {
				// find user from user.xml
				if (xmlUserReader.ReadElementContentAsString ().Equals (gm.Username)) {
					xmlUserReader.Name.Contains ("User");
					if (xmlUserReader.ReadToNextSibling("IslandA")) {
						print ("user already have the island");

						if (xmlUserReader.ReadToDescendant (type)) {
							userStream.Close ();
							return true;

						} else {
							userStream.Close ();
							return false;
						}
					} else {
						userStream.Close ();
						return false;
					}
				}
			}
		}
		userStream.Close ();
		return false;
	}

	int GetTotalScore() {
		XmlDocument xmlUserDoc = new XmlDocument ();

		var userpath = System.IO.Path.Combine (Application.dataPath, "Resources/users.xml");
		xmlUserDoc.Load (userpath);
		XmlNode usernameNode = xmlUserDoc.SelectSingleNode ("//Username");


		while (usernameNode.InnerText != gm.Username) {
			usernameNode = usernameNode.ParentNode.NextSibling.FirstChild;
		}

		var totals = usernameNode.ParentNode.SelectSingleNode(".//TotalScore").InnerText;
		xmlUserDoc.Save(userpath);
		return Int32.Parse(totals);
	}

	void SaveCoordinate() {
		var userpath = System.IO.Path.Combine (Application.dataPath, "Resources/users.xml");
		XmlDocument xmlUserDoc = new XmlDocument ();
		xmlUserDoc.Load (userpath);

		XmlNode usernameNode = xmlUserDoc.SelectSingleNode ("//Username");

		while (usernameNode.InnerText != gm.Username) {
			usernameNode = usernameNode.ParentNode.NextSibling.FirstChild;
		} 

		usernameNode.ParentNode.SelectSingleNode ("CurrentPosX").InnerText = gm.Coordinate.x.ToString();
		usernameNode.ParentNode.SelectSingleNode ("CurrentPosY").InnerText = gm.Coordinate.y.ToString();

		xmlUserDoc.Save (userpath);
	}

	public void ShowMissons() {
		missionsPanel.SetActive(true);
	}

	public void HideMissions() {
		missionsPanel.SetActive(false);
	}
}
