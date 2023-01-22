using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BossTakeDamageCollider : AbstractTogglelableCollider
{
    public GameObject parent;
    public UnityEvent<float> TakeDamageEvent;
}
