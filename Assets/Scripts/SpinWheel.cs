using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class SpinWheel : MonoBehaviour {


	// Use this for initialization
	void Start () {
		RandomFunc();
	}
	
	// Update is called once per frame
	void Update () {

	}

	void RandomFunc() {
		int num = (int)Random.Range (1f, 7f);
		//print (num);

	
		int val = 3600 + (int)Random.Range ((num - 1) * 60f + 3, 60f * num - 3);

		Sequence spinSequence = DOTween.Sequence ();  
		spinSequence.SetEase (Ease.OutCirc)
		.Append (transform.DORotate (new Vector3 (0, 0, val), 3, RotateMode.FastBeyond360));

	}


}
