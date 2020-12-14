using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speedX = 2.5f; //X方向的速度
    private float speedY = 1.0f; //Y方向的速度
    public float speed = 0.01f;
    private Transform sisterSoulTrans ;
    private float cloudMovement = 150.0f;

    private VitaSoul_particle sisterCanDrive;
    private float driveOutSpeedX;
    private float driveOutSpeedY;
    private float driveOutCounter = 0.0f;
    private bool driveOutStart = false;

    private int driveOutCount = 0;

    int changeAlpha = 0;

    [SerializeField]
    private GameObject VitaSoul;
    private VitaSoul_particle VitaParticleScript;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        sisterSoulTrans = GameObject.Find("VitaSoul").GetComponent<Transform>();

        sisterCanDrive = GameObject.Find("VitaSoul").GetComponent<VitaSoul_particle>();

        VitaParticleScript = VitaSoul.GetComponent<VitaSoul_particle>();
    }

    void FixedUpdate()
    {
        if (!driveOutStart)
        {
            rb.velocity = new Vector2(0.0f, 0.0f);
            //rb.velocity = new Vector2(speedX * speed, speedY * speed);
        }

        else
        {
            changeAlpha++;
            if(changeAlpha == 1)
                this.gameObject.GetComponent<SpriteRenderer>().color = new Vector4(this.gameObject.GetComponent<SpriteRenderer>().color.r, this.gameObject.GetComponent<SpriteRenderer>().color.g, this.gameObject.GetComponent<SpriteRenderer>().color.b, this.gameObject.GetComponent<SpriteRenderer>().color.a * 0.5f);
            rb.velocity = new Vector2(driveOutSpeedX * speed, driveOutSpeedY * speed);
            driveOutCounter += Time.deltaTime;
            if (driveOutCounter > 0.8f)
            {
                driveOutStart = false;
                driveOutCounter = 0.0f;
                driveOutCount++;
                changeAlpha = 0;
                cloudMovement = 250f;
                if (driveOutCount >= 2)
                    Destroy(this.gameObject);
                 
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
      

    }
   
    void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.name)
        {
            case "limit_left":
                speedX *= -1;
                break;

            case "limit_right":
                speedX *= -1;
                break;

            case "limit_top":
                speedY *= -1;
                break;

            case "limit_bottom":
                speedY *= -1;
                break;
            default:
                break;
        }
         


    }

    
    public float varY = 0.01f;
    void OnTriggerStay2D(Collider2D other)
    {
        switch (other.name)
        {
            case "zoneA":
                break;
            case "zoneB":
               
                speedY += varY;
                break;
            case "zoneC":
                
                speedY -= varY;
                break;
            case "zoneD":
                
                speedY += varY;
                break;
            case "zoneE":
                
                speedY -= varY;
                break;
            case "zoneF":
                
                speedY += varY;
                break;
            case "zoneG":
               
                speedY -= varY;
                break;
            case "zoneH":
                break;
            default:
                break;
        }
        
        if (other.name == "VitaSoul" && sisterCanDrive.canDriveOut && VitaParticleScript.SkillNUM == 1)
        {
            
            float x = this.gameObject.GetComponent<Transform>().position.x - sisterSoulTrans.position.x;
            float y = this.gameObject.GetComponent<Transform>().position.y - sisterSoulTrans.position.y ;
            float l =Mathf.Sqrt( x*x + y*y);
            driveOutSpeedX = x / l * cloudMovement;
            driveOutSpeedY = y / l * cloudMovement;
            driveOutStart = true;

        }


    }


}
