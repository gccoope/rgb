using UnityEngine;
using System.Collections;


/*
 * This script controls how the Lantern behaves.
 * 
 * Lanterns are initially unlit, but when shot by the player they absorb the light from the player's light_bullet.
 */


[RequireComponent(typeof(Light))]

public class LanternBehavior : MonoBehaviour {

	private bool lit; // is the lantern lit or unlit?
	private Light lanternLight;

	// Use this for initialization
	void Start () {
		lit = false; // start out unlit
		lanternLight = gameObject.GetComponent<Light> ();
		lanternLight.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "bullet")
		{
			Light bulletLight = col.gameObject.GetComponent<Light>();
			lanternLight.enabled = true;
			lanternLight.color = bulletLight.color;
			lanternLight.intensity = bulletLight.intensity;
			Destroy(col.gameObject);
		}
	}
}
