using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    public int lifeMax = 100;
    public int lifeAct;
    public int damage = 10;

    protected virtual void Start()
    {
        lifeAct = lifeMax;
    }

    public virtual void RecibirDaño(float cantidad)
    {
        lifeAct -= (int)cantidad;

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
