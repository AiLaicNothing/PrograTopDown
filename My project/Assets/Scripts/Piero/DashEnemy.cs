using UnityEngine;
using System.Collections;
using TMPro;

public class DashEnemy : EnemyBase, IObserver
{
    
    [SerializeField] private Detect detect;
    [SerializeField] private int speed;
    [SerializeField] private float speedDash;

    private int recoil;
    private GameObject player;
    private Rigidbody rbEnemy;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rbEnemy = GetComponent<Rigidbody>();
        detect = GetComponentInChildren<Detect>();
        recoil = 16;
    }

    protected override void Start()
    {
        base.Start();
        GameManager.Instance.Attach(this);
    }
    void Update()
    {
        EnemyMovement();
        ActivateDash();    
    }

    private void Dash()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        rbEnemy.AddForce(direction * speedDash, ForceMode.Impulse);
        Debug.Log("Dash finished!");
        detect.controllerDash = false;
    }

    private void EnemyMovement()
    {
        if (player != null && detect.controllerDash == false)
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody rbPlayer = player.GetComponent<Rigidbody>();
            if (rbPlayer != null)
            {
                Vector3 direction = (collision.transform.position - transform.position).normalized;
                rbPlayer.AddForce(direction * recoil, ForceMode.Impulse);
                Debug.Log("Player hit by dash enemy!"); 
            }
                     
        }
    }

    private void ActivateDash()
    {
       if (detect.controllerDash == true)
        {
            Dash();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            TakeDamage(10f);
            Debug.Log("Enemy took damage from bullet!");
        }
    }

    public void Execute(ISubject subject)
    {
        if(subject is GameManager)
        {
            speed =((GameManager)subject).progression;
        }
    }
}