using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject startingCharacter;
    public GameObject ethanMagus;
    public GameObject summoningBox;
    public GameObject dialogueBox;

    private void Start()
    {
        dialogueBox.SetActive(false);
        summoningBox.SetActive(false);

        Instantiate(startingCharacter, new Vector3(0, -50, -500), Quaternion.identity);
        Instantiate(ethanMagus, new Vector3(0, -50, -400), Quaternion.identity);

        Transform ethanInteractButton = ethanMagus.transform.Find("Interact Button");
        if (ethanInteractButton != null)
        {
            ethanInteractButton.gameObject.SetActive(false);
        }
        
    }
}
