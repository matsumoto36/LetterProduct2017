﻿using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;
using GamepadInput;

public class ControlButtonController : MonoBehaviour {

	const GamePad.Axis MOVE_KEY = GamePad.Axis.LeftStick;
	const GamePad.Button EXEC_KEY = GamePad.Button.A;


	const float MAX_SPEED = 10f;
	const float ACCEL = 0.1f;
	const float HIGHLIGHT_RATE = 2.5f;
	const float HIGHLIGHT_FREQ = 4;

	static List<ControlButtonController> controls = new List<ControlButtonController>();

	public int id;

	ControlButton focusButton;

	float accSpeed = 0;
	float speed = 1;

	Coroutine flashCoroutine;
	Image buttonImage;
	Color buttonColor;

	void Update() {
		CheckAndExecMove(id);
		CheckAndExecSelect(id);
	}

	/// <summary>
	/// フォーカスが移動できるかチェックして移動する
	/// </summary>
	/// <param name="controlID"></param>
	void CheckAndExecMove(int controlID) {

		Vector2 axis;

		//-1の場合はすべての入力からとる
		if(controlID != -1) axis = InputManager.GetAxis(controlID, MOVE_KEY, true);
		else axis = InputManager.GetAxisAny(MOVE_KEY, true);

		if(axis != new Vector2()) {

			accSpeed = Mathf.Min(accSpeed + ACCEL, MAX_SPEED);
			speed += accSpeed * Time.deltaTime;

			var moveAngle = CalcMoveAngle(axis);
			var nextButton = focusButton.moveButtonNum[moveAngle];
			if(nextButton && speed >= 1) {
				speed = 0;

				Focus(nextButton);
			}
		}
		else {
			accSpeed = 0;
			speed = 1;
		}
	}

	/// <summary>
	/// ボタンの機能を実行可能か調べて実行する
	/// </summary>
	/// <param name="controlID"></param>
	void CheckAndExecSelect(int controlID) {

		bool isDownButton;
		if(controlID != -1) isDownButton = InputManager.GetButtonDown(controlID, EXEC_KEY);
		else isDownButton = InputManager.GetButtonDownAny(EXEC_KEY);

		if(isDownButton) {
			focusButton.OnSelect();
		}
	}

	public void Focus(ControlButton button) {

		//フォーカス時の処理
		button.OnFocus();
		focusButton = button;

		//点滅アニメーション
		if(flashCoroutine != null) {
			StopCoroutine(flashCoroutine);
			buttonImage.color = buttonColor;
		}

		flashCoroutine = StartCoroutine(Flashing());
	}

	/// <summary>
	/// ベクトルから、ボタンの移動方向を計算する
	/// </summary>
	/// <param name="axis"></param>
	/// <returns></returns>
	int CalcMoveAngle(Vector2 axis) {
		//[0] 左  [1] 下  [2] 右  [3] 上
		var vec = Vector2.SignedAngle(new Vector2(1, 0), axis);
		int angle = (int)((vec + 180) / 90 + 0.5f);
		return angle % 4;
	}

	IEnumerator Flashing() {

		buttonImage = focusButton.GetComponent<Image>();
		buttonColor = buttonImage.color;
		var highLightColor = buttonColor * HIGHLIGHT_RATE;
		float t = 0;

		while(true) {
			t += Time.deltaTime;

			var ratio = Mathf.Abs(Mathf.Sin(t * HIGHLIGHT_FREQ));
			buttonImage.color = Color.Lerp(buttonColor, highLightColor, ratio);

			yield return null;
		}
	}

	/// <summary>
	/// コントローラーで操作するボタンシステムを生成
	/// </summary>
	/// <param name="controlID">紐づけるID。-1ですべて対象に</param>
	public static ControlButtonController CreateController(int controlID) {

		if(controls.Where(item => item.id == controlID).Count() > 0) {
			Debug.LogWarning("Do not create multiple controller!");
			return null;
		}

		var ctrl = new GameObject("[ButtonController - " + controlID + "]")
			.AddComponent<ControlButtonController>();
		ctrl.id = controlID;

		DontDestroyOnLoad(ctrl);
		controls.Add(ctrl);
		return ctrl;
	}

	/// <summary>
	/// コントローラーで操作するボタンシステムを削除
	/// </summary>
	/// <param name="controlID">削除したいID</param>
	public static void DestroyController(int controlID) {

		if(controls.Where(item => item.id == controlID).Count() == 0) {
			Debug.LogWarning("Failed Destroy controller.");
			return;
		}

		Destroy(controls.Find(item => item.id == controlID).gameObject);
	}
}