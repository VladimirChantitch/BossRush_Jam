using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BossDamageCollider : AbstractTogglelableCollider
{
    public abstract void TakeDamage(float value = 0);
    public GameObject parent;
    public UnityEvent<float> TakeDamageEvent;
}
