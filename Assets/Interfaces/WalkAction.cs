using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WalkAction : INodeAction
{
    private InstructionNode node;

    // We must provide this public propterty to satisfy the interface

    public WalkAction(InstructionNode node)
    {
        // This sets the private node field to the node provided in the constructor
        this.node = node;
    }

    // We must provide this public method to satisfy the interface
    public IEnumerator Execute()
    {
        string characterToMove = node.characterToPerformInstruction;
        string waypointToMoveTo = node.walkInformation.waypoint;
        float walkSpeed = node.walkInformation.walkSpeed;

        GameObject character = FindCharacterByName(characterToMove);
        GameObject waypoint = FindWayPointByName(waypointToMoveTo);

        if (character == null)
        {
            Debug.Log("Character not found");
        }

        // Move the character to the waypoint
        Transform transform = character.transform;
        Vector3 targetPosition = waypoint.transform.position;

        while (transform.position != targetPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, walkSpeed * Time.deltaTime);
            yield return null;
        }

        GameObject player = GameObject.FindGameObjectWithTag("Character");
        InteractWithNPC interactWithNPC = player.GetComponent<InteractWithNPC>();
        interactWithNPC.MoveToNextAction();
    }

    private GameObject FindCharacterByName(string name)
    {
        GameObject[] characters = GameObject.FindGameObjectsWithTag("NPC");
        foreach (GameObject character in characters)
        {
            if (character.name == name)
            {
                return character;
            }
        }
        return null;
    }

    private GameObject FindWayPointByName(string name)
    {
        GameObject[] wayPoints = GameObject.FindGameObjectsWithTag("Waypoint");
        foreach (GameObject waypoint in wayPoints)
        {
            if (waypoint.name == name)
            {
                return waypoint;
            }
        }
        return null;
    }
}
