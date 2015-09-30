using UnityEngine;
using System.Collections;

public class ShowSensor : MonoBehaviour {

	public string tagToFlip = "";
	public float lightIntensity = 10;

	private GameObject[] objects;

	// Use this for initialization
	void Start () {
		objects = GameObject.FindGameObjectsWithTag (tagToFlip);

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider col){

		foreach (GameObject g in objects) {
//			Instantiate(respawnPrefab, respawn.transform.position, respawn.transform.rotation);
			g.GetComponent<Light> ().intensity = lightIntensity;
		}


	}

	void OnTriggerExit(Collider col){

		foreach (GameObject g in objects) {
			//			Instantiate(respawnPrefab, respawn.transform.position, respawn.transform.rotation);
			g.GetComponent<Renderer>().enabled = false;
			g.GetComponent<Light> ().intensity = 0;
		}

	}
}
