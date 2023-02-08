using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAttackCollider_Big : AbstractTogglelableCollider
{
    [SerializeField] private CircleCollider2D circleCollider;

    float timeElapsed;
    float lerpDuration = 1;

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
        if (circleCollider.enabled)
        {
            transform.localPosition = pos;
            EnbiggenCollider();
        }
    }

    public override void OpenCollider()
    {
        circleCollider.radius = 0.5f;
        timeElapsed = 0;
        circleCollider.enabled = true;
        EnbiggenCollider();
    }

    public override void CloseCollider()
    {
        circleCollider.radius = 0.5f;
        timeElapsed = 0;
        circleCollider.enabled = false;
    }


    private void EnbiggenCollider()
    {
            circleCollider.radius = Mathf.Lerp(0.5f, 50f, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
    }
}
