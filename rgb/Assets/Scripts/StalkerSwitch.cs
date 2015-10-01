using UnityEngine;
using System.Collections;

public class StalkerSwitch : MonoBehaviour {

//	public GameObject stalker = null;

	public bool debugFlag = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider col){
		GameObject g = col.gameObject;

		if (g.tag == "stalker") {

			if(debugFlag)
			{
				Debug.Log ("Stalker has touched button");

			}

		}

	}
}
