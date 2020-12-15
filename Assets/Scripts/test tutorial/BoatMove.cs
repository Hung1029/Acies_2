using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatMove : MonoBehaviour
{
    [System.NonSerialized]
    public bool _bIsMove = false;
    private Rigidbody2D rb;

    [System.NonSerialized]
    public bool _bLightBlowEnable = true;

    public GameObject Target;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }
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
    //Boat floating
    public void BoatFloating()
    {
        _bIsMove = true;
        _bLightBlowEnable = false;

        if (this.transform.position.x < 55.15003)
            rb.velocity = new Vector2(1.0f, rb.velocity.y);
        else
        {
            _bIsMove = false;
        }
    }


}

