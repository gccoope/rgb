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
	
	//Enable to toggle moving over time
	public bool changePosition = false;
	public Vector3 moveAmount = new Vector3(0f, 0f, 0f);
	private Vector3 initialPosition;
	public float movingRate = 1.0f;
	
	//Set how fast scaling occurs and what new scale becomes.
	public float newScale = 2;
	public float scalingRate = 1.0f;
	
	//Only bullets of the same frequency can "hit" the object and push it around
	public bool freqaffectphysics = false;
	
	//Private variables
	
	//Variables for flipping gravity
	private bool isGravityFlipped = false;
	
	//Variables for scaling object over time
	private bool scaling = false;
	private bool scaled = false;
	private int scaleCount = 0;
	private int scaleMax;
	
	//Variables for moving platform
	private bool moving = false;
	private bool moved = false;
	private int moveCount = 0;
	private int moveMax;
	
	private int signx;
	private int signy;
	private int signz;
	
	private Vector3 initialScale;
	
	
	private void Start()
	{
		//set color based on frequency
		if (frequency == 1.0f)
			this.GetComponent<MeshRenderer> ().material = Resources.Load ("Red_Mat", typeof(Material)) as Material;
		else if (frequency == 2.0f)
			this.GetComponent<MeshRenderer> ().material = Resources.Load ("Green_Mat", typeof(Material)) as Material;
		else if (frequency == 3.0f)
			this.GetComponent<MeshRenderer> ().material = Resources.Load ("Blue_Mat", typeof(Material)) as Material;
		
		
		initialScale = this.transform.localScale;
		
		if (newScale > initialScale.x)
		{
			scaleMax = (int)Math.Round((scalingRate / Time.fixedDeltaTime)*(newScale-initialScale.x));
		}
		else
		{
			scaleMax = (int)Math.Round((scalingRate / Time.fixedDeltaTime) * (initialScale.x - newScale));
		}
		
		initialPosition = transform.position;
		moveMax = (int)Math.Round((movingRate / Time.fixedDeltaTime));
		
		//Calculate whether to shrink or grow
		signx = Math.Sign(((newScale * initialScale.x) - initialScale.x));
		signy = Math.Sign(((newScale * initialScale.y) - initialScale.y));
		signz = Math.Sign(((newScale * initialScale.z) - initialScale.z));
		
	}
	
	void OnCollisionEnter(Collision c)
	{
		BulletScript bScript = c.gameObject.GetComponent<BulletScript> ();
		if (bScript == null) return;
		Debug.Log ("The frequency is " + bScript.freq);
		
		
		
		
		
		if (frequency == bScript.freq) {
			
			if (freqaffectphysics) {
				
				
				Vector3 imp = new Vector3(0f,0f,0f);
				
				imp = bScript.GetComponent<Rigidbody>().mass * (c.rigidbody.velocity - bScript.GetComponent<Rigidbody>().velocity);
				
				c.rigidbody.AddForce(imp*10.0f, ForceMode.Impulse);
				
				//				b.GetComponent<Rigidbody> ().AddForce
				//					(m_Camera.transform.forward * newVelocity/10, ForceMode.Impulse);
				
			}
			
			
			if (destroyonhit) {
				
				Destroy (this.gameObject);
			}
			if (flipGravity) {
				if (isGravityFlipped == false) {
					try {
						this.GetComponent<Rigidbody> ().useGravity = false;
						isGravityFlipped = true;
					} catch {
						Debug.Log ("No rigid body found.");
					}
				} else {
					try {
						this.GetComponent<Rigidbody> ().useGravity = true;
						isGravityFlipped = false;
					} catch {
						Debug.Log ("No rigid body found.");
					}
				}
				
			}
			if (changeScale) {
				scaling = true;
			}
			if(changePosition)
			{
				moving = true;
			}
		}
	}
	
	void FixedUpdate()
	{
		//Debug.Log ("Mass of object is " + this.GetComponent<Rigidbody>().mass);
		
		if(isGravityFlipped)
		{
			this.GetComponent<Rigidbody>().AddForce(new Vector3(0, 10, 0));
		}
		
		if(moving)
		{
			if(moved)
			{
				transform.position = new Vector3(transform.position.x - (movingRate * Time.fixedDeltaTime)*moveAmount.x, transform.position.y - (movingRate * Time.fixedDeltaTime) * moveAmount.y, transform.position.z - (movingRate * Time.fixedDeltaTime) * moveAmount.z);
				moveCount++;
				
				if (moveCount >= moveMax)
				{
					transform.position = new Vector3(initialPosition.x, initialPosition.y, initialPosition.z);
					moving = false;
					moved = false;
					moveCount = 0;
				}
			}
			else
			{
				transform.position = new Vector3(transform.position.x + (movingRate * Time.fixedDeltaTime) * moveAmount.x, transform.position.y + (movingRate * Time.fixedDeltaTime) * moveAmount.y, transform.position.z + (movingRate * Time.fixedDeltaTime) * moveAmount.z);
				moveCount++;
				
				if (moveCount >= moveMax)
				{
					transform.position = new Vector3(initialPosition.x + moveAmount.x, initialPosition.y + moveAmount.y, initialPosition.z + moveAmount.z);
					moving = false;
					moved = true;
					moveCount = 0;
				}
			}
		}
		
		if(scaling)
		{
			if(scaled)
			{
				
				
				this.transform.localScale = new Vector3(this.transform.localScale.x + ((scalingRate * Time.fixedDeltaTime) * (-signx)),this.transform.localScale.y + ((scalingRate * Time.fixedDeltaTime)*(-signy)), this.transform.localScale.z + ((scalingRate * Time.fixedDeltaTime)*(-signz)));
				scaleCount++;
				
				this.GetComponent<Rigidbody>().mass += (scalingRate * Time.fixedDeltaTime) * -signx;
				
				
				
				if (scaleCount >= scaleMax)
				{
					this.GetComponent<Rigidbody>().mass = initialScale.x;
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
				
				this.GetComponent<Rigidbody>().mass += (scalingRate * Time.fixedDeltaTime) * signx;
				
				if (scaleCount >= scaleMax)
				{
					this.GetComponent<Rigidbody>().mass = newScale;
					
					this.transform.localScale = new Vector3(initialScale.x * newScale, initialScale.y * newScale, initialScale.z * newScale);
					scaling = false;
					scaled = true;
					scaleCount = 0;
				}
			}
		}
	}
	
}