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

    [System.NonSerialized]
    public bool bCanBeDetect = true;

    private void OnTriggerStay2D(Collider2D other)
    {

        if (other.name == "VitaSoul" && PlayerSkill.CURRENTSKILL == _iSkillNumToDetect && bCanTrigger)
        {
            //注視到技能符號 add music
            if (FindObjectOfType<AudioManager>() != null)
                FindObjectOfType<AudioManager>().Play("PlayerUseSkill");

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

    /////////////////////////////////////////////////////////VitaSoul core fade in, fade out when skill one
    private void Update()
    {

        
    }


}
