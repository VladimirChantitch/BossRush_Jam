using UnityEngine;

public interface IPooledBullet
{
    void OnObjectSpawn(GameObject spawn, int baseDir, int dir);
}
