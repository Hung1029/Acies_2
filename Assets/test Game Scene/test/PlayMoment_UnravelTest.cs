using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMoment_UnravelTest : MonoBehaviour
{
    //Horizontal move
    private float moveInput;

    private float speed = 5.0f;
    private Rigidbody2D rb;
    private bool faceRight = false;

    //Vertical move
    public Transform feetPos; //detector position
    public float checkRadius; //detect range
    public LayerMask whatIsGround; //which ground will trigger
    public float jumpForce = 13.0f;

    //can move or not
    public bool canMove = true;

    //Animator
    public Animator animator;


    //jump
    bool isGrounded = false;
    public int extraJumpsValue = 2;
    private int extraJumps;
    int JumpState = 0;

    private Vector3 originalScale;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        //if face right at the first
        if (transform.localScale.x < 0.0f)
            faceRight = true;

        //set original scale
        originalScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);



    }


    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround); //check if on the exactly ground

        //tell animator 
        animator.SetBool("Ground", isGrounded);

        //get how fast we are moving up or down from the rigid
        animator.SetFloat("Speed_Y", GetComponent<Rigidbody2D>().velocity.y);
    }


    void Update()
    {


        if (canMove)
        {
            moveInput = Input.GetAxis("Horizontal");
            Movement_x();
            Movement_y();
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

        //press jump
        if (isGrounded && Input.GetButtonDown("Jump") && canMove)
        {

            JumpState = 0;

            animator.SetBool("FinishJump", false);

            WaitJumpState(0.15f);

            animator.Play("ReadyJump");

            extraJumps = extraJumpsValue;

        }

        if (JumpState == 1)
        {
            //not on the ground
            animator.SetBool("Ground", false);

            //add jump force
            rb.velocity = Vector2.up * jumpForce;

            JumpState++;
        }

        //double jump
        if (Input.GetButtonDown("Jump") && extraJumps > 0 && !isGrounded)
        {
            JumpState = 0;

            WaitJumpState(0.15f);

            animator.Play("ReadyJump");


            extraJumps--;
        }

        //detect "FinishJump" animate start 
        if (JumpState == 2 && isGrounded)
        {
            JumpState++;
            WaitJumpState(0.25f);

        }

        //close "FinishJump" animate
        if (JumpState == 4)
        {
            animator.SetBool("FinishJump", true);
        }

    }

    public void TurnFace()
    {
        faceRight = !faceRight;
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
    }



    public bool bPlayerMove
    {
        get
        {
            return (moveInput != 0 || !Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround));
        }

    }

    private void WaitJumpState(float time)
    {

        StopAllCoroutines();
        StartCoroutine(WaitJumpStateIEnumerator(time));
    }



    IEnumerator WaitJumpStateIEnumerator(float time)
    {
        yield return new WaitForSeconds(time);
        JumpState++;

    }
}
