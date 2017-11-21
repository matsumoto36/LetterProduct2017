using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour {
	[SerializeField]
	private Text countDown;
	public float Cnt = 0.75f;


	// Use this for initialization

	void Start ()
	{
		countDown.text = "";
		StartCoroutine(CountdownCoroutine());
	}

	IEnumerator CountdownCoroutine()
	{
		//入力がonになったら終了 
		while (!Input.GetKey(KeyCode.Space))
		{
			yield return null;//１フレーム待つ
		}

		countDown.gameObject.SetActive(true);
		//3
		countDown.text = "3";
		yield return new WaitForSeconds(Cnt);
		//2
		countDown.text = "2";
		yield return new WaitForSeconds(Cnt);
		//1
		countDown.text = "1";
		yield return new WaitForSeconds(Cnt);
		//Start
		countDown.text = "Start";
		yield return new WaitForSeconds(Cnt);

		countDown.text = "";
		countDown.gameObject.SetActive(false);

	}

}
