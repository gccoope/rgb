using UnityEngine;
using System.Collections;

public class Global_Data : MonoBehaviour {

    GameObject player;

	// Use this for initialization
	void Start () {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        player = null; // when this start function is called, the player might not have spawned yet
    }
	
	// Update is called once per frame
	void Update () {
        if (player == null) player = GameObject.FindGameObjectWithTag("player");

        // respawn if player has fall out of the world
        if(player.transform.position.y < -50)
        {
            Debug.Log("player respawn!");
            GameObject spawnPad = GameObject.FindGameObjectWithTag("spawn");
            Destroy(player);
            spawnPad.GetComponent<SpawnPlayer>().spawnPlayer();
        }
	}
}
