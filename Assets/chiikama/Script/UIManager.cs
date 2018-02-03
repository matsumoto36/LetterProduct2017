using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UIManager : SingletonMonoBehaviour<UIManager>
{
	[SerializeField, Header("OptionRoot")]
	Option OptionRootInGame;
	Option OptionRootOutGame;
	HPbar HPbarRoot;
	GameObject ResultRoot;
	GameObject GameOver;
	

	Player[] player;
	protected override void Init()
	{
		player = new Player[4];
		base.Init();

		OptionRootInGame = Instantiate(Resources.Load<Option>("System/OptionCanvasInGame"));
		OptionRootOutGame = Instantiate(Resources.Load<Option>("System/OptionCanvasOutGame"));
		DontDestroyOnLoad(OptionRootInGame);
		DontDestroyOnLoad(OptionRootOutGame);
		OptionRootInGame.gameObject.SetActive(false);
		OptionRootOutGame.gameObject.SetActive(false);

		HPbarRoot = Instantiate(Resources.Load<HPbar>("System/HP"));
		DontDestroyOnLoad(HPbarRoot);
		HPvarSwich(false);

		ResultRoot = Instantiate(Resources.Load<GameObject>("System/Result"));
		DontDestroyOnLoad(ResultRoot);
		ResultSwich(false);

		GameOver = Instantiate(Resources.Load<GameObject>("System/gameover"));
		DontDestroyOnLoad(GameOver);
		GameOverSwich(false);
	}

	// Use this for initialization
	void Start ()
	{
		
	}
	

	public void OptionSwich(bool OptionFlg)
	{

		if (OptionFlg == true)
		{
			if(GameManager.instance.nowGamePlay) {
				OptionRootInGame.gameObject.SetActive(true);
				OptionRootInGame.OnActive();
			}
			else {
				OptionRootOutGame.gameObject.SetActive(true);
				OptionRootOutGame.OnActive();
			}
			Pause.Pauser();
		}
		else
		{
			Pause.Resume();
			if(GameManager.instance.nowGamePlay) {
				OptionRootInGame.OnHide();
				OptionRootInGame.gameObject.SetActive(false);
			}
			else {
				OptionRootOutGame.OnHide();
				OptionRootOutGame.gameObject.SetActive(false);
			}
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