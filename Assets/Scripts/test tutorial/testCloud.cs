using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testCloud : MonoBehaviour
{
    public ParticleSystem ps;

    // these lists are used to contain the particles which match
    // the trigger conditions each frame.
    List<ParticleSystem.Particle> inside = new List<ParticleSystem.Particle>();
    List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();

    ParticleSystem.Particle[] particlesAlive;

    float fPlayerHurtTimer = 4.0f;
    bool bInHurtAnimate = false;

    ParticleSystem.Particle[] _Particles;

    bool bExplo = false;

    int numInside;
    int numEnter;

    [System.NonSerialized]
    public bool _bLightBlowEnable = true;

    public GameObject Target;
    public GameObject Target2;


    void Start()
    {
        ps = this.gameObject.GetComponent<ParticleSystem>();
        Target2.GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>().enabled = false;
    }
    void Update()
    {
        // set light color
        float t = Mathf.PingPong(Time.time, 1.0f) / 1.0f;
        if (_bLightBlowEnable == true)
        {
            Target.GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>().intensity += t;
            
        }
        else
        {
            Target.GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>().intensity = 1.5f;
            Target2.GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>().enabled = true;
        }
    }

    void InitializeIfNeeded()
    {
        if (ps == null)
            ps = GetComponent<ParticleSystem>();

        if (_Particles == null || _Particles.Length < ps.main.maxParticles)
            _Particles = new ParticleSystem.Particle[ps.main.maxParticles];
    }

    void OnParticleTrigger()
    {
        numInside = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Inside, inside);
        numEnter = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
        if (PlayerSkill.CURRENTSKILL == 1)
        {
            ////////////////////////////////////////////////////////////////////////Vita can fade out fog
            // iterate through the particles which inside the trigger 
            for (int i = 0; i < numInside ; i++)
            {
            
                if (PlayerSkill.CURRENTSKILL == 1)
                {
                    ParticleSystem.Particle p = inside[i];

                    //color change
                    p.color = new Color32(p.color.r, p.color.g, p.color.b, (byte)(p.color.a - 10));

                    //Collision effect
                    Vector3 thePosition = ps.transform.TransformPoint(p.position.x, p.position.y, 0);
                    float x = thePosition.x - GameObject.Find("VitaSoul").GetComponent<Transform>().position.x;
                    float y = thePosition.y - GameObject.Find("VitaSoul").GetComponent<Transform>().position.y;

                    p.velocity = new Vector3(x * 10.0f, y * 10.0f);


                    //life time loss
                    if (p.color.a <= 175)
                    {
                        p.remainingLifetime = 0;
                    }

                    //set enter
                    inside[i] = p;
                    // re-assign the modified particles back into the particle system
                    ps.SetTriggerParticles(ParticleSystemTriggerEventType.Inside, inside);
                }


            }

            // iterate through the particles which enter the trigger 
            for (int i = 0; i < numEnter; i++)
            {

                if (PlayerSkill.CURRENTSKILL == 1)
                {
                    ParticleSystem.Particle p = enter[i];

                    //color change
                    p.color = new Color32(p.color.r, p.color.g, p.color.b, (byte)(p.color.a - 10));

                    //Collision effect
                    Vector3 thePosition = ps.transform.TransformPoint(p.position.x, p.position.y, 0);
                    float x = thePosition.x - GameObject.Find("VitaSoul").GetComponent<Transform>().position.x;
                    float y = thePosition.y - GameObject.Find("VitaSoul").GetComponent<Transform>().position.y;

                    p.velocity = new Vector3(x * 10.0f, y * 10.0f);


                    //life time loss
                    if (p.color.a <= 175)
                    {
                        p.remainingLifetime = 0;
                    }

                    //set enter
                    enter[i] = p;
                    // re-assign the modified particles back into the particle system
                    ps.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
                }


            }
        }



        ////////////////////////////////////////////////////////////////////////Dectect Player hurt by fog

        if (PlayerSkill.CURRENTSKILL == 0 && numInside + numEnter> 0 && fPlayerHurtTimer > 1.0f && bExplo==false)
        {
            GameObject.Find("Player").GetComponent<PlayerHealth>().Hurt();
        }
    }

    public void FadeOutAndDestory( Vector3 vCandlePosition)
    {
        StartCoroutine(FadeOutAndDestoryIEnumerator(vCandlePosition));
        
    }
    
    IEnumerator FadeOutAndDestoryIEnumerator(Vector3 vCandlePosition)
    {
        bExplo = true;
        _bLightBlowEnable = false;

        ps.Stop();
        InitializeIfNeeded();

        int numParticlesAlive = ps.GetParticles(_Particles);
       

        //candle position 47 , 2.74
        Vector3 candleLocalPosition = this.transform.InverseTransformPoint(vCandlePosition);

        //disable collision
        var coll = ps.collision;
        coll.enabled = false;

        for (int index = 0; index < numParticlesAlive; index++)
        {
            //Fog particle explotion
            _Particles[index].velocity = Vector3.Normalize(_Particles[index].position - candleLocalPosition) * 2.0f;

            //set particle remain life time
            _Particles[index].remainingLifetime = 10.0f;

            // Apply the particle changes to the Particle System
            ps.SetParticles(_Particles, numParticlesAlive);
           
        }       

        yield return new WaitForSeconds(10.0f);


        Destroy(this.gameObject) ;

    }



}
