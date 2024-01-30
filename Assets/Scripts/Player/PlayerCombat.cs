using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    public Transform attackPoint;

    public float attackRange = 0.5f;

    private bool hasAttacked = false;

    public LayerMask enemyLayer;

    private EnemyHealth enemyHealth;
    //public Dialogue dialogue;

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started && !hasAttacked)
        {
            hasAttacked = true;

            Attack();
        }
        else if (context.canceled)
        {
            StartCoroutine(AttackDelay());
        }
    }

    private void Attack()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);

        foreach (Collider enemy in hitEnemies)
        {
            enemyHealth = enemy.GetComponent<EnemyHealth>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(1);
            }
        }
    }

    private IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(0.5f);

        hasAttacked = false;
    }
}
