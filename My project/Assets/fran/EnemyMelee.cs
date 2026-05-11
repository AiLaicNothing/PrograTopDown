using UnityEngine;

public class EnemyMelee : EnemyBase
{
    // velocidad de giro de la espada
    public float rotationSpeed = 200f;

    // distancia máxima de detección
    public float detectionDistance = 10f;

    // referencia a la espada
    public GameObject blade;

    // verifica si está atacando
    private bool attacking;

    // verifica si detectó al jugador
    private bool playerDetected;

    // referencia al jugador
    private Transform player;

    // agregué referencia al script Movement
    // porque me di cuenta que EnemyBase
    // no tenía movimiento integrado
    // y preferí separar movimiento y combate
    private Movement movement;

    protected override void Start()
    {
        // llama al Start del EnemyBase
        base.Start();

        attacking = false;

        // agregué esto para obtener
        // el componente Movement del enemigo
        movement = GetComponent<Movement>();

        // busca el objeto con tag "Player"
        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        // si existe
        if (playerObject != null)
        {
            // guarda su transform
            player = playerObject.transform;
        }
    }

    private void Update()
    {
        // intenta detectar al jugador
        DetectPlayer();

        // si detectó al jugador
        if (playerDetected)
        {
            FollowPlayer();
        }

        // si está atacando
        if (attacking)
        {
            // gira la espada
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

        // origen del raycast
        Vector3 origin =
            transform.position + Vector3.up;

        // dibuja el raycast rojo
        Debug.DrawRay(
            origin,
            transform.forward * detectionDistance,
            Color.red
        );

        // lanza raycast hacia el frente del enemigo
        if (Physics.Raycast(
            origin,
            transform.forward,
            out hit,
            detectionDistance))
        {
            // si detecta al jugador
            if (hit.collider.CompareTag("Player"))
            {
                playerDetected = true;

                // agregué esto para activar
                // el movimiento solo cuando
                // el enemigo detecta al player
                movement.canMove = true;

                Debug.Log("Player detectado");
            }
        }
    }

    void FollowPlayer()
    {
        // calcula dirección hacia el jugador
        Vector3 direction =
            (player.position - transform.position).normalized;

        // evita inclinaciones raras
        direction.y = 0;

        // rota suavemente hacia el jugador
        Quaternion lookRotation =
            Quaternion.LookRotation(direction);

        transform.rotation =
            Quaternion.Slerp(
                transform.rotation,
                lookRotation,
                5f * Time.deltaTime
            );

        // eliminé el movimiento de aquí
        // porque ahora Movement.cs
        // se encarga de mover al enemigo
    }

    private void OnTriggerEnter(Collider other)
    {
        // si toca al jugador
        if (other.CompareTag("Player"))
        {
            attacking = true;

            // busca IDamageable
            IDamageable damageable =
                other.GetComponent<IDamageable>();

            // si existe
            if (damageable != null)
            {
                // aplica daño
                damageable.TakeDamage(damage);

                Debug.Log("Enemy hit player");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // si el jugador sale
        if (other.CompareTag("Player"))
        {
            attacking = false;
        }
    }
}