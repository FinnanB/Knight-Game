using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public Animator c_Animator;

    public bool swing;

    public float swingCost;

    public bool changed;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && GetComponent<PlayerController>().stamina >= swingCost)
        {
            GetComponent<PlayerController>().stamina -= swingCost;
            c_Animator.SetTrigger("Swing");
        }
        if(Input.GetKeyDown("r"))
        {
            c_Animator.SetTrigger("Switch");
            StartCoroutine(Switch());
        }
        /*if (Input.GetMouseButtonDown(1) && GetComponent<PlayerController>().mana >=5 && swing)
        {
            c_Animator.SetTrigger("Overhead");
            GetComponent<PlayerController>().mana -= 5;
        }*/
    }

    IEnumerator Switch()
    {
        while (changed)
        {
            yield return null;
        }
        GetComponent<PlayerController>().speed = 4;
        while (!changed)
        {
            yield return null;
        }
        GetComponent<PlayerController>().speed = 6;
    }
}
