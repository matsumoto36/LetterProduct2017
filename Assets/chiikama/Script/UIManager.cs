using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UIManager : SingletonMonoBehaviour<UIManager>
{
	[SerializeField, Header("OptionRoot")]
	GameObject OptionRoot;

	protected override void Init()
	{
		base.Init();

		OptionRoot = Instantiate(Resources.Load<GameObject>("System/OptionCanvas"));
		DontDestroyOnLoad(OptionRoot);

	}

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	

	public void OptionSwich(bool OptionFlg)
	{
		
		OptionRoot.SetActive(OptionFlg);
		if (OptionFlg == true)
		{
			Pause.Pauser();
		}
		else
		{
			Pause.Resume();
		}

	}


	
}