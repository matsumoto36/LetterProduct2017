using UnityEngine;
using System.Collections;

public class TestSpawnUnit : MonoBehaviour {

	// Use this for initialization
	void Start() {

		//Load処理はGameManagerに移行してください
		WeaponDataContainer.Load();
		UnitDataContainer.Load();

		var player1 = (Player)UnitManager.SpawnUnit(0, new Vector3(), Quaternion.identity, new int[2] { 0, 3 });
		player1.inputType = ControlType.Keyboard;
		player1.playerIndex = GamepadInput.GamePad.Index.One;

		var player2 = (Player)UnitManager.SpawnUnit(0, new Vector3(2, 0, 0), Quaternion.identity);
		player2.inputType = ControlType.Keyboard;
		player2.playerIndex = GamepadInput.GamePad.Index.Two;

		UnitManager.SpawnUnit(1, new Vector3(4, 0, 0), Quaternion.identity);
	}

}
