using UnityEngine;
using System.Collections;


/*
 * This script controls how the Lantern behaves.
 * 
 * Lanterns are initially unlit, but when shot by the player they absorb the light from the player's light_bullet.
 */


[RequireComponent(typeof(Light))]
[RequireComponent(typeof(SwitchBehavior))]
[RequireComponent(typeof(ParticleSystem))]

public class LanternBehavior : MonoBehaviour {

	private bool lit; // is the lantern lit or unlit?
	private Light lanternLight;
	private ParticleSystem insectParticles; // "moth effect"
	private float timeToStartPartSystem;
	private float counter;

	// Use this for initialization
	void Start () {
		lit = false; // start out unlit
		lanternLight = gameObject.GetComponent<Light> ();
		lanternLight.enabled = false;
		insectParticles = GetComponent<ParticleSystem> ();
		insectParticles.enableEmission = false;
		timeToStartPartSystem = 10 + Random.value * 110; // start particle system some time between 10s and 120s
		counter = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		if (lanternLight.enabled && !insectParticles.enableEmission) {
			// start counter to initiate
			Debug.Log ("Start countdown! t minus " + timeToStartPartSystem + " seconds");
			counter += Time.deltaTime;
			if(counter >= timeToStartPartSystem) insectParticles.enableEmission = true;
		}
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "bullet")
		{
			Light bulletLight = col.gameObject.GetComponent<Light>();
			lanternLight.enabled = true;
			lanternLight.color = bulletLight.color;
			lanternLight.intensity = bulletLight.intensity;
            lanternLight.range = bulletLight.range;
            GetComponent<SwitchBehavior>().setOn(true);
			Destroy(col.gameObject);
		}
	}
}
