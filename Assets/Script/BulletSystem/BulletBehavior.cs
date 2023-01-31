using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using player;

public class BulletBehavior : MonoBehaviour ,IPooledBullet
{
    private BulletPooler pooler;
    [HideInInspector] public bool destroyWhenOutOfView = true;

    private Camera mainCamera;

    [HideInInspector] public ShotType shotType;
    [HideInInspector] public string tag;

    [HideInInspector] public float damage;
    [HideInInspector] public bool canPassWall = false;

    [HideInInspector] public float upForce = 1f;
    [HideInInspector] public float sideForce = .1f;
    [SerializeField]
    private Rigidbody2D rb;
    private Vector2 force;
    private static int spreadID = 0;

    //Stat Circle
    private float radius = 5f;
    private float angleStep;
    private static float angle;

    [HideInInspector] public float timeBeforeAutodestruct = -1.0f;
    protected float m_Timer;

    public UnityEvent<PlayerTakeDamageCollider> applyDamageToTarget;

    private void Start()
    {
        pooler = BulletPooler.instance;
        mainCamera = Camera.main;

        applyDamageToTarget.AddListener(target => target?.takeDamage?.Invoke(-damage));
    }

    private void FixedUpdate()
    {
        if (destroyWhenOutOfView)
        {
            Vector3 screenPoint = mainCamera.WorldToViewportPoint(transform.position);
            bool onScreen = screenPoint.z > 0 && screenPoint.x > -0.01f &&
                            screenPoint.x < 1 + 0.01f && screenPoint.y > -0.01f &&
                            screenPoint.y < 1 + 0.01f;
            if (!onScreen)
                pooler.ReturnToPool(tag, gameObject);
        }

        if (timeBeforeAutodestruct > 0)
        {
            m_Timer += Time.deltaTime;
            if (m_Timer > timeBeforeAutodestruct)
            {
                pooler.ReturnToPool(tag, gameObject);
            }
        }
    }

    public void OnObjectSpawn(GameObject spawn, int baseDir, int dir)
    {
        m_Timer = 0;
        switch (shotType)
        {
            case ShotType.Normal:
                force = new Vector2(sideForce * baseDir * dir, 0);
                rb.velocity = force;
                break;

            case ShotType.Curved:

                force = new Vector2(sideForce * baseDir * dir, upForce);
                rb.velocity = force;
                break;

            case ShotType.Fountain:
                float xForce = Random.Range(-sideForce, sideForce);
                float yForce = Random.Range(upForce / 2, upForce);

                force = new Vector2(xForce, yForce);

                rb.velocity = force;
                break;
            case ShotType.Spread:
                if (spreadID == 4)
                    spreadID = 0;
                else
                    spreadID++;
                switch(spreadID)
                {
                    case (0):
                        force = new Vector2(sideForce* 0.975f * baseDir * dir, 0.8f * upForce);
                        rb.velocity = force;
                        break;
                    case (1):
                        force = new Vector2(sideForce * 0.99f * baseDir * dir, 0.4f * upForce);
                        rb.velocity = force;
                        break;
                    case (2):
                        force = new Vector2(sideForce * baseDir * dir, 0f);
                        rb.velocity = force;
                        break;
                    case (3):
                        force = new Vector2(sideForce * 0.99f * baseDir * dir, -0.4f * upForce);
                        rb.velocity = force;
                        break;
                    case (4):
                        force = new Vector2(sideForce * 0.975f * baseDir * dir, -0.8f * upForce  );
                        rb.velocity = force;
                        break;
                }
                break;
            case ShotType.Circle:
                angleStep = 360f / spawn.GetComponent<BulletSpawner>().nbrBulletToSpawn;

                float projectileDirXposition = spawn.transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180) * radius;
                float projectileDirYposition = spawn.transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180) * radius;

                Vector2 projectileVector = new Vector2(projectileDirXposition, projectileDirYposition);
                Vector2 projectileMoveDirection = (projectileVector - new Vector2(spawn.transform.position.x, spawn.transform.position.y)).normalized * sideForce;

                rb.velocity =
                    new Vector2(projectileMoveDirection.x, projectileMoveDirection.y);

                angle += angleStep;
                break;

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Boss")
            return;

        if (collision.gameObject.layer == 20)
        {
            applyDamageToTarget?.Invoke(collision.GetComponent<PlayerTakeDamageCollider>());
            pooler.ReturnToPool(tag, gameObject);
            return;
        }

        if (!canPassWall)
        {
            pooler.ReturnToPool(tag, gameObject);
            return;
        }
    }
}
