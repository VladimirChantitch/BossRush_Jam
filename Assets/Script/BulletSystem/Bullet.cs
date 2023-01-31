using UnityEngine;
using UnityEngine.Events;
using player;

[System.Serializable]
public class Bullet //: IPooledBullet
{
    public bool destroyWhenOutOfView = true;

    public ShotType shotType;

    public float damage;
    public bool canPassWall = false;

    public float upForce = 1f;
    public float sideForce = .1f;

    [Tooltip("If -1 never auto destroy, otherwise bullet is return to pool when that time is reached")]
    public float timeBeforeAutodestruct = -1.0f;
}
