using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class Unit : MonoBehaviour {


	public float moveSpeed;

	protected GameObject body;
	protected Vector3 moveVec;

	Rigidbody unitRig;

	// Use this for initialization
	public virtual void Awake () {
		body = transform.GetChild(0).gameObject;
		unitRig = GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	public virtual void FixedUpdate () {
		Move();
	}

	public virtual void Move() {
		unitRig.MovePosition(transform.position + moveVec * moveSpeed * Time.deltaTime);
	}

	public virtual void Attack() {

	}
}
