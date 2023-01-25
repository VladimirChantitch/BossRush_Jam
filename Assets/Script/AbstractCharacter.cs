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

    protected void SetHealth(float value)
    {
        health = value;
    }

    protected void SetMaxHealth(float value)
    {
        maxHealth = value;
    }
    /// <summary>
    /// take damage or heals character
    /// </summary>
    /// <param name="amout"></param>
    public virtual void AddDamage(float amount)
    {
        Debug.Log($"<color=purple> {gameObject.name} has taken {amount} damages </color>");
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

