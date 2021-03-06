﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
  
    //Horizontal move
    private float moveInput;

    public float speed = 5.0f;
    private Rigidbody2D rb;
    private bool faceRight = false;

    //Vertical move
    public Transform feetPos; //detector position
    public float checkRadius; //detect range
    public LayerMask[] whatIsGround; //which ground will trigger
    private float jumpForce = 8.0f;

    //can move or not
    [System.NonSerialized]
    public bool canMove_skill = true;
    public bool canMove_camera = true;

    //Animator
    public Animator animator;


    //jump
    bool isGrounded = false;
    int extraJumpsValue = 1;
    private int extraJumps = 0;
    int JumpState = -1;
    bool canMoveX = true;
    private Vector3 originalScale;

    bool bCanJump = true;
    float fJumpIntervalTime = 0.0f;

    Animator JumpEffect;

    /*//original value
    float fGravity;
    float fDrag;*/



    [System.NonSerialized]
    public bool bBearCatchPlayer = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        //if face right at the first
        if (transform.localScale.x < 0.0f)
            faceRight = true;

        //set original scale
        originalScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);


        //reset jump force
        jumpForce = Mathf.Abs(jumpForce * transform.localScale.y / 0.5f) ;

        //reset speed
        speed = Mathf.Abs(speed * transform.localScale.y / 0.5f);

        /*fGravity = this.gameObject.GetComponent<Rigidbody2D>().gravityScale;
        fDrag = this.gameObject.GetComponent<Rigidbody2D>().drag;*/

        //jump effect
        JumpEffect = GameObject.Find("JumpEffect").GetComponent<Animator>();

    }

 
    void FixedUpdate()
    {
        

        if (canMove_skill && canMoveX && canMove_camera)
        {
            moveInput = Input.GetAxis("Horizontal");
            Movement_x();
        }

        else
        {
            moveInput = 0.0f;
        }

    }


    void Update()
    {
        foreach (var item in whatIsGround)
        {
            isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, item); //check if on the exactly ground
            if (isGrounded)
                break;
        }
       
        //tell animator 
        animator.SetBool("Ground", isGrounded);

        //get how fast we are moving up or down from the rigid
        animator.SetFloat("Speed_Y", GetComponent<Rigidbody2D>().velocity.y);

        if (canMove_skill && canMove_camera)
        {
            Movement_y();
        }



        fJumpIntervalTime += Time.deltaTime;
        if (bCanJump ==false && fJumpIntervalTime > 0.15f )
        {
            bCanJump = true;
            fJumpIntervalTime = 0.0f;
        }

    }


    private void Movement_x()
    {
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
        //trun player
        if (moveInput > 0 && !faceRight)
        {
            transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
            

            faceRight = true;
        }
        else if (moveInput < 0 && faceRight)
        {
            transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);

            
            faceRight = false;
        }

        //set animate
        animator.SetFloat("Speed", Mathf.Abs(moveInput));

    }

    private void Movement_y()
    {
        //Debug.Log("JumpState = " + JumpState);
        /*if (!isGrounded)
            Debug.Log("!isGrounded");


        if(JumpState == 1)
            Debug.Log("JumpState = " + JumpState);*/


        //Debug.Log("JumpState = " + JumpState);

        //press jump
        if (isGrounded && Input.GetButtonDown("Jump") && canMove_skill && canMove_camera && (JumpState == -1 || JumpState > 2) && bCanJump)
        {
            //Debug.Log("Jump_1");
            

            JumpState++;

            WaitJumpState(0.15f);

            animator.Play("ReadyJump");

            //add music'
            if(FindObjectOfType<AudioManager>() != null)
                FindObjectOfType<AudioManager>().Play("PlayerJump");

            extraJumps = extraJumpsValue;

            canMoveX = false;
            rb.velocity = new Vector2(0, rb.velocity.y);

            bCanJump = false;

        }

        if (JumpState == 1)
        {
            //Debug.Log("Jump");
            
            //not on the ground
            animator.SetBool("Ground", false);

            //add jump force
            rb.velocity = Vector2.up * jumpForce;

            JumpState++;
        }

        //double jump
        if (Input.GetButtonDown("Jump") && extraJumps > 0 && !isGrounded && bCanJump)
        {
            JumpEffect.SetTrigger("tJump");

            //Debug.Log("Jump_2");
            JumpState = 1;

            animator.Play("ReadyJump");

            //add music
            if (FindObjectOfType<AudioManager>() != null)
                FindObjectOfType<AudioManager>().Play("PlayerJump");
            extraJumps--;

            bCanJump = false;
        }

        // finish jump 
        if (JumpState == 2 && isGrounded)
        {
            JumpState = -1;

        }


    }

    public void TurnFace()
    {
        faceRight = !faceRight;
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
    }

    bool OnTheGround()
    {
        bool bFlag = false;
        foreach (var item in whatIsGround)
        {
            bFlag = Physics2D.OverlapCircle(feetPos.position, checkRadius, item); //check if on the exactly ground
            if (bFlag)
                break;
        }
        return bFlag;
    }


    public void PlayerHurtAni(float fPreWaitTime)
    {
        StartCoroutine(PlayerHurtAniIEnumerator( fPreWaitTime));
    }

    IEnumerator PlayerHurtAniIEnumerator(float fPreWaitTime)
    {
        yield return new  WaitForSeconds(fPreWaitTime);

        animator.SetTrigger("tHurt");

    }



    public bool bPlayerMove
    {
        get
        {
            return (rb.velocity.x != 0.0f || rb.velocity.y != 0.0f || !OnTheGround());
        }

    }

    //wait for animation
    private void WaitJumpState(float time)
    {
        
        StopAllCoroutines();
        StartCoroutine(WaitJumpStateIEnumerator(time));
    }



    IEnumerator WaitJumpStateIEnumerator(float time)
    {
        yield return new WaitForSeconds(time);
        JumpState ++;
        canMoveX = true;

    }


    //detect bear
    private void OnTriggerStay2D(Collider2D collision)
    {
       
        if (collision.name == "DetectPlayer")
        {
            bBearCatchPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "DetectPlayer")
        {
            bBearCatchPlayer = false;
        }
    }



}