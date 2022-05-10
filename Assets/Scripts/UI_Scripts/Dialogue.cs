using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * REFERENCE: Brackeys - How to make a Dialogue System in Unity - https://www.youtube.com/watch?v=_nRzoTzeyxU
 */

/*
 * Class for Dialogue objects 
 */
[System.Serializable]
public class Dialogue
{
    public string name;
    [TextArea(3, 10)] public string[] sentences;
}

