using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class UIManager : SingletonMonoBehaviour<UIManager>
{

	public bool isOpenOption { get; private set; }
	Option OptionRootInGame;
	Option OptionRootOutGame;


	HPbar HPbarRoot;
	ResultRoot ResultRoot;
	GameOverUI GameOver;

	RectTransform gaugeCanvas;
	Gauge gaugePre;

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
		HPbarRoot.gameObject.SetActive(false);

		ResultRoot = Instantiate(Resources.Load<ResultRoot>("System/Result"));
		DontDestroyOnLoad(ResultRoot);
		ResultRoot.gameObject.SetActive(false);

		GameOver = Instantiate(Resources.Load<GameOverUI>("System/gameover"));
		DontDestroyOnLoad(GameOver);
		GameOver.gameObject.SetActive(false);

		gaugeCanvas = Instantiate(Resources.Load<RectTransform>("System/GaugeCanvas"));
		DontDestroyOnLoad(gaugeCanvas);
		gaugePre = Resources.Load<Gauge>("System/Gauge");
	}

	// Use this for initialization
	void Start ()
	{
		
	}
	

	public void OptionSwich(bool OptionFlg)
	{
		isOpenOption = OptionFlg;

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

		

		if (HPFlg){
			HPbarRoot.gameObject.SetActive(true);
			HPbarRoot.Init();
		}
		else {
			HPbarRoot.OnHide();
			HPbarRoot.gameObject.SetActive(false);
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

	/// <summary>
	/// ゲージを出す
	/// </summary>
	/// <returns></returns>
	public Gauge CreateGauge(Vector3 position) {
		return CreateGauge(position, gaugePre);
	}

	/// <summary>
	/// ゲージを出す(プレハブをオーバーライド)
	/// </summary>
	/// <param name="position"></param>
	/// <param name="prefab"></param>
	/// <returns></returns>
	public Gauge CreateGauge(Vector3 position, Gauge prefab) {
		var instance = Instantiate(prefab, gaugeCanvas);
		instance.transform.SetParent(gaugeCanvas);
		instance.SetPosition(position);
		return instance;
	}
}