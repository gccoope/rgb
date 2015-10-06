using UnityEngine;
using System.Collections;

public class ParentObj : MonoBehaviour {

    void OnTriggerEnter(Collider col)
    {
        Debug.Log("on platform!");
        if(col.gameObject.tag == "player" || 
            col.gameObject.tag == "light_linger")
            col.transform.parent = gameObject.transform;
    }

    void OnTriggerExit(Collider col)
    {
        if(col.gameObject.tag == "player" ||
            col.gameObject.tag == "light_linger")
            col.transform.parent = null;
    }

}
