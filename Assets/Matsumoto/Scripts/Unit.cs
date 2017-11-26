using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

/// <summary>
/// マップに存在するキャラクターの親クラス
/// (タレット等の構造物も含む)
/// </summary>
[RequireComponent(typeof(Rigidbody), typeof(Animator))]
public abstract class Unit : MonoBehaviour {

	const int CAN_EQUIPPED_WEAPON_COUNT = 2;
	const string HAND_ANCHOR = "[HandAnchor]";
	const string WEAPON_STATUS_MOD = "WEAPON_MOD";
	const string LEVELUP_STATUS_MOD = "LEVEL_UP";

	[SerializeField]
	int _level = 1;
	[SerializeField]
	int nextLevelEXP = 10;
	[SerializeField]
	int dropEXP;
	[SerializeField]
	int _maxHP;
	[SerializeField]
	int _nowHP;
	[SerializeField]
	float _moveSpeed;
	[SerializeField]
	float _rotSpeed;
	[SerializeField, Tooltip("パッシブ効果の合計値")]
	StatusModifier _statusMod = new StatusModifier();
	[SerializeField, Button("CalcStatus", "パッシブ効果を更新")]
	int dummy;

	public int level { get { return _level; } private set { _level = value; } }
	public int maxHP { get { return _maxHP; } private set { _maxHP = value; } }
	public int nowHP { get { return _nowHP; } protected set { _nowHP = value; } }
	public float moveSpeed { get { return _moveSpeed; } private set { _moveSpeed = value; } }
	public float rotSpeed { get { return _rotSpeed; } private set { _rotSpeed = value; } }
	public StatusModifier statusMod { get { return _statusMod; } private set { _statusMod = value; } }

	public Weapon[] equipWeapon { get; private set; }
	public int experience { get; private set; }
	public bool isPlayMeleeAnim { get; private set; }
	public bool isAttack { get; protected set; }
	public bool isDead { get; private set; }

	public bool canAttack { get; private set; }

	protected Transform body;
	protected Vector3 moveVec;
	protected Transform handAnchor;
	protected Rigidbody unitRig;

	//計算用
	int baseNextLevel;
	int baseHP;
	float baseMoveSpeed;
	float baseRotSpeed;
	float buffEXP = 0;

	List<DamageLog> attackedUnitList;
	Animator anim;
	int switchingWeaponNum = 0;
	StatusModifier levelUpStatus;
	Dictionary<string, StatusModifier> statusModStack;

	// Use this for initialization
	public virtual void Awake() {

		equipWeapon = new Weapon[CAN_EQUIPPED_WEAPON_COUNT];
		levelUpStatus = new StatusModifier();
		attackedUnitList = new List<DamageLog>();
		statusModStack = new Dictionary<string, StatusModifier>();
		canAttack = true;

		anim = GetComponent<Animator>();
		unitRig = GetComponent<Rigidbody>();

		//アンカーを取得
		body = transform.GetChild(0);
		foreach(Transform child in body.transform) {

			handAnchor = child.Find(HAND_ANCHOR);
			if(handAnchor) break;
		}

		//初期値の保存
		baseNextLevel = nextLevelEXP;
		baseHP = maxHP;
		baseMoveSpeed = moveSpeed;
		baseRotSpeed = rotSpeed;

		//レベルアップ用ステータスをパッシブ効果として実装
		ApplyModifier(levelUpStatus, LEVELUP_STATUS_MOD);

		//ステータスの計算
		CalcStatus();
		nowHP = maxHP;

		//***武器のロード処理は移行しました***
		WeaponDataContainer.Load();
	}

	/// <summary>
	/// パッシブ効果を適用
	/// </summary>
	/// <param name="mod"></param>
	/// <param name="key"></param>
	/// <returns>適用できたか</returns>
	public bool ApplyModifier(StatusModifier mod, string key) {

		if(statusModStack
			.Where((item) => item.Key == key)
			.Count() > 0)
			return false;

		statusModStack.Add(key, mod);
		//再計算
		CalcStatus();
		return true;
	}

	/// <summary>
	/// パッシブ効果を取り外す
	/// </summary>
	/// <param name="key"></param>
	/// <returns>取り外せたか</returns>
	public bool RemoveModifier(string key) {

		if(statusModStack
			.Where((item) => item.Key == key)
			.Count() == 0)
			return false;

		statusModStack.Remove(key);
		//再計算
		CalcStatus();
		return true;
	}

