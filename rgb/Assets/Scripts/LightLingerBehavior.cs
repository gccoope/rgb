using UnityEngine;
using System.Collections;

/*

    When a light bullet hits an object and doesn't bounce, it creates a lingering light source that stays for a few moments before fading.

*/

[RequireComponent(typeof(Light))] // require parent object to have a Transform component
[RequireComponent(typeof(SphereCollider))]

public class LightLingerBehavior : MonoBehaviour
{

    [SerializeField] float lifeTime; // total life time of the object
	[SerializeField] float moveSpeed;

    private float counter; // counts down to object's destruction
    private Light lightSource;
    private float startInstensity; // the initial intensity of this object's lightsource
    private bool moveToTarget; // if a LightLinger object is near an object that can absorb it, move toward that object
	private GameObject targetObj;
	private float sphereColRadius; // radius of sphere collider

    // Use this for initialization
    void Start()
    {
        counter = lifeTime;
        lightSource = GetComponent<Light>();
        startInstensity = lightSource.intensity;
        moveToTarget = false;
		targetObj = null;
		sphereColRadius = gameObject.GetComponent<SphereCollider> ().radius;
    }

    // Update is called once per frame
    void Update()
    {
        float step = Time.deltaTime;
        counter -= step;

        if (counter <= 0) Destroy(gameObject); // destroy this object when counter hits zero, otherwise
        else lightSource.intensity = startInstensity * (counter / lifeTime); // fade light to zero

        if (moveToTarget)
        {
            Vector3 newPos = Vector3.MoveTowards(transform.position, targetObj.transform.position, moveSpeed * Time.deltaTime);
			transform.position = newPos;

			float distance = Vector3.Distance(transform.position, targetObj.transform.position);
			if(distance <= 0.05f) {
				GameObject player = GameObject.FindGameObjectWithTag("player");
				player.GetComponent<PlayerLight>().incrLightByPrcnt(1.0f);
				Destroy(gameObject);
			}
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "player")
        {
			targetObj = col.gameObject;
			moveToTarget = true;
        }
	}
	
	void OnTriggerExit(Collider col)
	{
		if (col.gameObject.tag == "player")
		{
			targetObj = null;
			moveToTarget = false;
		}
	}
}
