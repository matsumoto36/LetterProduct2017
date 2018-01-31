using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManagerInput : MonoBehaviour
{
	bool OptionFlg;


	// Use this for initialization
	public void Awake()
	{
		Pause.ClearPauseList();

	}

	void Start()
	{
		OptionFlg = false;

	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.O))
		{
			OptionFlg = !OptionFlg;
			UIManager.instance.OptionSwich(OptionFlg);

		}
	}

}
