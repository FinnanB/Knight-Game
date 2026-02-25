using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using TMPro;
using UnityEngine.AI;

public class Skelly : MonoBehaviour
{
    public Animator c_Animator;
    public float wTime;
    public bool canSwing;
    // Start is called before the first frame update

    // Update is called once per frame

    public Collider sw1;

    private void Start()
    {
        canSwing = true;
    }

    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("");
        if (other.CompareTag("Player") && canSwing)
        {
            
            c_Animator.SetTrigger("Swing");
            StartCoroutine(PlayAnimation());
        }
    }

    IEnumerator PlayAnimation()
    {
        AnimatorStateInfo stateInfo = c_Animator.GetCurrentAnimatorStateInfo(0);
        int cur = stateInfo.shortNameHash;
        canSwing = false;
        yield return new WaitUntil(() => c_Animator.GetCurrentAnimatorStateInfo(0).shortNameHash != cur);
        stateInfo = c_Animator.GetCurrentAnimatorStateInfo(0);
        GetComponent<NavMeshAgent>().speed = 0;
        sw1.enabled = true;
        yield return new WaitForSeconds(stateInfo.length);
        GetComponent<NavMeshAgent>().speed = 5;
        sw1.enabled = false;
        yield return new WaitForSeconds(wTime);
        canSwing = true;
        yield return null;
    }
}
