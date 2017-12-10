﻿using System.Collections;
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
	}

	public override void Attack() {

		switch(state) {
			case LaserState.Charging:

				if((chargeTime += Time.deltaTime) > interval) {
					//照射準備
					laser = (BulletLaser)bulletData.Create(this, shotAnchor.position, shotAnchor.rotation);

					laser.transform.parent = shotAnchor;
					laser.transform.localPosition = new Vector3();
					laser.transform.localRotation = Quaternion.identity;

					maxLength = laser.GetBulletData<BulletLaserData>().maxLength;

					//照射ステートに変更
					state = LaserState.Shot;
				}
				break;
			case LaserState.Shot:

				//当たった場所もしくは最大の長さにする
				float length = maxLength;

				Debug.Log(hitMask.ToString());
				RaycastHit[] hitAll = Physics.RaycastAll(shotAnchor.position, shotAnchor.forward, length, hitMask);

				foreach(var hit in hitAll) {
					//自分のデータは除外
					if(ReferenceEquals(hit.collider.gameObject, unitOwner.gameObject)) continue;
					//当たったものの中で一番近い奴
					if(hit.distance < length) length = hit.distance;
				}

				//照射距離の設定
				laser.length = length;

				break;
			default:
				break;
		}
	}

	public override void AttackEnd() {

		//照射終了
		if(laser) Destroy(laser.gameObject);
		laser = null;

		//待機ステートに変更
		state = LaserState.Idle;
	}
}
