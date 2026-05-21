using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Boss : EnemyBase
{
    [Header("MovePoints")]
    [Tooltip("0-3 = corners | 4 = center")]
    [SerializeField] private Transform[] movePoints;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.5f;

    [Header("Projectile")]
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float projectileSpeed = 10f;

    [Header("Pattern 1 - Cone Shoot")]
    [SerializeField] private int coneShots = 5;
    [SerializeField] private float coneAngle = 45f;
    [SerializeField] private float coneShootDelay = 0.2f;

    [Header("Pattern 2 - Spin Shoot")]
    [SerializeField] private int spinShots = 24;
    [SerializeField] private float spinAngleStep = 15f;
    [SerializeField] private float spinShootDelay = 0.1f;

    private NavMeshAgent agent;
    private Rigidbody rb;

    private Transform player;

    protected override void Start()
    {
        base.Start();

        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();

        agent.speed = moveSpeed;
        agent.updateRotation = true;
        agent.updateUpAxis = true;

        // Recommended when using NavMeshAgent
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        GameObject target = GameObject.FindGameObjectWithTag("Player");

        if (target != null)
        {
            player = target.transform;
        }

        StartCoroutine(BossLoop());
    }

    private IEnumerator BossLoop()
    {
        // Initial move
        yield return MoveToRandomCorner();

        while (true)
        {
            int randomPattern = Random.Range(0, 3);

            switch (randomPattern)
            {
                case 0:
                    yield return Pattern1();
                    break;

                case 1:
                    yield return Pattern2();
                    break;

                case 2:
                    yield return Pattern3();
                    break;
            }

            yield return new WaitForSeconds(1f);
        }
    }

    // =========================================================
    // PATTERN 1
    // Move to random corner and shoot cone toward center
    // =========================================================
    private IEnumerator Pattern1()
    {
        yield return MoveToRandomCorner();

        Vector3 centerDirection =
            (movePoints[4].position - transform.position).normalized;

        centerDirection.y = 0;

        // Look toward center
        transform.rotation = Quaternion.LookRotation(centerDirection);

        float startAngle = -coneAngle * 0.5f;

        for (int i = 0; i < coneShots; i++)
        {
            float angle =
                startAngle + (coneAngle / (coneShots - 1)) * i;

            Vector3 dir =
                Quaternion.Euler(0, angle, 0) * transform.forward;

            Shoot(dir);

            yield return new WaitForSeconds(coneShootDelay);
        }
    }

    // =========================================================
    // PATTERN 2
    // Move to center and spin shooting
    // =========================================================
    private IEnumerator Pattern2()
    {
        yield return MoveToPoint(movePoints[4].position);

        float currentAngle = 0f;

        for (int i = 0; i < spinShots; i++)
        {
            transform.rotation =
                Quaternion.Euler(0, currentAngle, 0);

            Shoot(transform.forward);

            currentAngle += spinAngleStep;

            yield return new WaitForSeconds(spinShootDelay);
        }
    }

    // =========================================================
    // PATTERN 3
    // Dash toward player
    // =========================================================
    private IEnumerator Pattern3()
    {
        if (player == null)
            yield break;

        agent.enabled = false;

        Vector3 dashDirection =
            (player.position - transform.position).normalized;

        dashDirection.y = 0;

        transform.rotation = Quaternion.LookRotation(dashDirection);

        float timer = 0f;

        while (timer < dashDuration)
        {
            transform.position +=
                dashDirection * dashSpeed * Time.deltaTime;

            timer += Time.deltaTime;

            yield return null;
        }

        agent.enabled = true;

        // Sync navmesh position
        agent.Warp(transform.position);
    }

    // =========================================================
    // MOVE
    // =========================================================
    private IEnumerator MoveToRandomCorner()
    {
        int randomIndex = Random.Range(0, 4);

        yield return MoveToPoint(movePoints[randomIndex].position);
    }

    private IEnumerator MoveToPoint(Vector3 target)
    {
        if (!agent.enabled)
        {
            agent.enabled = true;
        }

        agent.isStopped = false;

        agent.SetDestination(target);

        while (true)
        {
            if (!agent.pathPending)
            {
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    break;
                }
            }

            yield return null;
        }

        agent.isStopped = true;
    }

    // =========================================================
    // SHOOT
    // =========================================================
    private void Shoot(Vector3 direction)
    {
        Vector3 spawnPos = shootPoint.position;

        GameObject bullet =
            Instantiate(
                projectile,
                spawnPos,
                Quaternion.LookRotation(direction)
            );

        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

        if (bulletRb != null)
        {
            bulletRb.linearVelocity =direction.normalized * projectileSpeed;
        }
    }

    protected override void Dead()
    {
        base.Dead();

        StopAllCoroutines();
    }
}
