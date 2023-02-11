using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class C_Boss_1_AttackCollider : AbstractTogglelableCollider
{
    public UnityEvent<PlayerTakeDamageCollider> applyDamageToTarget;
    public bool isDestroyed;
    [SerializeField] bool hasRespawned; 
    [SerializeField] TrailRenderer trailRenderer;
    [SerializeField] BulletSpawner bulletSpawner;
    [SerializeField] int time = 1000;
    [SerializeField] int duration = 500;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 20)
        {
            applyDamageToTarget?.Invoke(collision.GetComponent<PlayerTakeDamageCollider>());
        }
    }

    private void Update()
    {
        if(!hasRespawned && !isDestroyed)
        {
            RespawnAction();
        }
    }

    private async void RespawnAction()
    {
        await Task.Delay(time);
        hasRespawned = true;
        if(bulletSpawner != null)
        {
            bulletSpawner.enabled = true;
            await Task.Delay(duration);
            bulletSpawner.enabled = false;
        }
    }

    public override void OpenCollider()
    {
        if (!isDestroyed)
        {
            trailRenderer.enabled = true;
            base.OpenCollider();
        }
        else
        {
            hasRespawned = false;
        }
    }

    public override void CloseCollider()
    {
        base.CloseCollider();
        trailRenderer.enabled = false;
    }
}
