using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractCharacter : MonoBehaviour
{

    [SerializeField] private float health;
    [SerializeField] private float maxHealth;
    public float Health { get => health; }    
    public float MaxHealth { get => maxHealth; }
    public string name { get; private set; }

    /// <summary>
    /// take damage or heals character
    /// </summary>
    /// <param name="amout"></param>
    public virtual void AddDamage(float amount)
    {
        health += amount;
        if (health <= 0) 
        {
            health = 0;
        }

        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }
}

