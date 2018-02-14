using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorStopper : MonoBehaviour {

	Animator anim;

	public bool isStop;
	bool _isStop;

	void Start() {

		anim = GetComponent<Animator>();

	}

	// Update is called once per frame
	void Update () {

		if(isStop != _isStop) {
			if(!anim) anim = GetComponent<Animator>();
			anim.speed = isStop ? 0 : 1;
			_isStop = isStop;
		}
	}
}
