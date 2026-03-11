using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    public bool damageType;
    public float damageMult;
    public float trueDamage;

    public bool hitWall;

    public GameObject _Player;

    AudioSource m_MyAudioSource;


    void Start()
    {
        m_MyAudioSource = GetComponent<AudioSource>();
        _Player = GameObject.FindWithTag("Player");
    }

    public void Update()
    {
        if (damageType)
        {
            trueDamage = _Player.GetComponent<PlayerController>().strength * damageMult;
        }
        else
        {
            trueDamage = _Player.GetComponent<PlayerController>().dex * damageMult;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
          //  Debug.Log(other.gameObject);
            other.GetComponent<EnemyController>().Hit(trueDamage, _Player, damageType);
            _Player.GetComponent<PlayerController>().mana +=10;
            m_MyAudioSource.Play();
        }
        else if (other.tag != "Enemy" && !damageType && hitWall) 
        {
            //Debug.Log(other.gameObject);
            _Player.GetComponent<Animator>().SetTrigger("HitWrong");
        }
    }
}
