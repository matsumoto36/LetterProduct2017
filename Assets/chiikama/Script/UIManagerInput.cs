using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManagerInput : MonoBehaviour
{
	bool OptionFlg;

	public void Awake()
	{
		Pause.ClearPauseList();
	}

	void Start()
	{
		OptionFlg = false;
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.O))
		{
			OptionFlg = !OptionFlg;
			UIManager.instance.OptionSwich(OptionFlg);
		}
	}
}
