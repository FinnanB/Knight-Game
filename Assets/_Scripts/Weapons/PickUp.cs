using Unity.Burst.CompilerServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using TMPro;

public class PickUp : MonoBehaviour
{
    public int attackNum;
    public Animator c_Animator;

    public bool upgrade;

    public TMP_Text hint;
    public String hintText;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (!upgrade)
            {
                other.GetComponent<Sword>()._Tog[attackNum].gameObject.SetActive(true);
                other.GetComponent<Sword>().SetData();
                this.enabled = false;
            }
            else
            {
                other.GetComponent<Sword>().LevelUp();
            }
            StartCoroutine(_Hint());
            c_Animator.SetTrigger("Open");
            
        }
    }

    IEnumerator _Hint()
    {
        hint.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        hint.text = hintText;
        yield return new WaitForSeconds(3f);
        hint.text = "";
        hint.gameObject.SetActive(false);
    }
}
