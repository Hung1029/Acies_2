using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class SceneManagerLevel3_final : MonoBehaviour
{
    [System.Serializable]
    public struct LightFadeSetting
    {
        public Light2D light;
        public float targetIntensity;
        public float preTime ;
        public float duration;
    }


    public GameObject Vita;
    private VitaSoul_particle VitaParticleScript;

    public SkillOneTriggerIcon MuralTrigger1_1;
    //public ColorChange MuralSprite1;
    public ColorChange StoryMaterialSprite1;
    public LightFadeSetting[] Mural1LightSetting;
    bool bTriggerMurl1 = false;

    public SkillOneTriggerIcon MuralTrigger2_1;
    public SkillOneTriggerIcon MuralTrigger2_2;
    public ColorChange TriggerCircle2_1;
    public ColorChange TriggerCircle2_2;
    public LightFadeSetting[] Mural2LightSetting;
    //public ColorChange MuralSprite2;
    public ColorChange StoryMaterialSprite2_1;
    public ColorChange StoryMaterialSprite2_2;
    public ColorChange StoryMaterialSprite2_3;
    bool bTriggerMurl2 = false;

    public SkillOneTriggerIcon MuralTrigger4;
    public LightFadeSetting[] Mural4LightSetting;
    public ColorChange TriggerCircle4;
    //public ColorChange MuralSprite4;
    public ColorChange StoryMaterialSprite4_1;
    public ColorChange StoryMaterialSprite4_2;
    bool bTriggerMurl4 = false;


    //light up description
    public ColorChange Description1;
    public ColorChange title1;
    public ColorChange text1;
    public ColorChange continueA1;

    public ColorChange Description2;
    public ColorChange title2;
    public ColorChange text2;
    public ColorChange continueA2;

    public ColorChange Description3;
    public ColorChange title3;
    public ColorChange text3_1;
    public ColorChange text3_2;
    public ColorChange continueA3;

    public ColorChange Description4;
    public ColorChange title4;
    public ColorChange text4;
    public ColorChange continueA4;


    bool bTriggerMural3 = false;


    //mural submit
    public GameObject A1;
    public GameObject A2;
    public GameObject A3;
    public GameObject A4;
    bool A1_canDetectPlayer;
    bool A2_canDetectPlayer;
    bool A3_canDetectPlayer;
    bool A4_canDetectPlayer;

    bool canDetectTrigger1 = false;
    bool bLastDetectState1 = false;

    bool canDetectTrigger2 = false;
    bool bLastDetectState2 = false;

    bool canDetectTrigger3 = false;
    bool bLastDetectState3 = false;

    bool canDetectTrigger4 = false;
    bool bLastDetectState4 = false;

    bool openDes1 = false;
    bool openDes2 = false;
    bool openDes3 = false;
    bool openDes4 = false;

    public GameObject door;

    public PlayerTriggerDetect rightBoundary;

    private LevelLoader levelLoaderScript;

    [SerializeField]
    private GameObject levelLoader;
    // Start is called before the first frame update
    void Start()
    {
        VitaParticleScript = Vita.GetComponent<VitaSoul_particle>();

        //set trigger
        MuralTrigger2_1.VitaDetect.bCanBeDetect = false;
        MuralTrigger2_2.VitaDetect.bCanBeDetect = false;
        MuralTrigger4.VitaDetect.bCanBeDetect = false;

        //controlChangeScene
        levelLoaderScript = levelLoader.GetComponent<LevelLoader>();

    }

    private void FixedUpdate()
    {
        VitaParticleScript.FollowObj();

        

    }

    // Update is called once per frame
    void Update()
    {

        //detect trigger
        if (MuralTrigger1_1.bTriggerFinish == false)
            MuralTrigger1_1.DetectFinish();

        if (MuralTrigger2_1.bTriggerFinish == false && bTriggerMurl1)
            MuralTrigger2_1.DetectFinish();

        if(MuralTrigger2_2.bTriggerFinish == false && bTriggerMurl1)
            MuralTrigger2_2.DetectFinish();

        if (MuralTrigger4.bTriggerFinish == false && bTriggerMurl2)
            MuralTrigger4.DetectFinish();

        //murl 1 trigger
        if (MuralTrigger1_1.bTriggerFinish && bTriggerMurl1 == false)
        {

            //stop skill
            SkillManager_v2.bFinishSkill = true;
            StartCoroutine(ReverseSkillFinish(1.0f));

            //MuralSprite1.ColorChanging(new Color(1.0f, 1.0f, 1.0f, 1.0f) , 3.0f );
            StoryMaterialSprite1.ColorChanging(new Color(1.0f, 1.0f, 1.0f, 1.0f), 3.0f);

            //light setting
            for (int i = 0; i < Mural1LightSetting.Length; i++)
            {
                StartCoroutine(LightIntensity(Mural1LightSetting[i]));
            }

            //light up murl two light
            TriggerCircle2_1.ColorChanging(new Color(1.0f, 1.0f, 1.0f, 1.0f), 3.0f,3.0f);
            TriggerCircle2_2.ColorChanging(new Color(1.0f, 1.0f, 1.0f, 1.0f), 3.0f,3.0f);
            MuralTrigger2_1.VitaDetect.bCanBeDetect = true;
            MuralTrigger2_2.VitaDetect.bCanBeDetect = true;

            //start story
            StartCoroutine(Story1());


           
           

            bTriggerMurl1 = true;
        }

        //murl 2 trigger
        if (MuralTrigger2_1.bTriggerFinish && MuralTrigger2_2.bTriggerFinish && bTriggerMurl2 == false  && bTriggerMurl1)
        {
            //set camera
            //this.gameObject.GetComponent<CameraManager>().ShortFollowing(5.0f, new Vector3(3.47f, Camera.main.transform.position.y, Camera.main.transform.position.z));

            //MuralSprite2.ColorChanging(new Color(1.0f, 1.0f, 1.0f, 1.0f), 3.0f);
            StoryMaterialSprite2_1.ColorChanging(new Color(1.0f, 1.0f, 1.0f, 1.0f), 3.0f);
            StoryMaterialSprite2_2.ColorChanging(new Color(1.0f, 1.0f, 1.0f, 1.0f), 3.0f);
            StoryMaterialSprite2_3.ColorChanging(new Color(1.0f, 1.0f, 1.0f, 1.0f), 3.0f);

            //stop skill
            SkillManager_v2.bFinishSkill = true;
            StartCoroutine(ReverseSkillFinish(1.0f));

            //light setting
            for (int i = 0; i < Mural2LightSetting.Length; i++)
            {
                StartCoroutine(LightIntensity(Mural2LightSetting[i]));
            }

           

            //start story
            StartCoroutine(Story2());


            bTriggerMurl2 = true;
        }


        //mural 3
        if (!bTriggerMural3 && GameObject.Find("Player").transform.position.x > 12.82f && bTriggerMurl2)
        {
            StartCoroutine(Story3());


            //light up murl  light
            TriggerCircle4.ColorChanging(new Color(1.0f, 1.0f, 1.0f, 1.0f), 3.0f, 3.0f);
            MuralTrigger4.VitaDetect.bCanBeDetect = true;

            bTriggerMural3 = true;
        }



        //murl 4 trigger
        if (MuralTrigger4.bTriggerFinish && bTriggerMurl4 == false  && bTriggerMural3)
        {
            
            //set camera
            //this.gameObject.GetComponent<CameraManager>().ShortFollowing(5.0f, new Vector3(19.46f, Camera.main.transform.position.y, Camera.main.transform.position.z));

            //stop skill
            SkillManager_v2.bFinishSkill = true;
            StartCoroutine(ReverseSkillFinish(1.0f));

            //MuralSprite4.ColorChanging(new Color(1.0f, 1.0f, 1.0f, 1.0f), 3.0f);
            StoryMaterialSprite4_1.ColorChanging(new Color(1.0f, 1.0f, 1.0f, 1.0f), 3.0f);
            StoryMaterialSprite4_2.ColorChanging(new Color(1.0f, 1.0f, 1.0f, 1.0f), 3.0f);

            //light setting
            for (int i = 0; i < Mural4LightSetting.Length; i++)
            {
                StartCoroutine(LightIntensity(Mural4LightSetting[i]));
            }

            //start story
            StartCoroutine(Story4());


            bTriggerMurl4 = true;
        }



        //detect 1
        if (bLastDetectState1 != A1.GetComponent<PlayerTrigger>()._bPlayerTrigger)
        {
            canDetectTrigger1 = true;
        }

        if (canDetectTrigger1 && A1.GetComponent<PlayerTrigger>()._bPlayerTrigger && A1_canDetectPlayer)
        {
            canDetectTrigger1 = false;
            A1.GetComponent<ColorChange>().ColorChanging(new Color(1.0f, 1.0f, 1.0f, 1.0f) , 0.5f);
        }
        else if (canDetectTrigger1 && !A1.GetComponent<PlayerTrigger>()._bPlayerTrigger && A1_canDetectPlayer)
        {
            canDetectTrigger1 = false;
            A1.GetComponent<ColorChange>().ColorChanging(new Color(1.0f, 1.0f, 1.0f, 0.0f), 0.5f);
        }

        if (!openDes1 && A1_canDetectPlayer && Input.GetButtonDown("Submit") && A1.GetComponent<PlayerTrigger>()._bPlayerTrigger)
        {
           
            //stop skill
            SkillManager_v2.bFinishSkill = true;
            StartCoroutine(ReverseSkillFinish(1.0f));
            StartCoroutine(Story1());
        }

        bLastDetectState1 = A1.GetComponent<PlayerTrigger>()._bPlayerTrigger;


        //detect 2

        if (bLastDetectState2 != A2.GetComponent<PlayerTrigger>()._bPlayerTrigger)
        {
            canDetectTrigger2 = true;
        }

        if (canDetectTrigger2 && A2.GetComponent<PlayerTrigger>()._bPlayerTrigger && A2_canDetectPlayer)
        {
            canDetectTrigger2 = false;
            A2.GetComponent<ColorChange>().ColorChanging(new Color(2.0f, 2.0f, 2.0f, 2.0f), 0.5f);
        }
        else if (canDetectTrigger2 && !A2.GetComponent<PlayerTrigger>()._bPlayerTrigger && A2_canDetectPlayer)
        {
            canDetectTrigger2 = false;
            A2.GetComponent<ColorChange>().ColorChanging(new Color(2.0f, 2.0f, 2.0f, 0.0f), 0.5f);
        }

       

        if (!openDes2 && A2_canDetectPlayer && Input.GetButtonDown("Submit") && A2.GetComponent<PlayerTrigger>()._bPlayerTrigger)
        {

            //stop skill
            SkillManager_v2.bFinishSkill = true;
            StartCoroutine(ReverseSkillFinish(1.0f));
            StartCoroutine(Story2());
        }


        bLastDetectState2 = A2.GetComponent<PlayerTrigger>()._bPlayerTrigger;

       

        //detect 3
        if (bLastDetectState3 != A3.GetComponent<PlayerTrigger>()._bPlayerTrigger)
        {
            canDetectTrigger3 = true;
        }

        if (canDetectTrigger3 && A3.GetComponent<PlayerTrigger>()._bPlayerTrigger && A3_canDetectPlayer)
        {
            canDetectTrigger3 = false;
            A3.GetComponent<ColorChange>().ColorChanging(new Color(3.0f, 3.0f, 3.0f, 3.0f), 0.5f);
        }
        else if (canDetectTrigger3 && !A3.GetComponent<PlayerTrigger>()._bPlayerTrigger && A3_canDetectPlayer)
        {
            canDetectTrigger3 = false;
            A3.GetComponent<ColorChange>().ColorChanging(new Color(3.0f, 3.0f, 3.0f, 0.0f), 0.5f);
        }

        if (!openDes3 && A3_canDetectPlayer && Input.GetButtonDown("Submit") && A3.GetComponent<PlayerTrigger>()._bPlayerTrigger)
        {

            //stop skill
            SkillManager_v2.bFinishSkill = true;
            StartCoroutine(ReverseSkillFinish(1.0f));
            StartCoroutine(Story3());
        }


        bLastDetectState3 = A3.GetComponent<PlayerTrigger>()._bPlayerTrigger;

        


        //detect 4
        if (bLastDetectState4 != A4.GetComponent<PlayerTrigger>()._bPlayerTrigger)
        {
            canDetectTrigger4 = true;
        }

        if (canDetectTrigger4 && A4.GetComponent<PlayerTrigger>()._bPlayerTrigger && A4_canDetectPlayer)
        {
            canDetectTrigger4 = false;
            A4.GetComponent<ColorChange>().ColorChanging(new Color(4.0f, 4.0f, 4.0f, 4.0f), 0.5f);
        }
        else if (canDetectTrigger4 && !A4.GetComponent<PlayerTrigger>()._bPlayerTrigger && A4_canDetectPlayer)
        {
            canDetectTrigger4 = false;
            A4.GetComponent<ColorChange>().ColorChanging(new Color(4.0f, 4.0f, 4.0f, 0.0f), 0.5f);
        }

        if (!openDes4 && A4_canDetectPlayer && Input.GetButtonDown("Submit") && A4.GetComponent<PlayerTrigger>()._bPlayerTrigger)
        {

            //stop skill
            SkillManager_v2.bFinishSkill = true;
            StartCoroutine(ReverseSkillFinish(1.0f));
            StartCoroutine(Story4());
        }


        bLastDetectState4 = A4.GetComponent<PlayerTrigger>()._bPlayerTrigger;

        if (door != null && bTriggerMurl4)
        {
            Destroy(door);
        }


        if (rightBoundary.bTrigger)
        {
            levelLoaderScript.LoadNextLevel("StudyRoom");
        }


    }


    IEnumerator LightIntensity(LightFadeSetting lightFade)
    {
        yield return new WaitForSeconds(lightFade.preTime);

        float originalIntensity = lightFade.light.intensity;

        float timer = 0.0f;

        while (lightFade.light.intensity != lightFade.targetIntensity)
        {
            lightFade.light.intensity = Mathf.Lerp(originalIntensity , lightFade.targetIntensity, timer);

            timer +=   Time.deltaTime /lightFade.duration;


            Debug.Log(lightFade.light.intensity);

            yield return null;
        }
       
    }

    IEnumerator ReverseSkillFinish( float time)
    {
       
        yield return new WaitForSeconds(time);

        //stop skill
        SkillManager_v2.bFinishSkill = false;

    }

    IEnumerator Story1()
    {
        yield return new WaitForSeconds(0.1f);

        openDes1 = true;
        StartCoroutine(this.gameObject.GetComponent<CameraManager>().ChangeCameraFollowingPosition(0.0f, 0.5f, new Vector2(-3.5f, Camera.main.transform.position.y))) ;
        GameObject.Find("Player").GetComponent<PlayerMovement>().canMove_camera = false;
        GameObject.Find("Player").GetComponent<Animator>().SetFloat("Speed", 0.0f);
        PlayerSkill.bCanTriggerSkill = false;

        Description1.ColorChanging(new Color(1.0f, 1.0f, 1.0f, 1.0f) , 0.5f);
        title1.ColorChanging(new Color(1.0f, 1.0f, 1.0f, 1.0f), 0.5f,0.5f);
        text1.ColorChanging(new Color(1.0f, 1.0f, 1.0f, 1.0f), 0.5f, 0.5f);
        continueA1.ColorChanging(new Color(1.0f, 1.0f, 1.0f, 1.0f), 0.5f, 0.5f);

        yield return null;
        
        while (!Input.GetButtonDown("Submit"))
        {
            yield return null;
        }



        Description1.ColorChanging(new Color(1.0f, 1.0f, 1.0f, 0.0f), 0.5f);
        title1.ColorChanging(new Color(1.0f, 1.0f, 1.0f, 0.0f), 0.5f );
        text1.ColorChanging(new Color(1.0f, 1.0f, 1.0f, 0.0f), 0.5f);
        continueA1.ColorChanging(new Color(1.0f, 1.0f, 1.0f, 0.0f), 0.5f);


        this.gameObject.GetComponent<CameraManager>().ResetCamera();
        GameObject.Find("Player").GetComponent<PlayerMovement>().canMove_camera = true;
        PlayerSkill.bCanTriggerSkill = true;
        A1_canDetectPlayer = true;

        openDes1 = false;
    }

    IEnumerator Story2()
    {
        yield return new WaitForSeconds(0.1f);


        openDes2 = true;
        StartCoroutine(this.gameObject.GetComponent<CameraManager>().ChangeCameraFollowingPosition(0.0f, 0.5f, new Vector2(3.47f, Camera.main.transform.position.y)));
        GameObject.Find("Player").GetComponent<PlayerMovement>().canMove_camera = false;
        GameObject.Find("Player").GetComponent<Animator>().SetFloat("Speed", 0.0f);
        PlayerSkill.bCanTriggerSkill = false;

        Description2.ColorChanging(new Color(1.0f, 1.0f, 1.0f, 1.0f), 0.5f);
        title2.ColorChanging(new Color(1.0f, 1.0f, 1.0f, 1.0f), 0.5f, 0.5f);
        text2.ColorChanging(new Color(1.0f, 1.0f, 1.0f, 1.0f), 0.5f, 0.5f);
        continueA2.ColorChanging(new Color(1.0f, 1.0f, 1.0f, 1.0f), 0.5f, 0.5f);

        while (!Input.GetButtonDown("Submit"))
        {
            yield return null;
        }



        Description2.ColorChanging(new Color(1.0f, 1.0f, 1.0f, 0.0f), 0.5f);
        title2.ColorChanging(new Color(1.0f, 1.0f, 1.0f, 0.0f), 0.5f);
        text2.ColorChanging(new Color(1.0f, 1.0f, 1.0f, 0.0f), 0.5f);
        continueA2.ColorChanging(new Color(1.0f, 1.0f, 1.0f, 0.0f), 0.5f);

        this.gameObject.GetComponent<CameraManager>().ResetCamera();
        GameObject.Find("Player").GetComponent<PlayerMovement>().canMove_camera = true;
        PlayerSkill.bCanTriggerSkill = true;
        A2_canDetectPlayer = true;
        openDes2 = false ;

    }


    IEnumerator Story3()
    {
        yield return new WaitForSeconds(0.1f);

        openDes3 = true;
        StartCoroutine(this.gameObject.GetComponent<CameraManager>().ChangeCameraFollowingPosition(0.0f, 0.5f, new Vector2(12.82f, Camera.main.transform.position.y)));
        GameObject.Find("Player").GetComponent<PlayerMovement>().canMove_camera = false;
        GameObject.Find("Player").GetComponent<Animator>().SetFloat("Speed", 0.0f);


        Description3.ColorChanging(new Color(1.0f, 1.0f, 1.0f, 1.0f), 0.5f);
        title3.ColorChanging(new Color(1.0f, 1.0f, 1.0f, 1.0f), 0.5f, 0.5f);
        text3_1.ColorChanging(new Color(1.0f, 1.0f, 1.0f, 1.0f), 0.5f, 0.5f);
        continueA3.ColorChanging(new Color(1.0f, 1.0f, 1.0f, 1.0f), 0.5f, 0.5f);

        while (!Input.GetButtonDown("Submit"))
        {
            yield return null;
        }

        text3_1.ColorChanging(new Color(1.0f, 1.0f, 1.0f, 0.0f), 0.5f);
        text3_2.ColorChanging(new Color(1.0f, 1.0f, 1.0f, 1.0f), 0.5f, 0.5f);
        
        yield return new WaitForSeconds(0.1f);

        while (!Input.GetButtonDown("Submit"))
        {
            yield return null;
        }

        Description3.ColorChanging(new Color(1.0f, 1.0f, 1.0f, 0.0f), 0.5f );
        title3.ColorChanging(new Color(1.0f, 1.0f, 1.0f, 0.0f), 0.5f );
        text3_2.ColorChanging(new Color(1.0f, 1.0f, 1.0f, 0.0f),0.5f );
        continueA3.ColorChanging(new Color(1.0f, 1.0f, 1.0f, 0.0f), 0.5f);


        this.gameObject.GetComponent<CameraManager>().ResetCamera();
        GameObject.Find("Player").GetComponent<PlayerMovement>().canMove_camera = true;
        PlayerSkill.bCanTriggerSkill = true;

        A3_canDetectPlayer = true;
        openDes3 = false;
    }


    IEnumerator Story4()
    {
        yield return new WaitForSeconds(0.1f);

        openDes4 = true;
        StartCoroutine(this.gameObject.GetComponent<CameraManager>().ChangeCameraFollowingPosition(0.0f, 0.5f, new Vector2(19.46f, Camera.main.transform.position.y)));
        GameObject.Find("Player").GetComponent<PlayerMovement>().canMove_camera = false;
        GameObject.Find("Player").GetComponent<Animator>().SetFloat("Speed", 0.0f);


        Description4.ColorChanging(new Color(1.0f, 1.0f, 1.0f, 1.0f), 0.5f);
        title4.ColorChanging(new Color(1.0f, 1.0f, 1.0f, 1.0f), 0.5f, 0.5f);
        text4.ColorChanging(new Color(1.0f, 1.0f, 1.0f, 1.0f), 0.5f, 0.5f);
        continueA4.ColorChanging(new Color(1.0f, 1.0f, 1.0f, 1.0f), 0.5f, 0.5f);

        while (!Input.GetButtonDown("Submit"))
        {
            yield return null;
        }



        Description4.ColorChanging(new Color(1.0f, 1.0f, 1.0f, 0.0f), 0.5f);
        title4.ColorChanging(new Color(1.0f, 1.0f, 1.0f, 0.0f), 0.5f);
        text4.ColorChanging(new Color(1.0f, 1.0f, 1.0f, 0.0f), 0.5f);
        continueA4.ColorChanging(new Color(1.0f, 1.0f, 1.0f, 0.0f), 0.5f);



        this.gameObject.GetComponent<CameraManager>().ResetCamera();
        GameObject.Find("Player").GetComponent<PlayerMovement>().canMove_camera = true;
        PlayerSkill.bCanTriggerSkill = true;

        A4_canDetectPlayer = true;
        openDes4 = false;
    }




}






