using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneManager_Skill2 : MonoBehaviour
{
    [SerializeField]
    private GameObject magicLightObj;
    private magicLight magicLightScript;

    [SerializeField]
    private GameObject VitaSoul;
    private VitaSoul_particle VitaParticleScript;
    private GazeMovement VitaParticleGazeScript;
    private SpriteRenderer VitaSoulRenderer;


    private const int WallNUM = 3;
    [SerializeField]
    private GameObject[] Wall = new GameObject [WallNUM];
    private Dislove[] WallDisolveScript = new Dislove[WallNUM];

    private bool bVitaSoulCanGaze = false;

    private float VitaSoulCanGazeTimer = 0.0f;

    [SerializeField]
    private Image SkillIcon;


    [SerializeField]
    private Image SkillName;


    private IEnumerator coroutine;

    // Start is called before the first frame update
    void Start()
    {
        magicLightScript = magicLightObj.GetComponent<magicLight>();

        VitaParticleScript = VitaSoul.GetComponent<VitaSoul_particle>();
        VitaParticleGazeScript = VitaSoul.GetComponent<GazeMovement>();

        for (int i = 0; i < 3; i++)
        {
            WallDisolveScript[i] = Wall[i].GetComponent<Dislove>();
        }

        VitaParticleScript.MoveSpeed = 9.5f;

        VitaSoulRenderer = VitaSoul.GetComponent<SpriteRenderer>();

        //coroutine = FadingVitaSoulIEnumerator();
    }

    // Update is called once per frame
    void Update()
    {
        //////////////////////////////偷吃步
        if (!bVitaSoulCanGaze)
        {
            VitaParticleScript.FollowObj();
        }

        if (Input.GetButtonDown("skillOne") || Input.GetButtonDown("skillTwo"))
        {
            bVitaSoulCanGaze = false; 
            VitaParticleGazeScript.bVitaSoulCanGaze = bVitaSoulCanGaze;

            //start prompt
            VitaParticleScript.PromptFadeIn();
            
            FadeInUI();

            DelayCanGaze(1.8f); //wait for raise hand time

        }

        //count to 2.0f can't gaze
        if (bVitaSoulCanGaze)
        {
            VitaSoulCanGazeTimer += Time.deltaTime;
            if (VitaSoulCanGazeTimer > 6.5f)
            {
                bVitaSoulCanGaze = false;
                VitaParticleGazeScript.bVitaSoulCanGaze = bVitaSoulCanGaze;

                VitaParticleScript.StopSkillAfterTime(0.0f); //reset particle
                VitaSoulCanGazeTimer = 0.0f;


                FadeOutUI();

                VitaParticleScript.animator.SetBool("StartSkill", false);

                VitaSoulRenderer.color = new Color(VitaSoulRenderer.color.r, VitaSoulRenderer.color.g, VitaSoulRenderer.color.b, 1f);
                //StopCoroutine(coroutine);

            }
        }

        //////////////////////////////偷吃步

        //Vita touch magicWound => Light up Vita
        if (magicLightScript.LightUpVita)
        {
            VitaParticleScript.LightUpVita(magicLightScript.magicLt.color);
            VitaParticleScript.SkillNUM = PlayerSkill.CURRENTSKILL;

           
            VitaParticleScript.animator.SetBool("StartSkill" , true);
            //FadingVitaSoul(); // start vita soul fading

            magicLightScript.LightUpVita = false; // stop light 

        }


        //Wall Disolve
        for (int i = 0; i < 3; i++)
        {
            if (VitaParticleScript.SkillNUM == 2 && WallDisolveScript[i].canDisolve)
            {
                WallDisolveScript[i].FadeOut();
            }
        }


        

    }
    void FadeInUI()
        {
            //stop UI Ienumerator
            StopCoroutine(FadeInSkillIconIEnumerator());
            StopCoroutine(FadeOutSkillIconIEnumerator());

            StartCoroutine(FadeInSkillIconIEnumerator());
        }

        void FadeOutUI()
        {
            //stop UI Ienumerator
            StopCoroutine(FadeInSkillIconIEnumerator());
            StopCoroutine(FadeOutSkillIconIEnumerator());

            StartCoroutine(FadeOutSkillIconIEnumerator());
        }


        IEnumerator FadeInSkillIconIEnumerator()
        {
            

            SkillIcon.color = new Color(SkillIcon.color.r, SkillIcon.color.g, SkillIcon.color.b, 0.0f);
            SkillName.color = new Color(SkillIcon.color.r, SkillIcon.color.g, SkillIcon.color.b, 0.0f);

            yield return new WaitForSeconds(0.8f); //Wait for Raise hand
            for (float a = 0.0f; SkillIcon.color.a < 1.0f; a += 0.06f)
            {
                SkillIcon.color = new Color(SkillIcon.color.r, SkillIcon.color.g, SkillIcon.color.b, a);
                SkillName.color = new Color(SkillIcon.color.r, SkillIcon.color.g, SkillIcon.color.b, a);
                yield return new WaitForSeconds(0.05f);
            }
            SkillIcon.color = new Color(SkillIcon.color.r, SkillIcon.color.g, SkillIcon.color.b, 1.0f);
            SkillName.color = new Color(SkillIcon.color.r, SkillIcon.color.g, SkillIcon.color.b, 1.0f);
        }

        IEnumerator FadeOutSkillIconIEnumerator()
        {
            SkillIcon.color = new Color(SkillIcon.color.r, SkillIcon.color.g, SkillIcon.color.b, 1.0f);
            SkillName.color = new Color(SkillIcon.color.r, SkillIcon.color.g, SkillIcon.color.b, 1.0f);

            for (float a = 1.0f; SkillIcon.color.a > 0.0f; a -= 0.06f)
            {
                SkillIcon.color = new Color(SkillIcon.color.r, SkillIcon.color.g, SkillIcon.color.b, a);
                SkillName.color = new Color(SkillIcon.color.r, SkillIcon.color.g, SkillIcon.color.b, a);
                yield return new WaitForSeconds(0.05f);
            }
            SkillIcon.color = new Color(SkillIcon.color.r, SkillIcon.color.g, SkillIcon.color.b, 0.0f);
            SkillName.color = new Color(SkillIcon.color.r, SkillIcon.color.g, SkillIcon.color.b, 0.0f);
        }

        void DelayCanGaze(float time)
        {

            StopCoroutine(DelayCanGazeIEnumerator(time));

            StartCoroutine(DelayCanGazeIEnumerator(time));
        }


        IEnumerator DelayCanGazeIEnumerator(float time)
        {
            
            yield return new WaitForSeconds(time);
            bVitaSoulCanGaze = true;
            VitaParticleGazeScript.bVitaSoulCanGaze = bVitaSoulCanGaze;

            //close prompt
            VitaParticleScript.PromptFadeOut();

        }


        /*void FadingVitaSoul()
        {
           StartCoroutine(coroutine);
        }


        IEnumerator FadingVitaSoulIEnumerator()
        {
            StartCoroutine(StopFadingVitaSoulAfterIEnumerator(5.8f));
            for (float a = 0; true; a += 0.1f)
            {
                VitaSoulRenderer.color = new Color(VitaSoulRenderer.color.r, VitaSoulRenderer.color.g, VitaSoulRenderer.color.b, Mathf.Abs( Mathf.Sin(a) + 0.1f));
                yield return new WaitForSeconds(0.005f);
            }

        }

        IEnumerator StopFadingVitaSoulAfterIEnumerator(float time)
        {
            yield return new WaitForSeconds(time);
            VitaSoulRenderer.color = new Color(VitaSoulRenderer.color.r, VitaSoulRenderer.color.g, VitaSoulRenderer.color.b, 1f);
            StopCoroutine(coroutine);
        }*/
        
}
