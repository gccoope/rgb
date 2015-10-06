using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

public class LightRefillBehavior : MonoBehaviour {
	
	[SerializeField] float oscSpeed;
	[SerializeField] float oscAmplitude;
	[SerializeField] float distStartMoveToPlayer;
	[SerializeField] float moveSpeed;
	[SerializeField] bool debugMode;
	private float step; // used to step through angles for sin wave functions
	private Vector3 startPosition;
	private GameObject player = null;
	private bool moveToTarget;

	// Use this for initialization
	void Start () {
		startPosition = gameObject.transform.position;
		moveToTarget = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (player == null) {
			player = GameObject.FindGameObjectWithTag("player");
			return;
		}

		if (!moveToTarget && Vector3.Distance (transform.position, player.transform.position) < distStartMoveToPlayer)
			moveToTarget = true;
		else
			moveToTarget = false;

		if (moveToTarget) {
			float move = moveSpeed * Time.deltaTime;
			Vector3 movePos = Vector3.MoveTowards(transform.position, player.transform.position, move);
			transform.position = movePos;
		}

		Transform t = gameObject.GetComponent<Transform>(); // set this object's transform to 't' for easier coding
		Vector3 newPos = new Vector3(t.position.x, t.position.y, t.position.z);
		step += oscSpeed * Time.deltaTime;
		if (step > 2 * Mathf.PI) step -= 2 * Mathf.PI; // since Mathf is in radians, once step completes a full circle, reset it smoothly
		newPos.y = startPosition.y + oscAmplitude * Mathf.Sin(step);
		t.position = newPos;
	}

	void OnTriggerEnter(Collider col) {
		if (col.gameObject.tag == "player") {
			PlayerLight pLight = player.GetComponent<PlayerLight>();
			pLight.lightLeft = pLight.maxLight;
            col.GetComponent<FirstPersonController>().ShrinkGunFunction();
			if(debugMode) Debug.Log("player light replenished!");
			Destroy(gameObject);
		}
	}
}
