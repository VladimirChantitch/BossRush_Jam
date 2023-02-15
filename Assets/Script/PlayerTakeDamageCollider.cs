using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class PlayerTakeDamageCollider : AbstractTogglelableCollider
{
    public UnityEvent<float> takeDamage; 
}
