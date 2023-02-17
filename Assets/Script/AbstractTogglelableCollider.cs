using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractTogglelableCollider : MonoBehaviour
{
    [SerializeField] protected Collider2D h_collider;
    [SerializeField] protected Vector3 pos;

    public virtual void Start()
    {
        h_collider = GetComponent<Collider2D>();
    }

    public virtual void OpenCollider()
    {
        h_collider.enabled = true;
    }

    public virtual void CloseCollider()
    {
        h_collider.enabled = false;
    }

}
