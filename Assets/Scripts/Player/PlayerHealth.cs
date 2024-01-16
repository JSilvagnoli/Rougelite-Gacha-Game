using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;

    private bool isInvincible = false;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (!isInvincible)
        {
            currentHealth -= damage;

            if (currentHealth <= 0)
            {
                StartCoroutine(Die());
            }

            StartCoroutine(Invincibility());
        }
    }

    private IEnumerator Invincibility()
    {
        isInvincible = true;
        yield return new WaitForSeconds(1.0f);
        isInvincible = false;
    }

    public IEnumerator Die()
    {
        yield return new WaitForSeconds(0.1f);

        currentHealth = maxHealth;
    }
}
