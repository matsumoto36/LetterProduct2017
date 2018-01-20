using UnityEngine;
using System.Collections;

/// <summary>
/// プレイヤーをスポーンするクラス。GameManagerに移行する予定。
/// </summary>
public class TestSpawnUnit : MonoBehaviour {

	// Use this for initialization
	void Start() {

		//処理はGameManagerに移行してください
		//UnitManager.LoadUnitData();

		var playerData = Resources.Load<PlayerData>("Data/Unit/Player");

		var player1 = (Player)playerData.Spawn(new Vector3(), Quaternion.identity);
		InputManager.SetControllerData(0, ControlType.Keyboard, GamepadInput.GamePad.Index.One);
		player1.playerIndex = 0;

		//var player2 = (Player)playerData.Spawn(new Vector3(2, 0, 0), Quaternion.identity);
		//InputManager.SetControllerData(1, ControlType.GamePadXBOX, GamepadInput.GamePad.Index.Two);
		//player2.playerIndex = 1;

		//var player3 = (Player)playerData.Spawn(new Vector3(4, 0, 0), Quaternion.identity);
		//InputManager.SetControllerData(2, ControlType.GamePadXBOX, GamepadInput.GamePad.Index.Three);
		//player3.playerIndex = 2;

		//var player4 = (Player)playerData.Spawn(new Vector3(6, 0, 0), Quaternion.identity);
		//InputManager.SetControllerData(3, ControlType.GamePadXBOX, GamepadInput.GamePad.Index.Four);
		//player4.playerIndex = 3;

	}

}
