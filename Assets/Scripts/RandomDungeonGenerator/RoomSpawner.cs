using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    private RoomTemplates roomTemplates;
    private RoomInformation roomInformation;

    public int maxNumberOfRooms;
    public int numberOfRoomsSpawned;

    private Vector3 currentRoomPosition = Vector3.zero;
    private Vector3 newRoomPosition = Vector3.zero;

    GameObject newRoom = null;

    private bool opening;

    private void Start()
    {
        maxNumberOfRooms = Random.Range(22, 30);

        roomTemplates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();

        // Spawn the starting room
        GameObject startingRoom = Instantiate(roomTemplates.startingRoom, transform.position, Quaternion.identity);

        currentRoomPosition = startingRoom.transform.position;

        Spawn();

        SpawnBossRoom();
    }

    private void Spawn()
    {
        int maxConsecutiveEnemyRooms = Random.Range(4, 6);

        while (numberOfRoomsSpawned < maxNumberOfRooms)
        {
            if (Random.Range(0, 100) < 30)
            {
                // Spawn a room with an opening for special room
                int rand = Random.Range(0, roomTemplates.enemyRoomsWithOpenings.Length);
                newRoom = roomTemplates.enemyRoomsWithOpenings[rand];
                opening = true;
            }
            else
            {
                // Spawn a normal top bottom enemy room
                int rand = Random.Range(0, roomTemplates.enemyRooms.Length);
                newRoom = roomTemplates.enemyRooms[rand];
            }

            // Spawn a new room
            newRoomPosition = new Vector3(currentRoomPosition.x, currentRoomPosition.y + 100, currentRoomPosition.z);
            Instantiate(newRoom, newRoomPosition, Quaternion.identity);

            currentRoomPosition = newRoomPosition;

            numberOfRoomsSpawned++;

            if (opening)
            {
                GameObject[] newRoomSpawnPoints = newRoom.transform.GetComponentsInChildren<Transform>()
                    .Where(child => child.CompareTag("SpawnPoint"))
                    .Select(child => child.gameObject)
                    .ToArray();

                foreach (GameObject spawnPoint in newRoomSpawnPoints)
                {
                    roomInformation = spawnPoint.GetComponent<RoomInformation>();
                    ChooseRoom();
                }
            }

            opening = false;
        }
    }

    private void ChooseRoom()
    {
        if (roomInformation.openingDirection == 1)
        {
            if (Random.Range(0, 100) < 5)
            {
                newRoomPosition = new Vector3(currentRoomPosition.x + 100, currentRoomPosition.y, currentRoomPosition.z);
                Instantiate(roomTemplates.treasureRoomLeft, newRoomPosition, Quaternion.identity);
            }
            else
            {
                int rand = Random.Range(0, roomTemplates.specialRoomsLeft.Length);
                newRoom = roomTemplates.specialRoomsLeft[rand];

                newRoomPosition = new Vector3(currentRoomPosition.x + 100, currentRoomPosition.y, currentRoomPosition.z);
                Instantiate(newRoom, newRoomPosition, Quaternion.identity);
            }
        }
        else
        {
            if (Random.Range(0, 100) < 5)
            {
                newRoomPosition = new Vector3(currentRoomPosition.x - 100, currentRoomPosition.y, currentRoomPosition.z);
                Instantiate(roomTemplates.treasureRoomRight, newRoomPosition, Quaternion.identity);
            }
            else
            {
                int rand = Random.Range(0, roomTemplates.specialRoomsRight.Length);
                newRoom = roomTemplates.specialRoomsRight[rand];

                newRoomPosition = new Vector3(currentRoomPosition.x - 100, currentRoomPosition.y, currentRoomPosition.z);
                Instantiate(newRoom, newRoomPosition, Quaternion.identity);
            }
        }
    }

    private void SpawnBossRoom()
    {
        newRoom = roomTemplates.bossRoom;

        // Spawn a new room at random
        Vector3 newRoomPosition = new Vector3(currentRoomPosition.x, currentRoomPosition.y + 100, currentRoomPosition.z);
        Instantiate(newRoom, newRoomPosition, Quaternion.identity);
    }
}