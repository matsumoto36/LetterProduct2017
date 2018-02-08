using UnityEngine;
using System.Collections;
using System;

[Serializable]
public struct RangeFloat : IRange<float>{

	[SerializeField]
	float min;
	public float Min {
		get { return min; }
		set { min = Mathf.Min(value, min); }
	}


	[SerializeField]
	float max;
	public float Max {
		get { return max; }
		set { max = Mathf.Max(value, max); }
	}

	public float Mid {
		get { return min + (max - min) / 2; }
	}

	public float Length {
		get { return Mathf.Abs(max - min); }
	}

	public float RandomValue {
		get { return min < max ? UnityEngine.Random.Range(min, max) : min; }
	}

	public RangeFloat(float value) {
		min = max = value;
	}

	public RangeFloat(float min, float max) {
		this.min = min;
		this.max = Mathf.Max(min, max);
	}
}
