using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UIManager : SingletonMonoBehaviour<UIManager>
{
	[SerializeField, Header("OptionRoot")]
	GameObject OptionRoot;
	HPbar HPbarRoot;

	Player[] player;
	protected override void Init()
	{
		player = new Player[4];
		base.Init();

		OptionRoot = Instantiate(Resources.Load<GameObject>("System/OptionCanvas"));
		HPbarRoot = Instantiate(Resources.Load<HPbar>("System/HP"));
		DontDestroyOnLoad(OptionRoot);
		DontDestroyOnLoad(HPbarRoot);

		OptionSwich(false);
		HPvarSwich(false);
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

	public void HPvarSwich(bool HPFlg)
	{

		HPbarRoot.gameObject.SetActive(HPFlg);

		if (HPFlg){
			HPbarRoot.Init();
		}
	}
}