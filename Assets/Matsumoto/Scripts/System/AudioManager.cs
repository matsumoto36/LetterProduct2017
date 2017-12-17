using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// BGMのリストネーム。
/// ここに名前を書くと読み込まれる。
/// 例 : Title -> Resources/Sounds/BGM/Title.mp3が読み込まれる
/// </summary>
public enum BGMType {
	Title,
}

/// <summary>
/// SEのリストネーム。
/// /// ここに名前を書くと読み込まれる。
/// 例 : Button -> Resources/Sounds/SE/Button.mp3が読み込まれる
/// </summary>
public enum SEType {
	Button,
}

/// <summary>
/// 音の管理をする
/// </summary>
public sealed class AudioManager : SingletonMonoBehaviour<AudioManager> {

	AudioMixerGroup[] mixerGroups = new AudioMixerGroup[2];	//ミキサーのグループ [0]SE [1]BGM
	AudioClip[] SEclips;									//再生用リスト
	AudioClip[] BGMclips;									//再生用リスト
	AudioSource nowPlayingBGM;								//現在再生されているBGM
	BGMType latestPlayBGMType = BGMType.Title;				//再生されているBGMの種類

	Coroutine fadeInCol;									//フェードインのコルーチン
	AudioSource fadeInAudio;

	/// <summary>
	/// 各音情報を読み込み
	/// </summary>
	public static void Load() {

		//LoadMixer
		var mixer = Resources.Load<AudioMixer>("Sounds/MainAudioMixer");
		instance.mixerGroups[0] = mixer.FindMatchingGroups("SE")[0];
		instance.mixerGroups[1] = mixer.FindMatchingGroups("BGM")[0];


		//BGM読み込み
		instance.BGMclips = new AudioClip[System.Enum.GetNames(typeof(BGMType)).Length];
		for (int i = 0; i < instance.BGMclips.Length; i++) {
			//enumで定義された名前と同じものを読み込む
			instance.BGMclips[i] = Resources.Load<AudioClip>("Sounds/BGM/" + System.Enum.GetName(typeof(BGMType), i));
		}

		//SE読み込み
		instance.SEclips = new AudioClip[System.Enum.GetNames(typeof(SEType)).Length];
		for (int i = 0; i < instance.SEclips.Length; i++) {
			//enumで定義された名前と同じものを読み込む
			instance.SEclips[i] = Resources.Load<AudioClip>("Sounds/SE/" + System.Enum.GetName(typeof(SEType), i));
		}

	}

	#region Play SE

	/// <summary>
	/// SEを再生する
	/// </summary>
	/// <param name="type">SEの内容</param>
	/// <returns>再生しているSE</returns>
	public static AudioSource Play(SEType type) {
		return Play(type, 1.0f);
	}

	/// <summary>
	/// SEを再生する
	/// </summary>
	/// <param name="type">SEの内容</param>
	/// <param name="vol">音量</param>
	/// <returns>再生しているSE</returns>
	public static AudioSource Play(SEType type, float vol) {
		return Play(type, vol, true);
	}

	/// <summary>
	/// SEを再生する
	/// </summary>
	/// <param name="type">SEの内容</param>
	/// <param name="vol">音量</param>
	/// <param name="autoDelete">再生終了時にSEを削除するか</param>
	/// <returns>再生しているSE</returns>
	public static AudioSource Play(SEType type, float vol, bool autoDelete) {

		AudioSource src = new GameObject("[Audio SE - " + type.ToString() + "]").AddComponent<AudioSource>();
		src.transform.SetParent(instance.transform);
		src.clip = instance.SEclips[(int)type];
		src.volume = vol;
		src.outputAudioMixerGroup = instance.mixerGroups[0];
		src.Play();

		if(autoDelete)
			Destroy(src.gameObject, instance.SEclips[(int)type].length + 0.1f);

		return src;

	}

	#endregion

	#region Play BGM

	/// <summary>
	/// BGMを再生する
	/// </summary>
	/// <param name="type">BGMの内容</param>
	/// <returns>再生しているSE</returns>
	public static AudioSource Play(BGMType type) {
		return Play(type, 1.0f);
	}

