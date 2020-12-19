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
    bool bTrap1Hurt = false;
    bool bTrap2Hurt = false;


    enum BearMovementStage
    {
        DetectPlayer = 1,
        BearRoar = 3,
        FogBlow = 5,
        Run = 7,
        StopBearMove = 8,
        Run2 = 10,
        WaitForJump = 11 ,
        Jump = 12,
        FinishClimbingDown = 13,
        BearStartRunAgain = 15,
        BearTouchRock = 16,
        RockDamage = 18,
        Run3 = 20,
        HurtAgain = 21,
        GoThroughtDogGate = 23,
        OverGate = 24,
        HitFloor1 = 25,
        Run4 = 27,
        HitFloor2 = 28,
        FinalRun = 30,

    }
    BearMovementStage BearStage = BearMovementStage.Run2;

    //water destory detect
    public GameObject Trap1;
    public GameObject Trap2;
    public GameObject Plant;


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

        //water fall detect
        if(WaterParticles != null)
        {
            GameObject particle;
            for (int i = 0;i < 174 ; i++)
            {
                particle = WaterParticles.transform.GetChild(i).gameObject;
                if (particle == null)
                {
                    break;
                }

                if (particle.transform.position.y < -6)
                {
                    Destroy(particle);
                }
            }


        }


        ///////////////////////////////////////////////////////////////////////////////////////////Bear Movement
        ///
        if (!bBearHurting)
        {
            //Player run to check point
            if (GameObject.Find("Player").GetComponent<Transform>().position.x >= BearCheckPointTransform[0].position.x && BearStage == BearMovementStage.DetectPlayer)
            {

                //Camera 
                CameraParallaxManager.ShortFollowing(4.5f, Bear.transform.position);

                //Wait for 1.0f second turn to "BearRoar" stage
                StartCoroutine(BearNextStageWait(1.0f));

                BearStage++;

            }

            else if (BearStage == BearMovementStage.BearRoar)
            {
                //Bear fade out color
                StartCoroutine(BearColorFadeOut());

                //Bear start Roar
                Bear.GetComponent<BearMovement>().Howl();

                //turn Bear color

                //Set Camera Shaking
                StartCoroutine(CameraParallaxManager.Shake(0.5f, 1.5f, 0.15f));

                //Wait for 1.0f second turn to "Fog blow" stage
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
                StartCoroutine(BearNextStageWait(3.0f));
            }



            //Run
            else if (BearStage == BearMovementStage.Run)
            {
                Bear.GetComponent<SpriteRenderer>().color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);


                Bear.GetComponent<Rigidbody2D>().velocity = new Vector2(11.0f, 0.0f);

                //if touch Jump check point
                if (GameObject.Find("Bear").GetComponent<Transform>().position.x >= BearCheckPointTransform[1].position.x)
                {
                    BearStage++;
                }
            }

            //stop bear move and back to original position
            else if (BearStage == BearMovementStage.StopBearMove)
            {
                //back to original position
                Bear.GetComponent<Transform>().position = new Vector3(142.457f, 13.225f, 0f);
                Bear.GetComponent<Transform>().rotation = Quaternion.Euler(0f, 0f, 0f);

                //stop move
                Bear.GetComponent<Rigidbody2D>().velocity = new Vector3(0f, 0f, 0f);

                BearStage++;

                //Wait for 1.0f second turn to "Run" stage
                StartCoroutine(BearNextStageWait(0.5f));
            }

            //Run
            else if (BearStage == BearMovementStage.Run2)
            {
                Bear.GetComponent<Rigidbody2D>().velocity = new Vector2(11.0f, 0.0f);

                //if touch Jump check point
                if (GameObject.Find("Bear").GetComponent<Transform>().position.x >= BearCheckPointTransform[1].position.x)
                {
                    BearStage++;
                }
            }

            //
            else if (BearStage == BearMovementStage.WaitForJump)
            {
                //減速
                Debug.Log("減速");
                Bear.GetComponent<Rigidbody2D>().velocity = new Vector2(5.0f, 0.0f);

                if (GameObject.Find("Bear").GetComponent<Transform>().position.x >= BearCheckPointTransform[2].position.x)
                {
                    BearStage++;
                }

            }

            else if (BearStage == BearMovementStage.Jump)
            {
                //抬頭
                Debug.Log("抬頭");
                Bear.GetComponent<Rigidbody2D>().velocity = new Vector2(5f, 0.0f);

                //盡量讓他轉一次就好減少bug
                Bear.GetComponent<Transform>().Rotate(0f, 0f, 3.0f);


                if (GameObject.Find("Bear").GetComponent<Transform>().position.x >= BearCheckPointTransform[3].position.x)
                {
                    BearStage++;
                }

            }

            else if (BearStage == BearMovementStage.FinishClimbingDown)
            {
                Debug.Log("結束下牆");
                Bear.GetComponent<Rigidbody2D>().velocity = new Vector2(12.0f, 0.0f);

                //Avoid Bear Fall Down rotation
                if (AvoidBearFallDownTrigger.GetComponent<AvoidFallDownTrigger>()._bSkillOneTrigger && bAvoid == false)
                {
                    Bear.GetComponent<Transform>().rotation = Quaternion.Euler(0f, 0f, -10f);
                    bAvoid = true;
                }

            }

            /////////////////////////////////////////////////Bear Hurt
            else if (Bear.GetComponent<Transform>().position.y < 2.8f && FogGateCandleScript == null && bHurting == false)
            {
                //stop run, stop last stage
                BearStage++;

                //hurt animation
                Bear.GetComponent<BearMovement>().Hurt();

                //hurt animation switch
                bHurting = true;

                //start next stage 
                StartCoroutine(BearNextStageWait(Bear.GetComponent<BearMovement>().fBearHurtTime));
            }

            /////////////////////////////////////////////////Bear Run
            else if (BearStage == BearMovementStage.BearStartRunAgain)
            {
                Debug.Log("StartRunAgain");
                Bear.GetComponent<Rigidbody2D>().velocity = new Vector2(12.0f, 0.0f);

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

            else if (BearStage == BearMovementStage.HurtAgain)
            {
                //stop run, stop last stage
                BearStage++;

                //hurt animation
                Bear.GetComponent<BearMovement>().Hurt();

                //hurt animation switch
                bHurting = true;

                //start next stage 
                StartCoroutine(BearNextStageWait(Bear.GetComponent<BearMovement>().fBearHurtTime));
            }


            else if (BearStage == BearMovementStage.GoThroughtDogGate)
            {
                //Gate Down
                if (BearGateAnimate)
                {

                    //Climb tree
                    Bear.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 13.0f);

                    if (Bear.GetComponent<Transform>().position.y >= 15.5)
                    {
                        Bear.GetComponent<Rigidbody2D>().velocity = new Vector2(13.0f, 0.0f);

                    }

                }

                //Gate doesn't get down
                else
                {
                    Bear.GetComponent<Rigidbody2D>().velocity = new Vector2(12.0f, 0.0f);
                }

                if (GameObject.Find("Bear").GetComponent<Transform>().position.x >= BearCheckPointTransform[5].position.x)
                {
                    BearStage++;
                }

            }

            else if (BearStage == BearMovementStage.OverGate)
            {
                Bear.GetComponent<Transform>().rotation = Quaternion.Euler(0f, 0f, 0f);

                //if Bear On the floor
                if (Bear.GetComponent<Transform>().position.y < 5.0f)
                {
                    Bear.GetComponent<Rigidbody2D>().velocity = new Vector2(12.0f, 0.0f);

                    //Bear hit floor
                    if (FloorDetectScript1._bSkillOneTrigger)
                    {
                        BearStage++;
                    }
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
        

        if(Trap1 && bCanDetectHurt && bTrap1Hurt == false)
            if (Trap1.GetComponent<DetectBearTrigger>()._bSkillOneTrigger && bBearHurting == false)
            {
                bBearHurting = true;
                bCanDetectHurt = false;
                bTrap1Hurt = true;
                Bear.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 0.0f);
                Bear.GetComponent<BearMovement>().Hurt();
                StartCoroutine(BearHurtCount(Bear.GetComponent<BearMovement>().fBearHurtTime));
            
            }

        if(Trap2 && bCanDetectHurt && bTrap2Hurt == false)
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

        Plant.GetComponent<Animator>().SetTrigger("tGrowUp");
        Plant.GetComponent<EdgeCollider2D>().enabled = true;
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
