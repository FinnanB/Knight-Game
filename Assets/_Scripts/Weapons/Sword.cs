using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public Animator c_Animator;

    public bool swing;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("h");
            c_Animator.SetTrigger("Swing");
        }
        if (Input.GetMouseButtonDown(1) && GetComponent<PlayerController>().mana >=5 && swing)
        {
            c_Animator.SetTrigger("Overhead");
            GetComponent<PlayerController>().mana -= 5;
        }
    }
}
