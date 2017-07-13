using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class SpinWheel : MonoBehaviour {
	[SerializeField]
	GameObject player;
	[SerializeField]
	int angleToAvoid = 8;
	bool rotatable;



	void Awake () {
		setWheelRotatable(true);
	}

	public void setWheelRotatable(bool state) {
		rotatable = state;
	}

	public void RandomFunc() {
		// Check if button is active to avoid useless click
		if (!rotatable)
			return;
		// Start random
		setWheelRotatable(false);
		player = GameObject.FindGameObjectWithTag("Player");

		int[] values = new int[4];
		values [0] = 1;
		values [1] = 10;
		values [2] = 30;
		values [3] = 60;
		int num = values[Random.Range(0,values.Length)];


		//int num = Random.Range (1, 9);
		//var chance = num > 4 ? num - 4: num;
		print(num);
		int val;
		if (num == 1) {
			int[] degreesForOne = new int[3];
			degreesForOne [0] = 60;
			degreesForOne [1] = 180;
			degreesForOne [2] = 300;
			val = degreesForOne [Random.Range (0, degreesForOne.Length)];
		} else if (num == 10) {
			int[] degreesForTen = new int[4];
			degreesForTen [0] = 30;
			degreesForTen [1] = 120;
			degreesForTen [2] = 270;
			degreesForTen [3] = 330;
			val = degreesForTen [Random.Range (0, degreesForTen.Length)];
		} else if (num == 30) {
			int[] degreesForThirty = new int[3];
			degreesForThirty [0] = 0;
			degreesForThirty [1] = 90;
			degreesForThirty [2] = 240;
			val = degreesForThirty [Random.Range (0, degreesForThirty.Length)];
		} else {
			val = 150;
		}
		print (val);
		val = 3600 + Random.Range(val, val+30);
		//int val = 3600 + Random.Range ((num - 1) * 45 + angleToAvoid, 45 * num - angleToAvoid);

		GameObject star = GameObject.Find("Star");
		star.GetComponent<Animator>().SetBool("onClick", true);

		Sequence spinSequence = DOTween.Sequence ();  
		spinSequence.SetEase (Ease.OutCirc)
			.Append (transform.DORotate (new Vector3 (0, 0, val), 5, RotateMode.FastBeyond360))
			.OnComplete(() => {
				
				star.GetComponent<Animator>().SetBool("onClick", false);
			});
			//.OnComplete(() => player.GetComponent<Player>().Move(chance));



	}

}
