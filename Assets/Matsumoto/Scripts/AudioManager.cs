using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// BGMのリストネーム。
/// ここに名前を書くと読み込まれる。
/// 例 : Title -> Resources/Sounds/BGM/Title.mp3が読み込まれる
/// </summary>
public enum BGMType {
	Game,
	Menu,
	Result,//
	Title,
}

/// <summary>
/// SEのリストネーム。
/// /// ここに名前を書くと読み込まれる。
/// 例 : Button -> Resources/Sounds/SE/Button.mp3が読み込まれる
/// </summary>
public enum SEType {

	//アイテム
	Poison,
	Heal,
	Jump,

	//ゲームフロー
	Start,
	Countdown,//
	Pause_Game_Back,//
	Game_Over,
	Clear_Goal,

	//ボタン
	Menu_Back_Button,//メニューに行くとき？
	Button,//

	//ギミック
	Hole_in_Drop,
	Worm,

	//死亡
	Bomb_Big,
	Bomb_Small,//
	Drop_Out,//
}

/// <summary>
/// 音の管理をする
/// </summary>
public class AudioManager : MonoBehaviour {

	static AudioManager myManager;							//自分のリファレンス

	AudioMixerGroup[] mixerGroups = new AudioMixerGroup[2];	//ミキサーのグループ [0]SE [1]BGM
	AudioClip[] SEclips;									//再生用リスト
	AudioClip[] BGMclips;									//再生用リスト
	AudioSource nowPlayingBGM;								//現在再生されているBGM
	BGMType latestPlayBGMType = BGMType.Title;				//再生されているBGMの種類

	Coroutine fadeInCol;									//フェードインのコルーチン
	AudioSource fadeInAudio;

	/// <summary>
	/// 一回生成
	/// </summary>
	static AudioManager() {
		new GameObject("[AudioManager]").AddComponent<AudioManager>();
	}

	void Awake() {

		//シングルトン化
		if(!myManager) {
			myManager = this;
			DontDestroyOnLoad(gameObject);

			Initiarize();
		}
		else {
			Destroy(gameObject);
		}

	}

	void Initiarize() {

		//LoadMixer
		var mixer = Resources.Load<AudioMixer>("Sounds/MainAudioMixer");
		mixerGroups[0] = mixer.FindMatchingGroups("SE")[0];
		mixerGroups[1] = mixer.FindMatchingGroups("BGM")[0];


        //BGM読み込み
		BGMclips = new AudioClip[System.Enum.GetNames(typeof(BGMType)).Length];
		for (int i = 0; i < BGMclips.Length; i++) {
			//enumで定義された名前と同じものを読み込む
			BGMclips[i] = Resources.Load<AudioClip>("Sounds/BGM/" + System.Enum.GetName(typeof(BGMType), i));
		}

        //SE読み込み
        SEclips = new AudioClip[System.Enum.GetNames(typeof(SEType)).Length];
		for (int i = 0; i < SEclips.Length; i++) {
			//enumで定義された名前と同じものを読み込む
			SEclips[i] = Resources.Load<AudioClip>("Sounds/SE/" + System.Enum.GetName(typeof(SEType), i));
		}

	}

	AudioSource _Play(SEType type, float vol, bool autoDelete) {

		AudioSource src = new GameObject("[Audio SE - " + type.ToString() +  "]").AddComponent<AudioSource>();
		src.transform.SetParent(myManager.transform);
		src.clip = SEclips[(int)type];
		src.volume = vol;
		src.outputAudioMixerGroup = mixerGroups[0];
		src.Play();

		if(autoDelete)
			Destroy(src.gameObject, SEclips[(int)type].length + 0.1f);

		return src;
	}

	AudioSource _Play(BGMType type, float vol, bool isLoop) {

		if(nowPlayingBGM) Destroy(nowPlayingBGM.gameObject);

		AudioSource src = new GameObject("[Audio BGM - " + type.ToString() + "]").AddComponent<AudioSource>();
		src.transform.SetParent(myManager.transform);
		src.clip = BGMclips[(int)type];
		src.volume = vol;
		src.outputAudioMixerGroup = mixerGroups[1];
		src.Play();

		if(isLoop) {
			src.loop = true;
		}
		else {
			Destroy(src.gameObject, BGMclips[(int)type].length + 0.1f);
		}

		nowPlayingBGM = src;
		latestPlayBGMType = type;

		return src;
	}

	void _FadeIn(float fadeTime, BGMType type, float vol, bool isLoop) {
		fadeInCol = myManager.StartCoroutine(FadeInAnim(fadeTime, type, vol, isLoop));
	}

	void _FadeOut(float fadeTime) {
		myManager.StartCoroutine(FadeOutAnim(fadeTime));
	}

	#region Play SE

	/// <summary>
	/// SEを再生する
	/// </summary>
	/// <param name="type">SEの内容</param>
	/// <param name="vol">音量</param>
	public static AudioSource Play(SEType type) {
		return myManager._Play(type, 1.0f, true);
	}

	public static AudioSource Play(SEType type, float vol) {
		return myManager._Play(type, vol, true);
	}

	public static AudioSource Play(SEType type, float vol, bool autoDelete) {
		return myManager._Play(type, vol, autoDelete);

	}

	#endregion

	#region Play BGM

	/// <summary>
	/// BGMを再生する
	/// </summary>
	/// <param name="type">BGMの内容</param>
	/// <param name="vol">音量</param>
	/// <param name="isLoop">ループ再生するか</param>
	public static AudioSource Play(BGMType type) {
		return myManager._Play(type, 1.0f, true);
	}

	public static AudioSource Play(BGMType type, float vol) {
		return myManager._Play(type, vol, true);
	}

	public static AudioSource Play(BGMType type, float vol, bool isLoop) {
		return myManager._Play(type, vol, isLoop);
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
		myManager._FadeIn(fadeTime, type, 1.0f, true);
	}

	public static void FadeIn(float fadeTime, BGMType type, float vol) {
		myManager._FadeIn(fadeTime, type, vol, true);
	}

	public static void FadeIn(float fadeTime, BGMType type, float vol, bool isLoop) {
		myManager._FadeIn(fadeTime, type, vol, isLoop);
	}

	#endregion


	/// <summary>
	/// BGMをフェードアウトさせる
	/// </summary>
	/// <param name="fadeTime">フェードする時間</param>
	public static void FadeOut(float fadeTime) {
		myManager._FadeOut(fadeTime);
	}

	IEnumerator FadeInAnim(float fadeTime, BGMType type, float vol, bool isLoop) {

		//初期設定
		fadeInAudio = new GameObject("[Audio BGM - " + type.ToString() + " - FadeIn ]").AddComponent<AudioSource>();
		fadeInAudio.transform.SetParent(myManager.transform);
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
			myManager.StopCoroutine(fadeInCol);
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
