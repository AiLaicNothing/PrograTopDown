using UnityEngine;

public abstract class EnemyBase : MonoBehaviour, IDamageable
{
    public float lifeMax = 100f;

    public float lifeAct;

    public float damage = 10f;

    public int scoreValue = 100;


    protected EnemySpawner spawner;

    protected virtual void Start()
    {
        lifeAct = lifeMax;
    }

    public void SetSpawner(EnemySpawner enemySpawner)
    {
        spawner = enemySpawner;
    }

    public virtual void TakeDamage(float damage)
    {
        lifeAct -= damage;

        if (lifeAct <= 0)
        {
            Dead();
        }
    }

    protected virtual void Dead()
    {
        GameManager.Instance.AddKill(scoreValue);

        if (spawner != null)
        {
            spawner.EnemyKilled();
        }

        Destroy(gameObject);
    }
}
