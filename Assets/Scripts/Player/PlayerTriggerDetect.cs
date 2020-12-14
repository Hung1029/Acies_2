using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTriggerDetect : MonoBehaviour
{
    //[System.NonSerialized]
    public bool bTrigger = false;
    //[System.NonSerialized]
    public string TriggerName = null;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == TriggerName)
        {
            bTrigger = true;
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.name == TriggerName)
        {
            bTrigger = false;
        }
    }
}
