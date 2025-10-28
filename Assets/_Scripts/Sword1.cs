using UnityEngine;

public class Sword1 : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("hit");
        other.gameObject.SetActive(false);
    }
}
