using UnityEngine;

public class Player_Bullet : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private float lifeTime = 3f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            IDamageable damageable = other.GetComponent<IDamageable>();

            if (damageable != null)
            {
                damageable.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}
