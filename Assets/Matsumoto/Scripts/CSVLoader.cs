using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static class CSVLoader {

	/// <summary>
	/// データを読み込む
	/// </summary>
	/// <param name="path"></param>
	/// <returns></returns>
	public static List<List<string>> LoadData(string path) {
		var reader = new StringReader(Resources.Load<TextAsset>(path).text);
		var data = new List<List<string>>();

		while(reader.Peek() > -1) {
			data.Add(reader.ReadLine().Split(',').ToList());
		}

		return data;
	}
}
