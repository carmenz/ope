using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;


public class SpinWheel : MonoBehaviour {
	[SerializeField]
	int angleToAvoid = 2;
	bool rotatable;
	int point;
	public GameObject panel;
	private int currentScore = 0;

	void Awake () {
		setWheelRotatable(true);
	}

	public void setWheelRotatable(bool state) {
		rotatable = state;
	}

	public void RandomFunc() {

		var map = new int[] {30, 10, 1, 30, 10, 60, 1, 10, 30, 10, 1, 10};
		// Check if button is active to avoid useless click
		if (!rotatable)
			return;
		// Start random
		setWheelRotatable(false);

		var num = Random.Range(1, 13);
		point = map[num - 1];
		int val = 3600 + Random.Range ((num - 1) * 30 + angleToAvoid, 30 * num - angleToAvoid);
		Debug.Log("Player got " + point);

		GameObject star = GameObject.Find("Star");
		star.GetComponent<Animator>().SetBool("onClick", true);

		Sequence spinSequence = DOTween.Sequence ();  
		spinSequence.SetEase (Ease.OutCirc)
			.Append (transform.DORotate (new Vector3 (0, 0, val), 5, RotateMode.FastBeyond360))
			.OnComplete(() => {
				star.GetComponent<Animator>().SetBool("onClick", false);
				panel.SetActive (true);

				Text panelScore = GameObject.Find("PanelScore").GetComponent<Text>();
				GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
				panelScore.text = point.ToString ();
				gm.updateDBTotalScore(point);
			});
	}






}
