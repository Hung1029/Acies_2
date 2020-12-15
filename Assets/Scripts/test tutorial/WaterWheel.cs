using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterWheel : MonoBehaviour
{
    [System.NonSerialized]
    public bool _bSkillOneTrigger = false;

    [System.NonSerialized]
    public bool _bIsRotate = false;

    [System.NonSerialized]
    public bool _bLightBlowEnable = true;

    public GameObject Target;
    public float i;


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

   

    void Update()
    {
        // set light color
        float t = Mathf.PingPong(Time.time, 1.0f) / 1.0f;
        if (_bLightBlowEnable == true)
        {
           Target.GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>().intensity += t;
            Debug.Log(Target.GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>().intensity);
        }
        else {
            Target.GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>().intensity = 1.5f;
        }
    }

    ////water wheel rotate

    public void PlayWaterWheelRotate()
    {
        StartCoroutine("WaterWheelRotateIEnumerator");
    }

    IEnumerator WaterWheelRotateIEnumerator()
    {
        _bIsRotate = true;
        _bLightBlowEnable = false;
        //how many circle
        for (int circle = 0; circle < 1; circle++)
         {
             //rotate angle
             for (float a = 0.0f; a < 90; a++)
             {
                 transform.Rotate(0.0f, 0.0f, 1.0f);
                 yield return new WaitForSeconds(0.05f);
             }
         }
        _bIsRotate = false;
       


    }

   ////

}
