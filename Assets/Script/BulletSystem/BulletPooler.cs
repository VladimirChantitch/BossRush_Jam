using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPooler : MonoBehaviour
{
    public bool lookingLeft;

    [System.Serializable]
    public class SpawnSetting
    {
        public int nbrBulletToSpawn;
        public float interval;
    }

    [System.Serializable]
    public class Pool
    {
        [Header("Pool")]
        public string tag;
        public GameObject prefab;
        public Bullet bullet;
        public int size;
        [Header("Spawn")]
        public Transform spawn;
        public SpawnSetting spawnSetting;
    }

    #region Singleton
    public static BulletPooler instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            SetSpawner(pool);
            SetBulletPrefab(pool);

            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    private void SetBulletPrefab(Pool pool)
    {
        Bullet bullet = pool.bullet;
        BulletBehavior bulletBehavior = pool.prefab.GetComponent<BulletBehavior>();
        bulletBehavior.tag = pool.tag;
        bulletBehavior.destroyWhenOutOfView = bullet.destroyWhenOutOfView;
        bulletBehavior.shotType = bullet.shotType;
        bulletBehavior.damage = bullet.damage;
        bulletBehavior.canPassWall = bullet.canPassWall;
        bulletBehavior.upForce = bullet.upForce;
        bulletBehavior.sideForce = bullet.sideForce;
        bulletBehavior.timeBeforeAutodestruct = bullet.timeBeforeAutodestruct;
        SetGravity(pool, bulletBehavior.shotType);
    }

    private void SetGravity(Pool pool, ShotType type)
    {
        if (ShotType.Curved == type || ShotType.Fountain == type)
            pool.prefab.GetComponent<Rigidbody2D>().gravityScale = 1;
        else
            pool.prefab.GetComponent<Rigidbody2D>().gravityScale = 0;
    }

    private void SetSpawner(Pool pool)
    {
        BulletSpawner spawn = pool.spawn.gameObject.GetComponent<BulletSpawner>();
        spawn.tag = pool.tag;
        if (lookingLeft) spawn.baseDir = -1; else spawn.baseDir = 1;
        spawn.nbrBulletToSpawn = pool.spawnSetting.nbrBulletToSpawn;
        spawn.interval = pool.spawnSetting.interval;
    }    

    public void ReturnToPool(string tag, GameObject obj)
    {
        poolDictionary[tag].Enqueue(obj);
        obj.SetActive(false);
    }
}
