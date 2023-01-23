using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class C_Boss_1_AttackCollider : AbstractTogglelableCollider
{
    public UnityEvent<PlayerTakeDamageCollider> applyDamageToTarget;
    public bool isDestroyed;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 20)
        {
            applyDamageToTarget?.Invoke(collision.GetComponent<PlayerTakeDamageCollider>());
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
