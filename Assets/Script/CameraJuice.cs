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

    public void Shake(float duration, float magnitude)
    {
        StartCoroutine(CameraShake(duration, magnitude));
    }

    private IEnumerator CameraShake(float magnitude, float duration)
    {
        Vector3 originalPos = camTarget.localPosition;

        float elapsed = 0.0f;

        while(elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            camTarget.localPosition = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;
            yield return null;
        }
        Default();
    }

    public void Default()
    {
        camTarget.localPosition = defaultOffset;
    }
}
