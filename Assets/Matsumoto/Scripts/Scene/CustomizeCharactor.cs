using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using GamepadInput;
using System.Linq;

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

	public Transform viewModelAnchor;

	public bool isFreeze { get; set; }
	public bool isReady { get; private set; }

	SelectState state = SelectState.Entry;
	SelectWeaponButton[] selectedButton;

	int playerNum;

	ControlButtonController controller;
	GameObject viewModel;

	// Use this for initialization
	public void Init () {

		selectedButton = new SelectWeaponButton[2];

		//使用する武器のデータをロード
		foreach(var item in weaponDataButton) {
			//初期化処理
			item.Init();
			//カーソル合わせた時
			item.button.onFocus += () => {
				if(viewModel) Destroy(viewModel);
				viewModel = Instantiate(item.viewData.model, viewModelAnchor.position, viewModelAnchor.rotation);
				viewModel.transform.SetParent(viewModelAnchor);
			};
			//クリックされたとき
			item.button.onSelect += () => {
				OnButtonDown(item);
			};
		}

		ShowPanel(PanelName.Enrty);
	}

	// Update is called once per frame
	void Update () {

		if(isFreeze) return;

		if(InputManager.GetButtonDown(playerNum, BACK_BUTTON)) {
			Back();
		}

		viewModelAnchor.rotation *= Quaternion.AngleAxis(MODEL_ROTATE_SPEED * Time.deltaTime, Vector3.up);

	}

	void Back() {

		switch(state) {
			case SelectState.FirstWeapon:
				state--;

				Exit();
				break;
			case SelectState.SecondWeapon:
				state--;

				controller.Focus(selectedButton[0].button);
				RemoveWeapon(0);
				break;
			case SelectState.Ready:
				state--;

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

				SetWeapon(0, button);
				controller.Focus(button.button);
				break;
			case SelectState.SecondWeapon:
				state++;

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

		//var unitData = Instantiate(customizeTarget);
		Debug.Log(customizeTarget);

		customizeTarget.weaponData = selectedButton
			.Select(item => item.viewData)
			.ToArray();

		//AssetDatabase.CreateAsset(unitData, );
		//AssetDatabase.Refresh();
	}
}
