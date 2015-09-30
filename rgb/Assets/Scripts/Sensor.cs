using UnityEngine;
using System.Collections;

public class Sensor : MonoBehaviour {


	public bool active = true;
	public bool canTripAgain = false;
	public float lightIntensity = 5;

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<Light> ().intensity = lightIntensity;

	
	}

	void Update() {
		if(active)
		{
//			Debug.Log ("Object is active");
			gameObject.GetComponent<Light> ().intensity = lightIntensity;
		}
		else {
//			Debug.Log ("Object is deactivated");
			gameObject.GetComponent<Light> ().intensity = 0;
		}
	}

	void OnCollisionEnter (Collision col){
		
		
		if (canTripAgain) {
			active = !active;
			
		}
		else
			active = false;
		
	}
	
	
}



