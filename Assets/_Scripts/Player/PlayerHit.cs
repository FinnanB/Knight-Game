using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    public float damage;
    float trueDamage;

    public GameObject _Player;

    AudioSource m_MyAudioSource;


    void Start()
    {
        m_MyAudioSource = GetComponent<AudioSource>();
        _Player = GameObject.FindWithTag("Player");
    }
    private void OnTriggerEnter(Collider other)
    {
        trueDamage = damage * _Player.GetComponent<PlayerController>().playerData.damage;
        
        if (other.tag == "Enemy")
        {
            Debug.Log(other.gameObject);
            other.GetComponent<EnemyController>().Hit(trueDamage, _Player);
            _Player.GetComponent<PlayerController>().mana +=10;
            m_MyAudioSource.Play();
        }
    }
}
