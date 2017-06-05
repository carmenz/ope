using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;

public class User{

	[XmlAttribute("username")]
		public string username;
	

	[XmlElement("Password")]
		public int password;

	[XmlElement("Currentpos")]
	public int currentpos;



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
