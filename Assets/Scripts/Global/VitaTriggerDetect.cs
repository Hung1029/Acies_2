using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VitaTriggerDetect : MonoBehaviour
{
    [System.NonSerialized]
    public bool _bSkillTrigger = false;
    
    //Which skill can trigger
    public int _iSkillNumToDetect;

    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "VitaSoul" && PlayerSkill.CURRENTSKILL == _iSkillNumToDetect)
        {
            _bSkillTrigger = true;

        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.name == "VitaSoul" && PlayerSkill.CURRENTSKILL == _iSkillNumToDetect)
        {
            _bSkillTrigger = false;

        }
    }
}
