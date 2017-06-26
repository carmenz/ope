using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Missions {
	public List<int> quizRequired = new List<int>();
	public string quizInfo;

	public List<int> storyRequired = new List<int>();
	public string storyInfo;

	public List<int> gameRequired = new List<int>();
	public string gameInfo;

	public List<int> itemsRequired = new List<int>();
	public string itemInfo;
}
