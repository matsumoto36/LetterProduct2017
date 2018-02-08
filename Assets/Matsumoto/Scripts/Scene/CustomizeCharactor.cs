using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using GamepadInput;
using System.Linq;
using System;

enum PanelName { Enrty, Weapon, Ready}
enum SelectState { Entry, FirstWeapon, SecondWeapon, Ready}

public class CustomizeCharactor : MonoBehaviour {

	const GamePad.Button BACK_BUTTON = GamePad.Button.B;
	const float MODEL_ROTATE_SPEED = 100;
	const string ASSET_SAVE_PATH = "Data/Unit/Players/";

	public SelectSceneController owner;

	public UnitData customizeTarget;

	public RectTransform entryPanel;
	public RectTransform weaponPanel;
	public RectTransform readyPanel;

	public SelectWeaponButton[] weaponDataButton;

	public Image[] inventory;

	public Text[] passiveText;

	public Transform viewModelAnchor;

	public bool isFreeze { get; set; }
	public bool isReady { get; private set; }

	SelectState state = SelectState.Entry;
	SelectWeaponButton[] selectedButton;

	int playerNum;

	ControlButtonController controller;

	StatusModifier tempMod;
	StatusModifier viewMod;
	StatusModifier firstMod;
	

	GameObject viewModel;

	// Use this for initialization
	public void Init () {

		tempMod = new StatusModifier();
		viewMod = new StatusModifier();
		firstMod = new StatusModifier();

		selectedButton = new SelectWeaponButton[2];

		//使用する武器のデータをロード
		foreach(var item in weaponDataButton) {
			//初期化処理
			item.Init();
			//カーソル合わせた時
			item.button.onFocus += () => {

				AudioManager.PlaySE("Weapon_Button");

				if(viewModel) Destroy(viewModel);
				viewModel = Instantiate(item.viewData.model, viewModelAnchor.position, viewModelAnchor.rotation);
				viewModel.transform.SetParent(viewModelAnchor);

				//カーソルにあっている武器のパッシブ効果を計算して表示
				viewMod = firstMod + item.viewData.mod;
			};
			//クリックされたとき
			item.button.onSelect += () => {
				OnButtonDown(item);
			};
		}

		ShowPanel(PanelName.Enrty);
	}

	// Update is called once per frame
	public void SelectUpdate () {

		if(isFreeze) return;

		if(InputManager.GetButtonDown(playerNum, BACK_BUTTON)) {
			Back();
		}

		viewModelAnchor.rotation *= Quaternion.AngleAxis(MODEL_ROTATE_SPEED * Time.deltaTime, Vector3.up);

		PassiveUpdate();
	}

	void Back() {

		switch(state) {
			case SelectState.FirstWeapon:
				state--;

				AudioManager.PlaySE("back_button");

				Exit();
				break;
			case SelectState.SecondWeapon:
				state--;

				AudioManager.PlaySE("back_button");

				controller.Focus(selectedButton[0].button);

				firstMod = new StatusModifier();
				viewMod = selectedButton[0].viewData.mod;
				RemoveWeapon(0);
				break;
			case SelectState.Ready:
				state--;

				AudioManager.PlaySE("back_button");

				isReady = false;
				owner.ReadyCancel();

				ShowPanel(PanelName.Weapon);

				controller = ControlButtonController.CreateController(playerNum);
				controller.Focus(selectedButton[1].button);
				RemoveWeapon(1);
				break;
			default:
				break;
		}

	}

	void OnButtonDown(SelectWeaponButton button) {

		switch(state) {
			case SelectState.FirstWeapon:
				state++;

				AudioManager.PlaySE("Weapon_select");

				SetWeapon(0, button);

				firstMod = button.viewData.mod;
				viewMod = firstMod + button.viewData.mod;

				button.button.Flash();
				break;
			case SelectState.SecondWeapon:
				state++;

				AudioManager.PlaySE("Weapon_select");

				SetWeapon(1, button);
				isReady = true;
				Ready();
				break;
			default:
				break;
		}
	}

	void Ready() {

		foreach(var item in weaponDataButton) {
			item.button.AnimCancel();
		}

		ShowPanel(PanelName.Ready);

		ControlButtonController.DestroyController(playerNum);

		owner.Ready();
	}

