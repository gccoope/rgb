using UnityEngine;
using System.Collections;

public class DetectPuz2 : MonoBehaviour
{

    //ugly way to hold references to lights and gameobjects in puzzle sensors
    public GameObject trig1, trig2, trig3, trig4, trig5;
    private Light l1, l2, l3, l4, l5;

    // Use this for initialization
    void Start()
    {

        //get light references
        l1 = trig1.GetComponent<Light>();
        l2 = trig2.GetComponent<Light>();
        l3 = trig3.GetComponent<Light>();
        l4 = trig4.GetComponent<Light>();
        l5 = trig5.GetComponent<Light>();

    }

    // Update is called once per frame
    void Update()
    {

        //if they are all set to off, player has completed puzzle. fire off platform movement script here?
        if (l1.intensity == 0 && l2.intensity == 0 && l3.intensity == 0 && l4.intensity == 0 && l5.intensity == 0)
        {
            Debug.Log("Puzzle2 complete!");
        }


    }
}
