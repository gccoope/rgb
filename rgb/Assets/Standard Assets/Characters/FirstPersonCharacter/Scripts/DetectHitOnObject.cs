using UnityEngine;
using System.Collections;
using Bullet;

	[RequireComponent ( typeof(Collider))]
public class DetectHitOnObject : MonoBehaviour {

	public bool destroyonhit = false;
	public float frequency = 1.0f;

	void OnCollisionEnter(Collision c)
	{
		BulletScript bScript = c.gameObject.GetComponent<BulletScript> ();
		Debug.Log ("The frequency is " + bScript.freq);

		if (destroyonhit) {
			if(frequency == bScript.freq)
				Destroy(this.gameObject);
		}
	}



}

