using UnityEngine;
using System.Collections;
using Bullet;
using System;

[RequireComponent ( typeof(Collider))]
public class DetectHitOnObject : MonoBehaviour 
{

    //Public variables

    //Variable to set the frequency (color) enable trigger
    public float frequency = 1.0f;

    //Enable to destroy on matched frequency
	public bool destroyonhit = false;

    //Enable to flip gravity on hit
    public bool flipGravity = false;

    //Enable to toggle scale of object over time
    public bool changeScale = false;

    //Set how fast scaling occurs and what new scale becomes.
    public float newScale = 2;
    public float scalingRate = 1.0f;
	
    //Private variables

    //Variables for flipping gravity
    private bool isGravityFlipped = false;

    //Variables for scaling object over time
    private bool scaling = false;
    private bool scaled = false;
    private int scaleCount = 0;
    private int scaleMax;
    private int signx;
    private int signy;
    private int signz;
    
    private Vector3 initialScale;

    private void Start()
    {
        initialScale = this.transform.localScale;

        if (newScale > initialScale.x)
        {
            scaleMax = (int)Math.Round((scalingRate / Time.fixedDeltaTime)*(newScale-initialScale.x));
        }
        else
        {
            scaleMax = (int)Math.Round((scalingRate / Time.fixedDeltaTime) * (initialScale.x - newScale));
        }
        

        //Calculate whether to shrink or grow
        signx = Math.Sign(((newScale * initialScale.x) - initialScale.x));
        signy = Math.Sign(((newScale * initialScale.y) - initialScale.y));
        signz = Math.Sign(((newScale * initialScale.z) - initialScale.z));

    }

	void OnCollisionEnter(Collision c)
	{
		BulletScript bScript = c.gameObject.GetComponent<BulletScript> ();
		Debug.Log ("The frequency is " + bScript.freq);

		if (destroyonhit) {
			if(frequency == bScript.freq)
				Destroy(this.gameObject);
		}
        if (flipGravity)
        {
            if (isGravityFlipped == false)
            {
                try
                {
                    this.GetComponent<Rigidbody>().useGravity = false;
                    isGravityFlipped = true;
                }
                catch
                {
                    Debug.Log("No rigid body found.");
                }
            }
            else
            {
                try
                {
                    this.GetComponent<Rigidbody>().useGravity = true;
                    isGravityFlipped = false;
                }
                catch
                {
                    Debug.Log("No rigid body found.");
                }
            }
            
        }
        if(changeScale)
        {
            scaling = true;
        }
	}

    void FixedUpdate()
    {
        if(isGravityFlipped)
        {
            this.GetComponent<Rigidbody>().AddForce(new Vector3(0, 10, 0));
        }

        if(scaling)
        {
            if(scaled)
            {


                this.transform.localScale = new Vector3(this.transform.localScale.x + ((scalingRate * Time.fixedDeltaTime) * (-signx)),this.transform.localScale.y + ((scalingRate * Time.fixedDeltaTime)*(-signy)), this.transform.localScale.z + ((scalingRate * Time.fixedDeltaTime)*(-signz)));
                scaleCount++;

                if (scaleCount >= scaleMax)
                {
                    this.transform.localScale = new Vector3(initialScale.x, initialScale.y, initialScale.z);
                    scaling = false;
                    scaled = false;
                    scaleCount = 0;
                }
            }
            else
            {
                this.transform.localScale = new Vector3(this.transform.localScale.x + ((scalingRate * Time.fixedDeltaTime) * (signx)), this.transform.localScale.y + ((scalingRate * Time.fixedDeltaTime) * (signy)), this.transform.localScale.z + ((scalingRate * Time.fixedDeltaTime) * (signz)));
                scaleCount++;

                if (scaleCount >= scaleMax)
                {
                    this.transform.localScale = new Vector3(initialScale.x * newScale, initialScale.y * newScale, initialScale.z * newScale);
                    scaling = false;
                    scaled = true;
                    scaleCount = 0;
                }
            }
        }
    }

}

