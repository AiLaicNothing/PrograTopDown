using UnityEngine;

public class FlyingEnemy : EnemyBase
{
    public float speed = 5f;
    public Transform target; // poner al player como target oki9s

    public float attackCooldown = 1f;
    private float lastAttackTime;

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if (target == null) return;

        Vector3 targetPos = target.position;

        targetPos.y += Mathf.Sin(Time.time * 2f) * 0.5f;

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPos,
            speed * Time.deltaTime
        );
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