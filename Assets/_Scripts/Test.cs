using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using UnityEngine.AI;

public class Test : MonoBehaviour
{
    public Transform targetObject;
    Rigidbody m_Rigidbody;
    public float m_Thrust = 20f;
    NavMeshAgent navAgent;
    public bool close;
    public float remDis;

    void Start()
    {
        close = false;
        m_Rigidbody = GetComponent<Rigidbody>();
        navAgent = GetComponent<NavMeshAgent>();
    }

   void Update()
    {
        navAgent.destination = targetObject.position;
        remDis = navAgent.remainingDistance;
        Vector3 targetDirection = targetObject.position - transform.position;
        //m_Rigidbody.AddForce(-targetDirection * m_Thrust);
        if (navAgent.stoppingDistance >= navAgent.remainingDistance)
        {
            close = true;
            navAgent.Move(-targetDirection.normalized * Time.deltaTime);
        }
            
    }
}
