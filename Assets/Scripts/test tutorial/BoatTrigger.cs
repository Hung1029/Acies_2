using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatTrigger : MonoBehaviour
{

    [System.NonSerialized]
    public bool _bSkillOneTrigger = false;

   

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "VitaSoul")
        {
            _bSkillOneTrigger = true;

        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.name == "VitaSoul")
        {
            _bSkillOneTrigger = false;

        }
    }


}
