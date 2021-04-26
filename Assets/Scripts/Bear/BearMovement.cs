using System.Collections;
using UnityEngine;

public class BearMovement : MonoBehaviour
{
    Animator animator;
    ParticleSystem ps;

    [System.NonSerialized]
    public float fBearHurtTime = 4.0f;

    [System.NonSerialized]
    public float fBearHowlTime = 4.0f;

    [System.NonSerialized]
    public float fBearAttackTime = 2f;

    [System.NonSerialized]
    public float fBearAttackRightTime = 2.8f;


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
}
