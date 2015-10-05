using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SwitchBehavior))]

public class PressureSwitchBehavior : MonoBehaviour {

    void OnTriggerEnter(Collider col)
    {
        SwitchBehavior sb = GetComponent<SwitchBehavior>();
        sb.setOn(true);
    }

    void OnTriggerExit(Collider col)
    {
        SwitchBehavior sb = GetComponent<SwitchBehavior>();
        sb.setOn(false);
    }
}
