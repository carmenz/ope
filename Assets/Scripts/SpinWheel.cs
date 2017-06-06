using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class SpinWheel : MonoBehaviour {
	[SerializeField]
	GameObject player;
	bool rotatable;

	void Awake () {
		setWheelRotatable(true);
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

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
		int num = (int)Random.Range (1f, 7f);
		print(num);
		
		int val = 3600 + (int)Random.Range ((num - 1) * 45f + 3, 45f * num - 3);

		Sequence spinSequence = DOTween.Sequence ();  
		spinSequence.SetEase (Ease.OutCirc)
			.Append (transform.DORotate (new Vector3 (0, 0, val), 3, RotateMode.FastBeyond360))
			.OnComplete(() => player.GetComponent<Player>().Move(num));

	}


}
