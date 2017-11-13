using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum LaserState {
	Idle,
	Charging,
	Shot
}

public class WeaponLaser : WeaponRanged {

	BulletLaser laser;
	float chargeTime;
	LaserState state = LaserState.Idle;

	public override void AttackStart() {
		Debug.Log("Charge");

		//チャージ開始
		state = LaserState.Charging;
		chargeTime = 0;
	}

	public override void Attack() {

		//チャージ
		switch(state) {
			case LaserState.Charging:

				if((chargeTime += Time.deltaTime) > interval) {
					//照射準備
					Debug.Log("Shot");
					laser = (BulletLaser)Bullet.CreateBullet(bData, shotAnchor);
					laser.transform.parent = shotAnchor;
					laser.transform.localPosition = new Vector3();
					laser.transform.localRotation = Quaternion.identity;

					//照射ステートに変更
					state = LaserState.Shot;
				}
				break;
			case LaserState.Shot:

				//当たった場所もしくは最大の長さにする
				var length = laser.bData.maxLength;
				var mask = ~(LayerMask.GetMask("PlayerLayer") + LayerMask.GetMask("BulletLayer"));

				RaycastHit hit;

				if(Physics.Raycast(shotAnchor.position, shotAnchor.forward, out hit, length, mask)){

					length = hit.distance;
				}

				//照射距離の設定
				laser.length = length;

				break;
			default:
				break;
		}
	}

	public override void AttackEnd() {

		Debug.Log("End");

		//照射終了
		if(laser) Destroy(laser.gameObject);
		laser = null;

		//待機ステートに変更
		state = LaserState.Idle;
	}
}
