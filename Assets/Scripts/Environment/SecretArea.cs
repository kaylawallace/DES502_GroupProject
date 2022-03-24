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
            if (t == 0)
            {
                Reveal();
            }
           

        }
    }

    private void Update()
    {
    }

    public void Reveal()
    {
        Color temp = gameObject.GetComponent<SpriteRenderer>().color;
        temp.a = Mathf.MoveTowards(temp.a, 0, t);
        gameObject.GetComponent<SpriteRenderer>().color = temp;
        //print("changing opacity");
        //Color col = GetComponent<SpriteRenderer>().color;
        //col.a = 0;
        ////GetComponent<SpriteRenderer>().color = col;
        //t = Mathf.PingPong(Time.time, 2) / 2;
        //this.GetComponent<SpriteRenderer>().color = Color.Lerp(this.GetComponent<SpriteRenderer>().color, col, t);


        ////GetComponent<SpriteRenderer>().color = lerpCol;
        ////print(GetComponent<SpriteRenderer>().color.a);
        ////Mathf.Lerp(col.a, 0, t);

        t += Time.deltaTime * 0.1f;

        //if (t > 1f)
        //{
        //    t = 0f;
        //}

        
    }
}
