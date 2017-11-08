using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLaser : Bullet {

	public float length {
		get { return _length; }
		set {
			var s = transform.localScale;
			s.z = _length = value;
			transform.localScale = s;
		}
	}
	float _length;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
