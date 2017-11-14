using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// レーザー光線
/// </summary>
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

	public override void OnHitEnter(Collider other) {

	}
}
