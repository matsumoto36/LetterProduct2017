using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SE : MonoBehaviour {
	[SerializeField]
	UnityEngine.Audio.AudioMixer mixer;

	// Use this for initialization
	void Start () {
		float seVolume;
		mixer.GetFloat("SE", out seVolume);
		GetComponent<Slider>().value = seVolume;
		
	}

	public float BGMvolume
	{
		set
		{
			mixer.SetFloat("SE", value);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
