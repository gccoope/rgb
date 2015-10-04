using UnityEngine;
using Bullet;
using System.Collections;

[RequireComponent(typeof(SwitchBehavior))]
[RequireComponent(typeof(Light))]

public class LightSensorBehavior : MonoBehaviour {

    [SerializeField] TypeOfLight lightType;
    [SerializeField] bool alwaysVisible; // if false, then sensor is only visible when the player is the same type of light
    [SerializeField] bool debugMode;

    private bool visible;
    private GameObject player = null; // for quick reference
    private Light light;

	// Use this for initialization
	void Start () {
        if (!alwaysVisible)
        {
            visible = false;
            GetComponent<MeshRenderer>().enabled = false;
        }
        else visible = true;

        light = GetComponent<Light>();
        light.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (player == null) player = GameObject.FindGameObjectWithTag("player");

        if (!alwaysVisible)
        {
            if (player.GetComponent<PlayerLight>().playerLightType == lightType)
            {
                GetComponent<MeshRenderer>().enabled = true;
                visible = true;
            }
        }
	}

    void OnTriggerEnter(Collider col)
    {
        if (visible)
        {
            if(col.gameObject.tag == "bullet")
            {
                BulletScript bs = col.gameObject.GetComponent<BulletScript>();
                Light bulletLight = col.gameObject.GetComponent<Light>();

                if (bs.lightType == lightType)
                {
                    GetComponent<SwitchBehavior>().setOn(true);
                    light.enabled = true;
                    light.color = bulletLight.color;
                    light.range = bulletLight.range;
                    light.intensity = bulletLight.intensity;

                    Destroy(col.gameObject);
                     
                    if (debugMode) Debug.Log("light sensor activated!");
                }
            }
        }
    }
}
