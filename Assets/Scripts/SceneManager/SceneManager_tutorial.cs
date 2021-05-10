using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneManager_tutorial : MonoBehaviour
{
    [SerializeField]
    private Camera MainCamera;

    [SerializeField]
    private GameObject VitaSoul;
    private VitaSoul_particle VitaParticleScript;
    private GazeMovement VitaParticleGazeScript;

    //Pop Skill Direction UI
    [SerializeField]
    private ReadSkillDirection DetectDirectionUICollider;
    [SerializeField]
    private SkillDirection SkillDirectionUI;
    bool bRead = false;

    //water wheel
    [SerializeField]
    private GameObject WaterWheel;
    private WaterWheel WaterWheelScript;

    //particle system
    [SerializeField]
    private ParticleSystem Splash;

    //plant
    [SerializeField]
    private Plant PlantScript;

    //boat
    [SerializeField]
    private BoatTrigger BoatTriggerScript;

    [SerializeField]
    private BoatMove BoatMoveScript;

    //candle
    [SerializeField]
    private GameObject CloudToDestroy;
    [SerializeField]
    private TriggerCandle TriggerCandleScript;
    bool isCloudDestory = false;

    //Ropeway
    [SerializeField]
    private RopewayTrigger RopewayTriggerScript;

    [SerializeField]
    private Ropeway RopewayScript;

    //candle-2
    [SerializeField]
    private GameObject CloudToDestroy2;
    [SerializeField]
    private TriggerCandle TriggerCandleScript2;
    bool isCloudDestory2 = false;

    //levelloader script
    [SerializeField]
    private LevelLoader levelLoaderScript;
    [SerializeField]
    private BoxCollider2D ChangeSceneCollider;



    // Start is called before the first frame update
    void Start()
    {
        VitaParticleScript = VitaSoul.GetComponent<VitaSoul_particle>();
        VitaParticleGazeScript = VitaSoul.GetComponent<GazeMovement>();

        VitaParticleScript.MoveSpeed = 9.5f;

        WaterWheelScript = WaterWheel.GetComponent<WaterWheel>();

        //Splash.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        ////Skill Direction UI fade in
        if (DetectDirectionUICollider.bPlayerTouch && bRead == false)
        {
            //SkillDirectionUI.StartCoroutine(SkillDirectionUI.FadeInTextMainTitleIEnumerator());
            Debug.Log("in direction");
            SkillDirectionUI.GetComponent<Animator>().SetTrigger("tStart");

            //reset UI collider bool
            DetectDirectionUICollider.bPlayerTouch = false;

            //player can't move
            GameObject.Find("Player").GetComponent<PlayerMovement>().canMove_skill = false;
            GameObject.Find("Player").GetComponent<Animator>().SetFloat("Speed", Mathf.Abs(0));

            bRead = true;
        }

        //skill Directio UI fade out when pressA Key
        else if (bRead && Input.GetButtonDown("NPC_trigger"))
        {
            SkillDirectionUI.StartCoroutine(SkillDirectionUI.FadeOutTextMainTitleIEnumerator());

            //player can move
            GameObject.Find("Player").GetComponent<PlayerMovement>().canMove_skill = true;

            //reset Reading bool if need to read again
            //bRead = false;

        }


        //

        if (!VitaParticleGazeScript.bVitaSoulCanGaze)
        {
            VitaParticleScript.FollowObj();
        }


        ////water wheel rotate

        if (WaterWheelScript._bSkillOneTrigger && WaterWheelScript._bIsRotate == false && PlayerSkill.CURRENTSKILL == 1)
        {
            Splash.Play();
            WaterWheelScript.PlayWaterWheelRotate();
            PlantScript.GrowUp();

            //set camera 
            this.gameObject.GetComponent<CameraParallaxManager_Level1>().ShortFollowing(2.0f, new Vector3(16.6f, -0.66f, 0.0f));
            StartCoroutine(this.gameObject.GetComponent<CameraParallaxManager_Level1>().Shake(1.0f,1.5f, 0.08f));
        }


        ///


        ////Boat

        if ((BoatTriggerScript._bSkillOneTrigger && PlayerSkill.CURRENTSKILL == 1) || BoatMoveScript._bIsMove == true )
        {

            BoatMoveScript.BoatFloating();
        }

        ////

        ////Candle

        if (TriggerCandleScript._bSkillOneTrigger && isCloudDestory ==false && PlayerSkill.CURRENTSKILL == 1)
        {
            CloudToDestroy.GetComponentInChildren<testCloud>().FadeOutAndDestory(TriggerCandleScript.GetComponent<Transform>().position);
            isCloudDestory = true;
        }

        ////

        ////RopeWay

        if (RopewayTriggerScript._bSkillOneTrigger && RopewayScript._bRopewayMoving == false && PlayerSkill.CURRENTSKILL == 1)
        {
            RopewayScript.RopewayDown();
        }

        ////
       
        ////Candle-2

        if (TriggerCandleScript2._bSkillOneTrigger && isCloudDestory2 == false && PlayerSkill.CURRENTSKILL == 1)
        {
            CloudToDestroy2.GetComponentInChildren<testCloud>().FadeOutAndDestory(TriggerCandleScript2.GetComponent<Transform>().position);
            isCloudDestory2 = true;
        }



        //change scene
        if (ChangeSceneCollider.GetComponent<PlayerTriggerDetect>().bTrigger)
        {
            levelLoaderScript.LoadNextLevel("Level1-1");
        }



    }





  

  
    ////Cloud

   

    ////


}
