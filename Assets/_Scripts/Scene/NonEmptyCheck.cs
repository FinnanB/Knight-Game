using UnityEngine;

public class NonEmptyCheck : MonoBehaviour
{
    public GameObject player;
    public GameObject resumeButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<PlayerController>().playerData.lvls[0] == 0)
        {
            resumeButton.SetActive(false);
        }
        else 
        {
            resumeButton.SetActive(true);
        }
    }
}
