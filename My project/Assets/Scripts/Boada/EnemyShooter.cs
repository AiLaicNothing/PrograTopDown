using UnityEngine;

public class EnemyShooter : EnemyBase
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float shootRate = 2f;
    [SerializeField] private float detectionRange = 10f;

    private float shootTimer;
    private Transform player;

    protected override void Start()
    {
        base.Start();

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("Player not found in scene");
        }
    }

    private void Update()
    {
        if (player == null)
            return;

        LookAtPlayer();

        shootTimer += Time.deltaTime;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= detectionRange && shootTimer >= shootRate)
        {
            Shoot();
            shootTimer = 0f;
        }
    }

    private void LookAtPlayer()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        transform.rotation = Quaternion.LookRotation(direction);
    }

    private void Shoot()
    {
        if (bulletPrefab == null || shootPoint == null)
        {
            Debug.LogError("BulletPrefab or ShootPoint is missing");
            return;
        }

        GameObject bullet = Instantiate(
            bulletPrefab,
            shootPoint.position,
            shootPoint.rotation
        );

        EnemyBullet enemyBullet = bullet.GetComponent<EnemyBullet>();

        if (enemyBullet != null)
        {
            Vector3 direction =
                (player.position - shootPoint.position).normalized;

            enemyBullet.SetDirection(direction);
        }
    }
}