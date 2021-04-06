using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager_Level3 : MonoBehaviour
{
    public VitaTriggerDetect movePlatformTrigger;

    public GameObject Vita;

    public MovingPlatform MovingPlatform;
    private ColorChange MovingPlatformTriggerColorChange;
    


    private VitaSoul_particle VitaParticleScript;

    private float ftimer_moving  = 0.0f;
    private float ftimer_water = 0.0f;

    //player
    private PlayerSkill playerSkillScript;

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

    //Short camera following
    public CameraParallaxManager_Level1 CameraParallaxManager;

    //Buddha Candle
    public GameObject[] StartCandle;
    public GameObject[] GameCandle;
    public SpriteRenderer Go;
    public SpriteRenderer Finish;
    int iBuddhaLevel = 0;
    int[] array = new int[3]; //set candle number
    int[] ilightUpArray = new int[3]; //set  candle light up number

    //Trigger skill 2 token
    public GameObject TriggerSkill;

    //Status Jisaw
    public MatchingJigsaw[] StatusMatchingJigsawScript;
    bool MatchingJigsawFinish = false;


    //Status light up
    public GameObject[] StatusToLightUp;
    public Color[] CColor;
    private int[] iLightUpOrder = { -1,-1,-1};
    private float[] ftimer_statusLightUp = { 0, 0, 0 };
    int iCountOrder = 0;
    bool bStatusLightUpCorrect = false;


    //status gate open
    public GameObject StatusGate;

    //Bear
    public GameObject Bear;
    public GameObject BearFog;
    public GameObject BearWakeUpCheckPoint;


    //Rock
    public GameObject RockDamage;
    public ParticleSystem RockDamageParticle;

    //climb wall
    public GameObject BearClimbCheckPoint;

    //jump
    public GameObject BearJumpCheckPoint;


    enum BearStageNUM
    {
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
        ClimbFinish = 19,
        Run3 = 20,
        Jump = 21


    }
    BearStageNUM BearStage = BearStageNUM.Run;

    // Start is called before the first frame update
    void Start()
    {
        VitaParticleScript = Vita.GetComponent<VitaSoul_particle>();

        MovingPlatformTriggerColorChange = MovingPlatform.GetComponentInChildren<ColorChange>();

        WaterWheelScript = WaterWheel.GetComponent<WaterWheel>();

        playerSkillScript = GameObject.Find("Player").GetComponent<PlayerSkill>();


           
    }

    private void FixedUpdate()
    {   
        Vita.GetComponent<VitaSoul_particle>().FollowObj();       
    }


    // Update is called once per frame
    void Update()
    {
        //move platform
        if (MovingPlatform._bCanMove == false)
        {
           
            if (movePlatformTrigger._bSkillTrigger)
            {
                if (ftimer_moving == 0)
                {
                    MovingPlatformTriggerColorChange.ColorChanging(Color.green, VitaParticleScript.fSkillOneGatheringTime);
                }

                ftimer_moving += Time.deltaTime;
            
                if (ftimer_moving >= VitaParticleScript.fSkillOneGatheringTime)
                {
                    MovingPlatform._bCanMove = true;

                    //reset timer
                    ftimer_moving = 0.0f;
                }
            }
            //if  move platform is not trigger
            else 
            {
                if (ftimer_moving != 0)
                {
                    //reset timer
                    ftimer_moving = 0.0f;
                    MovingPlatformTriggerColorChange.ColorChanging(Color.yellow, VitaParticleScript.fSkillOneGatheringTime * 0.5f);
                }

            }
        }



        //water wheel rotate
        if (WaterWheelScript._bIsRotate == false)
        {
            if (WaterWheelScript._bSkillOneTrigger && PlayerSkill.CURRENTSKILL == 1)
            {
                ftimer_water += Time.deltaTime;

                if (ftimer_water >= VitaParticleScript.fSkillOneGatheringTime)
                {
                    Splash.Play();
                    WaterWheelScript.PlayWaterWheelRotate();
                    PlantScript.GrowUp();

                    //reset timer
                    ftimer_water = 0.0f;
                }
           
                //set camera 
                /*this.gameObject.GetComponent<CameraParallaxManager_Level1>().ShortFollowing(2.0f, new Vector3(16.6f, -0.66f, 0.0f));
                StartCoroutine(this.gameObject.GetComponent<CameraParallaxManager_Level1>().Shake(1.0f, 1.5f, 0.08f));*/
            }
            else
            {
                ftimer_water = 0;
            }
        }



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
            if (iCandleCounter == 1)
            {
                if (!GameCandle[array[0]].GetComponent<BuddhaCandle>().DetectCandleFinish()) // if is not first one 
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

        }


        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////// status keep check trigger
        for (int i = 0; i < StatusToLightUp.Length; i++)
        {
            if (iLightUpOrder[i] < 0)
            {
                //status trigger
                if (StatusToLightUp[i].GetComponent<VitaTriggerDetect>()._bSkillTrigger)
                {
                    if (ftimer_statusLightUp[i] == 0)
                    {
                        StatusToLightUp[i].GetComponent<ColorChange>().ColorChanging(CColor[i], VitaParticleScript.fSkillOneGatheringTime);
                    }

                    ftimer_statusLightUp[i] += Time.deltaTime;

                    //if jigsaw is match, can start check light up order
                    if (ftimer_statusLightUp[i] >= VitaParticleScript.fSkillOneGatheringTime && MatchingJigsawFinish)
                    {
                        //set light up order
                        iLightUpOrder[i] = iCountOrder;
                        iCountOrder++;

                        //reset timer
                        ftimer_statusLightUp[i] = 0.0f;
                    }
                }
                //if  status is not trigger
                else
                {
                    if (ftimer_statusLightUp[i] != 0)
                    {
                        //reset timer
                        ftimer_statusLightUp[i] = 0.0f;
                        StatusToLightUp[i].GetComponent<ColorChange>().ColorChanging(Color.white, VitaParticleScript.fSkillOneGatheringTime * 0.5f);
                    }

                }
            }

        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        //get skill 2
        if (TriggerSkill)
            if (TriggerSkill.GetComponent<PlayerTrigger>()._bPlayerTrigger)
            {
                playerSkillScript.CanUseSkill2 = true;
                Destroy(TriggerSkill);
            }


        //Status match
        if (!MatchingJigsawFinish)
        {
            int count = 0;
            for (int i = 0; i < StatusMatchingJigsawScript.Length; i++)
            {
                if (StatusMatchingJigsawScript[i].bMatch)
                    count++;
            }
            if (count == 4)
            {
                MatchingJigsawFinish = true;

                //run light up buddha clue
                StartCoroutine(StatusClueIEnumerator());

            }
               
        }


        //light up status after jigsaw is all match 
        if (!bStatusLightUpCorrect && MatchingJigsawFinish)
        {
            //if all status is light up
            if (iLightUpOrder[0] > -1 && iLightUpOrder[1] > -1 && iLightUpOrder[2] > -1)
            {
                //check if all three status light up in correct order
                bool bIfStatusLightUpInOrder = true;
                iCountOrder = 0;
                for (int i = 0; i < array.Length; i++)
                {
                    
                    if (iLightUpOrder[array[i]] != iCountOrder)
                    {   
                        bIfStatusLightUpInOrder = false;
                        break;
                    }
                    iCountOrder++;
                }
                //status light up correct
                if (bIfStatusLightUpInOrder)
                {
                    bStatusLightUpCorrect = true;
                }

                //reset status
                else 
                {
                    StartCoroutine(resetStatusIEnumerator());
                }
                
            }
            
        }
        
        if (StatusGate && bStatusLightUpCorrect)
        {
            Destroy(StatusGate);
            BearStage = BearStageNUM.DetectingPlayer;
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////Bear

        //wait for player run to checkpoint
        if (BearStage == BearStageNUM.DetectingPlayer && GameObject.Find("Player").GetComponent<Transform>().position.x >= BearWakeUpCheckPoint.transform.position.x)
        {
            //camera action

            BearStage++;

            //test
            BearStage++;
            //
        }

        //Bear roar
        else if (BearStage == BearStageNUM.Roar)
        {
            Bear.GetComponent<ColorChange>().ColorChanging(new Color(1.0f, 1.0f, 1.0f, 1.0f),5.0f);
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
            //StartCoroutine(CameraParallaxManager.Shake(1.0f, 2.5f, 0.15f));

            //set camera projection
            //CameraParallaxManager.ChangeCameraProjectionSize(FinalCamera, 7.5f, 2.0f);
            //CameraParallaxManager.ChangeCamraFollowingTargetPosition(-6.0f, 5f, 0.8f, true, false);

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
            //StartCoroutine(CameraParallaxManager.Shake(1.0f, 2.5f, 0.15f));


            //Wait for next stage
            StartCoroutine(BearNextStageWait(Bear.GetComponent<BearMovement>().fBearHowlTime - 1));
            BearStage++;
        }

        //Run
        else if (BearStage == BearStageNUM.Run)
        {
            Bear.GetComponent<Animator>().Play("Bear_Run");
            Bear.GetComponent<Rigidbody2D>().velocity = new Vector2(3.0f, 0.0f);
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
            Bear.GetComponent<Rigidbody2D>().velocity = new Vector2(3.0f, 0.0f);
            if (GameObject.Find("Bear").GetComponent<Transform>().position.x >= BearClimbCheckPoint.transform.position.x)
            {
                BearStage++;
            }
        }

        else if (BearStage == BearStageNUM.GoThroughtDogGate)
        {
            //Gate Down
            if (true)
            {
                Bear.GetComponent<Animator>().SetTrigger("tClimb");

                //start next stage 
                StartCoroutine(BearNextStageWait(3.08f));
                BearStage++;

            }

            //Gate doesn't get down
            /*else
            {
                Bear.GetComponent<Rigidbody2D>().velocity = new Vector2(12.0f, 0.0f);
            }

            if (GameObject.Find("Bear").GetComponent<Transform>().position.x >= BearCheckPointTransform[5].position.x)
            {
                BearStage = BearMovementStage.OverGate;
            }*/
        }

        //Finish Climb change rigibody position
        else if (BearStage == BearStageNUM.ClimbFinish)
        {

            Bear.GetComponent<Animator>().Play("Bear_Run");

            //change position
            Bear.transform.position = new Vector2(81.89f, Bear.transform.position.y);

            BearStage++;

        }

        else if (BearStage == BearStageNUM.Run3)
        {

            Bear.GetComponent<Rigidbody2D>().velocity = new Vector2(3.0f, 0.0f);


            //touch jump check point
            if (GameObject.Find("Bear").GetComponent<Transform>().position.x >= BearJumpCheckPoint.transform.position.x)
            {
                Debug.Log("in");
                BearStage++;
            }

        }

    }

    ////Buddha Level Delay
    IEnumerator BuddhaLevelDelayIEnumerator(float fTime)
    {
        yield return new WaitForSeconds(fTime);
        iBuddhaLevel += 1;
        StopAllCoroutines();
    }

    ////reset candle
    void resetCandle()
    {
        GameCandle[0].GetComponent<BuddhaCandle>().ResetCandle();
        GameCandle[1].GetComponent<BuddhaCandle>().ResetCandle();
        GameCandle[2].GetComponent<BuddhaCandle>().ResetCandle();
        GameCandle[3].GetComponent<BuddhaCandle>().ResetCandle();
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

    IEnumerator StatusClueIEnumerator()
    {
        yield return new WaitForSeconds(1.0f);
        //set jigsaw color to white
        for (int i = 0; i < StatusMatchingJigsawScript.Length; i++)
        {
            StatusMatchingJigsawScript[i].GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }
        yield return new WaitForSeconds(1.0f);

        //pick color order
        for (int i = 0; i < array.Length;)
        {
            bool flag = true;
            int ii = Random.Range(0, 3);
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

        // light up status in particular color order
        for (int i = 0; i < array.Length; i++)
        {
            for (int j = 0; j < StatusMatchingJigsawScript.Length; j++)
            {
                StatusMatchingJigsawScript[j].GetComponent<SpriteRenderer>().color = CColor[array[i]];
            }
            
            yield return new WaitForSeconds(2.0f);
        }

        //set jigsaw color to white
        for (int i = 0; i < StatusMatchingJigsawScript.Length; i++)
        {
            StatusMatchingJigsawScript[i].GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }



    }

    IEnumerator resetStatusIEnumerator()
    {
        //set status color to red
        for (int i = 0; i < StatusToLightUp.Length; i++)
        {
            StatusToLightUp[i].GetComponent<SpriteRenderer>().color = Color.red;
        }

        yield return new WaitForSeconds(0.5f);


        for (int i = 0; i < iLightUpOrder.Length; i++)
        {
            StatusToLightUp[i].GetComponent<ColorChange>().ColorChanging(Color.white, VitaParticleScript.fSkillOneGatheringTime * 0.5f);
        }

        yield return new WaitForSeconds(VitaParticleScript.fSkillOneGatheringTime * 0.5f);

        for (int i = 0; i < iLightUpOrder.Length; i++)
        {
            iLightUpOrder[i] = -1;
        }

        iCountOrder = 0;

    }

    //Wait for Bear next stage
    IEnumerator BearNextStageWait(float time)
    {
        yield return new WaitForSeconds(time);
        BearStage++;

    }

}
