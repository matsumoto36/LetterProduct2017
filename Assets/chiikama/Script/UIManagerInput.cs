using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManagerInput : MonoBehaviour
{
	bool OptionFlg;
	bool HPFlg;

	public void Awake()
	{
		Pause.ClearPauseList();
	}

	void Start()
	{
		OptionFlg = false;
		HPFlg = false;

	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.O))
		{
			OptionFlg = !OptionFlg;
			UIManager.instance.OptionSwich(OptionFlg);
		}
		if(Input.GetKeyDown(KeyCode.P))
		{
			HPFlg = !HPFlg;
			UIManager.instance.HPvarSwich(HPFlg);
		}
	}
}
