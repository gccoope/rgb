using UnityEngine;
using Bullet;
using System.Collections;

/*

    This class handles what happens when an object like a wall or a mirror is hit by a light bullet

*/

[RequireComponent(typeof(Collider))]

public class InteractWithLight : MonoBehaviour
{

    // Reflect = the light bullet simply bounces off the surface
    // Refract = the light source is "split" into multiple beams/bullets
    // Absorb = the light source is destroyed and a glow lingers for a few moments
    public enum Type
    {
        Reflect, Refract, Absorb
    }

    [SerializeField]
    Type interactType;
    [SerializeField]
    TypeOfLight lightType;
    //[SerializeField] GameObject lightLinger; // Light linger object that is created when light is absorbed
    // !!!! It would probably be a good idea not to make this a serialized field.

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision col)
    {
        BulletScript bScript = col.gameObject.GetComponent<BulletScript>();
        if (bScript != null)
        {
            Transform bulletTransform = col.gameObject.GetComponent<Transform>();
            Light bulletLight = col.gameObject.GetComponent<Light>();
            Vector3 bulletPos = bulletTransform.position;
            Quaternion bulletRot = bulletTransform.rotation;

            switch (interactType)
            {
                case Type.Reflect:
                    // do nothing? right?
                    break;
                case Type.Refract:
                    // this is for prisms and stuff
                    break;
                case Type.Absorb:
                    GameObject l = Instantiate(Resources.Load("LightLinger", typeof(GameObject))) as GameObject;
                    //GameObject paint = Instantiate(Resources.Load("PaintArea", typeof(GameObject))) as GameObject;
                    l.transform.position = col.gameObject.transform.position;

                    
                    Color lightColor = bulletLight.color;
                    l.GetComponent<Light>().color = lightColor;
                    l.GetComponent<Light>().range = bulletLight.range;
                    l.GetComponent<LightLingerBehavior>().lightType = bScript.lightType;
                    //paint.transform.position = col.gameObject.transform.position;



                    
                    Destroy(col.gameObject); // destroy bullet
                    break;
            }
        }
    }
}
