using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField]
    private  Vector2 parallaxEffectMultiplier;

    public Transform CamaraTransform;
    private Vector3 lastCamaraPosition;
    private void Start()
    {
        lastCamaraPosition = CamaraTransform.position;
    }

    private void LateUpdate()
    {
        Vector3 deltaMovement = CamaraTransform.position - lastCamaraPosition;
        transform.position += new Vector3( deltaMovement.x * parallaxEffectMultiplier.x, deltaMovement.y * parallaxEffectMultiplier.y);
        lastCamaraPosition = CamaraTransform.position; 
    }
}
