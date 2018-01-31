using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDoorManager : MonoBehaviour
{
	Animator doorMove;

	// Use this for initialization
	void Start ()
	{
		doorMove = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerStay(Collider other)
	{
		doorMove.SetBool("character_nearby", true);
	}

	private void OnTriggerExit(Collider other)
	{
		doorMove.SetBool("character_nearby", false);
	}
}
