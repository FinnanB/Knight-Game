using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Door : MonoBehaviour
{
    public bool isClosed;
    public bool stayOpen;
    public bool inRange;
    public Collider _col;
    public GameObject _text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && inRange)
        {
            Open(false);
        }

        if(!isClosed && !inRange && !stayOpen)
        {
            Open(true);
        }
    }

    void Open(bool t)
    {
        //Debug.Log("h");
        isClosed = t;
        _col.enabled = t;
        GetComponent<MeshRenderer>().enabled = t;
        GetComponent<NavMeshObstacle>().carving = t;
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
