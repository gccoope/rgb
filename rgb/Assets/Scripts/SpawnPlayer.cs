using UnityEngine;
using System.Collections;

/*

    This class will spawn the player at this objects transform. (Like a spawn pad, etc.)

*/

[RequireComponent(typeof(Transform))] // require parent object to have a Transform component

public class SpawnPlayer : MonoBehaviour {

    [SerializeField] GameObject player; // the player prefab that will be spawned.

    public bool respawn = false; // when the player dies, respawn will toggle to "true," and reposition the player to the spawn point.
    Transform spawnPoint; // the position at which the player will spawn

	// Use this for initialization
	void Start () {
        //spawnPoint = GetComponent<Transform>(); // set spawnPoint to this object's Transform

        GameObject p = (GameObject)Instantiate(player, transform.position, transform.rotation);
        respawn = false;
    }
	
	// Update is called once per frame
	void Update () {

	    if(respawn) spawnPlayer();

	}

    public void spawnPlayer()
    {
        GameObject p = (GameObject)Instantiate(player, transform.position, transform.rotation);
        Transform pTransform = player.GetComponent<Transform>();
        Vector3 newPlayerPosit = new Vector3(pTransform.position.x, pTransform.position.y, pTransform.position.z);
        pTransform.position = newPlayerPosit;
        respawn = false;
    }
}
