using UnityEngine;
using UnityEngine.AI;
public class Teleport : MonoBehaviour
{
    private Transform playerTransform;
    private NavMeshAgent navMeshAgent;
    private float range;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        range = Vector3.Distance(transform.position, playerTransform.position);
        if (range > 10)
        {
            transform.position = new Vector3(1, 0, 1) + playerTransform.position;
        }
        else if (range > 3)
        {
            navMeshAgent.SetDestination(playerTransform.position);
            navMeshAgent.isStopped = false;
        }
        else
        {
            navMeshAgent.isStopped = true;
        }
    }
}
