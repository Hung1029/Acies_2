using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_following : MonoBehaviour
{

    private Rigidbody2D rb;
    //Horizontal move
    private float moveInput;
    private float speed = 5.0f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        moveInput = Input.GetAxis("Horizontal");
        Movement_x();
        
    }

    private void Movement_x()
    {
        rb.velocity = new Vector2(moveInput * speed,0.0f);

    }

}
