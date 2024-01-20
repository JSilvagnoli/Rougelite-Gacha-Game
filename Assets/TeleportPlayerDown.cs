using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPlayerDown : MonoBehaviour
{
    public GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Character");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Character"))
        {
            TeleportToPreviousFloor();
        }
    }

    private void TeleportToPreviousFloor()
    {
        Debug.Log("Teleporting player down");
        Vector3 teleportLocation = new Vector3(this.transform.position.x, this.transform.position.y - 100, this.transform.position.z + 100);
        player.transform.position = teleportLocation;
    }
}
