using UnityEngine;
using System.Collections;

public class ControlReflectionProbe : MonoBehaviour {

    // hmm... is it possible to just figure this out programmatically by looking at the gameObject.transform.rotation?
    public enum Direction
    {
        X, Y, Z
    }

    [SerializeField] GameObject mirror;

    private GameObject playerCam;
    public float offset;
    public Direction dir;


	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update ()
    {


        // Works now! Needed to set playerCam to the "MainCamera" object rather than "player"

        if(playerCam == null)playerCam = GameObject.FindGameObjectWithTag("MainCamera");
        Vector3 probePos = gameObject.GetComponent<Transform>().position;

        if (dir == Direction.X)
        {
            offset = mirror.transform.position.x - playerCam.transform.position.x;

            probePos.x = mirror.transform.position.x + offset;
            probePos.y = playerCam.transform.position.y;
            probePos.z = playerCam.transform.position.z;
        }

        else if (dir == Direction.Y)
        {
            offset = mirror.transform.position.y - playerCam.transform.position.y;

            probePos.x = playerCam.transform.position.x;
            probePos.y = mirror.transform.position.y + offset;
            probePos.z = playerCam.transform.position.z;
        }

        else if (dir == Direction.Z)
        {
            offset = mirror.transform.position.z - playerCam.transform.position.z;

            probePos.x = playerCam.transform.position.x;
			probePos.y = playerCam.transform.position.y;
            probePos.z = mirror.transform.position.z + offset;
        }

        gameObject.GetComponent<Transform>().position = probePos;

    }
}
