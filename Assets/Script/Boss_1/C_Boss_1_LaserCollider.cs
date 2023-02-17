using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class C_Boss_1_LaserCollider : C_Boss_1_AttackCollider
{
    List<Vector2> colliderPoints = new List<Vector2>();

    [SerializeField] PolygonCollider2D polygonCollider2D;

    public void UpdateCollider(Vector3[] positions, float width)
    {
        Vector2[] pos = new Vector2[(int)positions.Length];
        for(int i = 0; i < positions.Length; i++)
        {
            pos[i] = new Vector2(positions[i].x, positions[i].y);
        }
        colliderPoints = CalculateColliderPoints(pos, width);
        polygonCollider2D.SetPath(0, colliderPoints.ConvertAll(p => (Vector2)transform.InverseTransformPoint(p)));
    }

    private List<Vector2> CalculateColliderPoints(Vector2[] positions, float width)
    {
        float slope = (positions[1].y - positions[0].y)/ (positions[1].x - positions[0].x);
        float deltaX = (width / 2f) * (slope / Mathf.Pow(slope * slope + 1, 0.5f));
        float deltaY = (width / 2f) * (1 / Mathf.Pow(1 + slope * slope, 0.5f));

        Vector2[] offsets = new Vector2[2];
        offsets[0] = new Vector2(-deltaX, deltaY);
        offsets[1] = new Vector2(deltaX, -deltaY);

        List<Vector2> points = new List<Vector2>
        {
            positions[0] + offsets[0],
            positions[1] + offsets[0],
            positions[1] + offsets[1],
            positions[0] + offsets[1]
        };

        return points;
    }

    public override void OpenCollider()
    {
        polygonCollider2D.enabled = true;
    }

    public override void CloseCollider()
    {
        polygonCollider2D.enabled = false;
    }
}
