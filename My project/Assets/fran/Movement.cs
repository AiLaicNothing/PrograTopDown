using UnityEngine;

public class Movement : MonoBehaviour
{
    // velocidad de movimiento
    public float speed = 3f;

    // rigidbody del objeto
    private Rigidbody rb;

    private void Start()
    {
        // obtiene el rigidbody del objeto
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        MoveForward();
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