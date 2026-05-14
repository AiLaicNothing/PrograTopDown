using Unity.VisualScripting;
using UnityEngine;

public class LeaderDeath : EnemyBase
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
