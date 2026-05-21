using UnityEngine;

public class FlyingEnemy : EnemyBase
{
    [Header("Movimiento")]
    public float speed = 5f;

    [Header("Player")]
    public string playerTag = "Player";
    private Transform target;

    [Header("Ataque")]
    public float attackCooldown = 1f;
    private float lastAttackTime;

    protected override void Start()
    {
        base.Start();

        GameObject player = GameObject.FindGameObjectWithTag(playerTag);

        if (player != null)
        {
            target = player.transform;
        }
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if (target == null) return;

        Vector3 targetPos = target.position;

        targetPos.y += Mathf.Sin(Time.time * 2f) * 0.5f;

        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
    }

    private void OnTriggerStay(Collider other)
    {
        if (Time.time < lastAttackTime + attackCooldown) return;

        IDamageable damageable = other.GetComponent<IDamageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(damage);
            lastAttackTime = Time.time;
        }
    }

    protected override void Dead()
    {
        base.Dead();
    }
}