using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Audio;
using System;

/// <summary>
/// 音の管理をする
/// </summary>
public sealed class AudioManager : SingletonMonoBehaviour<AudioManager> {

	const string MIXER_PATH = "Sounds/MainAudioMixer";		//ミキサーのパス
	const string BGM_PATH = "Sounds/BGM/";					//BGMのフォルダーパス
	const string SE_PATH = "Sounds/SE/";                    //SEのフォルダーパス

	const int MAX_PLAYING_SE_COUNT = 10;					//同時再生できるSEの数

	public AudioMixer mixer { get; private set; }			//ミキサー

	AudioMixerGroup[] mixerGroups = new AudioMixerGroup[2];	//ミキサーのグループ [0]SE [1]BGM

	Dictionary<string, AudioClipInfo> SEclips;				//SE再生用リスト
	Dictionary<string, AudioClip> BGMclips;					//BGM再生用リスト

	AudioSource nowPlayingBGM;								//現在再生されているBGM
	string latestPlayBGM = "";								//再生されているBGMの種類

	Coroutine fadeInCol;									//BGMフェードインのコルーチン
	AudioSource fadeInAudio;								//BGMフェードイン用のAudioSource

	protected override void Init() {
		base.Init();

		Load();
	}

	/// <summary>
	/// 各音情報を読み込み
	/// </summary>
	public static void Load() {

		//LoadMixer
		instance.mixer = Resources.Load<AudioMixer>(MIXER_PATH);
		if(instance.mixer) {
			instance.mixerGroups[0] = instance.mixer.FindMatchingGroups("SE")[0];
			instance.mixerGroups[1] = instance.mixer.FindMatchingGroups("BGM")[0];
		}
		else {
			Debug.LogError("Failed Load AudioMixer! Path=" + MIXER_PATH);
		}


		//BGM読み込み
		instance.BGMclips = new Dictionary<string, AudioClip>();
		foreach(var item in Resources.LoadAll<AudioClip>(BGM_PATH)) {
			instance.BGMclips.Add(item.name, item);
		}

		//SE読み込み
		instance.SEclips = new Dictionary<string, AudioClipInfo>();
		foreach(var item in Resources.LoadAll<AudioClip>(SE_PATH)) {
			instance.SEclips.Add(item.name, new AudioClipInfo(item));
		}

	}

	/// <summary>
	/// SEを再生する
	/// </summary>
	/// <param name="type">SEの名前</param>
	/// <param name="vol">音量</param>
	/// <param name="autoDelete">再生終了時にSEを削除するか</param>
	/// <returns>再生しているSE</returns>
	public static AudioSource PlaySE(string SEName, float vol = 1.0f, bool autoDelete = true) {

		//SE取得
		var info = GetSEInfo(SEName);
		if(info == null) return null;

		if(info.stockList.Count > 0) {
			//stockListから空で且つ番号が一番若いSEInfoを受け取る
			var seInfo = info.stockList.Values[0];

			//ストックを削除
			info.stockList.Remove(seInfo.index);

			//情報を取り付ける
			var src = new GameObject("[Audio SE - " + SEName + "]").AddComponent<AudioSource>();
			src.transform.SetParent(instance.transform);
			src.clip = info.clip;
			src.volume = seInfo.volume * vol;
			src.outputAudioMixerGroup = instance.mixerGroups[0];
			src.Play();

			//管理用情報を付加
			var playSE = src.gameObject.AddComponent<PlayingSE>();
			playSE.onDestroy += () => { info.stockList.Add(seInfo.index, seInfo); };

			//自動削除の場合は遅延で削除を実行する
			if(autoDelete)
				Destroy(src.gameObject, src.clip.length + 0.1f);

			return src;
		}

		return null;
	}

	/// <summary>
	/// BGMを再生する
	/// </summary>
	/// <param name="BGMName">BGMの名前</param>
	/// <param name="vol">音量</param>
	/// <param name="isLoop">ループ再生するか</param>
	/// <returns>再生しているSE</returns>
	public static AudioSource PlayBGM(string BGMName, float vol = 1.0f, bool isLoop = true) {

		//BGM取得
		var clip = GetBGM(BGMName);
		if(!clip) return null;
		if(instance.nowPlayingBGM) Destroy(instance.nowPlayingBGM.gameObject);

		var src = new GameObject("[Audio BGM - " + BGMName + "]").AddComponent<AudioSource>();
		src.transform.SetParent(instance.transform);
		src.clip = clip;
		src.volume = vol;
		src.outputAudioMixerGroup = instance.mixerGroups[1];
		src.Play();

		if(isLoop) {
			src.loop = true;
		}
		else {
			Destroy(src.gameObject, clip.length + 0.1f);
		}

		instance.nowPlayingBGM = src;
		instance.latestPlayBGM = BGMName;

		return src;
	}

