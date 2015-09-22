using UnityEngine;
using System.Collections;

public class ChangeGravity : MonoBehaviour {

	private Vector3 tempg =  new Vector3(0,0,0);

	// Use this for initialization
	void Start () {
	
		 tempg = Physics.gravity;
	}
	
	// Update is called once per frame
	void Update () {
	
		if (Input.GetKeyDown (KeyCode.Alpha8)) {
			Physics.gravity = new Vector3 (0, -1.0F, 0);
			Debug.Log("Altered Gravity");
		}
		if (Input.GetKeyDown (KeyCode.Alpha9)) {
			Physics.gravity = tempg;
			Debug.Log ("Restored gravity");
		}
	}
}
