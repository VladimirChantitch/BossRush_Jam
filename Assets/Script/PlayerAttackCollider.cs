using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class PlayerAttackCollider : AbstractTogglelableCollider
{
    public UnityEvent<BossTakeDamageCollider> applyDamageToTarget;
    public bool isDestroyed;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 21)
        {
            applyDamageToTarget?.Invoke(collision.GetComponent<BossTakeDamageCollider>());
        }
    }

    private void Update()
    {
        if (h_collider.enabled)
        {
            transform.localPosition = pos;
        }
    }

    public override void OpenCollider()
    {
        transform.localPosition = pos;
        base.OpenCollider();
    }

    public override void CloseCollider()
    {
        transform.localPosition = pos;
        base.CloseCollider();
    }
}
