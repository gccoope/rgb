using UnityEngine;
using System.Collections;

/*

    This is code on a basic enemy that moves to the nearest light source, mostly for demo purposes

*/

public class LightStalker_Behavior : MonoBehaviour {

    [SerializeField] public float maxDistance; // the maximum distance a light source can be for the LightStalker to start moving toward it
    [SerializeField] public float moveSpeed; // how fast the light stalker can travel

    private Light[] lightSources; // every light source in the level
    private Vector3 target; // the point that the light stalker is moving to

	// Use this for initialization
	void Start () {
        target = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        // is this a performance issue? Does every LightStalker_Behavior object carry a duplicate array of all light sources?
        lightSources = GameObject.FindObjectsOfType<Light>();

        // pick a light source to move toward
        // THERE IS PROBABLY A MORE EFFECIENT WAY TO DO THIS SEARCH !!!
        float distance = maxDistance;
        int d = -1; // index number to keep track of which object in lightSource[] is chosen
        for(int i = 0; i < lightSources.Length; i++)
        {
            Vector3 thisPos = transform.position; // this object's position
            Vector3 lightPos = lightSources[i].GetComponent<Transform>().position; // the position of the light source

            float distToLight = Vector3.Distance(thisPos, lightPos); // calculate distance to light source

            if (distToLight < distance) // check if a closer light source has been found
            {
                distance = distToLight;
                d = i;
            }
        } // end for

        // set target
        if (d >= 0) target = lightSources[d].GetComponent<Transform>().position; // set target to the closest light source if one exists
        else target = transform.position; // if there is no lightSource in range, the target is the position the light stalker is already at

        // move to target
        float step = moveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target, step); // thanks, Unity, that's convenient
	}
}
