using UnityEngine;
using System.Collections;


/*
 * This script controls how doors behave.

   Doors have an array of "triggers" which are GameObjects that MUST have a SwitchBehavior component. When each of the Triggers' SwitchBehavior components
   have been turned on, the door will open.
 */

[RequireComponent(typeof(SwitchBehavior))]

public class DoorBehavior : MonoBehaviour {

    [SerializeField] GameObject[] triggers;
    [SerializeField] bool debugMode;

    private bool hasOpened, hasClosed;

    // Use this for initialization
    void Start () {
        hasOpened = false;
        hasClosed = true;
	}
	
	// Update is called once per frame
	void Update () {

        // loop through array of triggers to see if they are on
        int numberOn = 0; // variable to count how many triggers are on
	    for(int t = 0; t < triggers.Length; t++)
        {
            SwitchBehavior sb = triggers[t].GetComponent<SwitchBehavior>();
            if (sb.isSwitchOn()) numberOn++;
        }

        if (numberOn >= triggers.Length) openDoor();
        else closeDoor();
    }

    public void openDoor()
    {
        if (hasOpened) return; // prevent this code from running more than once

        if (debugMode) Debug.Log("door open!");
        gameObject.GetComponent<SwitchBehavior>().setOn(true);

        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;

        hasOpened = true;
        hasClosed = false;
    }

    public void closeDoor()
    {
        if (hasClosed) return; // prevent this code from running more than once

        if (debugMode) Debug.Log("door closed!");
        gameObject.GetComponent<SwitchBehavior>().setOn(false);

        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<BoxCollider>().enabled = true;

        hasOpened = false;
        hasClosed = true;
    }
}
