using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraChange : MonoBehaviour
{
   [SerializeField] private float newSize; 

   private CinemachineVirtualCamera cam; 
   private float currSize;
   
   
    // Start is called before the first frame update
    void Start()
    {
        cam = FindObjectOfType<CinemachineVirtualCamera>();
        currSize = cam.m_Lens.OrthographicSize;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) 
        {
            currSize = cam.m_Lens.OrthographicSize;
            StartCoroutine(ZoomCamera(currSize, newSize, 2f, 100f));
            Destroy(gameObject, 2.2f);
        }
    }

    IEnumerator ZoomCamera(float from, float to, float time, float steps)
    {
        float f = 0;

        while (f <= 1) //Changes the camera zoom over 2 seconds/'time' variable
        {
            cam.m_Lens.OrthographicSize = Mathf.Lerp(from, to, f);
            f += 1f / steps;
            yield return new WaitForSeconds(time/steps);
        }
    }
}
