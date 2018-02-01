using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UIManager : SingletonMonoBehaviour<UIManager>
{
	[SerializeField, Header("OptionRoot")]
	GameObject OptionRoot;
	HPbar HPbarRoot;
	GameObject ResultRoot;
	GameObject GameOver;
	

	Player[] player;
	protected override void Init()
	{
		player = new Player[4];
		base.Init();

		OptionRoot = Instantiate(Resources.Load<GameObject>("System/OptionCanvas"));
		HPbarRoot = Instantiate(Resources.Load<HPbar>("System/HP"));
		ResultRoot = Instantiate(Resources.Load<GameObject>("System/Result"));
		GameOver = Instantiate(Resources.Load<GameObject>("System/gameover"));
		DontDestroyOnLoad(OptionRoot);
		DontDestroyOnLoad(HPbarRoot);
		DontDestroyOnLoad(ResultRoot);
		DontDestroyOnLoad(GameOver);

		OptionSwich(false);
		HPvarSwich(false);
		ResultSwich(false);
		GameOverSwich(false);
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
	public void ResultSwich(bool ResultFlg)
	{
		ResultRoot.gameObject.SetActive(ResultFlg);
		if (ResultFlg)
		{

		}
	}

	public void GameOverSwich(bool goFlg)
	{
		GameOver.gameObject.SetActive(goFlg);
	}
}