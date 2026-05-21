using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MiniBoss2 : EnemyBase
{
    [Header("Movement")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float stopDistance = 10f;

    [Header("Projectile")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float bulletSpeed = 12f;

    [Header("Cone Attack")]
    [SerializeField] private int coneShots = 5;
    [SerializeField] private float coneAngle = 45f;
    [SerializeField] private float coneDelay = 0.15f;

    [Header("Spiral Attack")]
    [SerializeField] private int spiralShots = 40;
    [SerializeField] private float spiralAngleStep = 20f;
    [SerializeField] private float spiralDelay = 0.05f;

    [Header("Attack Timing")]
    [SerializeField] private float attackCooldown = 2f;

    private NavMeshAgent agent;
    private Rigidbody rb;

    private Transform player;

    private bool isAttacking;

    protected override void Start()
    {
        base.Start();

        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();

        agent.speed = speed;

        if (rb != null)
        {
            rb.isKinematic = true;
        }

        GameObject target = GameObject.FindGameObjectWithTag("Player");

        if (target != null)
        {
            player = target.transform;
        }

        StartCoroutine(AI());
    }

    private IEnumerator AI()
    {
        while (true)
        {
            if (player == null)
            {
                yield return null;
                continue;
            }

            if (!isAttacking)
            {
                float distance =
                    Vector3.Distance(transform.position, player.position);

                // Follow player
                if (distance > stopDistance)
                {
                    agent.isStopped = false;
                    agent.SetDestination(player.position);
                }
                else
                {
                    agent.isStopped = true;

                    Vector3 lookDir =
                        (player.position - transform.position).normalized;

                    lookDir.y = 0;

                    if (lookDir != Vector3.zero)
                    {
                        transform.rotation =
                            Quaternion.LookRotation(lookDir);
                    }

                    int randomAttack = Random.Range(0, 2);

                    if (randomAttack == 0)
                    {
                        StartCoroutine(ConeAttack());
                    }
                    else
                    {
                        StartCoroutine(SpiralAttack());
                    }
                }
            }

            yield return null;
        }
    }

    // =========================================================
    // CONE ATTACK
    // =========================================================
    private IEnumerator ConeAttack()
    {
        isAttacking = true;

        Vector3 baseDirection =
            (player.position - transform.position).normalized;

        baseDirection.y = 0;

        float startAngle = -coneAngle * 0.5f;

        for (int i = 0; i < coneShots; i++)
        {
            float angle =
                startAngle + (coneAngle / (coneShots - 1)) * i;

            Vector3 dir =
                Quaternion.Euler(0, angle, 0) * baseDirection;

            Shoot(dir);

            yield return new WaitForSeconds(coneDelay);
        }

        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;
    }

    // =========================================================
    // SPIRAL ATTACK
    // Shoots one bullet direction each time while rotating
    // =========================================================
    private IEnumerator SpiralAttack()
    {
        isAttacking = true;

        float currentAngle = 0f;

        for (int i = 0; i < spiralShots; i++)
        {
            Vector3 dir =
                Quaternion.Euler(0, currentAngle, 0) * transform.forward;

            Shoot(dir);

            currentAngle += spiralAngleStep;

            transform.rotation =
                Quaternion.LookRotation(dir);

            yield return new WaitForSeconds(spiralDelay);
        }

        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;
    }

    // =========================================================
    // SHOOT
    // =========================================================
    private void Shoot(Vector3 direction)
    {
        Vector3 spawnPos = shootPoint.position;

        GameObject bullet =
            Instantiate(
                bulletPrefab,
                spawnPos,
                Quaternion.LookRotation(direction)
            );

        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

        if (bulletRb != null)
        {
            bulletRb.linearVelocity =
                direction.normalized * bulletSpeed;
        }
    }

    protected override void Dead()
    {
        base.Dead();

        StopAllCoroutines();
    }
}
