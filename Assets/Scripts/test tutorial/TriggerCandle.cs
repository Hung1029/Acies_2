using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCandle : MonoBehaviour
{

   

    [System.NonSerialized]
    public bool _bSkillOneTrigger = false;

    [System.NonSerialized]
    public bool _bLightBlowEnable = true;

    public GameObject Target;

    void Update()
    {
        // set light color
        float t = Mathf.PingPong(Time.time, 1.0f) / 1.0f;
        if (_bLightBlowEnable == true)
        {
            Target.GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>().intensity += t;
        }
        else
        {
            Target.GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>().intensity = 1.5f;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "VitaSoul")
        {
            _bSkillOneTrigger = true;
            _bLightBlowEnable = false;

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
