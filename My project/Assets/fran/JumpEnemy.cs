using UnityEngine;

public class JumpEnemy : EnemyBase
{
    [Header("Movimiento")]
    public float detectionRange = 10f;

    [Header("Salto")]
    public float jumpForce = 10f;
    public float jumpCooldown = 2f;

    private Transform player;
    private Rigidbody rb;

    private float jumpTimer;

    protected override void Start()
    {
        base.Start();

        rb = GetComponent<Rigidbody>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update()
    {
        if (player == null) return;

        jumpTimer -= Time.deltaTime;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= detectionRange)
        {
            transform.LookAt(player);

            if (jumpTimer <= 0)
            {
                JumpToPlayer();

                jumpTimer = jumpCooldown;
            }
        }
    }

    void JumpToPlayer()
    {
        Vector3 dir = (player.position - transform.position).normalized;

        // Reiniciar velocidad
        rb.linearVelocity = Vector3.zero;

        // Saltar hacia adelante y arriba
        Vector3 jump = new Vector3(
            dir.x * jumpForce,
            jumpForce,
            dir.z * jumpForce
        );

        rb.AddForce(jump, ForceMode.Impulse);

        Debug.Log("enemigo salto");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            IDamageable damageable =
                collision.gameObject.GetComponent<IDamageable>();

            if (damageable != null)
            {
                damageable.TakeDamage(damage);

                Debug.Log("enemigo hizo daño");
            }
        }
    }
}