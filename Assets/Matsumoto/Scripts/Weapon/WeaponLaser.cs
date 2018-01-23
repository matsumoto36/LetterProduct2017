using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum LaserState {
	Idle,
	Charging,
	Shot
}

/// <summary>
/// レーザー銃
/// </summary>
public class WeaponLaser : WeaponRanged {

	BulletLaser laser;
	LaserState state = LaserState.Idle;

	float maxLength;
	float chargeTime;

	public override void AttackStart() {

		//チャージ開始
		state = LaserState.Charging;
		chargeTime = 0;

		//発射準備
		laser = (BulletLaser)bulletData.Create(this, shotAnchor.position, shotAnchor.rotation);
		laser.transform.parent = shotAnchor;
		laser.transform.localPosition = new Vector3();
		laser.transform.localRotation = Quaternion.identity;
		laser.length = 0.01f;
		//laser.transform.localScale = Vector3.one;

		maxLength = laser.GetBulletData<BulletLaserData>().maxLength;
	}

	public override void Attack() {

		switch(state) {
			case LaserState.Charging:

				if((chargeTime += Time.deltaTime) > interval) {

					//照射ステートに変更
					state = LaserState.Shot;
				}
				break;
			case LaserState.Shot:

				//当たった場所もしくは最大の長さにする
				float length = maxLength;

				RaycastHit[] hitAll = Physics.RaycastAll(shotAnchor.position, shotAnchor.forward, length, hitMask);

				foreach(var hit in hitAll) {
					//自分のデータは除外
					if(ReferenceEquals(hit.collider.gameObject, unitOwner.gameObject)) continue;
					//当たったものの中で一番近い奴
					if(hit.distance < length) length = hit.distance;
				}

				//照射距離の設定
				laser.length = length;

				if(length == maxLength && laser.laserHitParticle) {
					//当たらなかった場合はヒットエフェクトの再生を止める
					Destroy(laser.laserHitParticle.gameObject);
					laser.laserHitParticle = null;
				}

				break;
			default:
				break;
		}
	}

	public override void AttackEnd() {

		//ヒットエフェクトがあれば消す
		if(laser.laserHitParticle) Destroy(laser.laserHitParticle.gameObject);

		//照射終了
		if(laser) {
			laser.GetComponent<Collider>().enabled = false;
			laser.length = 0;
			Destroy(laser.gameObject, 4);
			laser.GetComponentInChildren<PKFxFX>().StopEffect();
		}
		laser = null;

		//待機ステートに変更
		state = LaserState.Idle;
	}
}
