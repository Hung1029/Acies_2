using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockBearDamage_Level2 : MonoBehaviour
{

    [System.NonSerialized]
    public bool _bSkillOneTrigger = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name == "Bear")
        {
            _bSkillOneTrigger = true;
        }
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.name == "Bear")
        {
            _bSkillOneTrigger = false;

        }
    }


}
