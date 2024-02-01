using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;

    public bool hasBeenDefeated = false;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>().currentHealth <= 0)
        {
            currentHealth = maxHealth;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            if (gameObject.CompareTag("Enemy"))
            {
                Die();
            }
            else if (gameObject.CompareTag("Enemy NPC"))
            {
                hasBeenDefeated = true;
            }
        }
    }

    public void Die()
    {
        gameObject.SetActive(false);
    }
}
