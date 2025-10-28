using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : MonoBehaviour
{
    public Animator c_Animator;
    // Start is called before the first frame update

    // Update is called once per frame

    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("");
        if (other.CompareTag("Player"))
        {
            c_Animator.SetTrigger("Swing");
        }
    }
}
