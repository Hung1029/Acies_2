using System.Collections;
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
        Attack2 = 18,

    }
    BearStageNUM_test BearStage_test = BearStageNUM_test.DetectingPlayer;


    //Trap Plant Detect
    public DetectBearTrigger TrapPlantDetectBearScript;




    //Falling Rock
    public GameObject FallingRock;
    float fBearRockTimer = 0.0f;
    float fFallingRockTime = 5.0f;
    float fBearJumpDownTime = 15.0f;

    bool bTimeUpBearJumpDown = false;


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
            Bear.GetComponent<Rigidbody2D>().velocity = new Vector2(9.0f, 0.0f);
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
            Bear.GetComponent<Rigidbody2D>().velocity = new Vector2(9.0f, 0.0f);
            

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
            Bear.GetComponent<Rigidbody2D>().velocity = new Vector2(11.0f, 0.0f);

            if (GameObject.Find("Bear").GetComponent<Transform>().position.x >= BearClimbCheckPoint.transform.position.x)
            {
                BearStage_test++;
            }
        }


        else if (BearStage_test == BearStageNUM_test.Attack2)
        {

            Bear.GetComponent<Animator>().SetTrigger("tAttackRight");
            BearStage_test++;

            /* //Gate Down
             if (bGateDown)
             //if (true)
             {
                 Bear.GetComponent<Animator>().SetTrigger("tClimb");

                 BearStage_test++;

             }

             //Gate doesn't get down
             else
             {
                 BearStage_test = BearStageNUM_test.Run3;
                 Bear.GetComponent<Rigidbody2D>().velocity = new Vector2(10.0f, 0.0f);
             }*/

        }



        /* //start Run
         else if (BearStage_test == BearStageNUM_test.Run)
         {
             Bear.GetComponent<Animator>().Play("Bear_Run");
             Bear.GetComponent<Rigidbody2D>().velocity = new Vector2(5.0f, 0.0f);
         }*/




        /*//wait for player run to checkpoint
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
        }

        else if (BearStage == BearStageNUM.FogBlow)
        {
            BearFog.GetComponent<testCloud>().FadeOutAndDestory(Bear.transform.GetChild(0).position);

            BearStage++;

            //Wait for 1.0f second turn to "Run" stage
            StartCoroutine(BearNextStageWait(Bear.GetComponent<BearMovement>().fBearHowlTime - 1));

        }

        //Keep Howl
        else if (BearStage == BearStageNUM.Howl)
        {
            //Bear start Roar
            Bear.GetComponent<BearMovement>().Howl();

            //shaking
            //StartCoroutine(this.gameObject.GetComponent<CameraManager>().Shake(2.0f, 2.5f, 0.15f));

            //set camera projection
            this.gameObject.GetComponent<CameraManager>().ChangeCameraProjectionSize(Camera.main, 7.5f, 2.0f);
            this.gameObject.GetComponent<CameraManager>().ChangeCamraFollowingTargetPosition(-6.0f, 0f, 0.8f, true, false);
            
            

            //Wait for next stage
            StartCoroutine(BearNextStageWait(Bear.GetComponent<BearMovement>().fBearHowlTime - 1));
            BearStage++;
        }

        //Keep Howl
        else if (BearStage == BearStageNUM.Howl2)
        {
            //Bear start Roar
            Bear.GetComponent<BearMovement>().Howl();

            //shaking
            //StartCoroutine(this.gameObject.GetComponent<CameraManager>().Shake(2.0f, 2.5f, 0.15f));


            //Wait for next stage
            StartCoroutine(BearNextStageWait(Bear.GetComponent<BearMovement>().fBearHowlTime - 1));
            BearStage++;
        }

        //Run
        else if (BearStage == BearStageNUM.Run)
        {
            Bear.GetComponent<Animator>().Play("Bear_Run");
            Bear.GetComponent<Rigidbody2D>().velocity = new Vector2(10.0f, 0.0f);
        }

        //Bear touch rock, stop run
        if (RockDamage != null && BearStage < BearStageNUM.BearTouchRock)
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
            Destroy(RockDamage);

            //start next stage 
            StartCoroutine(BearNextStageWait(1.1f));
            BearStage++;

        }

        //
        else if (BearStage == BearStageNUM.Run2)
        {
            Bear.GetComponent<Rigidbody2D>().velocity = new Vector2(10.0f, 0.0f);
            if (GameObject.Find("Bear").GetComponent<Transform>().position.x >= BearClimbCheckPoint.transform.position.x)
            {
                BearStage++;
            }
        }

        else if (BearStage == BearStageNUM.GoThroughtDogGate)
        {
            //Gate Down
            if (bGateDown)
            //if (true)
            {
                Bear.GetComponent<Animator>().SetTrigger("tClimb");

                BearStage++;

            }

            //Gate doesn't get down
            else
            {
                BearStage = BearStageNUM.Run3;
                Bear.GetComponent<Rigidbody2D>().velocity = new Vector2(10.0f, 0.0f);
            }

        }

        //when bear on the top animation stop
        else if (BearStage == BearStageNUM.OnTopOfDogGate && Bear.GetComponent<SpriteRenderer>().sprite.name == "1-2_12")
        {
            //stop animation
            Bear.GetComponent<Animator>().enabled = false;
            
            BearStage++;
        }


        //falling rock
        else if (BearStage == BearStageNUM.BearRockTimer)
        {
            fBearRockTimer += Time.deltaTime;

            //active rock
            if (fBearRockTimer >= fFallingRockTime)
            {
                FallingRock.SetActive(true);
            }

            //time up bear jump down
            if (fBearRockTimer > fBearJumpDownTime)
            {
                Debug.Log("Time Up Bear jump down");
                
                //set camera projection
                this.gameObject.GetComponent<CameraManager>().ChangeCameraProjectionSize(Camera.main, 9.5f, 2.0f);
                this.gameObject.GetComponent<CameraManager>().ChangeCamraFollowingTargetPosition(-13.0f, 0f, 0.8f, true, true);



                bTimeUpBearJumpDown = true;
                BearStage = BearStageNUM.RestartBearAnimation;
            }


        }



        //Bear jump down after player over checkpoint
        else if (BearStage == BearStageNUM.RestartBearAnimation && (GameObject.Find("Player").transform.position.x >= BearRestartClimbCheckPoint.transform.position.x || bTimeUpBearJumpDown))
        {
            //start animation
            Bear.GetComponent<Animator>().enabled = true;


            StartCoroutine(this.gameObject.GetComponent<CameraManager>().Shake(0.5f, 0.69f, 0.08f));

            //finish jump after 1.2s
            StartCoroutine(BearNextStageWait(1.5f));
            BearStage++;
        }


        


        //Keep Howl
        else if (BearStage == BearStageNUM.Howl3)
        {
             //change position
            Bear.transform.position = BearAfterClimbCheckPoint.transform.position;

            //Bear start Roar
            Bear.GetComponent<BearMovement>().Howl();

            //Wait for next stage
            StartCoroutine(BearNextStageWait(Bear.GetComponent<BearMovement>().fBearHowlTime - 1));
            BearStage++;
        }

        else if (BearStage == BearStageNUM.Run3)
        {
            Bear.GetComponent<Animator>().Play("Bear_Run");
            Bear.GetComponent<Rigidbody2D>().velocity = new Vector2(10.0f, 0.0f);


            //touch jump check point
            if (GameObject.Find("Bear").GetComponent<Transform>().position.x >= BearJumpCheckPoint.transform.position.x)
            {
                BearStage++;
            }

        }

        //Jump
        else if (BearStage == BearStageNUM.Jump && !bGate2Down)
        {
            Bear.GetComponent<Animator>().SetTrigger("tJump");
            BearStage++;

            //Bear.GetComponent<Animator>().Play("Bear_Idle");
            //BearStage++;
        }

        //Can't Jump
        else if (BearStage == BearStageNUM.Jump && bGate2Down)
        {
            Bear.GetComponent<Animator>().Play("Bear_Idle");
            BearStage++;

        }


        //Bear gate2, jigsaw is match, gate goes down
        if (GateMatchJigsaw.GetComponent<MatchingJigsaw>().bMatch && bGate2Down == false)
        {
           
            bGate2Down = true;
            StartCoroutine(Gate2Down());
        }*/


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////Detect dog gate
        if (bGateDown == false)
        {

            if (DogGate.GetComponentInChildren<VitaTriggerDetect>()._bSkillTrigger)
            {
                if (fGateTimer == 0)
                {
                    DogGate.GetComponentInChildren<ColorChange>().ColorChanging(Color.green, VitaParticleScript.fSkillOneGatheringTime);
                }

                fGateTimer += Time.deltaTime;

                if (fGateTimer >= VitaParticleScript.fSkillOneGatheringTime)
                {
                    //set get down bool true
                    bGateDown = true;

                    // gate down animation
                    StartCoroutine(GateDown());


                    //reset timer
                    fGateTimer = 0.0f;
                }
            }
            //if  move platform is not trigger
            else
            {
                if (fGateTimer != 0)
                {
                    //reset timer
                    fGateTimer = 0.0f;
                    DogGate.GetComponentInChildren<ColorChange>().ColorChanging(Color.yellow, VitaParticleScript.fSkillOneGatheringTime * 0.5f);
                }

            }
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
