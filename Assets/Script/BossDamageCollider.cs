using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossDamageCollider : AbstractTogglelableCollider
{
    public abstract void TakeDamage(float value = 0);
}
