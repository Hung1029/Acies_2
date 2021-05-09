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
    bool bTriggerMurl3 = false;

    // Start is called before the first frame update
    void Start()
    {
        VitaParticleScript = Vita.GetComponent<VitaSoul_particle>();

        //set trigger
        MuralTrigger2_1.VitaDetect.bCanBeDetect = false;
        MuralTrigger2_2.VitaDetect.bCanBeDetect = false;
        MuralTrigger4.VitaDetect.bCanBeDetect = false;
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

            //set camera
            this.gameObject.GetComponent<CameraManager>().ShortFollowing(5.0f, new Vector3(-3.5f, Camera.main.transform.position.y, Camera.main.transform.position.z));


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

            bTriggerMurl1 = true;
        }

        //murl 2 trigger
        if (MuralTrigger2_1.bTriggerFinish && MuralTrigger2_2.bTriggerFinish && bTriggerMurl2 == false  && bTriggerMurl1)
        {
            //set camera
            this.gameObject.GetComponent<CameraManager>().ShortFollowing(5.0f, new Vector3(3.6f, Camera.main.transform.position.y, Camera.main.transform.position.z));

            //MuralSprite2.ColorChanging(new Color(1.0f, 1.0f, 1.0f, 1.0f), 3.0f);
            StoryMaterialSprite2_1.ColorChanging(new Color(1.0f, 1.0f, 1.0f, 1.0f), 3.0f);
            StoryMaterialSprite2_2.ColorChanging(new Color(1.0f, 1.0f, 1.0f, 1.0f), 3.0f);
            StoryMaterialSprite2_3.ColorChanging(new Color(1.0f, 1.0f, 1.0f, 1.0f), 3.0f);

            //light setting
            for (int i = 0; i < Mural2LightSetting.Length; i++)
            {
                StartCoroutine(LightIntensity(Mural1LightSetting[i]));
            }

            //light up murl  light
            TriggerCircle4.ColorChanging(new Color(1.0f, 1.0f, 1.0f, 1.0f), 3.0f, 3.0f);
            MuralTrigger4.VitaDetect.bCanBeDetect = true;

            bTriggerMurl2 = true;
        }

        //murl 4 trigger
        if (MuralTrigger4.bTriggerFinish && bTriggerMurl3 == false  && bTriggerMurl2)
        {
            
            //set camera
            this.gameObject.GetComponent<CameraManager>().ShortFollowing(5.0f, new Vector3(19.4f, Camera.main.transform.position.y, Camera.main.transform.position.z));

            //MuralSprite4.ColorChanging(new Color(1.0f, 1.0f, 1.0f, 1.0f), 3.0f);
            StoryMaterialSprite4_1.ColorChanging(new Color(1.0f, 1.0f, 1.0f, 1.0f), 3.0f);
            StoryMaterialSprite4_2.ColorChanging(new Color(1.0f, 1.0f, 1.0f, 1.0f), 3.0f);

            //light setting
            for (int i = 0; i < Mural4LightSetting.Length; i++)
            {
                StartCoroutine(LightIntensity(Mural4LightSetting[i]));
            }

            bTriggerMurl3 = true;
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

            yield return null;
        }
    }



}






