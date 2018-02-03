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
	ResultRoot ResultRoot;
	GameOverUI GameOver;
	

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

		ResultRoot = Instantiate(Resources.Load<ResultRoot>("System/Result"));
		DontDestroyOnLoad(ResultRoot);
		ResultSwich(false);

		GameOver = Instantiate(Resources.Load<GameOverUI>("System/gameover"));
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
			if(GameManager.nowPlayingGame) {
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
			if(GameManager.nowPlayingGame) {
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
		if(ResultFlg) {

			ResultRoot.gameObject.SetActive(true);
			ResultRoot.OnActive();
		}
		else {

			ResultRoot.OnHide();
			ResultRoot.gameObject.SetActive(false);
		}
	}

	public void GameOverSwich(bool goFlg)
	{
		if(goFlg) {
			
			GameOver.gameObject.SetActive(true);
			GameOver.OnActive();
		}
		else {
			
			GameOver.OnHide();
			GameOver.gameObject.SetActive(false);
		}

	}
}