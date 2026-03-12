using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHit : MonoBehaviour
{
    public float damage;
    public float _D;

    public GameObject enemy;
    public int hits;

    AudioSource m_MyAudioSource;

    void Start()
    {
        m_MyAudioSource = GetComponent<AudioSource>();
        //enemy = PrefabUtility.GetOutermostPrefabInstanceRoot(gameObject);
        //enemy = transform.root.gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
       // Debug.Log(hits);
        hits++;
        if (other.tag == "Player")
        {
            // m_MyAudioSource.Play();
            other.GetComponent<PlayerController>().Hit(damage, transform.position);
            //GetComponent<Collider>().enabled = false;
            if(other.GetComponent<PlayerController>()._block == 0)
            {
                Debug.Log("h");
                enemy.GetComponent<EnemyController>().sturdy += 5;
                other.GetComponent<PlayerController>().mana += 30;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        //Debug.Log(other);
        hits = 0;
    }
}