	/// <summary>
	/// ステータスを計算して適用する
	/// </summary>
	public void CalcStatus() {

		var tempHP = maxHP;

		statusMod = new StatusModifier() + statusModStack
			.Select((item) => item.Value)
			.Where((item) => item != null)
			.Aggregate((item1, item2) => item1 + item2);

		//ステータスの更新
		//HP上昇値の影響は受けるが、maxHPを超えず、nowHPを減らさないようにする
		maxHP = (int)(baseHP * statusMod.mulHP);
		nowHP = Mathf.Min(maxHP, Mathf.Max(nowHP, nowHP + maxHP - tempHP));

		moveSpeed = baseMoveSpeed * statusMod.mulMoveSpeed;
		rotSpeed = baseRotSpeed * statusMod.mulMoveSpeed;

		//武器ステータスの更新
		foreach(var weapon in equipWeapon) {
			if(weapon) weapon.UpdateStatus();
		}
	}

	/// <summary>
	/// 経験値を得る
	/// </summary>
	/// <param name="exp"></param>
	public void GainEXP(float exp) {

		//バッファに貯めた経験値を取り出す
		exp += buffEXP;

		//レベルアップする分だけ実行
		var isLevelUp = false;
		while(exp >= nextLevelEXP) {

			isLevelUp = true;

			//レベルアップ
			exp -= nextLevelEXP;
			level++;

			Debug.Log("Level UP! : " + level);

			//次のレベルに必要な経験値をセット
			nextLevelEXP = GameBalance.CalcNextLevelExp(baseNextLevel, level);
		}

		if(isLevelUp) {
			//ステータスの強化
			GameBalance.ApplyNextLevelStatus(levelUpStatus, level);
			CalcStatus();
		}

		//保存
		buffEXP = exp - (int)exp;
		nextLevelEXP -= (int)exp;
	}

	/// <summary>
	/// 移動 
	/// </summary>
	public abstract void Move();

	/// <summary>
	/// 攻撃
	/// </summary>
	public abstract void Attack();

	/// <summary>
	/// 死亡時に呼ばれる
	/// </summary>
	public virtual void Death() {

		Debug.Log("Death");

		nowHP = 0;
		isDead = true;

		//経験値分配
		var damageSum = attackedUnitList
			.Select((item) => item.damage)
			.Sum();

		foreach(var item in attackedUnitList) {
			if(!item.attackUnit) continue;
			item.attackUnit.GainEXP((item.damage / damageSum) * dropEXP);
			Debug.Log((item.damage / damageSum) * dropEXP);
		}
	}

	/// <summary>
	/// 武器を装備する
	/// </summary>
	/// <param name="weapon"></param>
	/// <param name="slot"></param>
	public void EquipWeapon(Weapon weapon, int slot) {

		//すでに装備してたら外す
		if(equipWeapon[slot]) {
			RemoveWeapon(slot);
		}

		//位置合わせ
		weapon.transform.parent = handAnchor;
		weapon.transform.localPosition = new Vector3();
		weapon.transform.localRotation = Quaternion.identity;

		//所持する
		equipWeapon[slot] = weapon;
		equipWeapon[slot].unitOwner = this;

		//パッシブ効果を登録
		ApplyModifier(equipWeapon[slot].weaponMod, WEAPON_STATUS_MOD + slot);

		//0番以外の武器は無効化
		if(slot != 0) equipWeapon[slot].gameObject.SetActive(false);
	}

	/// <summary>
	/// 武器を外す
	/// </summary>
	/// <param name="slot"></param>
	public void RemoveWeapon(int slot) {

		if(equipWeapon[slot]) {
			Destroy(equipWeapon[slot].gameObject);
		}

		//パッシブ効果を削除
		RemoveModifier(WEAPON_STATUS_MOD + slot);

		equipWeapon[slot] = null;
	}

	/// <summary>
	/// 所持している武器を手持ち( = 0番)と入れ替える
	/// </summary>
	/// <param name="animTriggerName"></param>
	/// <param name="weaponNum"></param>
	/// <param name="onComplete">完了時に実行</param>
	public void SwitchWeapon(string animTriggerName, int weaponNum, Action onComplete) {

		if(weaponNum == 0) return;
		if(!equipWeapon[0]) return;
		if(!equipWeapon[weaponNum]) return;
		if(isPlayMeleeAnim) return;

		//一定時間待つ
		StartCoroutine(SwitchWeaponAnim(animTriggerName, weaponNum, onComplete));

	}

	/// <summary>
	/// 近接攻撃用のアニメーションを再生
	/// </summary>
	/// <param name="triggerName"></param>
	/// <param name="speed"></param>
	/// <param name="onComplete">完了時に実行</param>
	public void PlayMeleeAnimation(string triggerName, float speed, Action onComplete) {
		StartCoroutine(PlayMeleeAnimWait(triggerName, speed, onComplete));
	}

