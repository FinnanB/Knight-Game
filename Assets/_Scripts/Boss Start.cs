using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossStart : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject bossHealth;
    // Start is called before the first frame update
    void OnTriggerEnter()
    {
        agent.enabled = true;
        bossHealth.SetActive(true);
    }
}
