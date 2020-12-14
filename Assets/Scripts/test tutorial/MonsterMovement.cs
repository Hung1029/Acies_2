using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMovement : MonoBehaviour
{
    public BoxCollider2D MoveDetectCollider;
    
    //Detect if touch player
    public BoxCollider2D PlayerDetectCollider1;
    private BoxCollider2D PlayerCollider;
    private Transform PlayerTransform;
    bool bFollow = false;
    float fDistance = 0.75f;

    private Rigidbody2D rb;

    float speed = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        PlayerCollider = GameObject.Find("Player").GetComponent<BoxCollider2D>();
        PlayerTransform = GameObject.Find("Player").GetComponent<Transform>();
    }



    // Update is called once per frame
    void Update()
    {

        //monster idle
        if (!bFollow)
        {
            MonsterIdleMovement();
            
            //Speed trun face
            if (speed > 0.0f && transform.localScale.x > 0.0f)
            {
                speed *= -1;
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            }
            else if (speed < 0.0f && transform.localScale.x < 0.0f)
            {
                speed *= -1;
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            }
        }
        //monster follow player
        else
        {
            FollowPlayer(); 
            
            //player on the right side 
            if (transform.position.x < PlayerTransform.position.x && transform.localScale.x > 0.0f)
            {
                speed *= -1;
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            }

            //player on the left side
            else if (transform.position.x > PlayerTransform.position.x && transform.localScale.x < 0.0f)
            {
               speed *= -1;
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            }
        }



     
    }

    public void MonsterIdleMovement()
    {
        //monster idle movement
        transform.position = new Vector2(transform.position.x + (speed * Time.deltaTime), transform.position.y);

    }

    public void FollowPlayer()
    {
        
        // more than follow distance
        if (Vector3.Distance(transform.position, PlayerTransform.position) > fDistance)
        {

            transform.position = Vector3.MoveTowards(new Vector3(transform.position.x, transform.position.y, transform.position.z), new Vector3(PlayerTransform.position.x, transform.position.y, transform.position.z), 3.0f * Time.deltaTime);

        }


    }


    void OnTriggerExit2D(Collider2D other)
    {

        //if game object away from floor
        if (!MoveDetectCollider.IsTouchingLayers(LayerMask.GetMask("Floor")))
        {
            speed *= -1;
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }

        //if game object away from Detect player
        if (!PlayerDetectCollider1.IsTouching(PlayerCollider) )
        {
            bFollow = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //if game object Detect player
        if (PlayerDetectCollider1.IsTouching(PlayerCollider) )
        {
            bFollow = true;
        }
      
    }


}