	/// <summary>
	/// BGMを再生する
	/// </summary>
	/// <param name="type">BGMの内容</param>
	/// <param name="vol">音量</param>
	/// <returns>再生しているSE</returns>
	public static AudioSource Play(BGMType type, float vol) {
		return Play(type, vol, true);
	}

	/// <summary>
	/// BGMを再生する
	/// </summary>
	/// <param name="type">BGMの内容</param>
	/// <param name="vol">音量</param>
	/// <param name="isLoop">ループ再生するか</param>
	/// <returns>再生しているSE</returns>
	public static AudioSource Play(BGMType type, float vol, bool isLoop) {

		if(instance.nowPlayingBGM) Destroy(instance.nowPlayingBGM.gameObject);

		AudioSource src = new GameObject("[Audio BGM - " + type.ToString() + "]").AddComponent<AudioSource>();
		src.transform.SetParent(instance.transform);
		src.clip = instance.BGMclips[(int)type];
		src.volume = vol;
		src.outputAudioMixerGroup = instance.mixerGroups[1];
		src.Play();

		if(isLoop) {
			src.loop = true;
		}
		else {
			Destroy(src.gameObject, instance.BGMclips[(int)type].length + 0.1f);
		}

		instance.nowPlayingBGM = src;
		instance.latestPlayBGMType = type;

		return src;
	}

	#endregion

	#region FadeIn

	/// <summary>
	/// BGMをフェードインさせる
	/// </summary>
	/// <param name="fadeTime">フェードする時間</param>
	/// <param name="type">新しいBGMのタイプ</param>
	/// <param name="vol">新しいBGMの大きさ</param>
	/// <param name="isLoop">新しいBGMがループするか</param>
	public static void FadeIn(float fadeTime, BGMType type) {
		FadeIn(fadeTime, type, 1.0f);
	}

	public static void FadeIn(float fadeTime, BGMType type, float vol) {
		FadeIn(fadeTime, type, vol, true);
	}

	public static void FadeIn(float fadeTime, BGMType type, float vol, bool isLoop) {
		instance.fadeInCol = instance.StartCoroutine(instance.FadeInAnim(fadeTime, type, vol, isLoop));
	}

	#endregion

	/// <summary>
	/// BGMをフェードアウトさせる
	/// </summary>
	/// <param name="fadeTime">フェードする時間</param>
	public static void FadeOut(float fadeTime) {
		instance.StartCoroutine(instance.FadeOutAnim(fadeTime));
	}

	IEnumerator FadeInAnim(float fadeTime, BGMType type, float vol, bool isLoop) {

		//初期設定
		fadeInAudio = new GameObject("[Audio BGM - " + type.ToString() + " - FadeIn ]").AddComponent<AudioSource>();
		fadeInAudio.transform.SetParent(instance.transform);
		fadeInAudio.clip = BGMclips[(int)type];
		fadeInAudio.volume = 0;
		fadeInAudio.outputAudioMixerGroup = mixerGroups[1];
		fadeInAudio.Play();

		//フェードイン
		float t = 0;
		while(t < 1.0f) {
			t += Time.deltaTime / fadeTime;
			fadeInAudio.volume = t * vol;
			yield return null;
		}

		fadeInAudio.volume = vol;
		fadeInAudio.name = "[Audio BGM - " + type.ToString() + "]";

		if(nowPlayingBGM) Destroy(nowPlayingBGM.gameObject);

		if(isLoop) {
			fadeInAudio.loop = true;
		}
		else {
			Destroy(fadeInAudio.gameObject, BGMclips[(int)type].length + 0.1f);
		}

		nowPlayingBGM = fadeInAudio;
	}

	IEnumerator FadeOutAnim(float fadeTime) {

		//初期設定
		AudioSource src = nowPlayingBGM;

		//フェードイン中にフェードアウトが呼ばれた場合
		if (!src) {
			//フェードイン処理停止
			instance.StopCoroutine(fadeInCol);
			src = fadeInAudio;
		}

		src.name = "[Audio BGM - " + latestPlayBGMType.ToString() + " - FadeOut ]";
		nowPlayingBGM = null;

		//フェードアウト
		float t = 0;
		float vol = src.volume;
		while(t < 1.0f) {
			t += Time.deltaTime / fadeTime;
			src.volume = (1 - t) * vol;
			yield return null;
		}

		Destroy(src.gameObject);
	}
}
