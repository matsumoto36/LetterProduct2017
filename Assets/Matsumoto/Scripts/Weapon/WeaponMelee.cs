using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// 近接武器
/// </summary>
[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class WeaponMelee : Weapon {

	const string EFFECT_POS1 = "[EffectPos1]";
	const string EFFECT_POS2 = "[EffectPos2]";

	public Transform effectPos1 { get; private set; }
	public Transform effectPos2 { get; private set; }
	public string attackParticleName { get; set; }
	public float motionSpeed { get; set; }

	bool isInit;
	string motionName;
	Collider meleeCol;
	PKFxFX effect;
	PKFxManager.Attribute pos1;
	PKFxManager.Attribute pos2;
	float effectSize;

	public override void Init() {
		base.Init();

		//エフェクト発生地点を取得
		effectPos1 = GetComponentsInChildren<Transform>()
			.Where(item => item.name == EFFECT_POS1)
			.ToArray()[0];
		effectPos2 = GetComponentsInChildren<Transform>()
			.Where(item => item.name == EFFECT_POS2)
			.ToArray()[0];

		//エフェクトを設置
		effect = ParticleManager.Spawn(attackParticleName, new Vector3(), Quaternion.identity, 0);
		var model = transform.GetChild(0);
		effect.transform.localPosition = new Vector3();
		effect.transform.localRotation = Quaternion.identity;

		pos1 = effect.GetAttribute("Pos1");
		pos2 = effect.GetAttribute("Pos2");

		effectSize = effect.GetAttribute("SizeMax").ValueFloat;
		SetEffectEnable(false);

		weaponType = WeaponType.Melee;
		meleeCol = GetComponent<Collider>();
		//あらかじめ当たり判定を無効にしておく
		meleeCol.enabled = false;

	}

	void Update() {

		if(!isInit) return;
		if(!(effectSize <= 0)) return;

		pos1.ValueFloat3 = effectPos1.position;
		pos2.ValueFloat3 = effectPos2.position;
	}

	public override void Attack() {

		if(!isInit) {
			isInit = true;
			//相手の勢力に当たるように設定
			var maskList = UnitGroupMatrix.GetAttackableGroup(unitOwner.group, true)
				.Select((item) => item.ToString() + "Layer")
				.ToList();

			maskList.Add("IgnoreHit");

			SetHitMask(maskList.ToArray());
		}

		//親クラスで攻撃ができるかの判断を行っている
		if(!canAction) return;

		//アニメーションを再生
		unitOwner.anim.SetTrigger("MeleeAttack");

		//intervalの設定(最後に呼ぶこと)
		base.Attack();
	}

	/// <summary>
	/// 必要なデータをセットする
	/// </summary>
	/// <param name="interval"></param>
	/// <param name="motionNum"></param>
	/// <param name="motionSpeed"></param>
	public void SetData(float interval, int power, string particleName, string motionName, float motionSpeed) {
		baseInterval = interval;
		basePower = power;
		attackParticleName = particleName;
		this.motionName = motionName;
		this.motionSpeed = motionSpeed;
	}

	/// <summary>
	/// 当たり判定を有効・無効にする
	/// </summary>
	/// <param name="enabled"></param>
	public void SetCollider(bool enabled) {
		meleeCol.enabled = enabled;
	}

	public void SetEffectEnable(bool enable) {
		effect.GetAttribute("SizeMax").ValueFloat = enable ? effectSize : 0;
	}

	/// <summary>
	/// 何かに当たった時の共通処理
	/// </summary>
	/// <param name="other"></param>
	void HitAny(Collider other) {

		var hitPos = other.ClosestPointOnBounds(transform.position);

		//エフェクトの再生
		ParticleManager.Spawn("MeleeHit", hitPos, Quaternion.identity);
		//SEの再生
		var se = AudioManager.PlaySE(hitSound);
		se.transform.position = hitPos;

	}

	void OnTriggerEnter(Collider other) {

		Unit unit;
		if(unit = other.GetComponent<Unit>()) {
			//攻撃
			if(CheckHit(other.gameObject)) {
				Unit.Attack(unitOwner, unit, power);

				HitAny(other);
			}
		}
		else {
			HitAny(other);
		}
	}
}
