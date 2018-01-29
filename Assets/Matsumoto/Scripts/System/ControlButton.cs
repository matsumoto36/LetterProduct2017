using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void ExecMethod();

public enum ButtonName {
	Title_Start,
	Option,

	Pl1Weapon1,
	Pl1Weapon2,
	Pl1Weapon3,
	Pl1Weapon4,
	Pl1Weapon5,

	Pl2Weapon1,
	Pl2Weapon2,
	Pl2Weapon3,
	Pl2Weapon4,
	Pl2Weapon5,

	Pl3Weapon1,
	Pl3Weapon2,
	Pl3Weapon3,
	Pl3Weapon4,
	Pl3Weapon5,

	Pl4Weapon1,
	Pl4Weapon2,
	Pl4Weapon3,
	Pl4Weapon4,
	Pl4Weapon5,
}

/// <summary>
/// コントローラー操作に対応したボタン
/// </summary>
[RequireComponent(typeof(Button))]
public class ControlButton : MonoBehaviour {

	const int MAX_BUTTONS = 100;                        //ゲームで登場するボタンの総数
	const float HIGHLIGHT_FREQ = 4;						//点滅表示の周波数
	const float SELECT_IMAGE_TIME = 0.3f;				//選択時の画像が表示される時間

	static ControlButton[] buttons = new ControlButton[MAX_BUTTONS];

	public ButtonName buttonName;						//ボタンの管理番号(重複しないでください)
	[Header("移動時にフォーカスするボタン [0] 左  [1] 下  [2] 右  [3] 上")]
	public ControlButton[] moveButtonNum;				//コントローラー入力で移動したときの次のボタン(nullで移動不可)
														//[0] 左  [1] 下  [2] 右  [3] 上
	public event ExecMethod onFocus;
	public event ExecMethod onSelect;

	public Shader buttonShader;

	bool isSelecting = false;
	Coroutine flashCoroutine;
	Button button;
	Image buttonImage;

	void Awake() {

		buttons[(int)buttonName] = this;

		//マテリアル回り
		button = GetComponent<Button>();
		buttonImage = GetComponent<Image>();
		buttonImage.material = new Material(buttonShader);
		buttonImage.material.SetTexture("_Texture1", button.targetGraphic.mainTexture);
		buttonImage.material.SetTexture("_Texture2", button.spriteState.highlightedSprite.texture);
	}

	IEnumerator Flashing() {

		float t = 0;
		while(true) {
			if(!buttonImage) yield break;

			t += Time.deltaTime;

			var ratio = Mathf.Abs(Mathf.Sin(t * HIGHLIGHT_FREQ));
			buttonImage.materialForRendering.SetFloat("_Blend", ratio);

			yield return null;
		}
	}

	IEnumerator SelectAnim() {

		isSelecting = true;

		buttonImage.material.SetTexture("_Texture1", button.spriteState.pressedSprite.texture);
		buttonImage.materialForRendering.SetFloat("_Blend", 0);

		yield return new WaitForSeconds(SELECT_IMAGE_TIME);

		isSelecting = false;

		buttonImage.material.SetTexture("_Texture1", button.targetGraphic.mainTexture);
	}

	public void Flash() {
		flashCoroutine = StartCoroutine(Flashing());
	}

	/// <summary>
	/// カーソルが来たとき
	/// </summary>
	public void OnFocus() {

		Flash();
		if(onFocus != null) onFocus();
	}

	/// <summary>
	/// 自分からフォーカスが移動する直前
	/// </summary>
	public void OnFocusChanged() {
		if(flashCoroutine != null) {
			StopCoroutine(flashCoroutine);
			buttonImage.materialForRendering.SetFloat("_Blend", 0);
		}
	}

	/// <summary>
	/// 選択・実行されたとき
	/// </summary>
	public void OnSelect() {

		if(isSelecting) return;

		if(flashCoroutine != null) StopCoroutine(flashCoroutine);
		StartCoroutine(SelectAnim());

		if(onSelect != null) onSelect();
	}

	public void AnimCancel() {

		if(flashCoroutine != null) StopCoroutine(flashCoroutine);
		StopCoroutine(SelectAnim());

		isSelecting = false;
		buttonImage.material.SetTexture("_Texture1", button.targetGraphic.mainTexture);
		buttonImage.materialForRendering.SetFloat("_Blend", 0);

	}

	public static ControlButton GetButton(ButtonName buttonName) {
		return buttons[(int)buttonName];
	}
}
