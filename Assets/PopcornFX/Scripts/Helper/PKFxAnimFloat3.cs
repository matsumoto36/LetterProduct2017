using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PKFxFX))]
public class PKFxAnimFloat3 : MonoBehaviour {
	public string propertyName;
	public Vector3 value;

	private PKFxFX fx;

	void Start()
	{
		this.fx = this.GetComponent<PKFxFX>();
		if (this.fx == null)
			this.enabled = false;
	}

	void LateUpdate()
	{
		this.fx.SetAttribute(new PKFxManager.Attribute(this.propertyName, this.value));
	}
}
