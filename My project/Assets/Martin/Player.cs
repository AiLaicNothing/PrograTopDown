using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, IDamageable
{
    [Header("Movement")]
    [SerializeField] private float speed = 5f;

    [Header("Health")]
    [SerializeField] private float maxHealth = 100f;

    [Header("Combat")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float bulletSpeed = 20f;

    private float currentHealth;

    private Vector2 moveInput;

    private Rigidbody rb;
    private Camera mainCam;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        mainCam = Camera.main;

        currentHealth = maxHealth;
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Update()
    {
        RotateToMouse();
    }

    private void Movement()
    {
        Vector3 moveDir = new Vector3(moveInput.x, 0f, moveInput.y);

        rb.linearVelocity = moveDir * speed;
    }

    private void RotateToMouse()
    {
        Ray ray = mainCam.ScreenPointToRay(Mouse.current.position.ReadValue());

        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        if (groundPlane.Raycast(ray, out float distance))
        {
            Vector3 mouseWorldPos = ray.GetPoint(distance);

            Vector3 direction = mouseWorldPos - transform.position;

            direction.y = 0f;

            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        Debug.Log(moveInput);
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        Shoot();
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate( bulletPrefab, firePoint.position, firePoint.rotation);

        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

        bulletRb.linearVelocity = firePoint.forward * bulletSpeed;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
