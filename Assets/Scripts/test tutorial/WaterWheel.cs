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

    [SerializeField]
    private ParticleSystem Splash;



    void Update()
    {
        
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
        //_bIsRotate = false;

        Splash.Stop();

    }

   ////

}
