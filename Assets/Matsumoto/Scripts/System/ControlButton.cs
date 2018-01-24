using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void ExecMethod();

public enum ButtonName {
	Title_Start,
	Option,

	Pl1Center,
	Pl1Left,
	Pl1Down,
	Pl1Right,
	Pl1Up,
	Pl2Center,
	Pl2Left,
	Pl2Down,
	Pl2Right,
	Pl2Up,
	Pl3Center,
	Pl3Left,
	Pl3Down,
	Pl3Right,
	Pl3Up,
	Pl4Center,
	Pl4Left,
	Pl4Down,
	Pl4Right,
	Pl4Up,
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

		buttonImage.material.SetTexture("_Texture1", button.spriteState.pressedSprite.texture);
		buttonImage.materialForRendering.SetFloat("_Blend", 0);

		yield return new WaitForSeconds(SELECT_IMAGE_TIME);

		buttonImage.material.SetTexture("_Texture1", button.targetGraphic.mainTexture);
	}

	/// <summary>
	/// カーソルが来たとき
	/// </summary>
	public void OnFocus() {

		flashCoroutine = StartCoroutine(Flashing());

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

		if(flashCoroutine != null) StopCoroutine(flashCoroutine);
		StartCoroutine(SelectAnim());

		if(onSelect != null) onSelect();
	}

	public static ControlButton GetButton(ButtonName buttonName) {
		return buttons[(int)buttonName];
	}
}
