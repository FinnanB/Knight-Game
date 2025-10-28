using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Bow : MonoBehaviour
{
    public bool launch;
    public GameObject prefab;
    public float push;

    public int arrows;
    public int maxArrows;
    public TMP_Text hText;

    public GameObject arrow;
    public bool knocked;

    public Material mat;
    // Start is called before the first frame update
    void Start()
    {
        arrows = 10;
        launch = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (knocked)
        {
            Destroy(arrow);
            StopAllCoroutines();
            launch = false;
        }

        if (Input.GetMouseButtonDown(0) && launch && arrows > 0 && Time.timeScale != 0)
        {
            StartCoroutine(Launch(1f));
        }
        if (Input.GetMouseButtonDown(1) && launch && arrows > 0 && GetComponent<PlayerController>().mana >= 2 && Time.timeScale != 0)
        {
            StartCoroutine(Launch(2f));
            GetComponent<PlayerController>().mana -= 2;
        }
        arrows = Mathf.Min(maxArrows, arrows);
        if (hText != null)
        {
            hText.text = (arrows + "/" + maxArrows);
        }
    }

    IEnumerator Launch(float _time)
    {
        GetComponent<PlayerController>().speed = 6/(3f*_time);
        launch = false;
        Vector3 spawn = transform.position - (transform.right * 0.6f);//new Vector3(transform.localPosition.x - 0.5f, transform.localPosition.y, transform.localPosition.z);
        arrow = Instantiate(prefab, spawn, transform.rotation, transform);
        yield return new WaitForSeconds(_time*1.5f);
        Destroy(arrow);
        spawn = transform.position + (transform.forward *2*_time);//new Vector3(transform.position.x, transform.position.y, transform.position.z + 1.5f);
        arrow = Instantiate(prefab, spawn, transform.rotation);
        yield return new WaitForSeconds(0.01f);
        arrow.GetComponent<Collider>().enabled = true;
        arrow.GetComponent<Arrow>().pArrow = true;

        arrow.transform.localScale = arrow.transform.localScale * _time;
        if(_time > 1f)
        {
            arrow.GetComponent<Arrow>().pierce = true;
            Renderer rend = arrow.GetComponent<Renderer>();
            rend.material = mat;
        }
        Rigidbody rb = arrow.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.AddRelativeForce(Vector3.forward * push*(_time/1.5f));
        GetComponent<PlayerController>().speed = 6;
        launch = true;
        arrow = null;
        arrows--;
    }
}
