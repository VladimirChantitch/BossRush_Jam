using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using player;
using Unity.Mathematics;

public class CameraJuice : MonoBehaviour
{
    [SerializeField] PlayerManager playerManager;

    [SerializeField] Transform camTarget;
    public Vector3 defaultOffset;
    public Vector3 movementOffset;
    public Vector3 zoomOffset;

    public void MovementDezoom()
    {
        camTarget.localPosition = SetOffset(movementOffset);
    }

    public void DashZoom()
    {
        camTarget.localPosition = SetOffset(zoomOffset);
    }

    public void AtkZoom()
    {
        camTarget.localPosition = SetOffset(defaultOffset);
    }

    private float ComboModifier()
    {
        float combo = playerManager.GetStat(Boss.stats.StatsType.combo).Value;
        float maxCombo = playerManager.GetStat(Boss.stats.StatsType.combo).MaxValue;
        float modifier = math.remap(0, maxCombo, 0, 6, combo);
        return modifier;
    }

    private Vector3 SetOffset( Vector3 pos)
    {
        Vector3 offset = new Vector3(pos.x, pos.y, pos.z + ComboModifier());
        return offset;
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
            float x = UnityEngine.Random.Range(-1f, 1f) * magnitude;
            float y = UnityEngine.Random.Range(-1f, 1f) * magnitude;

            camTarget.localPosition = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;
            yield return null;
        }
        Default();
    }

    public void Default()
    {
        camTarget.localPosition = SetOffset(defaultOffset);
    }
}
