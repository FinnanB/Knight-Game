using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using TMPro;

public class EnemySpawn : MonoBehaviour
{
    public GameObject enemies;
    public TMP_Text hint;
    public String hintText;

    private void Start()
    {
        Invoke(nameof(LateStart), 0.2f);
    }

    void LateStart()
    {
        enemies.SetActive(false);
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(_Hint());
            enemies.SetActive(true);
        }
    }

    IEnumerator _Hint()
    {
        hint.gameObject.SetActive(true);
        hint.text = hintText;
        yield return new WaitForSeconds(3f);
        hint.gameObject.SetActive(false);
        hint.text = "";
    }
}
