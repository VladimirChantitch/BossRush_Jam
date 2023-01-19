using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractTogglelableCollider : MonoBehaviour
{
    [SerializeField] Collider h_collider;

    private void Start()
    {
        h_collider = GetComponent<Collider>();
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
