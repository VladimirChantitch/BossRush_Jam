using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraJuice : MonoBehaviour
{
    [SerializeField] Transform camTarget;
    public Vector3 defaultOffset;
    public Vector3 movementOffset;
    public Vector3 zoomOffset;

    public void MovementDezoom()
    {
        camTarget.localPosition = movementOffset;
    }

    public void DashZoom()
    {
        camTarget.localPosition = zoomOffset;
    }

    public void Default()
    {
        camTarget.localPosition = defaultOffset;
    }
}
