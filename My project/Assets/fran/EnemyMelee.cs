using UnityEngine;

public class EnemyMelee : EnemyBase
{
    public float rotationSpeed = 200f;
    public float speed = 3f;
    public float detectionDistance = 10f;

    public GameObject blade;

    private bool attacking;
    private bool playerDetected;

    private Transform player;

    private Rigidbody rb;

    protected override void Start()
    {
        base.Start();

        attacking = false;

        rb = GetComponent<Rigidbody>();

        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    private void Update()
    {
        DetectPlayer();

        // solo sigue si ya detectó
        if (playerDetected)
        {
            FollowPlayer();
        }

        // gira espada
        if (attacking)
        {
            blade.transform.Rotate(
                0,
                rotationSpeed * Time.deltaTime,
                0
            );
        }
    }

    void DetectPlayer()
    {
        RaycastHit hit;

        Vector3 origin =
            transform.position + Vector3.up;

        // raycast hacia el frente REAL del enemigo
        Debug.DrawRay(
            origin,
            transform.forward * detectionDistance,
            Color.red
        );

        if (Physics.Raycast(
            origin,
            transform.forward,
            out hit,
            detectionDistance))
        {
            if (hit.collider.CompareTag("Player"))
            {
                playerDetected = true;

                Debug.Log("Player detectado");
            }
        }
    }

    void FollowPlayer()
    {
        // dirección hacia el jugador
        Vector3 direction =
            (player.position - transform.position).normalized;

        direction.y = 0;

        // rotación suave hacia el jugador
        Quaternion lookRotation =
            Quaternion.LookRotation(direction);

        transform.rotation =
            Quaternion.Slerp(
                transform.rotation,
                lookRotation,
                5f * Time.deltaTime
            );

        // mover rigidbody
        rb.MovePosition(
            transform.position +
            direction * speed * Time.deltaTime
        );
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            attacking = true;

            IDamageable damageable =
                other.GetComponent<IDamageable>();

            if (damageable != null)
            {
                damageable.TakeDamage(damage);

                Debug.Log("Enemy hit player");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            attacking = false;
        }
    }
}