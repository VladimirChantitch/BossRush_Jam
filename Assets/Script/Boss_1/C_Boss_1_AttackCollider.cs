using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_BossAttackCollider : MonoBehaviour
{
    [SerializeField] Collider collider;

    public void OpenCollider()
    {
        collider.enabled = true;
    }

    public void CloseCollider()
    {
        collider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        /// check if player collision damage layer
    }
}
