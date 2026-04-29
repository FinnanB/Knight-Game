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
        player = GameObject.FindGameObjectWithTag("Player");
        //Debug.Log(player);
        StartCoroutine(DelayStart());
       /* if(player.GetComponent<Sword>().swordData.level == 2)
        {
            Debug.Log(gameObject);
            isOpen = true;
        }*/
        
    }

    IEnumerator DelayStart()
    {
        yield return new WaitForSeconds(0.1f);
        //Debug.Log(player.GetComponent<Sword>().swordData.unlocked[attackNum]);
        if (!upgrade)
        {

            isOpen = player.GetComponent<Sword>().swordData.unlocked[attackNum];
        }
        else if (upgrade)
        {
            if(player.GetComponent<Sword>().swordData.level == 2)
            {
                isOpen = true;
            }
        }
        //return null;
    }

    void Update()
    {
        //Debug.Log(player.GetComponent<Sword>().swordData.unlocked[attackNum]);
        if (Input.GetKeyDown(KeyCode.Q) && inRange && player != null)
        {
            isOpen = true;
            m_MyAudioSource.Play();
            if (!upgrade)
            {
                Debug.Log("f ");
                player.GetComponent<Sword>()._Tog[attackNum].gameObject.SetActive(true);
                player.GetComponent<Sword>().SetData(2);
                Debug.Log("g ");
                this.enabled = false;
            }
            else
            {
                player.GetComponent<Sword>().LevelUp();
            }
            _InfoText.text = hint;
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
            //player = other.gameObject;
            _text.SetActive(true);
            _InfoText.enabled = true;
            
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
