using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatMove : MonoBehaviour
{
    [System.NonSerialized]
    public bool _bIsMove = false;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }

    //Boat floating
    public void BoatFloating()
    {
        _bIsMove = true;

        if (this.transform.position.x <53.64)
            rb.velocity = new Vector2(1.0f, rb.velocity.y);
        else
        {
            _bIsMove = false;
        }
    }


}

