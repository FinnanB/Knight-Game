using UnityEngine;

public class PickUp : MonoBehaviour
{
    public int attackNum;
    public Animator c_Animator;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            c_Animator.SetTrigger("Open");
            other.GetComponent<Sword>()._Tog[attackNum].gameObject.SetActive(true);
            other.GetComponent<Sword>().SetData();
            this.enabled = false;
        }
    }
}
