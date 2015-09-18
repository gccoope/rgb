using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;

namespace Bullet
{

public class BulletScript : MonoBehaviour {

	public float freq = 1.0f;




	// Use this for initialization
	void Start () {
		if (freq == 1.0f)
			this.GetComponent<MeshRenderer> ().material = Resources.Load ("Red_Mat", typeof(Material)) as Material;
		else if (freq == 2.0f)
			this.GetComponent<MeshRenderer> ().material = Resources.Load ("Green_Mat", typeof(Material)) as Material;
		else if (freq == 3.0f)
			this.GetComponent<MeshRenderer> ().material = Resources.Load ("Blue_Mat", typeof(Material)) as Material;
	}
	
	// Update is called once per frame
	void Update () {
	
	}


}
}
