using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VitaTriggerDetect : MonoBehaviour
{
    [System.NonSerialized]
    public bool _bSkillTrigger = false;
    
    //Which skill can trigger
    public int _iSkillNumToDetect;

    [System.NonSerialized]
    public bool bCanTrigger = true;
    

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.name == "VitaSoul" && PlayerSkill.CURRENTSKILL == _iSkillNumToDetect && bCanTrigger)
        {
            _bSkillTrigger = true;

        }

        else if (!bCanTrigger)
        {
            _bSkillTrigger = false;
        }
    }
    

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.name == "VitaSoul" )
        {
            _bSkillTrigger = false;

        }
    }
}
