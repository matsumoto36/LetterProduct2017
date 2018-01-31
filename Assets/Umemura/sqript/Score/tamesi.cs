using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tamesi : MonoBehaviour
{
    public int count1 = 1000;
    public int count2 = 1000;
    public int count3 = 1000;
    public int count4 = 1000;

    public DamageScript damageScript;

    // Use this for initialization
    void Start ()
    {
        damageScript.ShowDamage(count1,count2,count3,count4);

    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
