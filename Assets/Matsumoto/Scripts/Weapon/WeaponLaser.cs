﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

enum LaserState {
	Idle,
	Charging,
	Shot
}

/// <summary>
/// レーザー銃
/// </summary>
public class WeaponLaser : WeaponRanged {

	const string MOVE_MOD = "MOVE_MOD";

	public float shootingMoveMul;
	public string chargeSound;
	AudioSource laserSE;
	
	BulletLaser laser;
	LaserState state = LaserState.Idle;

	float maxLength;
	float chargeTime;

	public override void AttackStart() {

		//チャージ開始
		state = LaserState.Charging;
		chargeTime = 0;

		//SE再生
		laserSE = AudioManager.PlaySE(chargeSound, autoDelete:false);
		if(laserSE) {
			laserSE.loop = true;
			laserSE.transform.SetParent(transform);
		}

		//発射準備
		laser = (BulletLaser)bulletData.Create(this, shotAnchor.position, shotAnchor.rotation);
		laser.transform.parent = shotAnchor;
		laser.transform.localPosition = new Vector3();
		laser.transform.localRotation = Quaternion.identity;
		laser.length = 0.01f;

		maxLength = laser.GetBulletData<BulletLaserData>().maxLength;

		//移動速度を変更する処理をパッシブ効果で実装
		unitOwner.ApplyModifier(new StatusModifier(1, shootingMoveMul, 1, 1), MOVE_MOD);
	}

	public override void Attack() {

		switch(state) {
			case LaserState.Charging:

				if((chargeTime += Time.deltaTime) > interval) {

					//チャージSE終了
					if(laserSE) Destroy(laserSE.gameObject);

					//SE再生
					laserSE = AudioManager.PlaySE(useSound, autoDelete: false);
					if(laserSE) {
						laserSE.loop = true;
						laserSE.transform.SetParent(transform);
					}

					//照射ステートに変更
					state = LaserState.Shot;
				}
				break;
			case LaserState.Shot:

				//当たった場所もしくは最大の長さにする
				float length = maxLength;

				var hitAll = Physics.RaycastAll(shotAnchor.position, shotAnchor.forward, maxLength, hitMask)
					.Where(item => item.collider.gameObject != unitOwner.gameObject)
					.ToList();

				if(hitAll.Count != 0) {

					var nearestHit = hitAll
						.Aggregate((item, item2) => item.distance < item2.distance ? item : item2);

					length = nearestHit.distance;

					//当たった相手を伝える
					laser.currentAttackTarget = nearestHit.collider.gameObject;
				}


				//foreach(var hit in hitAll) {
				//	//自分のデータは除外
				//	if(ReferenceEquals(hit.collider.gameObject, unitOwner.gameObject)) continue;
				//	//当たったものの中で一番近い奴
				//	if(hit.distance < length) length = hit.distance;
				//}

				//照射距離の設定
				laser.length = length;

				if(length == maxLength) {

					//登録していたターゲットを解除
					laser.currentAttackTarget = null;

					//当たらなかった場合はヒットエフェクトの再生を止める
					if(laser.laserHitParticle) {
						ParticleManager.StopParticle(laser.laserHitParticle);
						laser.laserHitParticle = null;
					}
					//ヒットSEも止める
					if(laser.laserHitSE) {
						Destroy(laser.laserHitSE.gameObject);
						laser.laserHitSE = null;
					}
				}

				break;
			default:
				break;
		}
	}

	public override void AttackEnd() {

		//照射終了
		if(laser) {
			//ヒットエフェクトがあれば消す
			if(laser.laserHitParticle) ParticleManager.StopParticle(laser.laserHitParticle);
			//ヒットSEがあれば消す
			if(laser.laserHitSE) Destroy(laser.laserHitSE.gameObject);

			ParticleManager.StopParticle(laser.attackParticle);
			laser.GetComponent<Collider>().enabled = false;
			laser.length = 0;

			BulletData.DestroyBullet(laser);
			//Destroy(laser.gameObject, 4);
		}

		//SE終了
		if(laserSE) Destroy(laserSE.gameObject);

		//移動速度を変更する処理を解除
		unitOwner.RemoveModifier(MOVE_MOD);

		//待機ステートに変更
		state = LaserState.Idle;
	}
}
