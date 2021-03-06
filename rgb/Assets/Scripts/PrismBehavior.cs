﻿using UnityEngine;
using System.Collections;

public class PrismBehavior : MonoBehaviour {

    [SerializeField] TypeOfLight prismType;
    private Vector3 startPosition;
    private float oscAmplitude; // the amplitude of the the prism's up and down movement
    private float oscSpeed; // how fast the prism moves up and down
    private float spinSpeed; // how fast the prism spins
    private float step = 0;

	// Use this for initialization
	void Start () {
        Transform t = gameObject.GetComponent<Transform>(); // set this object's transform to 't' for easier coding
        startPosition = new Vector3(t.position.x, t.position.y, t.position.z);
        oscAmplitude = 0.3f; // <------- arbitrary numbers, adjust to whatever looks best
        oscSpeed = 0.5f; // <-------/
        spinSpeed = 50.0f; // <----/
	}
	
	// Update is called once per frame
	void Update ()
    {
        //float up and down with a sin wave
        Transform t = gameObject.GetComponent<Transform>(); // set this object's transform to 't' for easier coding
        Vector3 newPos = new Vector3(t.position.x, t.position.y, t.position.z);
        step += oscSpeed * Time.deltaTime;
        if (step > 2 * Mathf.PI) step -= 2 * Mathf.PI; // since Mathf is in radians, once step completes a full circle, reset it smoothly
        newPos.y = startPosition.y + oscAmplitude * Mathf.Sin(step);
        t.position = newPos;

        // spin cube
        transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime);
	}


    void OnTriggerEnter(Collider col)
    {
        //GameObject player = GameObject.FindGameObjectWithTag("player");
        if (col.gameObject.tag == "player")
        {
            // change player's color
            col.gameObject.GetComponent<PlayerLight>().changeLightType(prismType);


            //Set the gun object
            GameObject gun = GameObject.FindGameObjectWithTag("gun");

            //Set the gun color
            switch (col.gameObject.GetComponent<PlayerLight>().getLightType())
            {
                case TypeOfLight.White:
                    gun.GetComponent<Renderer>().material = Resources.Load("Solid_Always_Lit/White_Mat") as Material;
                    break;
                case TypeOfLight.Red:
                    gun.GetComponent<Renderer>().material = Resources.Load("Solid_Always_Lit/Red_Mat") as Material;
                    break;
                case TypeOfLight.Orange:
                    gun.GetComponent<Renderer>().material = Resources.Load("Solid_Always_Lit/Orange_Mat") as Material;
                    break;
                case TypeOfLight.Yellow:
                    gun.GetComponent<Renderer>().material = Resources.Load("Solid_Always_Lit/Yellow_Mat") as Material;
                    break;
                case TypeOfLight.Green:
                    gun.GetComponent<Renderer>().material = Resources.Load("Solid_Always_Lit/Green_Mat") as Material;
                    break;
                case TypeOfLight.Blue:
                    gun.GetComponent<Renderer>().material = Resources.Load("Solid_Always_Lit/Blue_Mat") as Material;
                    break;
                case TypeOfLight.Violet:
                    gun.GetComponent<Renderer>().material = Resources.Load("Solid_Always_Lit/Violet_Mat") as Material;
                    break;
            }
        }
    }
}
