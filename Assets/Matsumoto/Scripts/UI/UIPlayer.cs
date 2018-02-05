using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayer : MonoBehaviour {

	[SerializeField] Text levelText;

	[SerializeField] Image hpBar;
	[SerializeField] Image expBar;

	[SerializeField] Image inventoryMain;
	[SerializeField] Image inventorySub;

	[SerializeField] Image warningImage;

	Coroutine warningAnim = null;

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
