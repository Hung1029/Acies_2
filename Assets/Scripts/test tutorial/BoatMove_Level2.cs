using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatMove_Level2 : MonoBehaviour
{
    [System.NonSerialized]
    public bool _bIsMove = false;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }
    void Update()
    {
      
        BoatFloating();
       

        if (this.gameObject.transform.position.y < -11 )
        {
            this.gameObject.transform.position = new Vector2(10.15f, -9.56f);
            this.gameObject.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        }


    }


    //Boat floating
    public void BoatFloating()
    {
        rb.velocity = new Vector2( 1.0f, rb.velocity.y);

    }



}
