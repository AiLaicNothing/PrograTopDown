using UnityEngine;

public class Detect : MonoBehaviour
{

    public bool controllerDash;
    void Awake()
    {
        controllerDash = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            controllerDash = true;
            Debug.Log("Player detected!");
        }
    }
}
