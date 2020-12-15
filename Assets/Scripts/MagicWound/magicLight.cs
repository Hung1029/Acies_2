using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class magicLight : MonoBehaviour
{
    //normal light data
    Color color_normal = new Color(0.85f, 0.85f, 0.85f);
    float intensity_normal = 0.0f;

    //skill one light data
    Color color_skillOne = new Color(1.0f, 0.79f, 0.49f);
    float lightIntensity = 1.0f;


    //skill two light data
    Color color_skillTwo = new Color(0.0f, 0.94f, 1.0f);

    //light
    [System.NonSerialized]
    public Light2D magicLt;

    
    //bool conneted with VitaSoul
    [System.NonSerialized]
    public bool LightUpVita = false;


    //Sister Soul Light management
    [System.NonSerialized]
    public bool canLightUpVita = false;

    //Skill time
    [System.NonSerialized]
    public float fRaiseHand = 0.8f;


    void Start()
    {
        magicLt = GetComponent<Light2D>();

    }

    

    public void ChangeLightColor(int skillNUM)
    {
      
        switch (skillNUM)
        {
            case 1:
                magicLt.color = color_skillOne;
             break;
            case 2:
                magicLt.color = color_skillTwo;
                break;
            default:
                break;
        }
    }


    public void FadeIn()
    {
        StopAllCoroutines();
        StartCoroutine(setMagicLtIntensity_1());


    }


    public void FadeOut()
    {
        StopAllCoroutines();
        StartCoroutine(setMagicLtIntensity_0());
    }
    public void FadeOutCountDown(float time)
    {
        StartCoroutine(intensityCountDown(time));
    }

    public void ChangeIntensity(float intensity)
    {
        StopAllCoroutines();
        magicLt.intensity = intensity;
    }




    IEnumerator intensityCountDown(float time)
    {
        for (float i = 0; i < time; i += 0.1f)
        {
            yield return new WaitForSeconds(0.1f);
        }
        FadeOut();

    }


    IEnumerator setMagicLtIntensity_1()
    {
       
       
        yield return new WaitForSeconds(fRaiseHand);
       
        for (; magicLt.intensity < lightIntensity;)
        {
            magicLt.intensity += 0.05f;
            yield return new WaitForSeconds(0.015f);
        }
        magicLt.intensity = 1.0f;

        // start permission light up vita
        canLightUpVita = true;

    }

    IEnumerator setMagicLtIntensity_0()
    {
      
        for (; magicLt.intensity > 0.0f;)
        {
            magicLt.intensity -= 0.05f;
            yield return new WaitForSeconds(0.015f);
        }
        magicLt.intensity = 0.0f;

        //stop permission light up vita
        canLightUpVita = false;
    }


    
    private void OnTriggerEnter2D(Collider2D other)
    {
       
        if (other.name == "VitaSoul" && canLightUpVita )
        {
            LightUpVita = true;
            
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        
        if (other.name == "VitaSoul")
        {
            LightUpVita = false;
           

        }
    }
}
