using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

    public float speed ;

    public float leftBoundary_x;
    public float rightBoundary_x;

    [System.NonSerialized]
    public bool _bCanMove = false;


    private Rigidbody2D rb;

   

    // Start is called before the first frame update
    void Start()
    { 
        rb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
      
        if (this.transform.position.x <= leftBoundary_x || this.transform.position.x >= rightBoundary_x)
            speed = speed * -1;
        
    }

    private void FixedUpdate()
    {
        if (_bCanMove)
            rb.velocity = new Vector2(speed, 0.0f);

    }

}
