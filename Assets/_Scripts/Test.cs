using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public class Test : MonoBehaviour
{
    public Animator c_Animator;

    public bool run;

    public AnimatorStateInfo stateInfo;

    public Collider bx;

    void Update()
    {
        stateInfo = c_Animator.GetCurrentAnimatorStateInfo(0);
        if (run)
        {
            run = false;
            c_Animator.SetTrigger("Start");
            StartCoroutine(Play());
            
        }
    }



    IEnumerator Play()
    {
        Debug.Log(Time.time);
        Debug.Log(stateInfo.shortNameHash);
        int cur = stateInfo.shortNameHash;
        
        yield return new WaitUntil(() => stateInfo.shortNameHash != cur);
        bx.enabled = true;
        yield return new WaitForSeconds(stateInfo.length);
        bx.enabled = false;
        c_Animator.SetTrigger("Return");
        yield return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        bx.enabled = false;
    }
}
