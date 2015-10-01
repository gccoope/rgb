using UnityEngine;
using System.Collections;

/*

    When a light bullet hits an object and doesn't bounce, it creates a lingering light source that stays for a few moments before fading.

*/

[RequireComponent(typeof(Light))] // require parent object to have a Transform component

public class LightLingerBehavior : MonoBehaviour
{

    [SerializeField]
    float lifeTime; // total life time of the object

    private float counter; // counts down to object's destruction
    private Light lightSource;
    private float startInstensity; // the initial intensity of this object's lightsource
    private bool moveToTarget; // if a LightLinger object is near an object that can absorb it, move toward that object
    private Vector3 targetPos;
    private float moveSpeed;

    // Use this for initialization
    void Start()
    {
        counter = lifeTime;
        lightSource = GetComponent<Light>();
        startInstensity = lightSource.intensity;
        moveToTarget = false;
        targetPos = transform.position;
        moveSpeed = 1.0f; // arbitrary number, use whatever works best
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
            Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "player")
        {
            moveToTarget = true;
            targetPos = col.gameObject.transform.position;
        }
    }
}
