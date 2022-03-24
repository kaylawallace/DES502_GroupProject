using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretArea : MonoBehaviour
{
    static float t = 0f;
    bool reveal = false;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //print("player collide");
            if (t >= 0)
            {
                Reveal();
            }
        }
    }

    public void Reveal()
    {
        Color temp = gameObject.GetComponent<SpriteRenderer>().color;
        temp.a = Mathf.MoveTowards(temp.a, 0, t);
        gameObject.GetComponent<SpriteRenderer>().color = temp;

        t += Time.deltaTime * 0.1f;

        if (t == 1f) {
            t = 0;
        }
    }
}
