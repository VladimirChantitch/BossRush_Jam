using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{

    private BulletPooler pooler;
    [HideInInspector] public string tag;
    [HideInInspector] public int nbrBulletToSpawn;
    [HideInInspector] public int baseDir;
    [HideInInspector] public int dir;

    [HideInInspector] public float interval;
    private float timer = 0f;
    private bool canShoot;

    private void Start()
    {
        pooler = BulletPooler.instance;
    }

    private void FixedUpdate()
    {
        timer += Time.deltaTime;
        if (timer >= interval)
        {
            canShoot = true;
            timer = 0f;
        }
        else
            canShoot = false;

        if (canShoot)
        {
            for (int i = 0; i < nbrBulletToSpawn; i++)
            {
                SpawnFromPool(tag, transform.position, Quaternion.identity, dir);
            }
        }
            
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation, int dir)
    {
        if (!pooler.poolDictionary.ContainsKey(tag))
            return null;

        GameObject objectToSpawn = pooler.poolDictionary[tag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        //potentiellement remonte le test case ici
        IPooledBullet pooledObj = objectToSpawn.GetComponent<IPooledBullet>();

        if (pooledObj != null)
        {
            pooledObj.OnObjectSpawn(gameObject, baseDir, dir);
        }

        pooler.poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }

}


