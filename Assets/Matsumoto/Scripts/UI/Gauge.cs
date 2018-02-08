using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Gauge : MonoBehaviour {

	public Image fillImage;
	public new RectTransform transform;

	/// <summary>
	/// ゲージの比率をセットする
	/// </summary>
	/// <param name="ratio"></param>
	public void SetRatio(float ratio) {
		fillImage.fillAmount = Mathf.Clamp(ratio, 0, 1);
	}

	/// <summary>
	/// 移動する
	/// </summary>
	/// <param name="position"></param>
	public void SetPosition(Vector3 position) {
		transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, position);
	}
}
