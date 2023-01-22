using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class C_Boss_1_AttackCollider : AbstractTogglelableCollider
{
    public UnityEvent<AbstractCharacter> applyDamageToTarget;
    public bool isDestroyed;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 20)
        {
            applyDamageToTarget?.Invoke(other.GetComponent<AbstractCharacter>());
        }
    }

    public override void OpenCollider()
    {
        if (!isDestroyed)
        {
            base.OpenCollider();
        }
    }
}
