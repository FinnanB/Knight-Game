using Unity.Burst.CompilerServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using TMPro;

public class Hint : MonoBehaviour
{
    public TMP_Text _InfoText;

    public string hint;
    public GameObject sign;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            sign.SetActive(true);
            _InfoText.enabled = true;
            _InfoText.text = hint;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            sign.SetActive(false);
            _InfoText.enabled = false;
            _InfoText.text = "";
        }
    }
}
