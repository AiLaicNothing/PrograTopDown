using UnityEngine;

public class FlyingEnemy : EnemyBase
{
    public float speed = 5f;
    public Transform target; // poner al player como target oki9s

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

    protected override void Dead()
    {
        base.Dead();
    }
}