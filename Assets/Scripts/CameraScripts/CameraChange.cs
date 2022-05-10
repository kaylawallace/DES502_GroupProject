using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/*
 * Script to handle zooming the camera in/out upon player collision with trigger boxes in the scene 
 */
public class CameraChange : MonoBehaviour
{
   [SerializeField] private float newSize; 

   private CinemachineVirtualCamera cam; 
   private float currSize;  
   
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

    /*
     * Coroutine to smoothly zoom the camera in or out 
     * Params:  from - start size of the camera 
     *          to - end size of the camera
     *          time - time to complete the transition in
     *          steps - number of steps to complete the transition in (how smooth the transition will be)
     */
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
