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
public class ControlButton : MonoBehaviour {

	const int MAX_BUTTONS = 100;    //ゲームで登場するボタンの総数

	static ControlButton[] buttons = new ControlButton[MAX_BUTTONS];

	public ButtonName buttonName;                       //ボタンの管理番号(重複しないでください)
	[Header("移動時にフォーカスするボタン [0] 左  [1] 下  [2] 右  [3] 上")]
	public ControlButton[] moveButtonNum;               //コントローラー入力で移動したときの次のボタン(nullで移動不可)
														//[0] 左  [1] 下  [2] 右  [3] 上
	public event ExecMethod onFocus;
	public event ExecMethod onSelect;

	void Awake() {
		buttons[(int)buttonName] = this;
	}

	/// <summary>
	/// カーソルが来たとき
	/// </summary>
	public void OnFocus() {
		if(onFocus != null) onFocus();
	}

	/// <summary>
	/// 選択・実行されたとき
	/// </summary>
	public void OnSelect() {
		if(onSelect != null) onSelect();
	}

	public static ControlButton GetButton(ButtonName buttonName) {
		return buttons[(int)buttonName];
	}
}
