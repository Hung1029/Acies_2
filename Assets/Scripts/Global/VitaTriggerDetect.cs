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

    bool bLastVitaSoulCoreState = false;
    bool bCanChangeVitaSoulCore = true;

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

    /////////////////////////////////////////////////////////VitaSoul core fade in, fade out when skill one
    private void Update()
    {

        if (bLastVitaSoulCoreState != _bSkillTrigger)
        {
            bCanChangeVitaSoulCore = true;
        }

        //if is skill one and is triggered => fade in 
        if (_iSkillNumToDetect == 1 && !_bSkillTrigger && bCanChangeVitaSoulCore)
        {
            StartCoroutine(GameObject.Find("VitaSoul").GetComponent<VitaSoul_particle>().VitaSoulCoreFadeIn());
            bCanChangeVitaSoulCore = false;
        }
        else if (_iSkillNumToDetect == 1 && _bSkillTrigger && bCanChangeVitaSoulCore)
        {
            StartCoroutine(GameObject.Find("VitaSoul").GetComponent<VitaSoul_particle>().VitaSoulCoreFadeOut());
            bCanChangeVitaSoulCore = false;
        }


        bLastVitaSoulCoreState = _bSkillTrigger;
    }


}