	/// <summary>
	/// 攻撃する
	/// </summary>
	/// <param name="from">攻撃するキャラクター</param>
	/// <param name="to">攻撃を受けるキャラクター</param>
	/// <param name="damage"></param>
	/// <returns>成功したかどうか</returns>
	public static bool Attack(Unit from, Unit to, int damage) {

		if(!from || !to) return false;
		if(from.isDead || to.isDead) return false;

		Debug.Log("Attack " + from.name + " -> " + to.name);

		//経験値分配用
		bool findFromUnit = to.attackedUnitList
			.Where((item) => item.attackUnit == from)
			.Count() > 0;

		if(findFromUnit) {
			to.attackedUnitList
			.Where((item) => item.attackUnit == from)
			.Select((item) => item.damage += damage);
		}
		else {
			Debug.Log("New Unit");
			to.attackedUnitList.Add(new DamageLog(from, damage));
		}

		//ダメージを与える
		to.ApplyDamage(damage);

		return true;
	}
	/// <summary>
	/// 回復する
	/// </summary>
	/// <returns></returns>
	public static bool Heal(Unit from, Unit to, int heal) {
		if(!from || !to) return false;
		Debug.Log("Heal " + from.name + " -> " + to.name);

		int healPoint = Mathf.Min(heal, to.maxHP - to.nowHP);
		//回復した分だけ経験値を得る
		from.GainEXP(GameBalance.CalcHealExp(healPoint));
		//回復する
		to.ApplyDamage(-healPoint);

		return true;
	}

	/// <summary>
	/// ダメージを与える
	/// </summary>
	/// <param name="from">攻撃するキャラクター</param>
	/// <param name="damage"></param>
	protected virtual void ApplyDamage(int damage) {
		nowHP -= damage;
		if(nowHP <= 0) Death();
	}

	/// <summary>
	/// 武器が入れ替わる瞬間
	/// </summary>
	void OnSwitchWeaponModel() {

		//0番同士の交換はありえないので実行しない
		if(switchingWeaponNum == 0) return;

		//モデルの表示・非表示を切り替えるのみ
		equipWeapon[0].gameObject.SetActive(false);
		equipWeapon[switchingWeaponNum].gameObject.SetActive(true);
	}

	/// <summary>
	/// 近接攻撃の判定が始まったとき
	/// </summary>
	void OnMeleeAttackStart() {

		if(!equipWeapon[0]) return;

		var weaponMelee = equipWeapon[0].GetComponent<WeaponMelee>();
		if(!weaponMelee) return;
		weaponMelee.SetCollider(true);
	}

	/// <summary>
	/// 近接攻撃の判定が終わったとき
	/// </summary>
	void OnMeleeAttackEnd() {

		if(!equipWeapon[0]) return;

		var weaponMelee = equipWeapon[0].GetComponent<WeaponMelee>();
		if(!weaponMelee) return;
		weaponMelee.SetCollider(false);
	}

	IEnumerator PlayMeleeAnimWait(string clipName, float speed, Action onComplete) {

		Debug.Log("MeleeAnim");
		isPlayMeleeAnim = true;
		yield return StartCoroutine(PlayAnimation(0, clipName, speed));
		yield return new WaitForSeconds(0.2f);
		isPlayMeleeAnim = false;

		//Idleに戻す
		StartCoroutine(PlayAnimation(0, "Idle", 1));

		//完了時に実行
		onComplete();
	}

	IEnumerator SwitchWeaponAnim(string clipName, int weaponNum, Action onComplete) {

		//攻撃キャンセル
		isAttack = false;
		switchingWeaponNum = weaponNum;

		//交換中は攻撃できません
		canAttack = false;
		yield return StartCoroutine(PlayAnimation(0, clipName, 1));
		canAttack = true;

		//内部的に交換
		var w = equipWeapon[0];
		equipWeapon[0] = equipWeapon[weaponNum];
		equipWeapon[weaponNum] = w;

		//Idleに戻す
		StartCoroutine(PlayAnimation(0, "Idle", 1));

		//完了後に実行
		onComplete();
	}

	IEnumerator PlayAnimation(int layer, string clipName, float speed) {

		var clip = anim.runtimeAnimatorController
			.animationClips
			.Where((item) => item.name == clipName)
			.ToArray()[0];

		if(!clip) yield break;

		anim.CrossFade(clipName, 0, layer, 0);
		anim.Update(0);

		float t = 0;
		float duration = clip.length / speed;
		while(true) {

			anim.speed = speed;

			if((t += Time.deltaTime) >= duration) break;
			yield return null;
		}
		anim.speed = 1;
	}
}
