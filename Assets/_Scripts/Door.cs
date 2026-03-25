using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Door : MonoBehaviour
{
    public bool isOpen;
    public bool stayOpen;
    public bool inRange;
    public Collider _col;
    public GameObject _text;

    public Animator c_Animator;
    // Start is called before the first frame update
    void Start()
    {
        c_Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && inRange)
        {
            isOpen = true;
        }
        c_Animator.SetBool("Open", isOpen);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            inRange = true;
            _text.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            inRange = false;
            _text.SetActive(false);
        }
    }
}
