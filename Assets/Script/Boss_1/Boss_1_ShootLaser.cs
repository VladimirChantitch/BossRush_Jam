using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_1_ShootLaser : MonoBehaviour
{
    [Header("refed Objects")]
    [SerializeField] LineRenderer right;
    [SerializeField] LineRenderer left;
    [SerializeField] Transform rightTarget;
    [SerializeField] Transform leftTarget;
    [SerializeField] C_Boss_1_LaserCollider laserRightCollider;
    [SerializeField] C_Boss_1_LaserCollider laserLeftCollider;


    Vector3[] rightPositions = new Vector3[2];
    Vector3[] leftPositions = new Vector3[2];

    [SerializeField] bool isOpen;

    private void Start()
    {
        right.enabled = false;
        left.enabled = false;
        right.useWorldSpace = true;
        left.useWorldSpace = true;
    }

    public void OpenLaser()
    {
        right.enabled = true;
        left.enabled = true;
        laserLeftCollider.OpenCollider();
        laserRightCollider.OpenCollider();
        isOpen = true;
    }

    public void SetUpEvents(float damageAmount)
    {
        laserRightCollider.applyDamageToTarget.AddListener(target => target.takeDamage?.Invoke(damageAmount));
        laserLeftCollider.applyDamageToTarget.AddListener(target => target.takeDamage?.Invoke(damageAmount));
    }

    private void Update()
    {
        if (isOpen)
        {
            rightPositions[0] = right.transform.position;
            rightPositions[1] = rightTarget.position;
            leftPositions[0] = left.transform.position;
            leftPositions[1] = leftTarget.position;

            right.SetPositions(rightPositions);
            left.SetPositions(leftPositions);

            laserRightCollider.UpdateCollider(rightPositions, right.endWidth);
            laserLeftCollider.UpdateCollider(leftPositions, left.endWidth);
        }
    }

    public void CloseLaser()
    {
        right.enabled = false;
        left.enabled = false;
        laserRightCollider.CloseCollider();
        laserLeftCollider.CloseCollider();
        isOpen = false;
    }
}
