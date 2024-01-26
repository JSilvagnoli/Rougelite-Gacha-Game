using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TeleportPlayerUp : MonoBehaviour
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
            TeleportToNextFloor();
        }
    }

    private void TeleportToNextFloor()
    {
        Vector3 teleportLocation = new Vector3(this.transform.position.x, this.transform.position.y + 250, this.transform.position.z - 900);
        player.transform.position = teleportLocation;
    }
}
