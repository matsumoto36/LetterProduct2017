using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;

/// <summary>
/// コントローラタイプと番号を対応付けるクラス
/// </summary>
public class ControllerData {

	public ControlType type;
	public GamePad.Index controllerIndex;

	public ControllerData(ControlType type, GamePad.Index controllerIndex) {
		this.type = type;
		this.controllerIndex = controllerIndex;
	}
}
