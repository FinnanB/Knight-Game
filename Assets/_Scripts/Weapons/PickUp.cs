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

    public bool isOpen;
    public bool inRange;

    public bool upgrade;

    public GameObject _text;
    public TMP_Text _InfoText;

    public GameObject player;

    public string hint;

    AudioSource m_MyAudioSource;

    void Start()
    {
        m_MyAudioSource = GetComponent<AudioSource>();
        c_Animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && inRange && player != null)
        {
            isOpen = true;
            m_MyAudioSource.Play();
            if (!upgrade)
            {
                player.GetComponent<Sword>()._Tog[attackNum].gameObject.SetActive(true);
                player.GetComponent<Sword>().SetData();
                this.enabled = false;
            }
            else
            {
                player.GetComponent<Sword>().LevelUp();
            }
        }
        c_Animator.SetBool("Open", isOpen);
    }

    /*void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            m_MyAudioSource.Play();
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
    }*/

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            inRange = true;
            player = other.gameObject;
            _text.SetActive(true);
            _InfoText.enabled = true;
            _InfoText.text = hint;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            inRange = false;
            _text.SetActive(false);
            _InfoText.enabled = false;
            _InfoText.text = "";
        }
    }
}
