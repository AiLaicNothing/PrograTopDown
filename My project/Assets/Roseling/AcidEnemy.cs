using UnityEngine;

public class AcidEnemy : EnemyBase
{
    public float speed = 4f;

    public string playerTag = "Player";
    private Transform target;

    public GameObject acidPrefab;

    protected override void Start()
    {
        base.Start();

        GameObject player = GameObject.FindGameObjectWithTag(playerTag);

        if (player != null)
        {
            target = player.transform;
        }
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if (target == null) return;

        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            Explode();
        }
    }

    private void Explode()
    {
        Instantiate(acidPrefab, transform.position, Quaternion.identity);

        Dead();
    }


}
