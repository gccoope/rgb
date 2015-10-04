using UnityEngine;
using System.Collections;
using Bullet;

public class Sensor : MonoBehaviour
{

    [SerializeField] TypeOfLight lightType;

    //is sensor active to begin with?
    public bool active = true;

    //place holder for intensity of light (determined within the inspector window of the gameobject)
    private float lightIntensity;

    public bool debugFlag = false;

    //uncomment these lines to allow sensor to be "retripped"
    //public bool canTripAgain = false;

    // Use this for initialization
    void Start()
    {

        //grab the light's intensity
        lightIntensity = gameObject.GetComponent<Light>().intensity;

    }

    void Update()
    {
        if (active)
        {
            if (debugFlag)
            {
                Debug.Log("Object is active");
            }

            gameObject.GetComponent<Light>().intensity = lightIntensity;
        }
        else
        {

            if (debugFlag)
            {
                Debug.Log("Object is deactivated");
            }
            gameObject.GetComponent<Light>().intensity = 0;
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "bullet")
        {
            GameObject bullet = col.gameObject;
            // only trip sensor when bullet is struck by the right type of bullet
            if (bullet.GetComponent<BulletScript>().lightType == lightType) active = false; 
        }
    }


}



