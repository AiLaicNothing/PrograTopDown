using UnityEngine;

public class MeeleeEnemy : EnemyBase
{
    [Header("Movimiento")]
    public float speed = 3f;
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float attackCooldown = 1.5f;

    [Header("Arma")]
    public GameObject weaponPrefab;
    public Transform weaponHolder;

    private Transform player;
    private float attackTimer;

    protected override void Start()
    {
        base.Start();

        // Buscar player
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        // Crear arma
        if (weaponPrefab != null && weaponHolder != null)
        {
            Instantiate(
                weaponPrefab,
                weaponHolder.position,
                weaponHolder.rotation,
                weaponHolder
            );
        }
    }

    void Update()
    {
        if (player == null) return;

        attackTimer -= Time.deltaTime;

        float distance = Vector3.Distance(transform.position, player.position);

        // Detectar jugador
        if (distance <= detectionRange)
        {
            // Mirar jugador
            transform.LookAt(player);

            // Perseguir
            if (distance > attackRange)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    player.position,
                    speed * Time.deltaTime
                );
            }
            else
            {
                Attack();
            }
        }
    }

    void Attack()
    {
        if (attackTimer > 0) return;

        attackTimer = attackCooldown;

        IDamageable damageable = player.GetComponent<IDamageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(damage);
        }

        Debug.Log("enemigo pego");
    }
}