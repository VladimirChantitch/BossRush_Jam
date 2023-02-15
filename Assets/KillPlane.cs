using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlane : MonoBehaviour
{
    [SerializeField] GameObject checkPoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.GetComponent<PlayerMovement>().gameObject.transform.position = checkPoint.transform.position;
    }
}
