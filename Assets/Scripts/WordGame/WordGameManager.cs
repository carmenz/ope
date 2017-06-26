using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;

public class WordGameManager : MonoBehaviour {

	private GameManager gm;
	private WordGameManager wgm;

	// Use this for initialization
	void Start () {
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		wgm = GameObject.Find("WordGameManager").GetComponent<WordGameManager>();

		XmlDocument xmlWordGameDoc = new XmlDocument ();

		xmlWordGameDoc.Load (gm.Path);
		XmlNode indexNode = xmlWordGameDoc.SelectSingleNode ("//Index");
	}


	public void ThisPillarOnClick()
	{


	}
	public void OtherPillarOnClick()
	{


	}

	// Update is called once per frame
	void Update () {
		
	}
}
