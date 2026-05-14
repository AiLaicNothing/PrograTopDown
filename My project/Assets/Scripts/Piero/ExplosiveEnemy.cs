using UnityEngine;

public class ExplosiveEnemy :  EnemyBase
{
    private float explosionRadius;
    
    [SerializeField] private int speed;

    private int recoil;
    private GameObject player;


    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        recoil = 6;
        explosionRadius = 8;
    }

    private void Update()
    {
        if (player != null)
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Bullet"))
        {
            TakeDamage(20f);
            Debug.Log("Enemy took damage from bullet!");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Explosive();
        }
    }

    private void Explosive()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider collider in hitColliders)
        {
            IDamageable explosiveEnemyEffect = collider.gameObject.GetComponent<IDamageable>();
            if (explosiveEnemyEffect != null)
            {
                explosiveEnemyEffect.TakeDamage(damage);
            }

            Rigidbody rb = collider.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 explosionDirection = (collider.transform.position - transform.position).normalized;
                rb.AddForce(explosionDirection * recoil, ForceMode.Impulse);
            }
        } 
        Dead();
        
    }
    protected override void Dead()
    {
        Destroy(gameObject);
        Debug.Log("The explosive enemy is dead!");
    }
}