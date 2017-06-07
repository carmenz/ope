using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public Button saveButton;
	public Button loadButton;
	public const string playerPath = "Prefebs/player";

	private static string datapath = string.Empty;


	void Awake() {
		if (Application.platform == RuntimePlatform.WindowsPlayer) {
			datapath = System.IO.Path.Combine (Application.persistentDataPath, "Resources/users.xml");
		} else {
			datapath = System.IO.Path.Combine (Application.dataPath, "Resources/users.xml");
		}

	}

	void Start() {
		CreateUser (playerPath, new Vector3 (0,0,0), Quaternion.identity);
	}



	public static User CreateUser(string path, Vector3 position, Quaternion rotation) {
		
		GameObject prefab = Resources.Load<GameObject> (path);

		GameObject go = GameObject.Instantiate (prefab, position, Quaternion.identity) as GameObject;
		User user = go.GetComponent<User> () ?? go.AddComponent<User> ();

		return user;
	}
		

	public static User CreateUser(UserData data, string path, Vector3 position, Quaternion rotation) {
		
		GameObject prefab = Resources.Load<GameObject> (path);
		GameObject go = GameObject.Instantiate (prefab, position, rotation) as GameObject;
		User user = go.GetComponent<User> () ?? go.AddComponent<User> ();

		user.data = data;
		return user;
	}

	void OnEnable() {
		saveButton.onClick.AddListener (delegate {
			SaveData.Save (datapath, SaveData.userContainer);
		});
		// loadButton.onClick.AddListener(delegate {
		// 	SaveData.Load(datapath);
		// });
	}

	void OnDisable() {
		saveButton.onClick.RemoveListener (delegate {
			SaveData.Save (datapath, SaveData.userContainer);
		});
		loadButton.onClick.RemoveListener(delegate {
			SaveData.Load(datapath);
		});
	}

}
