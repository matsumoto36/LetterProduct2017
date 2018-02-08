using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using GamepadInput;

public class BGM : MonoBehaviour {

	const float VOLUME_CHANGE_VALUE = 10;
	const float VOLUME_CHANGE_WAIT = 0.2f;

	public ControlButton button;
	AudioMixer mixer;
	Slider slider;
	bool isFocus;
	bool canChangeVolume = true;

	// Use this for initialization
	public void Init()
	  {
		button = GetComponent<ControlButton>();
		slider = GetComponent<Slider>();
		mixer = AudioManager.instance.mixer;

		float bgmVolume;
		mixer.GetFloat("BGMvol", out bgmVolume);
		slider.value = bgmVolume;


		button.onFocus += () => {
			isFocus = true;
			AudioManager.PlaySE("button");
		};

		button.onFocusChanged += () => {
			isFocus = false;
		};
	}

	public float BGMvolume
	{
		set
		{
			slider.value = value;
			mixer.SetFloat("BGMvol", value);
		}
	}

	// Update is called once per frame
	void Update() {

		if(!isFocus) return;
		if(!canChangeVolume) return;

		float volume;
		if(!mixer.GetFloat("BGMvol", out volume)) return;

		var axis = InputManager.GetAxisAny(GamePad.Axis.Dpad, true);
		if(axis == new Vector2()) axis = InputManager.GetAxisAny(GamePad.Axis.LeftStick, true);
		if(axis == new Vector2()) axis = InputManager.GetAxisAny(GamePad.Axis.RightStick, true);

		if(axis != new Vector2()) {

			Debug.Log(axis);

			//[0] 左  [1] 下  [2] 右  [3] 上
			var angle = ControlButtonController.CalcMoveAngle(axis);

			if(angle == 0) {
				BGMvolume = Mathf.Clamp(volume - VOLUME_CHANGE_VALUE, -70, 10);
				AudioManager.PlaySE("Button_select");
			}
			if(angle == 2) {
				BGMvolume = Mathf.Clamp(volume + VOLUME_CHANGE_VALUE, -70, 10);
				AudioManager.PlaySE("Button_select");
			}

			canChangeVolume = false;
			StartCoroutine(WaitVolumeChangeTime());
		}
	}

	IEnumerator WaitVolumeChangeTime() {
		yield return new WaitForSeconds(VOLUME_CHANGE_WAIT);
		canChangeVolume = true;
	}
}
