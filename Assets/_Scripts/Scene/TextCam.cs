using UnityEngine;

public class TextCam : MonoBehaviour
{
    private Transform mainCam;

    private void OnEnable()
    {
        mainCam = Camera.main.transform;
       // Debug.Log("Main Cam = " + mainCam.name);
    }

    private void LateUpdate()
    {
        transform.LookAt(mainCam);
        transform.RotateAround(transform.position, transform.up, 180f);
    }
}
