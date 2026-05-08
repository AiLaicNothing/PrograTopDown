using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    public float lifeMax = 100f;
    public float lifeAct;
    public float damage = 10f;

    protected virtual void Start()
    {
        lifeAct = lifeMax;
    }

    public virtual void RecibirDaño(float cantidad)
    {
        lifeAct -= cantidad;

        if (lifeAct <= 0)
        {
            Dead();
        }
    }

    protected virtual void Dead()
    {

        Destroy(gameObject);
    }
}
