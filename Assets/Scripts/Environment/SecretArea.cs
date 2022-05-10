using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Method to handle the reveal of the secret area upon the player entering 
 */
public class SecretArea : MonoBehaviour
{
    private static float t = 0f;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (t >= 0)
            {
                Reveal();
            }
        }
    }

    /*
     * Method repsonsible for changing the opacity of the secret area cover to reveal the area 
     */
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
