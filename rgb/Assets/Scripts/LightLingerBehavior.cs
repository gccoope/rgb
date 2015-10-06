using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

/*

    When a light bullet hits an object and doesn't bounce, it creates a lingering light source that stays for a few moments before fading.

*/

[RequireComponent(typeof(Light))] // require parent object to have a Transform component
[RequireComponent(typeof(SphereCollider))]


public class LightLingerBehavior : MonoBehaviour
{

    [SerializeField] float lifeTime; // total life time of the object
	[SerializeField] float moveSpeed;
    [SerializeField] public TypeOfLight lightType; // set the type of light in the inspector

    private float counter; // counts down to object's destruction
    private Light lightSource;
    private float startInstensity; // the initial intensity of this object's lightsource
    private bool moveToTarget; // if a LightLinger object is near an object that can absorb it, move toward that object
	private GameObject targetObj;
	private float sphereColRadius; // radius of sphere collider

    //Save the particle emitter prefab
    GameObject particle;
    private float initialAlpha;

    // Use this for initialization
    void Start()
    {
        counter = lifeTime;
        lightSource = GetComponent<Light>();
        startInstensity = lightSource.intensity;
        moveToTarget = false;
		targetObj = null;
		sphereColRadius = gameObject.GetComponent<SphereCollider> ().radius;



        switch (lightType)
        {
            case TypeOfLight.White:
                particle = Instantiate(Resources.Load("LightLingerParticle_White") as GameObject);
                particle.transform.parent = this.transform;
                particle.transform.localPosition = Vector3.zero;
                initialAlpha = particle.GetComponent<Renderer>().material.GetColor("_TintColor").a;
                //GetComponent<Renderer>().material.mainTexture = Resources.Load("White_Orb_Tex") as Texture;
                //GetComponent<Renderer>().material = Resources.Load("Solid_Always_Lit/White_Mat") as Material;
                break;
            case TypeOfLight.Red:
                particle = Instantiate(Resources.Load("LightLingerParticle_Red") as GameObject);
                particle.transform.parent = this.transform;
                particle.transform.localPosition = Vector3.zero;
                initialAlpha = particle.GetComponent<Renderer>().material.GetColor("_TintColor").a;
                break;
            case TypeOfLight.Orange:
                //GetComponent<Renderer>().material = Resources.Load("Solid_Always_Lit/Orange_Mat") as Material;
                break;
            case TypeOfLight.Yellow:
                //GetComponent<Renderer>().material = Resources.Load("Solid_Always_Lit/Yellow_Mat") as Material;
                break;
            case TypeOfLight.Green:
                particle = Instantiate(Resources.Load("LightLingerParticle_Green") as GameObject);
                particle.transform.parent = this.transform;
                particle.transform.localPosition = Vector3.zero;
                initialAlpha = particle.GetComponent<Renderer>().material.GetColor("_TintColor").a;
                break;
            case TypeOfLight.Blue:
                particle = Instantiate(Resources.Load("LightLingerParticle_Blue") as GameObject);
                particle.transform.parent = this.transform;
                particle.transform.localPosition = Vector3.zero;
                initialAlpha = particle.GetComponent<Renderer>().material.GetColor("_TintColor").a;
                break;
            case TypeOfLight.Violet:
                //GetComponent<Renderer>().material = Resources.Load("Solid_Always_Lit/Violet_Mat") as Material;
                break;
        }

    }

    // Update is called once per frame
    void Update()
    {
        float step = Time.deltaTime;
        counter -= step;

        if (counter <= 0) Destroy(gameObject); // destroy this object when counter hits zero, otherwise
        else
        {
            lightSource.intensity = startInstensity * (counter / lifeTime); // fade light to zero

            //Reduce particle alpha with time
            Color color = particle.GetComponent<Renderer>().material.GetColor("_TintColor");
            color.a = initialAlpha * (counter / lifeTime);
            particle.GetComponent<Renderer>().material.SetColor("_TintColor", color);
        }

        if (moveToTarget)
        {
            Vector3 newPos = Vector3.MoveTowards(transform.position, targetObj.transform.position, moveSpeed * Time.deltaTime);
			transform.position = newPos;

			float distance = Vector3.Distance(transform.position, targetObj.transform.position);
			if(distance <= 0.05f) {
				GameObject player = GameObject.FindGameObjectWithTag("player");
				player.GetComponent<PlayerLight>().incrLightByPrcnt(0.1f);
                player.GetComponent<FirstPersonController>().ShrinkGunFunction();
				Destroy(gameObject);
                Debug.Log("player absorbed light!");
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
