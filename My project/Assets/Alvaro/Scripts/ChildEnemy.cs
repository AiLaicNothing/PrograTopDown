using UnityEngine;
using System.Collections;
public class ChildEnemy : EnemyBase
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform shootPoint;
    private GameObject player;
    private int speed = 2;
    private void Start()
    {
        base.Start();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(Shooting());
    }
    private IEnumerator Shooting()
    {
        Shoot();
        yield return new WaitForSeconds(3);
    }
    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.linearVelocity = new Vector3(rb.linearVelocity.x * speed, 0, 0);
    }
}
