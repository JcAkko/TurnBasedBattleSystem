using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField]
    private int health;

    [SerializeField]
    private int maxHealth = 100;


    // event called upon unit death
    public event EventHandler onUnitDeath;

    // event called upon unit health damaged
    public event EventHandler onUnitDamaged;


    private void Awake()
    {
        health = maxHealth;
    }



    // function used to receive damage
    public void TakeDamage(int damageAmount_)
    {
        // decrement the health
        health -= damageAmount_;

        // call damaged event
        onUnitDamaged?.Invoke(this, EventArgs.Empty);

        if (health < 0)
        {
            health = 0;
        }

        // check if unit dead
        if (health <= 0)
        {
            // call die function
            Death();
        }

        Debug.Log(health);
    }


    // death event
    private void Death()
    {
        onUnitDeath?.Invoke(this, EventArgs.Empty);
    }


    // function used to return a normalized number of the health for the UI to use
    public float GetHealthPercentage()
    {
        return (float)health / maxHealth;
    }

}
