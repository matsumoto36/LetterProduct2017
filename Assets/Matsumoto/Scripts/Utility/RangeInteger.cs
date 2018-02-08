using UnityEngine;
using System.Collections;
using System;

[Serializable]
public struct RangeInteger : IRange<int>{

	[SerializeField]
	int min;
	public int Min {
		get { return min; }
		set { min = Mathf.Min(value, min); }
	}
	[SerializeField]
	int max;
	public int Max {
		get { return max; }
		set { max = Mathf.Max(value, max); }
	}

	public int Mid {
		get { return min + (max - min) / 2; }
	}

	public int Length {
		get { return Mathf.Abs(max - min); }
	}

	public int RandomValue {
		get { return min < max ? UnityEngine.Random.Range(min, max + 1) : min; }
	}

	public RangeInteger(int value) {
		min = max = value;
	}

	public RangeInteger(int min, int max) {
		this.min = min;
		this.max = Mathf.Max(min, max);
	}
}
