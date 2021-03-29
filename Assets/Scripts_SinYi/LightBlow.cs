using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;


public class LightBlow : MonoBehaviour
{
    private bool _bLightTrigger;
    private float _bLightIntensity;

    private void Start()
    {
        this.gameObject.GetComponent<Light2D>().intensity = 1;
        _bLightTrigger = this.gameObject.GetComponent<Light2D>().enabled;
    }
    private void Update()
    {

        this.gameObject.GetComponent<Light2D>().enabled = _bLightTrigger;
        this.gameObject.GetComponent<Light2D>().intensity = _bLightIntensity;
    }
}





