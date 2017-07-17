using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TicketsController : MonoBehaviour {

	public GameObject player;
	public GameObject score;
	public GameObject backButton;
	public GameObject buyButton;
	public GameObject popup;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ClosePanel() {
		gameObject.SetActive(false);
	}

	public void OpenPanel() {
		gameObject.SetActive(true);
	}

	public void GetTicket() {
		popup.SetActive(true);
	}

	public void Comfirmed() {
		ClosePanel();
		var ferry = GameObject.Find("Ferry");
		// move character to the ferry
		player.transform.DOMove(new Vector3(-479.2394f, 177.2796f, 0f), 3f).OnComplete(() => {
			player.transform.parent = ferry.transform;
			
			var seq = DOTween.Sequence ();  
			// move ferry 
			seq.Append(ferry.GetComponent<DOTweenPath>().tween)
				.OnComplete(() => {
					// move character to the target island
					player.transform.parent = null;
					player.transform.DOMove(new Vector3(830f, -30f, 0f), 3f);
				});
		});
	}

	public void NotComfirmed() {
		popup.SetActive(false);
	}
}
