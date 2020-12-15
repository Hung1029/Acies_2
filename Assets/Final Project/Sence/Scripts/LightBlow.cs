using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;   //OLD VERSIONS LIKE 2018
using UnityEngine.Experimental.Rendering.Universal; //2019 VERSIONS


public class LightBlow : MonoBehaviour
{
    public bool _bLightTrigger;
    public float _bLightIntensity;

    private void Start()
    {
        GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>().intensity = 1;
        _bLightTrigger =GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>().enabled;
    }
    private void Update()
    {
        
        GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>().enabled = _bLightTrigger;
        GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>().intensity = _bLightIntensity;
    }
}





