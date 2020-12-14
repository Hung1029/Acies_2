using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField]
    private  Vector2 parallaxEffectMultiplier;

    private Transform CamaraTransform;
    private Vector3 lastCamaraPosition;
    private void Start()
    {
        CamaraTransform = Camera.main.transform;
        lastCamaraPosition = CamaraTransform.position;
    }

    private void LateUpdate()
    {
        Vector3 deltaMovement = CamaraTransform.position - lastCamaraPosition;
        transform.position += new Vector3( deltaMovement.x * parallaxEffectMultiplier.x, deltaMovement.y * parallaxEffectMultiplier.y);
        lastCamaraPosition = CamaraTransform.position; 
    }
}
