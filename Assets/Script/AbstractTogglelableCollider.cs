using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractTogglelableCollider : MonoBehaviour
{
    [SerializeField] Collider2D h_collider;

    private void Start()
    {
        h_collider = GetComponent<Collider2D>();
    }

    public virtual void OpenCollider()
    {
        Debug.Log($"<color=orange> Open collider of {gameObject.name} </color>");
        h_collider.enabled = true;
    }

    public virtual void CloseCollider()
    {
        Debug.Log($"<color=yellow> Close collider of {gameObject.name} </color>");
        h_collider.enabled = false;
    }

}
