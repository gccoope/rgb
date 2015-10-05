using UnityEngine;
using System.Collections;

/*
 * This script moves a platform that follows a path of any length that can be set in the inspector. The platform goes to each point in a loop.
 */

public class MovingPlatformBehavior : MonoBehaviour {

    [SerializeField] Vector3[] path; // an array of points describing the path the platform will take, relative to its starting position.
    [SerializeField] float moveSpeed; // how fast the platform moves

    private Vector3[] actualPath; // a converted array describing the path in world space rather than relative to starting position.
    private int pathIndex; // which point in the path array the platform is currently moving towards

	// Use this for initialization
	void Start () {
        actualPath = new Vector3[path.Length + 1]; // the actualPath array will include the starting position, so make this array larger by 1
        actualPath[0] = transform.position; // set the starting point

        // convert points to world space.
        for(int i = 0; i < path.Length; i++)
        {
            Vector3 worldPoint = new Vector3(path[i].x + actualPath[0].x, path[i].y + actualPath[0].y, path[i].z + actualPath[0].z);
            actualPath[i + 1] = worldPoint;
        }

        pathIndex = 1; // start moving towards the first point in the path
	}
	
	// Update is called once per frame
	void Update () {

	    if(GetComponent<Triggerable>().isTriggered())
        {
            GetComponent<SwitchBehavior>().setOn(true);

            // move towards path point
            float step = moveSpeed * Time.deltaTime;
            Vector3 newPos = Vector3.MoveTowards(transform.position, actualPath[pathIndex], step);
            transform.position = newPos;

            // check if path point is reached
            if(transform.position == actualPath[pathIndex])
            {
                pathIndex++; // go to next point
                if (pathIndex >= actualPath.Length) pathIndex = 0; // loop around
            }
        }
        else GetComponent<SwitchBehavior>().setOn(false);

    }
}
