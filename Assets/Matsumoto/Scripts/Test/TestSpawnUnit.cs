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
		player1.inputType = ControlType.Keyboard;
		player1.playerIndex = GamepadInput.GamePad.Index.One;

		//var player2 = (Player)playerData.Spawn(new Vector3(2, 0, 0), Quaternion.identity);
		//player2.inputType = ControlType.GamePadXBOX;
		//player2.playerIndex = GamepadInput.GamePad.Index.One;

		//var player3 = (Player)playerData.Spawn(new Vector3(4, 0, 0), Quaternion.identity);
		//player3.inputType = ControlType.GamePadXBOX;
		//player3.playerIndex = GamepadInput.GamePad.Index.Two;

		//var player4 = (Player)playerData.Spawn(new Vector3(6, 0, 0), Quaternion.identity);
		//player4.inputType = ControlType.GamePadXBOX;
		//player4.playerIndex = GamepadInput.GamePad.Index.Three;

		//var player5 = (Player)playerData.Spawn(new Vector3(8, 0, 0), Quaternion.identity);
		//player5.inputType = ControlType.GamePadXBOX;
		//player5.playerIndex = GamepadInput.GamePad.Index.Four;
	}

}
