using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifeTime = 3f;

    private Vector3 direction;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    public void SetDirection(Vector3 newDirection)
    {
        direction = newDirection.normalized;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player hit");

            Destroy(gameObject);
        }
    }
}