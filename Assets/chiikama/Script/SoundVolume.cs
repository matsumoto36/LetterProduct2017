using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundVolume : MonoBehaviour {

	public AudioMixer mixer;
	// Use this for initialization

	public void SetMaster(float masterVolume)
	{
		mixer.SetFloat("MASvol", masterVolume);
	}
	
	public void SetBGM(float BGMvolume)
	{
		mixer.SetFloat("BGMvol", BGMvolume);
	}
	public void SetSE(float SEvolume)
	{
		mixer.SetFloat("SEvol", SEvolume);
	}
	// Update is called once per frame
	void Update () {
		
	}
}
