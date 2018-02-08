using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void ExecMethod();

public enum ButtonName {
	Title_Start,
	Title_Option,
	Title_Exit,

	Option_BGM,
	Option_SE,
	Option_Retry,
	Option_Back,

	Result_Retry,
	Result_Title,

	GameOver_Retry,
	GameOver_Select,

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
public class ControlButton : MonoBehaviour {

	const int MAX_BUTTONS = 100;                        //ゲームで登場するボタンの総数
	const float HIGHLIGHT_FREQ = 4;						//点滅表示の周波数
	const float SELECT_IMAGE_TIME = 0.3f;				//選択時の画像が表示される時間

	static ControlButton[] buttons = new ControlButton[MAX_BUTTONS];

	public bool executeSelect = true;					//ボタンのOnSelectが発動するか
	public ButtonName buttonName;						//ボタンの管理番号(重複しないでください)
	[Header("移動時にフォーカスするボタン [0] 左  [1] 下  [2] 右  [3] 上")]
	public ControlButton[] moveButtonNum;               //コントローラー入力で移動したときの次のボタン(nullで移動不可)
														//[0] 左  [1] 下  [2] 右  [3] 上
	public Sprite mainSprite;
	public Sprite highlightedSprite;
	public Sprite selectedSprite;

	public event ExecMethod onFocus;
	public event ExecMethod onFocusChanged;
	public event ExecMethod onSelect;

	public Shader buttonShader;

	bool isSelecting = false;
	Coroutine flashCoroutine;
	public Image buttonImage;

	void Awake() {

		buttons[(int)buttonName] = this;

		//マテリアル回り
		if(!buttonImage) buttonImage = GetComponent<Image>();
		buttonImage.material = new Material(buttonShader);
		buttonImage.material.SetTexture("_Texture1", mainSprite.texture);
		buttonImage.material.SetTexture("_Texture2", highlightedSprite.texture);
	}

	void OnDisable() {
		AnimCancel();
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

		buttonImage.material.SetTexture("_Texture1", selectedSprite.texture);
		buttonImage.materialForRendering.SetFloat("_Blend", 0);

		yield return new WaitForSeconds(SELECT_IMAGE_TIME);

		isSelecting = false;

		buttonImage.material.SetTexture("_Texture1", mainSprite.texture);
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

		if(onFocusChanged != null) onFocusChanged();
	}

	/// <summary>
	/// 選択・実行されたとき
	/// </summary>
	public void OnSelect() {

		if(!executeSelect) return;
		if(isSelecting) return;

		if(flashCoroutine != null) StopCoroutine(flashCoroutine);
		StartCoroutine(SelectAnim());

		if(onSelect != null) onSelect();
	}

	public void AnimCancel() {

		if(flashCoroutine != null) StopCoroutine(flashCoroutine);
		StopCoroutine(SelectAnim());

		isSelecting = false;
		buttonImage.material.SetTexture("_Texture1", mainSprite.texture);
		buttonImage.materialForRendering.SetFloat("_Blend", 0);

	}

	public static ControlButton GetButton(ButtonName buttonName) {
		return buttons[(int)buttonName];
	}
}
