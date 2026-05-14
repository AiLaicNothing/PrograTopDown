using Unity.VisualScripting;
using UnityEngine;

public class LeaderDeath : MonoBehaviour
{
    [SerializeField] GameObject leader;
    [SerializeField] GameObject group;
    void Update()
    {
        if(leader==null)
        {
            group.SetActive(true);
        }
    }
}
