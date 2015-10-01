using UnityEngine;
using System.Collections;

public class MovePlatform : MonoBehaviour {

	public GameObject platform;
	public GameObject prismswitch;
	private float start;
	private bool s = false;

	// Use this for initialization
	void Start () {
		start = gameObject.GetComponent<Light> ().intensity;
	
	}
	
	// Update is called once per frame
	void Update () {

		Debug.Log (s);

//		Debug.Log (gameObject.GetComponent<Light> ().intensity);



		if (gameObject.GetComponent<Light> ().intensity != start) {
			s = true;

			if(s){
				platform.GetComponent<DetectHitOnObject>().moving = true;
				gameObject.GetComponent<Light>().intensity = start;
				s = false;

			}
		
//			gameObject.GetComponent<Light> ().intensity = start;




		}


	
	}

}
