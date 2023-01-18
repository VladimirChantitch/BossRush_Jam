using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class C_BossAttackCollider : AbstractTogglelableCollider
{
    public UnityEvent<AbstractCharacter> applyDamageToTarget;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 20)
        {
            applyDamageToTarget?.Invoke(other.GetComponent<AbstractCharacter>());
        }
    }
}
