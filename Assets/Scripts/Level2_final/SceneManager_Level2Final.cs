﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneManager_Level2Final : MonoBehaviour
{

    public GameObject Vita;
    private VitaSoul_particle VitaParticleScript;


    //player
    private PlayerSkill playerSkillScript;

    //Bear
    public GameObject Bear;
    public GameObject BearFog;
    public GameObject BearWakeUpCheckPoint;
    


    //Rock
    public GameObject RockDamage;
    public ParticleSystem RockDamageParticle;

    //Rock2
    public GameObject RockDamage2;
    public ParticleSystem RockDamageParticle2;

    //climb wall
    public GameObject BearClimbCheckPoint;

    public GameObject BearRestartClimbCheckPoint;

    public GameObject BearAfterClimbCheckPoint;

    //jump
    public GameObject BearJumpCheckPoint;

    //Bear gate
    public GameObject DogGate;
    bool bGateDown = false;
    float fGateTimer = 0.0f;


    //Bear gate 2
    public GameObject Gate2;
    public GameObject GateMatchJigsaw;
    bool bGate2Down = false;
    bool bStandAndWaitForBear = false;


    //scene check point
    public GameObject Point2;
    public GameObject Point3;
    public GameObject Point4;

    //bear stage
    enum BearStageNUM
    {
        Pause = -100,
        DetectingPlayer = 1,
        Roar = 3,
        FogBlow = 5,
        CameraSetting = 7,
        Run = 9,
        BearTouchRock = 11,
        BearDamageRock = 12,
        Run2 = 14,
        Hurt = 15,
        Run3 = 17,
        GoThroughtDogGate = 18,
        StartClimb = 20,
        OnTopOfDogGate = 21,
        BearRockTimer = 22,
        RestartBearAnimation = 23,
        Howl1 = 25,
        Run4 = 27,

        BearTouchRock2 = 29,
        BearDamageRock2 = 30,
        Run5 = 32,
        Jump = 33,
        CameraSettingAfterJump = 35

    }
    BearStageNUM BearStage = BearStageNUM.DetectingPlayer;


    //Trap Plant Detect
    public DetectBearTrigger TrapPlantDetectBearScript;




    //Falling Rock
    public GameObject[] FallingRock;
    float fBearRockTimer = 0.0f;
    float fFallingRock1Time = 1.5f;
    float fFallingRock2Time = 3.5f;
    float fFallingRock3Time = 4.5f;
    float fFallingRock4Time = 5.5f;
    float fBearJumpDownTime = 25.0f;


    IEnumerator RecordIEnumeratorCamraFollowing;
    IEnumerator RecordIEnumeratorCamraProjectionSize;

    //trigger icon
    public SkillOneTriggerIcon Gate1Trigger;

    //If bear catch player
    bool bCatchPlayer = false;

    //player dead loader
    private PlayerDeadLoader PlayerDeadLoaderScript;

    private static SceneManager_Level2Final instance;

    //right boundary
    public GameObject rightBoundary;
    private bool bBearStartCantJumpHowl = false;


    //if gate not down bear speed
    float fBearFastSpeed = 12.0f;

    bool bCameraTurn = false;
    bool bCameraTurn2 = false;


    public Transform Part2CameraTurnPos;

    //bear UI
    public ColorChange BearPrompt;


    private LevelLoader levelLoaderScript;

    [SerializeField]
    private GameObject levelLoader;

    private void Awake()
    {
        //check for the instance of the object
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        VitaParticleScript = Vita.GetComponent<VitaSoul_particle>();
            
        //set player skill
        playerSkillScript = GameObject.Find("Player").GetComponent<PlayerSkill>();
        playerSkillScript.CanUseSkill2 = true;

        //start camera position
        this.gameObject.GetComponent<CameraManager>().ChangeCamraFollowingTargetPosition(4.0f, 0f, 0.0f, true, false) ;

        PlayerDeadLoaderScript = GameObject.Find("PlayerDeadLoaderCanvas").GetComponent<PlayerDeadLoader>();

        FindObjectOfType<AudioManager>().Play("BearStay");
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
        //Debug.Log(BearStage);
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////If bear catch player

        if (GameObject.Find("Player").GetComponent<PlayerMovement>().bBearCatchPlayer && BearStage > BearStageNUM.CameraSetting && BearStage < BearStageNUM.Jump && !bCatchPlayer)
        {
            bCatchPlayer = true;


            StopAllCoroutines();

            BearStage = BearStageNUM.Pause;

            Bear.GetComponent<Animator>().SetTrigger("tAttack");
            Bear.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);

            //player stop
            GameObject.Find("Player").GetComponent<PlayerMovement>().canMove_skill = false;
            GameObject.Find("Player").GetComponent<Animator>().SetFloat("Speed", Mathf.Abs(0));

            //set player animation
            GameObject.Find("Player").GetComponent<PlayerMovement>().PlayerHurtAni(0.0f);

           //stop skill
            SkillManager_v2.bFinishSkill = true;

            //set animate to idle
            GameObject.Find("Player").GetComponent<PlayerSkill>().ResetAnimateToIdle();

            //trun player face to face bear 
            if (GameObject.Find("Player").GetComponent<Transform>().localScale.x < 0)
                GameObject.Find("Player").GetComponent<Transform>().localScale = new Vector2(GameObject.Find("Player").GetComponent<Transform>().localScale.x * -1f , GameObject.Find("Player").GetComponent<Transform>().localScale.y) ;

            PlayerDeadLoaderScript.TransitionAfterTime(0.35f);

            StartCoroutine(ReloadSceneAfterTimeIEnumerator(1.0f));


            //reset UI
            this.gameObject.GetComponent<SkillManager_v2>().SkillIconColorChange.ColorChanging(new Color(1.0f, 1.0f, 1.0f, 0.0f), 0.5f);
            this.gameObject.GetComponent<SkillManager_v2>().SkillNameColorChange.ColorChanging(new Color(1.0f, 1.0f, 1.0f, 0.0f), 0.5f);

           
        }



        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////Bear

        if (BearStage == BearStageNUM.DetectingPlayer && GameObject.Find("Player").GetComponent<Transform>().position.x >= BearWakeUpCheckPoint.transform.position.x)
        {
            //camera action
            this.gameObject.GetComponent<CameraManager>().ShortFollowing(3.7f, Bear.GetComponentInParent<Transform>().position);
            StartCoroutine(this.gameObject.GetComponent<CameraManager>().Shake(2.0f, 2.5f, 0.15f));
            this.gameObject.GetComponent<CameraManager>().BackToFollowPlayer();

            BearStage++;

            //Wait for 1.0f second turn to "Run" stage
            StartCoroutine(BearNextStageWait(1.0f));

            

        }

        //Bear roar
        else if (BearStage == BearStageNUM.Roar)
        {
            Bear.GetComponent<ColorChange>().ColorChanging(new Color(1.0f, 1.0f, 1.0f, 1.0f), 5.0f);
            Bear.GetComponent<BearMovement>().Howl();

            //Wait for next stage
            StartCoroutine(BearNextStageWait(1.0f));
            BearStage++;

            BearPrompt.ColorChanging(new Color(1.0f, 1.0f, 1.0f, 1.0f), 3.0f);
        }

        else if (BearStage == BearStageNUM.FogBlow)
        {
            BearFog.GetComponent<testCloud>().FadeOutAndDestory(Bear.transform.GetChild(0).position);

            BearStage++;

            //Wait for 1.0f second turn to "Run" stage
            StartCoroutine(BearNextStageWait(Bear.GetComponent<BearMovement>().fBearHowlTime - 1 ));

        }
        else if (BearStage == BearStageNUM.CameraSetting)
        {
            //set camera projection
            this.gameObject.GetComponent<CameraManager>().ChangeCameraProjectionSize(Camera.main, 7.5f, 0.8f);

            this.gameObject.GetComponent<CameraManager>().ChangeCamraFollowingTargetPosition(-5.0f, 4.9f , 0.8f, true,false);

            

            BearStage++;
            //Wait for 1.0f second turn to "Run" stage
            StartCoroutine(BearNextStageWait(1.0f));

            BearPrompt.ColorChanging(new Color(1.0f, 1.0f, 1.0f, 0.0f), 1.0f);

        }


        else if (BearStage == BearStageNUM.Run)
        {
            

            //set scene check point
            Point2.transform.position = new Vector2(GameObject.Find("Player").transform.position.x, Point2.transform.position.y);

            Bear.GetComponent<Animator>().Play("Bear_Run");
            Bear.GetComponent<Rigidbody2D>().velocity = new Vector2(9.5f, 0.0f);
        }

        //Debug.Log(BearStage);
        //Bear touch rock, stop run
        if (RockDamage != null && BearStage < BearStageNUM.BearTouchRock && BearStage >= BearStageNUM.Run)
        {
            if (RockDamage.GetComponent<RockBearDamage_Level2>()._bSkillOneTrigger)
            {
                Bear.GetComponent<Animator>().SetTrigger("tAttack");


                BearStage = BearStageNUM.BearTouchRock;

                //wait for bear animation to damage
                StartCoroutine(BearNextStageWait(0.9f));

            }
        }
        
        //Bear Damage Rock
        if (BearStage == BearStageNUM.BearDamageRock)
        {
            RockDamageParticle.Play();
            
            //add music
            FindObjectOfType<AudioManager>().Play("BearAttack");
            
            Destroy(RockDamage);

            //start next stage 
            StartCoroutine(BearNextStageWait(1.1f));
            BearStage++;

        }

        else if (BearStage == BearStageNUM.Run2)
        {
            Bear.GetComponent<Rigidbody2D>().velocity = new Vector2(7.0f, 0.0f);


            //Bear step on trap plant
            if (TrapPlantDetectBearScript._bSkillOneTrigger)
            {
                BearStage++;
            }
        }

        else if (BearStage == BearStageNUM.Hurt)
        {
            Bear.GetComponent<BearMovement>().Hurt();
            BearStage++;
            StartCoroutine(BearNextStageWait(Bear.GetComponent<BearMovement>().fBearHurtTime));
        }


        else if (BearStage == BearStageNUM.Run3)
        {
            Bear.GetComponent<Animator>().Play("Bear_Run");

            //if player is far away 
            if(!Gate1Trigger.bTriggerFinish  && !Gate1Trigger.VitaDetect._bSkillTrigger)                
                Bear.GetComponent<Rigidbody2D>().velocity = new Vector2(fBearFastSpeed, 0.0f);
               
            else
                Bear.GetComponent<Rigidbody2D>().velocity = new Vector2(10.5f, 0.0f);

            if (GameObject.Find("Bear").GetComponent<Transform>().position.x >= BearClimbCheckPoint.transform.position.x)
            {
                BearStage++;
            }
        }
        

        else if (BearStage == BearStageNUM.GoThroughtDogGate)
        {
            ///If Gate Down
            if (bGateDown)
            //if (true)
            {
                if (bGateDown)
                    Debug.Log("On time");
                else
                    Debug.Log("Not on time");


                Bear.GetComponent<Animator>().SetTrigger("tAttackRight");

                BearStage++;
                StartCoroutine(BearNextStageWait(Bear.GetComponent<BearMovement>().fBearAttackRightTime));

                //test set camera
                /*Debug.Log("set camera");
                if(RecordIEnumeratorCamraFollowing !=  null)
                    StopCoroutine(RecordIEnumeratorCamraFollowing);
                RecordIEnumeratorCamraFollowing = this.gameObject.GetComponent<CameraManager>().ChangeCamraFollowingTargetPositionIEnumerator( (playerSkillScript.transform.position.x +  Bear.transform.position.x) / 2.0f - playerSkillScript.transform.position.x, Camera.main.transform.position.y, 2.0f, true, false);
                StartCoroutine(RecordIEnumeratorCamraFollowing);*/

                if (RecordIEnumeratorCamraFollowing != null)
                    StopCoroutine(RecordIEnumeratorCamraFollowing);
                RecordIEnumeratorCamraFollowing = this.gameObject.GetComponent<CameraManager>().SetCameraBetweenPlayerBearIEnumerator(3.5f, false,true);
                StartCoroutine(RecordIEnumeratorCamraFollowing);

            }

            //Gate doesn't get down, keep running
            else
            {
                BearStage = BearStageNUM.Run4;
                Bear.GetComponent<Rigidbody2D>().velocity = new Vector2(fBearFastSpeed, 0.0f);
            }

        }
        
        else if (BearStage == BearStageNUM.StartClimb)
        {
            Bear.GetComponent<Animator>().SetTrigger("tClimb");
            BearStage++;
        }

        //when bear on the top animation stop
        else if (BearStage == BearStageNUM.OnTopOfDogGate && Bear.GetComponent<SpriteRenderer>().sprite.name == "1-2_12")
        {
            //stop animation
            Bear.GetComponent<Animator>().enabled = false;

            //set camera  
            Point3.transform.localPosition = new Vector3(18.2f, Point3.transform.position.y, Point3.transform.position.z);
            
            RecordIEnumeratorCamraProjectionSize =  this.gameObject.GetComponent<CameraManager>().ChangeCameraProjectionSizeIEnumerator(Camera.main, 5f, 1.0f);
            StartCoroutine(RecordIEnumeratorCamraProjectionSize);

            if (RecordIEnumeratorCamraFollowing != null)
                StopCoroutine(RecordIEnumeratorCamraFollowing);
            RecordIEnumeratorCamraFollowing = this.gameObject.GetComponent<CameraManager>().ChangeCamraFollowingTargetPositionIEnumerator(-2.0f, 2.33f, 1.0f, true, true);
            StartCoroutine(RecordIEnumeratorCamraFollowing);

            BearStage++;
        }
        

        //Turn camera when player over particular x
        else if (BearStage == BearStageNUM.BearRockTimer && GameObject.Find("Player").transform.position.x > 47f && !bCameraTurn)
        {
            if (RecordIEnumeratorCamraFollowing != null)
                StopCoroutine(RecordIEnumeratorCamraFollowing);
            RecordIEnumeratorCamraFollowing = this.gameObject.GetComponent<CameraManager>().ChangeCameraFollowingPosition(0.0f, 1.5f, new Vector2(50.3f,6.5f), true);
            StartCoroutine(RecordIEnumeratorCamraFollowing);

            StopCoroutine(RecordIEnumeratorCamraProjectionSize);
            RecordIEnumeratorCamraProjectionSize = this.gameObject.GetComponent<CameraManager>().ChangeCameraProjectionSizeIEnumerator(Camera.main, 8.5f, 1.5f);
            StartCoroutine(RecordIEnumeratorCamraProjectionSize);

            bCameraTurn = true;
        }



        //falling rock
        else if (BearStage == BearStageNUM.BearRockTimer)
        {
            fBearRockTimer += Time.deltaTime;

            //active rock
            //falling part 1
            if (fBearRockTimer >= fFallingRock1Time )
            {
                
                FallingRock[0].SetActive(true);

            }

            //falling part 2
            if (fBearRockTimer >= fFallingRock2Time)
            {
                FallingRock[1].SetActive(true);
            }

            //falling part 3
            if (fBearRockTimer >= fFallingRock3Time)
            {
                FallingRock[2].SetActive(true);
            }

            //falling part 4
            if (fBearRockTimer >= fFallingRock4Time)
            {
                FallingRock[3].SetActive(true);
            }

            //time up bear jump down
            if (fBearRockTimer > fBearJumpDownTime || GameObject.Find("Player").transform.position.x >= BearRestartClimbCheckPoint.transform.position.x)
            {
                //Debug.Log("Time Up Bear jump down");

                //set camera projection
               if (RecordIEnumeratorCamraFollowing != null)
                    StopCoroutine(RecordIEnumeratorCamraFollowing);
                RecordIEnumeratorCamraFollowing = this.gameObject.GetComponent<CameraManager>().ChangeCameraFollowingPosition(0.0f,1.0f, new Vector3(BearAfterClimbCheckPoint.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z));
                StartCoroutine(RecordIEnumeratorCamraFollowing);

                //this.gameObject.GetComponent<CameraManager>().ShortFollowing(1.8f, new Vector3(BearAfterClimbCheckPoint.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z));


                BearStage = BearStageNUM.RestartBearAnimation;
            }

            //after over limit height
            if (!bCameraTurn2 && GameObject.Find("Player").transform.position.y > Part2CameraTurnPos.position.y && GameObject.Find("Player").transform.position.x > Part2CameraTurnPos.position.x)
            {
                if (RecordIEnumeratorCamraFollowing != null)
                    StopCoroutine(RecordIEnumeratorCamraFollowing);
                this.gameObject.GetComponent<CameraManager>().bCameraFocusOtherObj = false;
                RecordIEnumeratorCamraFollowing = this.gameObject.GetComponent<CameraManager>().ChangeCamraFollowingTargetPositionIEnumerator(0.0f, Camera.main.transform.position.y, 2.5f, true, false);
                StartCoroutine(RecordIEnumeratorCamraFollowing);

                bCameraTurn2 = true;
            }
            
            

        }

        
        
        //Bear jump down after player over checkpoint
        else if (BearStage == BearStageNUM.RestartBearAnimation )
        {
            //start animation
            Bear.GetComponent<Animator>().enabled = true;

            //camera shake when bear jump down
            StartCoroutine(this.gameObject.GetComponent<CameraManager>().Shake(1.0f, 0.8f, 0.1f, false, true));

            

            //finish jump after 1.2s
            StartCoroutine(BearNextStageWait(2.0f));
            BearStage++;
        }

        //Keep Howl
        else if (BearStage == BearStageNUM.Howl1)
        {
            //change position
            Bear.transform.position = BearAfterClimbCheckPoint.transform.position;

            //Bear start Roar
            Bear.GetComponent<BearMovement>().Howl(); 
            //StartCoroutine(this.gameObject.GetComponent<CameraManager>().Shake(1.0f, 2.0f, 0.05f));


            //camera setting
            //set camera projection
            if (RecordIEnumeratorCamraFollowing != null)
                StopCoroutine(RecordIEnumeratorCamraFollowing);
            RecordIEnumeratorCamraFollowing = this.gameObject.GetComponent<CameraManager>().SetCameraBetweenPlayerBearIEnumerator(2.0f, false, true, 0.5f);
            StartCoroutine(RecordIEnumeratorCamraFollowing);



            //Wait for next stage
            StartCoroutine(BearNextStageWait(Bear.GetComponent<BearMovement>().fBearHowlTime - 1));
            BearStage++;
        }

        else if (BearStage == BearStageNUM.Run4)
        {
            Bear.GetComponent<Animator>().Play("Bear_Run");
            
            if(!bGateDown)
                Bear.GetComponent<Rigidbody2D>().velocity = new Vector2(fBearFastSpeed, 0.0f);

            else
                Bear.GetComponent<Rigidbody2D>().velocity = new Vector2(7.0f, 0.0f);

            //if bear hit wall, state back to climb wall
            if (Bear.GetComponent<BearMovement>().bHitWall && bGateDown)
            {
                //change bear position
                Bear.transform.position = new Vector2(BearClimbCheckPoint.transform.position.x + 3.0f, Bear.transform.position.y);

                //change state
                BearStage = BearStageNUM.GoThroughtDogGate;

            }

        }



        //Bear touch rock, stop run
        if (RockDamage2 != null && BearStage < BearStageNUM.BearTouchRock2)
        {
            if (RockDamage2.GetComponent<RockBearDamage_Level2>()._bSkillOneTrigger)
            {
                Bear.GetComponent<Animator>().SetTrigger("tAttack");
                
               

                BearStage = BearStageNUM.BearTouchRock2;

                //wait for bear animation to damage
                StartCoroutine(BearNextStageWait(0.9f));

            }
        }


        //Bear Damage Rock
        if (BearStage == BearStageNUM.BearDamageRock2)
        {
            RockDamageParticle2.Play();

            //add music
            FindObjectOfType<AudioManager>().Play("BearAttack");
            
            Destroy(RockDamage2);

            //start next stage 
            StartCoroutine(BearNextStageWait(1.1f));
            BearStage++;

        }

        else if (BearStage == BearStageNUM.Run5)
        {
            if(!bGateDown)
                Bear.GetComponent<Rigidbody2D>().velocity = new Vector2(fBearFastSpeed, 0.0f);

            else
                Bear.GetComponent<Rigidbody2D>().velocity = new Vector2(7.0f, 0.0f);


            //touch jump check point, and player is on right stair
            if (GameObject.Find("Bear").GetComponent<Transform>().position.x >= BearJumpCheckPoint.transform.position.x && GameObject.Find("Player").transform.position.y > BearRestartClimbCheckPoint.transform.position.y)
            {
                BearStage++;
            }
        }



        //Jump
        else if (BearStage == BearStageNUM.Jump && !bGate2Down)
        {
            Bear.GetComponent<Animator>().SetTrigger("tJump");
            BearStage++;

            //set player animation
            GameObject.Find("Player").GetComponent<PlayerMovement>().PlayerHurtAni(0.75f);

            //////////////////////////////////////////////////////////////////player hurt, level reload
            //player stop
            GameObject.Find("Player").GetComponent<PlayerMovement>().canMove_skill = false;
            GameObject.Find("Player").GetComponent<Animator>().SetFloat("Speed", Mathf.Abs(0));

            //set animate to idle
            GameObject.Find("Player").GetComponent<PlayerSkill>().ResetAnimateToIdle();

            //trun player face to face bear 
            if (GameObject.Find("Player").GetComponent<Transform>().localScale.x < 0)
                GameObject.Find("Player").GetComponent<Transform>().localScale = new Vector2(GameObject.Find("Player").GetComponent<Transform>().localScale.x * -1f, GameObject.Find("Player").GetComponent<Transform>().localScale.y);

            PlayerDeadLoaderScript.TransitionAfterTime(3.0f);

            StartCoroutine(ReloadSceneAfterTimeIEnumerator(4.0f));

        }

        //Can't Jump
        else if (BearStage == BearStageNUM.Jump && bGate2Down)
        {
            //Bear start Roar
            Bear.GetComponent<BearMovement>().Howl();
            BearStage++;

            //Trigger for player pose
            bBearStartCantJumpHowl = true;

            //start next stage 
            StartCoroutine(BearNextStageWait(3.0f));

        }

        else if (BearStage == BearStageNUM.CameraSettingAfterJump)
        {
            BearStage++;

            GameObject.Find("Player").GetComponent<PlayerMovement>().canMove_skill = true;
            this.gameObject.GetComponent<CameraManager>().bCameraFocusOtherObj = false;
            if (RecordIEnumeratorCamraProjectionSize != null)
                StopCoroutine(RecordIEnumeratorCamraProjectionSize);
            RecordIEnumeratorCamraProjectionSize = this.gameObject.GetComponent<CameraManager>().ChangeCameraProjectionSizeIEnumerator(Camera.main, 4f, 0.8f);
            StartCoroutine(RecordIEnumeratorCamraProjectionSize);


            if (RecordIEnumeratorCamraFollowing != null)
                StopCoroutine(RecordIEnumeratorCamraFollowing);
            RecordIEnumeratorCamraFollowing = this.gameObject.GetComponent<CameraManager>().ChangeCamraFollowingTargetPositionIEnumerator(0.0f, 7.804268f, 0.8f, true, true);
            StartCoroutine(RecordIEnumeratorCamraFollowing);


        }

        //if Bear can't climb up and player touch right boundary
        if (bStandAndWaitForBear == false && bGate2Down &&( GameObject.Find("Player").transform.position.x > 75 || bBearStartCantJumpHowl))
        {

            if (GameObject.Find("Player").transform.localScale.x < 0.0f)
                GameObject.Find("Player").transform.localScale = new Vector2(GameObject.Find("Player").transform.localScale.x * -1, GameObject.Find("Player").transform.localScale.y);

            
            GameObject.Find("Player").GetComponent<PlayerMovement>().canMove_camera = false;
            GameObject.Find("Player").GetComponent<Animator>().SetFloat("Speed", Mathf.Abs(0));
            SkillManager_v2.bFinishSkill = true;

            //player can't start trigger
            PlayerSkill.bCanTriggerSkill = false;

            StartCoroutine(ReverseSkillFinish(1.0f));

           bStandAndWaitForBear = true;
        }

        
        //after camera setting
        else if (BearStage == BearStageNUM.CameraSettingAfterJump + 1 && bStandAndWaitForBear)
        {

            if (GameObject.Find("Player").transform.localScale.x > 0.0f)
                GameObject.Find("Player").transform.localScale = new Vector2(GameObject.Find("Player").transform.localScale.x * -1, GameObject.Find("Player").transform.localScale.y);


            GameObject.Find("Player").GetComponent<PlayerMovement>().canMove_camera = true;

            Point4.transform.position = new Vector2(52.3f, Point4.transform.position.y);

            PlayerSkill.bCanTriggerSkill = true;

            rightBoundary.transform.position = new Vector2(106.4f, rightBoundary.transform.position.y);
            rightBoundary.GetComponent<BoxCollider2D>().isTrigger = true;

            BearStage++;

        }


        //Bear gate2, jigsaw is match, gate goes down
        if (GateMatchJigsaw.GetComponent<MatchingJigsaw>().bMatch && bGate2Down == false)
        {

            bGate2Down = true;
            StartCoroutine(Gate2Down());
        }

      


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////Detect dog gate
        Gate1Trigger.DetectFinish();
        
        if (bGateDown == false && Gate1Trigger.bTriggerFinish)
        {
            StartCoroutine(GateDown());
            bGateDown = true;
        }

        if (rightBoundary.GetComponent<PlayerTriggerDetect>().bTrigger)
        {
            levelLoaderScript.LoadNextLevel("Level3_final");
        }

    }


    //test
    IEnumerator BearNextStageWait(float time)
    {
        yield return new WaitForSeconds(time);
        BearStage++;

    }

    


    //dog gate
    IEnumerator GateDown()
    {
        while (DogGate.transform.localPosition.y > 23.7f)
        //while (DogGate.transform.localPosition.y > 0.1f)
        {
            DogGate.transform.position = new Vector2(DogGate.transform.position.x, DogGate.transform.position.y - 0.05f);
            yield return new WaitForSeconds(0.005f);
        }
    }


    //dog gate
    IEnumerator Gate2Down()
    {
        yield return new WaitForSeconds(1f);
        while (Gate2.transform.localPosition.y > 27.32)
        {
            Gate2.transform.position = new Vector2(Gate2.transform.position.x, Gate2.transform.position.y - 0.05f);
            yield return new WaitForSeconds(0.005f);
        }
    }

    IEnumerator ReloadSceneAfterTimeIEnumerator(float time)
    {
        yield return new WaitForSeconds(time);
        //reload scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    IEnumerator ReverseSkillFinish(float time)
    {
        yield return new WaitForSeconds(time);

        //stop skill
        SkillManager_v2.bFinishSkill = false;

    }
}
