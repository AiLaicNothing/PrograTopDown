using UnityEngine;

public class Movement : MonoBehaviour
{
    // velocidad de movimiento
    public float speed = 3f;

    // agregué esta variable para evitar
    // que el enemigo camine siempre hacia adelante
    // y termine cayéndose del mapa
    public bool canMove;

    // rigidbody del objeto
    private Rigidbody rb;

    private void Start()
    {
        // obtiene el rigidbody
        rb = GetComponent<Rigidbody>();

        // agregué esto para que
        // el enemigo empiece quieto
        canMove = false;
    }

    private void FixedUpdate()
    {
        // agregué esta condición para que
        // solo se mueva cuando detecte al player
        if (canMove)
        {
            MoveForward();
        }
    }

    void MoveForward()
    {
        // mueve el objeto hacia adelante
        rb.MovePosition(
            transform.position +
            transform.forward * speed * Time.fixedDeltaTime
        );
    }
}