	/// <summary>
	/// BGMをフェードインさせる
	/// </summary>
	/// <param name="fadeTime">フェードする時間</param>
	/// <param name="type">新しいBGMのタイプ</param>
	/// <param name="vol">新しいBGMの大きさ</param>
	/// <param name="isLoop">新しいBGMがループするか</param>
	public static void FadeIn(float fadeTime, string BGMName, float vol = 1.0f, bool isLoop = true) {
		instance.fadeInCol = instance.StartCoroutine(instance.FadeInAnim(fadeTime, BGMName, vol, isLoop));
	}

	/// <summary>
	/// BGMをフェードアウトさせる
	/// </summary>
	/// <param name="fadeTime">フェードする時間</param>
	public static void FadeOut(float fadeTime) {
		instance.StartCoroutine(instance.FadeOutAnim(fadeTime));
	}

	/// <summary>
	/// BGMをクロスフェードする
	/// </summary>
	/// <param name="fadeTime">フェードする時間</param>
	/// <param name="type">新しいBGMのタイプ</param>
	/// <param name="vol">新しいBGMの大きさ</param>
	/// <param name="isLoop">新しいBGMがループするか</param>
	public static void CrossFade(float fadeTime, string fadeInBGMName, float vol = 1.0f, bool isLoop = true) {
		instance.StartCoroutine(instance.FadeOutAnim(fadeTime));
		instance.fadeInCol = instance.StartCoroutine(instance.FadeInAnim(fadeTime, fadeInBGMName, vol, isLoop));
	}

	/// <summary>
	/// SEを取得する
	/// </summary>
	/// <param name="SEName">SEの名前</param>
	/// <returns>SE</returns>
	static AudioClipInfo GetSEInfo(string SEName) {

		if(!instance.SEclips.ContainsKey(SEName)) {
			Debug.LogWarning("SEName:" + SEName + " is not found.");
			return null;
		}
		return instance.SEclips[SEName];
	}

	/// <summary>
	/// BGMを取得する
	/// </summary>
	/// <param name="BGMName">BGMの名前</param>
	/// <returns>BGM</returns>
	static AudioClip GetBGM(string BGMName) {

		if(!instance.BGMclips.ContainsKey(BGMName)) {
			Debug.LogError("BGMName:" + BGMName + " is not found.");
			return null;
		}
		return instance.BGMclips[BGMName];
	}

	IEnumerator FadeInAnim(float fadeTime, string BGMName, float vol, bool isLoop) {

		//BGM取得
		var clip = GetBGM(BGMName);
		if(!clip) yield break;

		//初期設定
		fadeInAudio = new GameObject("[Audio BGM - " + BGMName + " - FadeIn ]").AddComponent<AudioSource>();
		fadeInAudio.transform.SetParent(instance.transform);
		fadeInAudio.clip = clip;
		fadeInAudio.volume = 0;
		fadeInAudio.outputAudioMixerGroup = mixerGroups[1];
		fadeInAudio.Play();

		//フェードイン
		var t = 0.0f;
		while((t += Time.deltaTime / fadeTime) < 1.0f) {
			fadeInAudio.volume = t * vol;
			yield return null;
		}

		fadeInAudio.volume = vol;
		fadeInAudio.name = "[Audio BGM - " + BGMName + "]";

		if(nowPlayingBGM) Destroy(nowPlayingBGM.gameObject);

		if(isLoop) {
			fadeInAudio.loop = true;
		}
		else {
			Destroy(fadeInAudio.gameObject, clip.length + 0.1f);
		}

		nowPlayingBGM = fadeInAudio;
	}

	IEnumerator FadeOutAnim(float fadeTime) {

		var src = nowPlayingBGM;

		//フェードイン中にフェードアウトが呼ばれた場合
		if (!src) {
			//フェードイン処理停止
			instance.StopCoroutine(fadeInCol);
			src = fadeInAudio;

			if(!src) yield break;
		}

		src.name = "[Audio BGM - " + latestPlayBGM + " - FadeOut ]";
		nowPlayingBGM = null;

		//フェードアウト
		var t = 0.0f;
		float vol = src.volume;
		while((t += Time.deltaTime / fadeTime) < 1.0f) {
			src.volume = (1 - t) * vol;
			yield return null;
		}

		Destroy(src.gameObject);
	}
}
