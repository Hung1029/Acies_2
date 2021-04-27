﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    //scene check point
    public GameObject Point2;
    public GameObject Point3;


    enum BearStageNUM
    {
        Pause = 0,
        DetectingPlayer = 1,
        Roar = 3,
        FogBlow = 5,
        Howl = 7,
        Howl2 = 9,
        Run = 11,
        BearTouchRock = 13,
        BearDamageRock = 14,
        Run2 = 16,
        GoThroughtDogGate = 17,
        OnTopOfDogGate = 18,
        BearRockTimer = 19,
        RestartBearAnimation = 20,
        Howl3 = 22,
        Run3 = 24,
        Jump = 25


    }
    BearStageNUM BearStage = BearStageNUM.DetectingPlayer;


    //test bear stage
    enum BearStageNUM_test
    {
        Pause = 0,
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
    BearStageNUM_test BearStage_test = BearStageNUM_test.DetectingPlayer;


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


    //Camera turn flag
    bool bCameraTurn = false;


    IEnumerator RecordIEnumerator;

    //test trigger icon
    public SkillOneTriggerIcon Gate1Trigger;



    // Start is called before the first frame update
    void Start()
    {
        VitaParticleScript = Vita.GetComponent<VitaSoul_particle>();
            
        //set player skill
        playerSkillScript = GameObject.Find("Player").GetComponent<PlayerSkill>();
        playerSkillScript.CanUseSkill2 = true;

        //start camera position
        this.gameObject.GetComponent<CameraManager>().ChangeCamraFollowingTargetPosition(4.0f, 0f, 0.0f, true, false) ;


        
    }

    private void FixedUpdate()
    {
        VitaParticleScript.FollowObj();
    }

    // Update is called once per frame
    void Update()
    {


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////Bear

        if (BearStage_test == BearStageNUM_test.DetectingPlayer && GameObject.Find("Player").GetComponent<Transform>().position.x >= BearWakeUpCheckPoint.transform.position.x)
        {
            //camera action
            this.gameObject.GetComponent<CameraManager>().ShortFollowing(3.7f, Bear.GetComponentInParent<Transform>().position);
            StartCoroutine(this.gameObject.GetComponent<CameraManager>().Shake(2.0f, 2.5f, 0.15f));
            this.gameObject.GetComponent<CameraManager>().BackToFollowPlayer();

            BearStage_test++;

            //Wait for 1.0f second turn to "Run" stage
            StartCoroutine(BearNextStageWaitTest(1.0f));

        }

        //Bear roar
        else if (BearStage_test == BearStageNUM_test.Roar)
        {
            Bear.GetComponent<ColorChange>().ColorChanging(new Color(1.0f, 1.0f, 1.0f, 1.0f), 5.0f);
            Bear.GetComponent<BearMovement>().Howl();

            //Wait for next stage
            StartCoroutine(BearNextStageWaitTest(1.0f));
            BearStage_test++;
        }

        else if (BearStage_test == BearStageNUM_test.FogBlow)
        {
            BearFog.GetComponent<testCloud>().FadeOutAndDestory(Bear.transform.GetChild(0).position);

            BearStage_test++;

            //Wait for 1.0f second turn to "Run" stage
            StartCoroutine(BearNextStageWaitTest(Bear.GetComponent<BearMovement>().fBearHowlTime - 1 ));

        }
        else if (BearStage_test == BearStageNUM_test.CameraSetting)
        {
            //set camera projection
            this.gameObject.GetComponent<CameraManager>().ChangeCameraProjectionSize(Camera.main, 7.5f, 0.8f);

            this.gameObject.GetComponent<CameraManager>().ChangeCamraFollowingTargetPosition(-5.0f, 4.9f , 0.5f, true,false);

            

            BearStage_test++;
            //Wait for 1.0f second turn to "Run" stage
            StartCoroutine(BearNextStageWaitTest(1.0f));

        }


        else if (BearStage_test == BearStageNUM_test.Run)
        {

            //set scene check point
            Point2.transform.position = new Vector2(GameObject.Find("Player").transform.position.x, Point2.transform.position.y);

            Bear.GetComponent<Animator>().Play("Bear_Run");
            Bear.GetComponent<Rigidbody2D>().velocity = new Vector2(8.0f, 0.0f);
        }


        //Bear touch rock, stop run
        if (RockDamage != null && BearStage_test < BearStageNUM_test.BearTouchRock)
        {
            if (RockDamage.GetComponent<RockBearDamage_Level2>()._bSkillOneTrigger)
            {
                Bear.GetComponent<Animator>().SetTrigger("tAttack");

                BearStage_test = BearStageNUM_test.BearTouchRock;

                //wait for bear animation to damage
                StartCoroutine(BearNextStageWaitTest(0.9f));

            }
        }


        //Bear Damage Rock
        if (BearStage_test == BearStageNUM_test.BearDamageRock)
        {
            RockDamageParticle.Play();
            Destroy(RockDamage);

            //start next stage 
            StartCoroutine(BearNextStageWaitTest(1.1f));
            BearStage_test++;

        }

        else if (BearStage_test == BearStageNUM_test.Run2)
        {
            Bear.GetComponent<Rigidbody2D>().velocity = new Vector2(7.0f, 0.0f);


            //Bear step on trap plant
            if (TrapPlantDetectBearScript._bSkillOneTrigger)
            {
                BearStage_test++;
            }
        }

        else if (BearStage_test == BearStageNUM_test.Hurt)
        {
            Bear.GetComponent<BearMovement>().Hurt();
            BearStage_test++;
            StartCoroutine(BearNextStageWaitTest(Bear.GetComponent<BearMovement>().fBearHurtTime));
        }


        else if (BearStage_test == BearStageNUM_test.Run3)
        {
            Bear.GetComponent<Animator>().Play("Bear_Run");
            Bear.GetComponent<Rigidbody2D>().velocity = new Vector2(10.5f, 0.0f);

            if (GameObject.Find("Bear").GetComponent<Transform>().position.x >= BearClimbCheckPoint.transform.position.x)
            {
                BearStage_test++;
            }
        }


        else if (BearStage_test == BearStageNUM_test.GoThroughtDogGate)
        {
            ///If Gate Down
            // if (bGateDown)
            if (true)
            {

                if (bGateDown)
                    Debug.Log("On time");
                else
                    Debug.Log("Not on time");

                Bear.GetComponent<Animator>().SetTrigger("tAttackRight");
                BearStage_test++;
                StartCoroutine(BearNextStageWaitTest(Bear.GetComponent<BearMovement>().fBearAttackRightTime));


            }

            //Gate doesn't get down, keep running
            else
            {
                BearStage_test = BearStageNUM_test.Run4;
                Bear.GetComponent<Rigidbody2D>().velocity = new Vector2(10.0f, 0.0f);
            }

        }

        else if (BearStage_test == BearStageNUM_test.StartClimb)
        {
            Bear.GetComponent<Animator>().SetTrigger("tClimb");
            BearStage_test++;
        }

        //when bear on the top animation stop
        else if (BearStage_test == BearStageNUM_test.OnTopOfDogGate && Bear.GetComponent<SpriteRenderer>().sprite.name == "1-2_12")
        {
            //stop animation
            Bear.GetComponent<Animator>().enabled = false;

            //set camera  
            Point3.transform.localPosition = new Vector3(18.2f, Point3.transform.position.y, Point3.transform.position.z);
            this.gameObject.GetComponent<CameraManager>().ChangeCameraProjectionSize(Camera.main, 5f, 1.0f);
            RecordIEnumerator = this.gameObject.GetComponent<CameraManager>().ChangeCamraFollowingTargetPositionIEnumerator(-2.0f, 2.33f, 1.0f, true, true);
            StartCoroutine(RecordIEnumerator);

            BearStage_test++;
        }
        

        //Turn camera when player over particular x
        else if (BearStage_test == BearStageNUM_test.BearRockTimer && bCameraTurn == false && GameObject.Find("Player").transform.position.x > 47f)
        {
            StopCoroutine(RecordIEnumerator);

            RecordIEnumerator = this.gameObject.GetComponent<CameraManager>().ChangeCamraFollowingTargetPositionIEnumerator(-5.0f, 5.3f, 1.6f, true, false);
            StartCoroutine(RecordIEnumerator);

            this.gameObject.GetComponent<CameraManager>().ChangeCameraProjectionSize(Camera.main, 8.0f, 2.0f);

            //set flag
            bCameraTurn = true;
        }



        //falling rock
        else if (BearStage_test == BearStageNUM_test.BearRockTimer)
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

            //Debug.Log(fBearRockTimer);
            //time up bear jump down
            if (fBearRockTimer > fBearJumpDownTime || GameObject.Find("Player").transform.position.x >= BearRestartClimbCheckPoint.transform.position.x)
            {
                Debug.Log("Time Up Bear jump down");

                //set camera projection
                StopCoroutine(RecordIEnumerator);
                this.gameObject.GetComponent<CameraManager>().ShortFollowing(1.8f, new Vector3(BearAfterClimbCheckPoint.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z));
                

                BearStage_test = BearStageNUM_test.RestartBearAnimation;
            }


        }


        
        //Bear jump down after player over checkpoint
        else if (BearStage_test == BearStageNUM_test.RestartBearAnimation )
        {
            //start animation
            Bear.GetComponent<Animator>().enabled = true;

            //camera shake when bear jump down
            StartCoroutine(this.gameObject.GetComponent<CameraManager>().Shake(1.0f, 1.0f, 0.1f, false, true));

            

            //finish jump after 1.2s
            StartCoroutine(BearNextStageWaitTest(2.0f));
            BearStage_test++;
        }

        //Keep Howl
        else if (BearStage_test == BearStageNUM_test.Howl1)
        {
            //change position
            Bear.transform.position = BearAfterClimbCheckPoint.transform.position;

            //Bear start Roar
            Bear.GetComponent<BearMovement>().Howl(); 
            StartCoroutine(this.gameObject.GetComponent<CameraManager>().Shake(1.0f, 2.0f, 0.15f));

            //Wait for next stage
            StartCoroutine(BearNextStageWaitTest(Bear.GetComponent<BearMovement>().fBearHowlTime - 1));
            BearStage_test++;
        }

        else if (BearStage_test == BearStageNUM_test.Run4)
        {
            Bear.GetComponent<Animator>().Play("Bear_Run");
            Bear.GetComponent<Rigidbody2D>().velocity = new Vector2(7.0f, 0.0f);
        }



        //Bear touch rock, stop run
        if (RockDamage2 != null && BearStage_test < BearStageNUM_test.BearTouchRock2)
        {
            if (RockDamage2.GetComponent<RockBearDamage_Level2>()._bSkillOneTrigger)
            {
                Bear.GetComponent<Animator>().SetTrigger("tAttack");

                BearStage_test = BearStageNUM_test.BearTouchRock2;

                //wait for bear animation to damage
                StartCoroutine(BearNextStageWaitTest(0.9f));

            }
        }


        //Bear Damage Rock
        if (BearStage_test == BearStageNUM_test.BearDamageRock2)
        {
            RockDamageParticle2.Play();
            Destroy(RockDamage2);

            //start next stage 
            StartCoroutine(BearNextStageWaitTest(1.1f));
            BearStage_test++;

        }

        else if (BearStage_test == BearStageNUM_test.Run5)
        {
            Bear.GetComponent<Rigidbody2D>().velocity = new Vector2(8.0f, 0.0f);


            //touch jump check point
            if (GameObject.Find("Bear").GetComponent<Transform>().position.x >= BearJumpCheckPoint.transform.position.x)
            {
                BearStage_test++;
            }
        }



        //Jump
        else if (BearStage_test == BearStageNUM_test.Jump && !bGate2Down)
        {
            Bear.GetComponent<Animator>().SetTrigger("tJump");
            BearStage_test++;

        }

        //Can't Jump
        else if (BearStage_test == BearStageNUM_test.Jump && bGate2Down)
        {
            //Bear start Roar
            Bear.GetComponent<BearMovement>().Howl();
            BearStage_test++;


            //start next stage 
            StartCoroutine(BearNextStageWaitTest(3.0f));

        }

        else if (BearStage_test == BearStageNUM_test.CameraSettingAfterJump)
        {
            
            this.gameObject.GetComponent<CameraManager>().ChangeCameraProjectionSize(Camera.main, 4f, 0.8f);

            this.gameObject.GetComponent<CameraManager>().ChangeCamraFollowingTargetPosition(0.0f, 7.804268f, 0.8f, true, true);
            /*if(RecordIEnumerator != null)
                StopCoroutine(RecordIEnumerator);
            RecordIEnumerator = this.gameObject.GetComponent<CameraManager>().ChangeCamraFollowingTargetPositionIEnumerator(0.0f, GameObject.Find("Player").transform.position.y, 1.0f, true, true);
            StartCoroutine(RecordIEnumerator);*/
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

    }


    //test
    IEnumerator BearNextStageWaitTest(float time)
    {
        yield return new WaitForSeconds(time);
        BearStage_test++;

    }

    //Wait for Bear next stage
    IEnumerator BearNextStageWait(float time)
    {
        yield return new WaitForSeconds(time);
        BearStage++;

    }

    


    //dog gate
    IEnumerator GateDown()
    {
        while (DogGate.transform.localPosition.y >23f)
        //while (DogGate.transform.localPosition.y > 0.1f)
        {
            DogGate.transform.position = new Vector2(DogGate.transform.position.x, DogGate.transform.position.y - 0.05f);
            yield return new WaitForSeconds(0.01f);
        }
    }


    //dog gate
    IEnumerator Gate2Down()
    {
        yield return new WaitForSeconds(1f);
        while (Gate2.transform.localPosition.y > 24.17f)
        {
            Gate2.transform.position = new Vector2(Gate2.transform.position.x, Gate2.transform.position.y - 0.05f);
            yield return new WaitForSeconds(0.01f);
        }
    }



}
