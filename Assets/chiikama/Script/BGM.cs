using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BGM : MonoBehaviour {

	[SerializeField]
	UnityEngine.Audio.AudioMixer mixer;

	// Use this for initialization
	void Start ()
	  {
		float bgmVolume;
		mixer.GetFloat("BGM", out bgmVolume);
		GetComponent<Slider>().value = bgmVolume;
		}

	public float BGMvolume
	{
		set
		{
			mixer.SetFloat("BGM", value);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
