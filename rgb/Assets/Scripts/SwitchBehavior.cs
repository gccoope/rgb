using UnityEngine;
using System.Collections;

/*
 * This is a generic component that can be attached to any object, such as a light sensor, button, etc., that gives it switch functionality.
 */

public class SwitchBehavior : MonoBehaviour {

    [SerializeField]
    bool isOn;

    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void setOn(bool o)
    {
        isOn = o;
    }

    public bool isSwitchOn()
    {
        return isOn;
    }
}
