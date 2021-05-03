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
            transform.position = new Vector2(transform.position.x + (speed * Time.deltaTime), transform.position.y);


    }


    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.name == "Player")
        {
            
            GameObject.Find("Player").GetComponent<Transform>().parent = this.transform;
            GameObject.Find("VitaSoul").GetComponent<Transform>().parent = this.transform;
        }
    }
 
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            GameObject.Find("Player").GetComponent<Transform>().parent = null;
            GameObject.Find("VitaSoul").GetComponent<Transform>().parent = null;
        }
    }

}
