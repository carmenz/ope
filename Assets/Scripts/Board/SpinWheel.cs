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

	protected Animation anim;

	public void RandomFunc() {
		// Check if button is active to avoid useless click
		if (!rotatable)
			return;
		// Start random
		setWheelRotatable(false);
		player = GameObject.FindGameObjectWithTag("Player");
		int num = Random.Range (1, 9);
		var chance = num > 4 ? num - 4: num;
		print(chance);
		
		int val = 3600 + Random.Range ((num - 1) * 45 + angleToAvoid, 45 * num - angleToAvoid);

		GameObject star = GameObject.Find("StarButton");
		if (!star.GetComponent<Animation> ().isPlaying) {
			print ("hahahah");
			star.GetComponent<Animation>().Play();
		}

		Sequence spinSequence = DOTween.Sequence ();  
		spinSequence.SetEase (Ease.OutCirc)
			.Append (transform.DORotate (new Vector3 (0, 0, val), 3, RotateMode.FastBeyond360));
			//.OnComplete(() => player.GetComponent<Player>().Move(chance));


	}

	public void OnClick() {
		GameObject star = GameObject.Find("StarButton");


		if (!star.GetComponent<Animation> ().isPlaying) {
			print ("hahahah");
			star.GetComponent<Animation>().Play();
		}
		print ("heheheh");

	}
}
