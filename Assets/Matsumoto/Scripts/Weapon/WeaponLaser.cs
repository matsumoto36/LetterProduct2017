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

	float maxLength;
	float chargeTime;

	LaserState state = LaserState.Idle;

	/// <summary>
	/// 必要なデータをセットする
	/// </summary>
	/// <param name="power"></param>
	/// <param name="interval">=チャージ時間</param>
	/// <param name="bullet"></param>
	/// <param name="maxLength"></param>
	public void SetData(int power, float interval, BulletLaser bullet, float maxLength) {
		this.power = power;
		this.interval = interval;
		this.bullet = bullet;
		this.maxLength = maxLength;
	}


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
					laser = Instantiate(bullet).GetComponent<BulletLaser>();
					laser.transform.parent = shotAnchor;
					laser.transform.localPosition = new Vector3();
					laser.transform.localRotation = Quaternion.identity;

					//照射ステートに変更
					state = LaserState.Shot;
				}
				break;
			case LaserState.Shot:
				//照射
				float length = maxLength;
				RaycastHit hit;

				if(Physics.Raycast(shotAnchor.position, shotAnchor.forward, out hit, maxLength,
					~(LayerMask.GetMask("PlayerLayer") + LayerMask.GetMask("BulletLayer")))){

					Debug.Log(hit.collider.gameObject.name);
					length = hit.distance;
				}

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
