using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using UnityEngine.TextCore.Text;

public class WalkAction : INodeAction
{
    private InstructionNode node;

    private string[] waypointsToMoveTo;
    private string[] charactersToMove;

    private GameObject[] characters;
    private GameObject[] waypoints;

    private bool[] characterReachedWaypoint;

    private float walkSpeed;

    public WalkAction(InstructionNode node)
    {
        // This sets the private node field to the node provided in the constructor
        this.node = node;
    }

    // We must provide this public method to satisfy the interface
    public IEnumerator Execute()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Character");
        InteractWithNPC interactWithNPC = player.GetComponent<InteractWithNPC>();

        interactWithNPC.canSkip = false;

        waypointsToMoveTo = node.walkInformation.waypoints;
        charactersToMove = node.walkInformation.charactersToMove;
        walkSpeed = node.walkInformation.walkSpeed;

        characters = new GameObject[charactersToMove.Length];
        waypoints = new GameObject[waypointsToMoveTo.Length];

        characterReachedWaypoint = new bool[charactersToMove.Length];

        // Retrieve references to characters and waypoints
        for (int i = 0; i < charactersToMove.Length; i++)
        {
            characters[i] = FindCharacterByName(charactersToMove[i]);
            characterReachedWaypoint[i] = false;
        }

        for (int i = 0; i < node.walkInformation.waypoints.Length; i++)
        {
            waypoints[i] = FindCharacterByName(waypointsToMoveTo[i]);
        }

        // Ensure we have characters and waypoints to move to
        if (characters.Length == 0 || waypoints.Length == 0 || characters.Length != waypoints.Length)
        {
            Debug.LogError("Invalid number of characters or waypoints.");
            yield break;
        }

        // Move each character to its respective waypoint simultaneously
        while (!AllCharactersReachedWaypoints())
        {
            for (int i = 0; i < characters.Length; i++)
            {
                if (!characterReachedWaypoint[i])
                {
                    MoveCharacter(characters[i], waypoints[i]);
                }
            }
            yield return null;
        }

        // Once all characters have reached their waypoints, proceed to the next action
        Debug.Log("All characters reached their destinations");
        interactWithNPC.MoveToNextAction();
    }

    private GameObject FindCharacterByName(string name)
    {
        GameObject character = GameObject.Find(name);

        if (character == null)
        {
            Debug.LogError("Character with name " + name + " not found in the scene.");
            return null;
        }

        return character;
    }

    private GameObject FindWayPointByName(string name)
    {
        GameObject waypoint = GameObject.Find(name);

        if (waypoint == null)
        {
            Debug.LogError("Waypoint with name " + waypoint + " not found in the scene.");
            return null;
        }

        return waypoint;
    }

    private void MoveCharacter(GameObject character, GameObject waypoint)
    {
        Transform transform = character.transform;
        Vector3 targetPosition = waypoint.transform.position;

        if (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, walkSpeed * Time.deltaTime);
        }
        else
        {
            MarkCharacterReachedWaypoint(character);
        }
    }

    private void MarkCharacterReachedWaypoint(GameObject character)
    {
        for (int i = 0; i < characters.Length; i++)
        {
            if (characters[i] == character)
            {
                characterReachedWaypoint[i] = true;
                break;
            }
        }
    }

    private bool AllCharactersReachedWaypoints()
    {
        foreach (bool reached in characterReachedWaypoint)
        {
            if (!reached)
            {
                return false;
            }
        }
        return true;
    }
}
