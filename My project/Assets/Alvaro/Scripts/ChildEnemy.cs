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
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(Shooting());
    }
    private IEnumerator Shooting()
    {
        while (player != null)
        {
            Shoot();
            yield return new WaitForSeconds(3);
            Debug.Log("I've shooted");
        }
    }
    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * speed, ForceMode.Impulse);
    }
}
