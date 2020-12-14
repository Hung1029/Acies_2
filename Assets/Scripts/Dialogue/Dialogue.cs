using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Dialogue //delete MonoBehaviour because we don't want it to sit on a script
{
    public Dialogue()
    {
    }
    public string name;
    [TextArea(3,10)]
    public string[] sentences;

}
