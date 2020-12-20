using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager_level1 : MonoBehaviour
{

    [SerializeField]
    private GameObject VitaSoul;
    private VitaSoul_particle VitaParticleScript;
    private GazeMovement VitaParticleGazeScript;


    //candle
    [SerializeField]
    private GameObject CloudToDestroy;
    [SerializeField]
    private TriggerCandle TriggerCandleScript1;
    bool isCloudDestory = false;

    //Stair Rotate
    [SerializeField]
    private TriggerCandle TriggerCandleScript2;

    [SerializeField]
    private RotateStair RotateStair;
    //


    //trap ground
    [SerializeField]
    private GameObject CloudToDestroy3;
    [SerializeField]
    private TriggerCandle TriggerCandleScript3;
    bool isCloudDestory3 = false;
    //

    //Buddha Candle
    public GameObject[] StartCandle;
    public GameObject[] GameCandle;
    public SpriteRenderer Go;
    public SpriteRenderer Finish;
    int iBuddhaLevel = 0;
    int[] array = new int[3]; //set candle number
    int[] ilightUpArray = new int[3]; //set  candle light up number
    public GameObject Stair;

    //Spread Fog
    public GameObject FogGate;
    private TriggerCandle FogGateCandleScript;

    //Bear Gate
    public GameObject BearGate;
    private TriggerCandle BearGateCandleScript;
    bool BearGateAnimate = false;

    //Spread water
    public GameObject WaterGate;
    private TriggerCandle WaterGateCandleScript;

    //water destory detect
    public GameObject WaterParticles;

    //Short camera following
    public CameraParallaxManager_Level1 CameraParallaxManager;

    //bear
    public GameObject Bear;
    public GameObject BearMovementStageCheckPoint;
    Transform[] BearCheckPointTransform;
    public GameObject BearFog;
    bool bHurting = false;
    public GameObject RockDamage;
    public ParticleSystem RockDamageParticle;
    public GameObject AvoidBearFallDownTrigger;
    bool bAvoid = false;
    public DetectBearCollision FloorDetectScript1;
    public DetectBearCollision FloorDetectScript2;

    //bear hurting
    bool bBearHurting = false;
    bool bCanDetectHurt = true;

    bool bFogHurt = false;
    bool bTrap1Hurt = false;
    bool bTrap2Hurt = false;


    enum BearMovementStage
    {
        DetectPlayer = 1,
        BearRoar = 3,
        FogBlow = 5,
        Run = 7,
        ResetBear = 8,
        Run2 = 10,
        DecreaseSpeed = 11,
        HeadUp = 12,
        FinishClimbingDown = 13,
        BearTouchRock = 15,
        RockDamage = 17,
        Run3 = 19,
        GoThroughtDogGate = 20,
        ClimbFinish = 22,
        OverGate = 23,
        HitFloor1 = 24,
        Run4 = 26,
        HitFloor2 = 27,
        FinalRun = 29,
    }
    BearMovementStage BearStage = BearMovementStage.DetectPlayer;

    //water destory detect
    public GameObject Trap1;
    public GameObject Trap2;
    public GameObject Plant;

    //camera
    public Camera FinalCamera;


    // Start is called before the first frame update
    void Start()
    {
        VitaParticleScript = VitaSoul.GetComponent<VitaSoul_particle>();
        VitaParticleGazeScript = VitaSoul.GetComponent<GazeMovement>();

        VitaParticleScript.MoveSpeed = 9.5f;

        FogGateCandleScript = FogGate.GetComponentInChildren<TriggerCandle>();

        BearGateCandleScript = BearGate.GetComponentInChildren<TriggerCandle>();

        WaterGateCandleScript = WaterGate.GetComponentInChildren<TriggerCandle>();

        //Bear check point position
        BearCheckPointTransform = new Transform[BearMovementStageCheckPoint.transform.childCount];
        for (int i = 0; i < BearMovementStageCheckPoint.transform.childCount; i++)
        {
            BearCheckPointTransform[i] = BearMovementStageCheckPoint.transform.GetChild(i).GetComponent<Transform>();
        }


    }

    // Update is called once per frame
    void Update()
    {
        if (!VitaParticleGazeScript.bVitaSoulCanGaze)
        {
            VitaParticleScript.FollowObj();
        }


        ////Candle

        
        if (TriggerCandleScript1._bSkillOneTrigger && isCloudDestory == false && PlayerSkill.CURRENTSKILL == 1)
        {
            CloudToDestroy.GetComponentInChildren<testCloud>().FadeOutAndDestory(TriggerCandleScript1.GetComponent<Transform>().position);
            isCloudDestory = true;
        }

        if (TriggerCandleScript3._bSkillOneTrigger && isCloudDestory3 == false && PlayerSkill.CURRENTSKILL == 1)
        {
            CloudToDestroy3.GetComponentInChildren<testCloud>().FadeOutAndDestory(TriggerCandleScript3.GetComponent<Transform>().position);
            isCloudDestory3 = true;
        }

        ////

        ////Stair rotate

        if (TriggerCandleScript2._bSkillOneTrigger && RotateStair._bIsRotate == false)
        {
            RotateStair.StairRotate();
        }

        ///


        //////////////////////////////////////////////////////////////////////////////////////////////////////////Dectect Buddha level start 
        bool bCandle1 = StartCandle[0].GetComponent<BuddhaCandle>().DetectCandleFinish();
        bool bCandle2 = StartCandle[1].GetComponent<BuddhaCandle>().DetectCandleFinish();
        if (bCandle1 && bCandle2 && iBuddhaLevel == 0)
        {
            iBuddhaLevel = 1;
        }


        //Choose 3number between 1-5  
        else if (iBuddhaLevel == 1)
        {
            for (int i = 0; i < array.Length;)
            {
                bool flag = true;
                int ii = Random.Range(1, 5);
                for (int j = 0; j < i; j++)
                {
                    if (ii == array[j])
                    {
                        flag = false;
                    }
                }
                if (flag)
                {
                    array[i] = ii;
                    i++;
                }
            }
            iBuddhaLevel += 1;
        }

        //fade in in order GameCandle 1 
        else if (iBuddhaLevel == 2)
        {
            
            GameCandle[array[0]].GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.987f, 0f, 1.0f); //Light up candle
            StartCoroutine(BuddhaLevelDelayIEnumerator(2.0f));

        }

        //fade in in order GameCandle 2
        else if (iBuddhaLevel == 3)
        {
            
            GameCandle[array[0]].GetComponent<SpriteRenderer>().color = new Color(0.9294118f, 0.9294118f, 0.9294118f, 1.0f); //Light up candle
            GameCandle[array[1]].GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.987f, 0f, 1.0f); //Light up candle
            StartCoroutine(BuddhaLevelDelayIEnumerator(2.0f));

        }

        //fade in in order GameCandle 3 
        else if (iBuddhaLevel == 4)
        {
           
            GameCandle[array[1]].GetComponent<SpriteRenderer>().color = new Color(0.9294118f, 0.9294118f, 0.9294118f, 1.0f); //Light up candle
            GameCandle[array[2]].GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.987f, 0f, 1.0f); //Light up candle
            StartCoroutine(BuddhaLevelDelayIEnumerator(2.0f));
            

        }

        //fade out 
        else if (iBuddhaLevel == 5)
        {


            for (int i = 0; i < 3; i++)
            {
                GameCandle[array[i]].GetComponent<SpriteRenderer>().color = new Color(0.9294118f, 0.9294118f, 0.9294118f, 1.0f); //Light up candle
                Go.color = new Color(Go.color.r, Go.color.g, Go.color.b, 1.0f);
            }
            iBuddhaLevel = 6;
        }

        //check if Buddha level end
        else if (iBuddhaLevel == 6)
        {

            int iCandleCounter = 0;

            //Detect all candle
            for (int i = 0; i < GameCandle.Length; i++)
            {
                //detect candle and count
                if (GameCandle[i].GetComponent<BuddhaCandle>().DetectCandleFinish())
                {
                    ilightUpArray[iCandleCounter] = i;
                    iCandleCounter++;
                }
            }
            

            //one candle light up
            if(iCandleCounter == 1)
            {
                if(!GameCandle[array[0]].GetComponent<BuddhaCandle>().DetectCandleFinish()) // if is not first one 
                {
                    resetCandle();
                    iCandleCounter = 0; // reset counter
                }

            }

            //two candle light up
            if (iCandleCounter == 2)
            {
                if (!GameCandle[array[1]].GetComponent<BuddhaCandle>().DetectCandleFinish()) // if is not first one 
                {
                    resetCandle();
                    iCandleCounter = 0; // reset counter
                }

            }


            //three candle light up
            if (iCandleCounter == 3)
            {
                if (!GameCandle[array[2]].GetComponent<BuddhaCandle>().DetectCandleFinish()) // if is not first one 
                {
                    resetCandle();
                    iCandleCounter = 0; // reset counter
                }
                //finish 
                else
                {
                    iBuddhaLevel++;
                }

            }


        }

        //finish
        else if (iBuddhaLevel == 7)
        {
            Go.color = new Color(Go.color.r, Go.color.g, Go.color.b, 0.0f);
            Finish.color = new Color(Finish.color.r, Finish.color.g, Finish.color.b, 1.0f);
            iBuddhaLevel += 1;

            //raise rock stair up
            StartCoroutine(RaiseGameObjectUp(Stair,2.5f));
            CameraParallaxManager.ShortFollowing( 2.0f , Stair.GetComponent<Transform>().position);
            
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //spread fog candle
        if (FogGateCandleScript != null && PlayerSkill.CURRENTSKILL == 1)
        {
            if (FogGateCandleScript._bSkillOneTrigger)
            {
                Destroy(FogGate);
            }

        }

        //BearGate
        if (BearGateCandleScript._bSkillOneTrigger && !BearGateAnimate && PlayerSkill.CURRENTSKILL == 1)
        {
            BearGateAnimate = true;
            StartCoroutine(BearGateDown());
        }

        //spread water
        if (WaterGateCandleScript != null )
        {
            if (WaterGateCandleScript._bSkillOneTrigger && PlayerSkill.CURRENTSKILL == 1)
            {
                Destroy(WaterGate);
                StartCoroutine(TrapGrowing());
            }

        }


        ///////////////////////////////////////////////////////////////////////////////////////////Bear on mountain
        ///
        if (!bBearHurting)
        {
            //Player run to check point
            if (BearStage == BearMovementStage.DetectPlayer && GameObject.Find("Player").GetComponent<Transform>().position.x >= BearCheckPointTransform[0].position.x)
            {
                //Camera + Shaking
                CameraParallaxManager.ShortFollowing(4.5f, Bear.transform.position);
                StartCoroutine(CameraParallaxManager.Shake(2.0f, 2.5f, 0.15f));


                //Wait 1.0s ,for camera position, than turn to "BearRoar" stage
                StartCoroutine(BearNextStageWait(1.0f));

                BearStage++;

            }

            //Bear roar
            else if (BearStage == BearMovementStage.BearRoar)
            {
                //Bear fade out color
                StartCoroutine(BearColorFadeOut());

                //Bear start Roar
                Bear.GetComponent<BearMovement>().Howl();

                //Wait for next stage
                StartCoroutine(BearNextStageWait(1.0f));
                BearStage++;
            }

            //Fog blow out
            else if (BearStage == BearMovementStage.FogBlow)
            {
                //Fog blow out from ripple position
                BearFog.GetComponent<testCloud>().FadeOutAndDestory(Bear.transform.GetChild(0).position);

                BearStage++;

                //Wait for 1.0f second turn to "Run" stage
                StartCoroutine(BearNextStageWait(Bear.GetComponent<BearMovement>().fBearHowlTime - 1));
            }

            //Run
            else if (BearStage == BearMovementStage.Run)
            {
                Bear.GetComponent<Rigidbody2D>().velocity = new Vector2(11.0f, 0.0f);

                //camera back to player
                if (Mathf.Abs(CameraParallaxManager.MainCameraPosition.transform.position.x - GameObject.Find("Player").transform.position.x) <= 5.0f)
                {
                    BearStage++;
                }
            }

            //Back to Start position
            else if (BearStage == BearMovementStage.ResetBear)
            {
                //Bear start Roar again
                Bear.GetComponent<BearMovement>().Howl();

                //back to original position
                Bear.GetComponent<Transform>().position = new Vector3(142.457f, 13.225f, 0f);
                Bear.GetComponent<Transform>().rotation = Quaternion.Euler(0f, 0f, 0f);


                //set camera projection
                CameraParallaxManager.ChangeCameraProjectionSize(FinalCamera , 7.5f , 2.0f);
                CameraParallaxManager.ChangeCamraFollowingTargetPosition(-6.0f, 5f, 0.8f, true, false);
                
                BearStage++;

                //Wait for 1.0f second turn to "Run" stage
                StartCoroutine(BearNextStageWait(4.0f));
            }

            //Run 
            else if (BearStage == BearMovementStage.Run2)
            {
              
                Bear.GetComponent<Rigidbody2D>().velocity = new Vector2(11.0f, 0.0f);

                //camera back to player
                if (Mathf.Abs(CameraParallaxManager.MainCameraPosition.transform.position.x - GameObject.Find("Player").transform.position.x) <= 5.0f)
                {
                    BearStage++;
                }
            }

            //decrease speed
            else if (BearStage == BearMovementStage.DecreaseSpeed)
            {
                //if touch "decrease velocity" check point 
                if (GameObject.Find("Bear").GetComponent<Transform>().position.x >= BearCheckPointTransform[1].position.x)
                {
                    Debug.Log("減速");
                    Bear.GetComponent<Rigidbody2D>().velocity = new Vector2(5.0f, 0.0f);
                }
                //regular speed 
                else
                {
                    Bear.GetComponent<Rigidbody2D>().velocity = new Vector2(11.0f, 0.0f);
                }


                if (GameObject.Find("Bear").GetComponent<Transform>().position.x >= BearCheckPointTransform[2].position.x)
                {
                    //HeadUp
                    Bear.GetComponent<Transform>().rotation = Quaternion.Euler(0f, 0f, -20f);
                    BearStage++;
                }
            }

            //Head Up
            else if (BearStage == BearMovementStage.HeadUp)
            {
                //抬頭
                Debug.Log("抬頭");
                Bear.GetComponent<Rigidbody2D>().velocity = new Vector2(5f, 0.0f);


                if (GameObject.Find("Bear").GetComponent<Transform>().position.x >= BearCheckPointTransform[3].position.x)
                {
                    BearStage++;
                }
            }

            //finish climbing, keep running
            else if (BearStage == BearMovementStage.FinishClimbingDown)
            {
                Debug.Log("結束下牆");
                Bear.GetComponent<Rigidbody2D>().velocity = new Vector2(12.0f, 0.0f);

                //Avoid Bear Fall Down rotation
                if (AvoidBearFallDownTrigger.GetComponent<AvoidFallDownTrigger>()._bSkillOneTrigger && bAvoid == false)
                {
                    Bear.GetComponent<Transform>().rotation = Quaternion.Euler(0f, 0f, 0f);
                    bAvoid = true;
                }

            }

            //Bear touch rock, stop run
            if (RockDamage != null && BearStage < BearMovementStage.BearTouchRock)
            {
                if (RockDamage.GetComponent<RockBearDamage_Level2>()._bSkillOneTrigger)
                {
                    Bear.GetComponent<Animator>().SetTrigger("tAttack");
                    BearStage = BearMovementStage.BearTouchRock;
                }
            }


            //Bear Damage Rock 
            else if (BearStage == BearMovementStage.BearTouchRock)
            {
                //start next stage 
                StartCoroutine(BearNextStageWait(0.9f));
                BearStage++;

            }

            //Bear Damage Rock 
            else if (BearStage == BearMovementStage.RockDamage)
            {
                RockDamageParticle.Play();
                Destroy(RockDamage);

                //start next stage 
                StartCoroutine(BearNextStageWait(1.1f));
                BearStage++;
            }

            else if (BearStage == BearMovementStage.Run3)
            {
                Bear.GetComponent<Rigidbody2D>().velocity = new Vector2(12.0f, 0.0f);
                if (GameObject.Find("Bear").GetComponent<Transform>().position.x >= BearCheckPointTransform[4].position.x)
                {
                    BearStage++;
                }
            }


            else if (BearStage == BearMovementStage.GoThroughtDogGate)
            {
                //Gate Down
                if (BearGateAnimate)
                {
                    Bear.GetComponent<Animator>().SetTrigger("tClimb");

                    //start next stage 
                    StartCoroutine(BearNextStageWait(3.08f));
                    BearStage++;

                }

                //Gate doesn't get down
                else
                {
                    Bear.GetComponent<Rigidbody2D>().velocity = new Vector2(12.0f, 0.0f);
                }

                if (GameObject.Find("Bear").GetComponent<Transform>().position.x >= BearCheckPointTransform[5].position.x)
                {
                    BearStage = BearMovementStage.OverGate;
                }
               

            }

            //Finish Climb change rigibody position
            else if (BearStage == BearMovementStage.ClimbFinish)
            {
                
                //change position
                Bear.transform.position = new Vector2(192.34f, 4.2259f);

                BearStage++;

            }

            else if (BearStage == BearMovementStage.OverGate)
            {

                Bear.GetComponent<Rigidbody2D>().velocity = new Vector2(12.0f, 0.0f);


                //Bear hit floor
                if (FloorDetectScript1._bSkillOneTrigger)
                {
                    BearStage++;
                }

            }

            else if (BearStage == BearMovementStage.HitFloor1)
            {
                //start bear animation
                Bear.GetComponent<Animator>().SetTrigger("tAttack");

                //start next stage 
                StartCoroutine(BearNextStageWait(2.0f));
                BearStage++;
            }


            else if (BearStage == BearMovementStage.Run4)
            {
                Bear.GetComponent<Rigidbody2D>().velocity = new Vector2(12.0f, 0.0f);

                //Bear hit floor
                if (FloorDetectScript2._bSkillOneTrigger)
                {
                    BearStage++;
                }
            }


            else if (BearStage == BearMovementStage.HitFloor2)
            {
                //start bear animation
                Bear.GetComponent<Animator>().SetTrigger("tAttack");

                //start next stage 
                StartCoroutine(BearNextStageWait(2.0f));
                BearStage++;
            }

            else if (BearStage == BearMovementStage.FinalRun)
            {
                Bear.GetComponent<Rigidbody2D>().velocity = new Vector2(12.0f, 0.0f);
            }


        }

        //Fog Gate Open Hurt
        if (FogGateCandleScript == null && bCanDetectHurt && bFogHurt == false)
            if (Bear.GetComponent<Transform>().position.y < 2.8f   && bBearHurting == false )
            {
                //set bool
                bBearHurting = true;
                bCanDetectHurt = false;
                bFogHurt = true;

                //hurt animation
                Bear.GetComponent<BearMovement>().Hurt();

                StartCoroutine(BearHurtCount(Bear.GetComponent<BearMovement>().fBearHurtTime));
            }


        if (bCanDetectHurt && bTrap1Hurt == false && !WaterGateCandleScript )
            if (Trap1.GetComponent<DetectBearTrigger>()._bSkillOneTrigger && bBearHurting == false)
            {
                bBearHurting = true;
                bCanDetectHurt = false;
                bTrap1Hurt = true;
                Bear.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 0.0f);
                Bear.GetComponent<BearMovement>().Hurt();
                StartCoroutine(BearHurtCount(Bear.GetComponent<BearMovement>().fBearHurtTime));
            
            }

        if(bCanDetectHurt && bTrap2Hurt == false && !WaterGateCandleScript)
            if (Trap2.GetComponent<DetectBearTrigger>()._bSkillOneTrigger && bBearHurting == false)
            {
                bBearHurting = true;
                bCanDetectHurt = false;
                bTrap2Hurt = true;
                Bear.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 0.0f);
                Bear.GetComponent<BearMovement>().Hurt();
                StartCoroutine(BearHurtCount(Bear.GetComponent<BearMovement>().fBearHurtTime));

            }

    }

    ////Bear Health
    IEnumerator BearHurtCount( float resetTimer)
    {
        yield return new WaitForSeconds(resetTimer);
        bBearHurting = false;
        yield return new WaitForSeconds(1.0f);
        bCanDetectHurt = true;
    }

    ///




    ////reset candle
    void resetCandle()
    {
        GameCandle[0].GetComponent<BuddhaCandle>().ResetCandle(); 
        GameCandle[1].GetComponent<BuddhaCandle>().ResetCandle(); 
        GameCandle[2].GetComponent<BuddhaCandle>().ResetCandle(); 
        GameCandle[3].GetComponent<BuddhaCandle>().ResetCandle(); 
    }

    ////

    ////Buddha Level Delay
    IEnumerator BuddhaLevelDelayIEnumerator(float fTime)
    {
        yield return new WaitForSeconds(fTime);
        iBuddhaLevel += 1;
        StopAllCoroutines();
    }


    ////Bear Gate
    IEnumerator BearGateDown()
    {
        for (float f = 0; f < 3; f+= 0.03f)
        {
            BearGate.transform.position = new Vector2(BearGate.transform.position.x, BearGate.transform.position.y - 0.03f );
            yield return new WaitForSeconds(0.003f);
        }
    }


    //turn trap big
    IEnumerator TrapGrowing()
    {

        yield return new WaitForSeconds(2.0f);

        if (Plant)
        {
            Plant.GetComponent<Animator>().SetTrigger("tGrowUp");
            Plant.GetComponent<EdgeCollider2D>().enabled = true;
        }
            
        for (float f = 0; f < 1.6; f += 0.03f)
        {
            Trap1.transform.localScale = new Vector2(Trap1.transform.localScale.x + 0.03f, Trap1.transform.localScale.y + 0.03f);
            Trap1.transform.position = new Vector2(Trap1.transform.position.x, Trap1.transform.position.y + 0.01f);

            Trap2.transform.localScale = new Vector2(Trap2.transform.localScale.x + 0.03f, Trap2.transform.localScale.y + 0.03f);
            Trap2.transform.position = new Vector2(Trap2.transform.position.x, Trap2.transform.position.y + 0.01f);

            //Plant.transform.position = new Vector2(Plant.transform.position.x, Plant.transform.position.y + 0.02f);
            yield return new WaitForSeconds(0.003f);
        }
    }

    IEnumerator RaiseGameObjectUp(GameObject gameObject, float heightDegree)
    {
        float targetY = gameObject.transform.position.y + heightDegree;
        
        while (gameObject.transform.position.y < targetY)
        {
            gameObject.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 0.03f);
            yield return new WaitForSeconds(0.003f);
        }
        

    }

    //Wait for a time to next stage
    IEnumerator BearNextStageWait(float time)
    {
        yield return new WaitForSeconds(time);
        BearStage++;


    }

    IEnumerator BearColorFadeOut()
    {
        SpriteRenderer BearSprite = Bear.GetComponent<SpriteRenderer>();

        while (BearSprite.color.a < 1.0f || BearSprite.color.r < 1.0f || BearSprite.color.g < 1.0f || BearSprite.color.b < 1.0f)
        {
            //r
            if (BearSprite.color.r < 1.0f)
            {
                BearSprite.color = new Vector4(BearSprite.color.r + 0.03f, BearSprite.color.g, BearSprite.color.b, BearSprite.color.a);
            }
            //g
            if (BearSprite.color.g < 1.0f)
            {
                BearSprite.color = new Vector4(BearSprite.color.r , BearSprite.color.g+ 0.03f, BearSprite.color.b, BearSprite.color.a);
            }
            //b
            if (BearSprite.color.b < 1.0f)
            {
                BearSprite.color = new Vector4(BearSprite.color.r , BearSprite.color.g, BearSprite.color.b+ 0.03f, BearSprite.color.a);
            }
            //a
            if (BearSprite.color.a < 1.0f)
            {
                BearSprite.color = new Vector4(BearSprite.color.r , BearSprite.color.g, BearSprite.color.b, BearSprite.color.a + 0.03f);
            }
            yield return new WaitForSeconds(0.03f);
        }
        BearSprite.color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
    }

   


}
