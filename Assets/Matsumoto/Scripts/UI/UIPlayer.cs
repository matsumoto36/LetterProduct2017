using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayer : MonoBehaviour {

	const float FADE_SPEED = 2;

	[SerializeField] Text levelText;

	[SerializeField] Image hpBar;
	[SerializeField] Image expBar;

	[SerializeField] Image inventoryMain;
	[SerializeField] Image inventorySub;

	[SerializeField] Image warningImage;

	[SerializeField] RectTransform[] anchors;

	CanvasGroup rendererGroup;
	Coroutine warningAnim = null;

	Rect playerUIArea;

	public float Alpha { get; set; }

	/// <summary>
	/// 点がUIに重なっているか
	/// </summary>
	/// <param name="screenPos"></param>
	/// <returns></returns>
	public bool CheckInsidePosition(Vector2 screenPos) {
		return playerUIArea.Contains(screenPos);
	}

	public void Init() {

		var pos1 = anchors[0].position;
		var pos2 = anchors[1].position;
		var diff = pos1 - pos2;
		var position = new Vector2(Mathf.Min(pos1.x, pos2.x), Mathf.Min(pos1.y, pos2.y));
		playerUIArea = new Rect(position, new Vector2(Mathf.Abs(diff.x), Mathf.Abs(diff.y)));

		rendererGroup = GetComponent<CanvasGroup>();
	}

	public void SetLevel(int level) {
		levelText.text = level.ToString();
	}

	public void SetHPRatio(float hpRatio) {
		hpBar.fillAmount = hpRatio;
	}

	public void SetEXPRatio(float expRatio) {
		expBar.fillAmount = expRatio;
	}

	public void SetWeaponIcon(Sprite main, Sprite sub) {
		inventoryMain.sprite = main;
		inventorySub.sprite = sub;
	}

	public void SwitchWarningAnim(bool enable) {

		if(enable == (warningAnim != null)) return;

		if(enable) {
			warningAnim = StartCoroutine(WarningAnim());
		}
		else {
			StopCoroutine(warningAnim);
			warningAnim = null;
			warningImage.color = new Color(1, 1, 1, 0);
		}
	}

	void Update() {

		rendererGroup.alpha = Mathf.MoveTowards(rendererGroup.alpha, Alpha, FADE_SPEED * Time.deltaTime);

	}

	IEnumerator WarningAnim() {

		var t = 0.0f;
		while(true) {

			t += Time.deltaTime;
			var col = warningImage.color;
			col.a = Mathf.Abs(Mathf.Sin(t * 8));
			warningImage.color = col;

			yield return null;
		}

	}
}
