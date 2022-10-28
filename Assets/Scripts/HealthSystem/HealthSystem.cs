using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField]
    private int health = 100;


    // event called upon unit death
    public event EventHandler onUnitDeath;

    // function used to receive damage
    public void TakeDamage(int damageAmount_)
    {
        health -= damageAmount_;

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
}
