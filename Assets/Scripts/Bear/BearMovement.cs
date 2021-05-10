using System.Collections;
using UnityEngine;

public class BearMovement : MonoBehaviour
{
    Animator animator;
    ParticleSystem ps;

    [System.NonSerialized]
    public float fBearHurtTime = 3.6f;

    [System.NonSerialized]
    public float fBearHowlTime = 4.0f;

    [System.NonSerialized]
    public float fBearAttackTime = 2f;

    [System.NonSerialized]
    public float fBearAttackRightTime = 1.4f;

    [System.NonSerialized]
    public bool bHitWall = false;

    //Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        animator = this.gameObject.GetComponent<Animator>();
        
        ps = this.gameObject.GetComponentInChildren<ParticleSystem>();

        //camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        //fix bear balance
        if (Input.GetKeyDown(KeyCode.B))
        {
            this.gameObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        }
    }

    public void Howl()
    {
        animator.SetTrigger("tHowl");
       
        //add music
        if(FindObjectOfType<AudioManager>().isPlaying("BearStay")) FindObjectOfType<AudioManager>().Pause("BearStay");

        //add music bg2
        if (FindObjectOfType<AudioManager>().isPlaying("Scenebg"))
        {
            FindObjectOfType<AudioManager>().Pause("Scenebg");
            FindObjectOfType<AudioManager>().Play("bg2_Run");
        }
        
        FindObjectOfType<AudioManager>().Play("BearHowl");
        
        StopCoroutine(HowlRippleIEnumerator(0.8f));
        StartCoroutine(HowlRippleIEnumerator(0.8f));
    }

    public void HowlRipple(float fWaitRippleTime)
    {
        StopCoroutine(HowlRippleIEnumerator( fWaitRippleTime));
        StartCoroutine(HowlRippleIEnumerator(fWaitRippleTime));
    }


    IEnumerator HowlRippleIEnumerator(float fWaitRippleTime)
    {
        
        yield return new WaitForSeconds(fWaitRippleTime);

        ps.Play();


    }

    public void Run()
    {
        
    }

    public void Hurt()
    {

        this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 0.0f);

        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.1f, 0.1f);
        animator.SetTrigger("tHurt");

        //add music
        FindObjectOfType<AudioManager>().Play("BearHowl");

        StartCoroutine(resetHurtIEnumerator());
    }

    IEnumerator resetHurtIEnumerator()
    {
        yield return new WaitForSeconds(fBearHurtTime);
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f);

    }

    public void Attack(ParticleSystem DamageParticle, GameObject DestoryGameObject)
    {
        this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 0.0f);
        
        StartCoroutine(AttackIEnumerator( DamageParticle,  DestoryGameObject));
       
        
    }

    IEnumerator AttackIEnumerator(ParticleSystem DamageParticle, GameObject DestoryGameObject)
    {
        this.gameObject.GetComponent<Animator>().SetTrigger("tAttack");


        yield return new WaitForSeconds(0.9f);
        DamageParticle.Play();
        Destroy(DestoryGameObject);

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.name == "DetectBearTrigger")
        {
            bHitWall = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "DetectBearTrigger")
        {
            bHitWall = false;
        }
    }

}
