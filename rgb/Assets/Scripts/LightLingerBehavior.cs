using UnityEngine;
using System.Collections;

/*

    When a light bullet hits an object and doesn't bounce, it creates a lingering light source that stays for a few moments before fading.

*/

[RequireComponent(typeof(Light))] // require parent object to have a Transform component

public class LightLingerBehavior : MonoBehaviour {

    [SerializeField] float lifeTime; // total life time of the object

    private float counter; // counts down to object's destruction
    private Light lightSource;
    private float startInstensity; // the initial intensity of this object's lightsource

	// Use this for initialization
	void Start () {
        counter = lifeTime;
        lightSource = GetComponent<Light>();
        startInstensity = lightSource.intensity;
	}
	
	// Update is called once per frame
	void Update () {
        float step = Time.deltaTime;
        counter -= step;

        if (counter <= 0) Destroy(gameObject); // destroy this object when counter hits zero, otherwise
        else lightSource.intensity = startInstensity * (counter / lifeTime); // fade light to zero
	}
}
