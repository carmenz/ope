using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FerryAnimController : MonoBehaviour {

	Animator animator;
	bool _curValue = false;

	void Awake () {
		animator = gameObject.GetComponent<Animator>();
		animator.SetBool("move", _curValue);
	}
	
	public void Toggle() {
		_curValue = !_curValue;
		animator.SetBool("move", _curValue);
	}
}
