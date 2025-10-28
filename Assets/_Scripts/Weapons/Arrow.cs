using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public bool pArrow;
    public bool pierce;

    GameObject hit;
    Rigidbody rb;
    void Start()
    {
        pierce = false;
        pArrow = false;
        rb = GetComponent<Rigidbody>();
        StartCoroutine(Launch(2f));

    }

    IEnumerator Launch(float _time)
    {
        if(pierce)
        {
            _time *= 2f;
        }
        yield return new WaitForSeconds(_time);
        rb.useGravity = true;
    }
    IEnumerator Hit()
    {
        yield return new WaitForFixedUpdate();
        Destroy(GetComponent<PlayerHit>());
        Launch(0f);
    }

    void OnTriggerEnter(Collider collision)
    {
        GameObject col = collision.gameObject;
        if (col == hit)
        {
            return;
        }
        else
        {
            hit = col;
        }
        if(col.tag != "Enemy")
        {
            pierce = false;
        }
        if (pArrow && !pierce)
        {
            rb.isKinematic = true;
            StartCoroutine(Hit());
        }
        else if (!pArrow)
        {
            Destroy(gameObject);
        }
        if(col.tag == "Player" && pArrow)
        {
            col.GetComponent<Bow>().arrows++;
            Destroy(gameObject);
        }
       // Debug.Log(col.tag);
    }
}
