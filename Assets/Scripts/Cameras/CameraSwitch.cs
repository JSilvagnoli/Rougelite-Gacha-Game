using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public CinemachineFreeLook thirdPersonCamera;
    public Camera mainCharacterCamera;

    public GameObject currentCharacter = null;
    GameObject mcCamera;

    private void Start()
    {
        mcCamera = mainCharacterCamera.gameObject;
    }

    private void Update()
    {
        currentCharacter = GameObject.FindGameObjectWithTag("Character");

        if (currentCharacter != null)
        {
            mcCamera.SetActive(true);

            Transform characterTransform = currentCharacter.transform;

            thirdPersonCamera.m_Follow = characterTransform;
            thirdPersonCamera.m_LookAt = characterTransform;
        }
        else
        {            
            mcCamera.SetActive(false);
        }
    }
}