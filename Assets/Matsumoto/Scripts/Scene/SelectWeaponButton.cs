using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SelectWeaponButton : MonoBehaviour {

	public WeaponData viewData;

	public Text buttonName;
	public Image icon;

	public ControlButton button { get; set; }


	// Use this for initialization
	public void Init() {
		button = GetComponent<ControlButton>();

		buttonName.text = viewData.name;
		icon.sprite = viewData.icon;
	}
}
