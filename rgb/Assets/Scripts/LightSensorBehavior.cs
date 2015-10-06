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

        light = GetComponent<Light>();

        if (!alwaysVisible)
        {
            visible = false;
            GetComponent<MeshRenderer>().enabled = false;
            light.enabled = false;
        }
        else {
            visible = true;
            light.enabled = true;
        }

        setLightVariables();

    }
	
	// Update is called once per frame
	void Update () {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("player");
            return;
        }

        if (!alwaysVisible)
        {
            if (player.GetComponent<PlayerLight>().playerLightType == lightType)
            {
                GetComponent<MeshRenderer>().enabled = true;
                visible = true;
                light.enabled = true;
            }
        }
	}

    void setLightVariables()
    {
        light.intensity = 2;
        light.range = 5;

        switch (lightType)
        {
            case TypeOfLight.White:
                light.color = PlayerLight.color_white;
                break;
            case TypeOfLight.Red:
                light.color = PlayerLight.color_red;
                break;
            case TypeOfLight.Orange:
                light.color = PlayerLight.color_orange;
                break;
            case TypeOfLight.Yellow:
                light.color = PlayerLight.color_yellow;
                break;
            case TypeOfLight.Green:
                light.color = PlayerLight.color_green;
                break;
            case TypeOfLight.Blue:
                light.color = PlayerLight.color_blue;
                break;
            case TypeOfLight.Violet:
                light.color = PlayerLight.color_violet;
                break;
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
                    MeshRenderer mr = GetComponent<MeshRenderer>();

                    // change material type to always lit
                    switch (lightType)
                    {
                        case TypeOfLight.White:
                            // lol "Mr. Material"
                            mr.material = Resources.Load("Solid_Always_Lit/White_Mat", typeof(Material)) as Material;
                            break;
                        case TypeOfLight.Red:
                            mr.material = Resources.Load("Solid_Always_Lit/Red_Mat", typeof(Material)) as Material;
                            break;
                        case TypeOfLight.Orange:
                            mr.material = Resources.Load("Solid_Always_Lit/Orange_Mat", typeof(Material)) as Material;
                            break;
                        case TypeOfLight.Yellow:
                            mr.material = Resources.Load("Solid_Always_Lit/Yellow_Mat", typeof(Material)) as Material;
                            break;
                        case TypeOfLight.Green:
                            mr.material = Resources.Load("Solid_Always_Lit/Green_Mat", typeof(Material)) as Material;
                            break;
                        case TypeOfLight.Blue:
                            mr.material = Resources.Load("Solid_Always_Lit/Blue_Mat", typeof(Material)) as Material;
                            break;
                        case TypeOfLight.Violet:
                            mr.material = Resources.Load("Solid_Always_Lit/Violet_Mat", typeof(Material)) as Material;
                            break;
                    }

                    Destroy(col.gameObject);
                     
                    if (debugMode) Debug.Log("light sensor activated!");
                }
            }
        }
    }
}
