using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;


/*
 * 
 * Bullet logic
 * 
 */

namespace Bullet
{
	public class BulletScript : MonoBehaviour {

        [SerializeField] public TypeOfLight lightType; // set the type of light in the inspector
        
        private float lifeTime = 5.0f; // bullets are alive for 5 seconds
        private float launchImpulse = 1.0f; // initial impulse that sends the bullet forward


        /*
		//frequency of bullet
		public float freq = 1.0f;

		//"life" of the bullet
		public float timeToDestroy;
	
		//for grabbing material
		private Renderer r;
		private Material m;
		private Color c;

		//step at which to decrement alpha
		private float step;

		//for debug
		private bool d = true;
        */

		// Use this for initialization
		void Start () {

            /*

			timeToDestroy = 2.0f;

			//Destory the bullet after a set amount of time
			Destroy(this.gameObject, timeToDestroy);

			//grab renderer
			r = GetComponent<Renderer> ();

			//set color based on frequency
			if (freq == 1.0f)
				this.GetComponent<MeshRenderer> ().material = Resources.Load ("Red_Mat", typeof(Material)) as Material;
			else if (freq == 2.0f)
				this.GetComponent<MeshRenderer> ().material = Resources.Load ("Green_Mat", typeof(Material)) as Material;
			else if (freq == 3.0f)
				this.GetComponent<MeshRenderer> ().material = Resources.Load ("Blue_Mat", typeof(Material)) as Material;

			//grab mat
			m = r.material;

			//calculate alpha step
			step = 1.0f / timeToDestroy * Time.fixedDeltaTime;

			if (d) {
				Debug.Log (step);
				Debug.Log ("Time to destroy: " + timeToDestroy);
			}

               */
	
		}
		
		// Update is called once per frame
		void Update () {

	
		}

		void OnDestroy() {
//            Debug.Log ("Destroyed bullet");
		}


		void FixedUpdate() {

            /*
			//decrement alpha of bullet
			if (m.color.a > 0) {
				c = m.color;
				c.a -= step;
				m.color = c;
			}
            */
		}

	}
}
