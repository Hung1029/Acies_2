using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class VitaSoul_particle : MonoBehaviour
{
    [System.NonSerialized]
    public float MoveSpeed = 2.5f; 
    [System.NonSerialized]
    public bool bMoveFinish = false;

    [System.NonSerialized]
    public bool canDriveOut = false;
    
    public  GameObject OuterParticleObj;
    private ParticleSystem OuterParticleSys;

    [SerializeField]
    private GameObject PromptArrow;
    private ColorChange PromptColorChange;

    private GameObject VitaSoulPicture;
    private SpriteRenderer VitaSoulPictureRenderer;

    private SpriteRenderer VitaSoulCoreSprite;

    private TrailRenderer ParticleTrail;

    [System.NonSerialized]
    public int SkillNUM = 0;

    [System.NonSerialized]
    //time for skill one need to gathering
    public float fSkillOneGatheringTime = 2.0f; 



    public Animator animator;

    //Follow Player
    private Transform target;
    private float stoppingDistance_x = 1f;
    private float stoppingDistance_y = 0.57f;

    private Rigidbody2D rb;

    [System.NonSerialized]
    public bool bCanFollow = true;

    IEnumerator RecordPrompt = null;

    // Start is called before the first frame update
    void Start()
    {

        //rescale stopping distance x
        stoppingDistance_x =  Mathf.Abs(stoppingDistance_x * transform.localScale.y / 0.1472596f);

        //rescale stopping distance y
        stoppingDistance_y = Mathf.Abs(stoppingDistance_y * transform.localScale.y / 0.1472596f);

        //particle system
        OuterParticleSys = OuterParticleObj.GetComponent<ParticleSystem>();

        //Trail
        ParticleTrail = OuterParticleObj.GetComponent<TrailRenderer>();

        //follow player
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();


        //prompt arrow
        PromptColorChange = PromptArrow.GetComponent<ColorChange>();


        rb = this.GetComponent<Rigidbody2D>();

        VitaSoulCoreSprite = GameObject.Find("VitaSoulCore").GetComponent<SpriteRenderer>();

    }

    public void LightUpVita(Color lightColor)
    {
        ParticleTrail.startColor = lightColor;
        ParticleTrail.endColor = new Color(1.0f, 1.0f, 1.0f, 0.0f);

        VitaSoulCoreSprite.color = lightColor;

        this.GetComponent<SpriteRenderer>().color = lightColor;

        //set Skill
        if (PlayerSkill.CURRENTSKILL == 1)
            canDriveOut = true;
        

    }



    public void VitaSpriteFadeIn()
    {
        VitaSoulPictureRenderer = this.transform.GetComponent<SpriteRenderer>();
        
        //stop IEnumerator
       StartCoroutine(VitaSpriteFadeInIEnumerator());
    }

    IEnumerator VitaSpriteFadeInIEnumerator() //Vita core
    {

        for (float a = 0.0f; a < 1.0f; a += 0.4f)
        {

            VitaSoulPictureRenderer.color = new Color(VitaSoulPictureRenderer.color.r, VitaSoulPictureRenderer.color.g, VitaSoulPictureRenderer.color.b, a);
            yield return new WaitForSeconds(0.15f);
        }
        VitaSoulPictureRenderer.color = new Color(VitaSoulPictureRenderer.color.r, VitaSoulPictureRenderer.color.g, VitaSoulPictureRenderer.color.b, 1.0f);

    }

    public void PromptFadeIn()
    {
       
        PromptColorChange.ColorChanging(new Color(1.0f, 1.0f, 1.0f, 1.0f), 0.5f);

    }

    public void PromptFadeOut()
    {
      
        PromptColorChange.ColorChanging(new Color(1.0f, 1.0f, 1.0f, 0.0f), 0.5f);


    }
    
    ////////////////////////////////////////////////////////////////////////////////////Vita Soul Core Fade in 
    public IEnumerator VitaSoulCoreFadeIn()
    {
        StopCoroutine("VitaSoulCoreFadeOut");

        this.gameObject.GetComponent<ParticleSystem>().enableEmission = true;
        OuterParticleSys.enableEmission = true;
        ParticleTrail.enabled = true;

        float fTimer = 0.0f;
        while (VitaSoulCoreSprite.color.a != 1.0f)
        {
            VitaSoulCoreSprite.color = Color.Lerp(new Color(VitaSoulCoreSprite.color.r, VitaSoulCoreSprite.color.g, VitaSoulCoreSprite.color.b, 0.0f), new Color(VitaSoulCoreSprite.color.r, VitaSoulCoreSprite.color.g, VitaSoulCoreSprite.color.b, 1.0f), fTimer);
            fTimer += Time.deltaTime / 1.0f;

            yield return null;
        }
    }


    ////////////////////////////////////////////////////////////////////////////////////Vita Soul Core Fade out 
    public IEnumerator VitaSoulCoreFadeOut()
    {
        StopCoroutine("VitaSoulCoreFadeIn");

        this.gameObject.GetComponent<ParticleSystem>().enableEmission = false;
        OuterParticleSys.enableEmission = false;
        ParticleTrail.enabled = false;

        float fTimer = 0.0f;
        while (VitaSoulCoreSprite.color.a != 0.0f)
        {
            VitaSoulCoreSprite.color = Color.Lerp(new Color(VitaSoulCoreSprite.color.r, VitaSoulCoreSprite.color.g, VitaSoulCoreSprite.color.b, 1.0f), new Color(VitaSoulCoreSprite.color.r, VitaSoulCoreSprite.color.g, VitaSoulCoreSprite.color.b, 0.0f), fTimer);

            fTimer += Time.deltaTime / 1.0f;
            yield return null;
        }
    }
    ////////////////////////////////////////////////////////////////////////////////////


    public void StopSkillAfterTime(float time)
    {
       
        StopAllCoroutines();
        StartCoroutine(FadeOutAfterTimeIEnumerator( time));
    }

    IEnumerator FadeOutAfterTimeIEnumerator(float time)
    {
        yield return new WaitForSeconds(time);
       
      

        //reset
        reset();

    }

    public void reset()
    {
        this.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        ParticleTrail.startColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        ParticleTrail.endColor = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        canDriveOut = false;

    }


    public void MoveToward(Vector2 target)
    {
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(target.x, target.y), MoveSpeed * Time.deltaTime);
        if (transform.position.x == target.x && transform.position.y == target.y)
        {
            bMoveFinish = true;
        }
        else
        {
            bMoveFinish = false;
        }
    }

    bool bChangeFace = true;

    public void FollowObj()
    {
        if (bCanFollow)
        {
            float moveInput = Input.GetAxis("Horizontal");

            if (GameObject.Find("Player").GetComponent<PlayerMovement>().canMove_skill && GameObject.Find("Player").GetComponent<PlayerMovement>().canMove_camera)
            {
                //change Scale
                if ((moveInput > 0 && transform.localScale.x < 0.0f) || (moveInput < 0 && transform.localScale.x > 0.0f))
                {
                    transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
                    bChangeFace = true;

                }
            }

            //change X
            if (bChangeFace || Mathf.Abs(transform.position.x - target.position.x) > stoppingDistance_x)
            {
                //change vita soul position
                if (transform.localScale.x > 0) // face left
                {
                    if (transform.position.x != target.position.x - stoppingDistance_x)
                        transform.position = Vector2.MoveTowards(transform.position, new Vector2(target.position.x - stoppingDistance_x, transform.position.y), 10.0f * Time.deltaTime);
                    else
                        bChangeFace = false;
                }
                else // face right
                {
                    if (transform.position.x != target.position.x + stoppingDistance_x)
                        transform.position = Vector2.MoveTowards(transform.position, new Vector2(target.position.x + stoppingDistance_x, transform.position.y), 10.0f * Time.deltaTime);
                    else
                        bChangeFace = false;
                }
            }
            else if (Mathf.Abs(transform.position.x - target.position.x) > stoppingDistance_x) // vita move with player
            {
                rb.velocity = new Vector2(moveInput * 5.0f, rb.velocity.y);
            }




            //change Y
            if (target.position.y + stoppingDistance_y != transform.position.y)
            {
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, target.position.y + stoppingDistance_y), 10.0f * Time.deltaTime); //only move x
            }
        }
    }


}