	void Exit() {

		foreach(var item in weaponDataButton) {
			item.button.AnimCancel();
		}

		ShowPanel(PanelName.Enrty);

		ControlButtonController.DestroyController(playerNum);

		owner.Exit(playerNum);
	}

	/// <summary>
	/// 武器をインベントリにセットする(見た目のみ)
	/// </summary>
	/// <param name="image"></param>
	/// <param name="icon"></param>
	void SetWeapon(int weaponNum, SelectWeaponButton button) {

		inventory[weaponNum].sprite = button.viewData.icon;
		inventory[weaponNum].color = new Color(1, 1, 1, 1);
		selectedButton[weaponNum] = button;		
	}

	/// <summary>
	/// 武器をインベントリから消す(見た目のみ)
	/// </summary>
	/// <param name="weaponNum"></param>
	void RemoveWeapon(int weaponNum) {
		inventory[weaponNum].color = new Color(1, 1, 1, 0);
	}

	/// <summary>
	/// パッシブ効果を表示
	/// </summary>
	void PassiveUpdate() {

		var speed = 2f * Time.deltaTime;

		tempMod.mulHP = Mathf.MoveTowards(tempMod.mulHP, viewMod.mulHP, speed);
		tempMod.mulMoveSpeed = Mathf.MoveTowards(tempMod.mulMoveSpeed, viewMod.mulMoveSpeed, speed);
		tempMod.mulPow = Mathf.MoveTowards(tempMod.mulPow, viewMod.mulPow, speed);
		tempMod.mulAttackSpeed = Mathf.MoveTowards(tempMod.mulAttackSpeed, viewMod.mulAttackSpeed, speed);

		passiveText[0].text = tempMod.mulHP.ToString("f1");
		passiveText[1].text = tempMod.mulMoveSpeed.ToString("f1");
		passiveText[2].text = tempMod.mulPow.ToString("f1");
		passiveText[3].text = tempMod.mulAttackSpeed.ToString("f1");
	}

	/// <summary>
	/// 各パネルを表示する
	/// </summary>
	/// <param name="panel"></param>
	void ShowPanel(PanelName panel) {

		switch(panel) {
			case PanelName.Enrty:
				entryPanel.gameObject.SetActive(true);
				weaponPanel.gameObject.SetActive(false);
				readyPanel.gameObject.SetActive(false);
				break;
			case PanelName.Weapon:
				entryPanel.gameObject.SetActive(false);
				weaponPanel.gameObject.SetActive(true);
				readyPanel.gameObject.SetActive(false);
				break;
			case PanelName.Ready:
				entryPanel.gameObject.SetActive(false);
				weaponPanel.gameObject.SetActive(true);
				readyPanel.gameObject.SetActive(true);
				break;
			default:
				break;
		}

	}

	public void Entry(int playerNum) {

		this.playerNum = playerNum;

		ShowPanel(PanelName.Weapon);

		controller = ControlButtonController.CreateController(playerNum);

		controller.Focus(weaponDataButton[0].button);

		state++;
	}

	public void Customize() {

		customizeTarget.hp = owner.playerBase.hp;
		customizeTarget.dropExp = owner.playerBase.dropExp;
		customizeTarget.nextLevelExp = owner.playerBase.nextLevelExp;
		customizeTarget.moveSpeed = owner.playerBase.moveSpeed;
		customizeTarget.rotSpeed = owner.playerBase.rotSpeed;
		customizeTarget.isDrawWeapon = owner.playerBase.isDrawWeapon;
		customizeTarget.deathSE = owner.playerBase.deathSE;
		customizeTarget.deathParticle = owner.playerBase.deathParticle;

		customizeTarget.weaponData = selectedButton
			.Select(item => item.viewData)
			.ToArray();

	}

	/// <summary>
	/// ババっと上がる
	/// </summary>
	/// <param name="Duration"></param>
	/// <param name="start"></param>
	/// <param name="end"></param>
	/// <returns></returns>
	IEnumerator NumberMoveAnim(float Duration, float start, float end, Action<float> execute) {

		float t = 0.0f;
		while((t += Time.deltaTime / Duration) < 1.0f) {
			execute(Mathf.Lerp(start, end, t));
			yield return null;
		}

		execute(end);
	}


}
