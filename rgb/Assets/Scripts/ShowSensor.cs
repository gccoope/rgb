using UnityEngine;
using System.Collections;

public class ShowSensor : MonoBehaviour
{

    //reference to light intesnity
    private float int1;
    private float int2;
    private float int3;

    //reference to sensors related to button
    public GameObject trigger1;
    public GameObject trigger2;
    public GameObject trigger3;

    public bool debugFlag = false;

    // Use this for initialization
    void Start()
    {

        int1 = trigger1.GetComponent<Light>().intensity;
        int2 = trigger2.GetComponent<Light>().intensity;
        int3 = trigger3.GetComponent<Light>().intensity;

    }

    void OnTriggerEnter(Collider col)
    {
        if (debugFlag)
        {
            Debug.Log("entered collider");
        }

        //if there exists a trigger
        if (trigger1 != null)
        {
            //make it visible, turn on the light, set the intensity
            trigger1.GetComponent<MeshRenderer>().enabled = true;
            trigger1.GetComponent<Light>().enabled = true;
            trigger1.GetComponent<Light>().intensity = int1;
        }

        if (trigger2 != null)
        {
            trigger2.GetComponent<MeshRenderer>().enabled = true;
            trigger2.GetComponent<Light>().enabled = true;
            trigger2.GetComponent<Light>().intensity = int2;
        }


        if (trigger3 != null)
        {
            trigger3.GetComponent<MeshRenderer>().enabled = true;
            trigger3.GetComponent<Light>().enabled = true;
            trigger3.GetComponent<Light>().intensity = int3;
        }




    }

    void OnTriggerExit(Collider col)
    {

        if (debugFlag)
        {
            Debug.Log("Exited collider");
        }

        //if there is a trigger
        if (trigger1 != null)
        {
            //make it invisible, turn off the light.
            trigger1.GetComponent<MeshRenderer>().enabled = false;
            trigger1.GetComponent<Light>().enabled = false;
            trigger1.GetComponent<Light>().intensity = 0;
        }

        if (trigger2 != null)
        {
            trigger2.GetComponent<MeshRenderer>().enabled = false;
            trigger2.GetComponent<Light>().enabled = false;
            trigger2.GetComponent<Light>().intensity = 0;
        }


        if (trigger3 != null)
        {
            trigger3.GetComponent<MeshRenderer>().enabled = false;
            trigger3.GetComponent<Light>().enabled = false;
            trigger3.GetComponent<Light>().intensity = 0;
        }

    }
}
