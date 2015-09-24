using UnityEngine;
using Bullet;
using System.Collections;

/*

    This class handles what happens when an object like a wall or a mirror is hit by a light bullet

*/

[RequireComponent(typeof(Collider))]

public class InteractWithLight : MonoBehaviour {

    // Reflect = the light bullet simply bounces off the surface
    // Refract = the light source is "split" into multiple beams/bullets
    // Absorb = the light source is destroyed and a glow lingers for a few moments
    public enum Type
    {
        Reflect, Refract, Absorb
    }

    [SerializeField] Type interactType;
    [SerializeField] GameObject lightLinger; // Light linger object that is created when light is absorbed

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision col)
    {
        BulletScript bScript = col.gameObject.GetComponent<BulletScript>();
        if(bScript != null)
        {
            Transform bulletTransform = col.gameObject.GetComponent<Transform>();
            Vector3 bulletPos = bulletTransform.position;
            Quaternion bulletRot = bulletTransform.rotation;

            switch (interactType) {
                case Type.Reflect:
                    // do nothing? right?
                    break;
                case Type.Refract:
                    // this is for prisms and stuff
                    break;
                case Type.Absorb:
                    GameObject l = (GameObject)Instantiate(lightLinger, bulletPos, bulletRot);// create a lightLinger object
					//l.GetComponent<Transform>().SetParent(gameObject, false);
                    Destroy(col.gameObject); // destroy bullet
                    break;
            }
        }
    }
}
