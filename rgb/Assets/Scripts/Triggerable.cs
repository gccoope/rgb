using UnityEngine;
using System.Collections;

/*
 * A generic script that allows an object to check if every object in an array of GameObjects with SwitchBehavior scripts have been activated,
 * thus triggering some event to take place.
 *
 * Settng the trigger array to size 0 will cause the trigger to automatically activate.
 */

public class Triggerable : MonoBehaviour {

    [SerializeField] GameObject[] triggers;

    private bool triggered;

    // Use this for initialization
    void Start () {
        triggered = false;
	}
	
	// Update is called once per frame
	void Update () {
        int numberOn = 0; // variable to count how many triggers are on
        for (int t = 0; t < triggers.Length; t++)
        {
            SwitchBehavior sb = triggers[t].GetComponent<SwitchBehavior>();
            if (sb.isSwitchOn()) numberOn++;
        }

        if (numberOn == triggers.Length) triggered = true;
        else triggered = false;
    }

    public bool isTriggered()
    {
        return triggered;
    }

    public void setTriggered(bool t)
    {
        triggered = t;
    }
}
