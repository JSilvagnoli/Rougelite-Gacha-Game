using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    private RoomTemplates roomTemplates;

    public int maxNumberOfRooms;
    public int numberOfRoomsSpawned;

    private Vector3 currentRoomPosition = Vector3.zero;
    private Vector3 newRoomPosition = Vector3.zero;

    GameObject newRoom = null;

    public bool healthRoomSpawned = false;
    public bool treasureRoomSpawned = false;
    public bool shopRoomSpawned = false;
    public bool weaponRoomSpawned = false;
    public bool abilityRoomSpawned = false;
    public bool bossRoomSpawned = false;

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
        int consecutiveEnemyRooms = 0;
        int maxConsecutiveEnemyRooms = Random.Range(4, 6);

        while (numberOfRoomsSpawned < maxNumberOfRooms)
        {
            if (Random.Range(0, 100) < 5)
            {
                newRoom = roomTemplates.treasureRoom;

                // 5% chance to spawn the treasure room
                newRoomPosition = new Vector3(currentRoomPosition.x, currentRoomPosition.y + 100, currentRoomPosition.z);
                Instantiate(newRoom, newRoomPosition, Quaternion.identity);

                currentRoomPosition = newRoomPosition;

                consecutiveEnemyRooms++;
            }
            else
            {
                // Spawn a special room
                if (consecutiveEnemyRooms > maxConsecutiveEnemyRooms)
                {
                    int rand = Random.Range(0, roomTemplates.specialRooms.Count);
                    newRoom = roomTemplates.specialRooms[rand];

                    // Spawn a new room at random
                    newRoomPosition = new Vector3(currentRoomPosition.x, currentRoomPosition.y + 100, currentRoomPosition.z);
                    Instantiate(newRoom, newRoomPosition, Quaternion.identity);

                    currentRoomPosition = newRoomPosition;

                    Debug.Log(newRoom.name + " spawned");

                    consecutiveEnemyRooms = 0;

                    maxConsecutiveEnemyRooms = Random.Range(3, 7);
                }
                // Spawn an enemy room
                else
                {
                    int rand = Random.Range(0, roomTemplates.enemyRooms.Length);
                    newRoom = roomTemplates.enemyRooms[rand];

                    // Spawn a new room at random
                    newRoomPosition = new Vector3(currentRoomPosition.x, currentRoomPosition.y + 100, currentRoomPosition.z);
                    Instantiate(newRoom, newRoomPosition, Quaternion.identity);

                    currentRoomPosition = newRoomPosition;

                    consecutiveEnemyRooms++;
                }
            }            

            numberOfRoomsSpawned++;
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