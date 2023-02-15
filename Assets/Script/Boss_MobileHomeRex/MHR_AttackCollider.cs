using UnityEngine;
using UnityEngine.Events;

public class MHR_AttackCollider : AbstractTogglelableCollider
{
    public UnityEvent<PlayerTakeDamageCollider> applyDamageToTarget;
    [SerializeField] private float damage;

    public override void Start()
    {
        base.Start();
        applyDamageToTarget.AddListener(target => target?.takeDamage?.Invoke(-damage));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 20)
        {
            applyDamageToTarget?.Invoke(collision.GetComponent<PlayerTakeDamageCollider>());
        }
    }
}
