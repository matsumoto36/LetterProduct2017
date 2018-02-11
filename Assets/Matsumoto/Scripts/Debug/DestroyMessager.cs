using UnityEngine;
using System.Collections;

public class DestroyMessager : MonoBehaviour {

	void OnDestroy() {
		Debug.Log(name + " is Destroy");
	}
}
