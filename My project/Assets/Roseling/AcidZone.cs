using UnityEngine;

public class AcidZone : MonoBehaviour
{
    public float acidDamage = 5f;

    public float attackCooldown = 1f;
    private float lastAttackTime;

    public float destroyTime = 10f;

    private void Start()
    {
        Destroy(gameObject, destroyTime);
    }

    private void OnTriggerStay(Collider other)
    {
        if (Time.time < lastAttackTime + attackCooldown) return;

        IDamageable damageable = other.GetComponent<IDamageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(acidDamage);

            lastAttackTime = Time.time;
        }
    }

}
