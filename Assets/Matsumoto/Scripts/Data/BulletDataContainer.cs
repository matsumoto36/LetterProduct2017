using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


/// <summary>
/// 弾のデータを格納しておく
/// </summary>
public class BulletDataContainter : SingletonMonoBehaviour<BulletDataContainter> {

	const string BULLET_DATA_PATH = "Data/BulletData";

	public BulletData this[int index] {
		get {
			if(index < 0 || index >= instance.bulletDataList.Count) return null;
			return instance.bulletDataList[index];
		}
	}

	List<BulletData> bulletDataList;

	//外部からのnew禁止
	private BulletDataContainter() { }

	public static void Load() {

		//CSVから読み込む
		instance.bulletDataList = CSVLoader.LoadData(BULLET_DATA_PATH)
			.Select((item) => new {
				name = item[0],
				bullet = Resources.Load<Bullet>(item[1]),
				model = Resources.Load<GameObject>(item[2]),
				speed = float.Parse(item[3]),
				expRadius = float.Parse(item[4]),
				expPow = int.Parse(item[5]),
				maxLength = float.Parse(item[6])
			})
			.Select((item) =>
				new BulletData(
					item.name,
					item.bullet,
					item.model,
					item.speed,
					item.expPow,
					item.expRadius,
					item.maxLength
					))
			.ToList();
	}
}
