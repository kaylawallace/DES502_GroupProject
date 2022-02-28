using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamouflageAbility : MonoBehaviour
{
    public float maxTime;

    private GameObject head;
    private SpriteRenderer[] renderers;
    private Color[] col;
    private float time;
    private bool invisible;

    // Start is called before the first frame update
    void Start()
    {
        renderers = GetComponentsInChildren<SpriteRenderer>();
        time = 0;
        invisible = false;
        if (renderers[0])
        {
            col = new Color[renderers.Length];
            for (int i = 0; i < renderers.Length; i++)
            {
                col[i] = renderers[i].color;
            }
        }
        else
        {
            renderers = GetComponentsInChildren<SpriteRenderer>();
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        CheckInvisibility(); 
    }

    void CheckInvisibility()
    {
        time -= Time.deltaTime;
        if (invisible && time <= 0)
        {
            invisible = false;

            for (int i = 0; i < renderers.Length; i++)
            {
                col[i].a = 1;
                renderers[i].color = col[i];
            }       
        }
    }

    void GoInvisible()
    {
        if (!invisible)
        {
            invisible = true;
            time = maxTime;

            for (int i = 0; i < renderers.Length; i++)
            {
                col[i].a = .2f;
                renderers[i].color = col[i];
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Invisible"))
        {
            GoInvisible();
            GameObject col = collision.gameObject;
            Destroy(col);
        }
    }
}
