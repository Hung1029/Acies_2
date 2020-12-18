﻿using System.Collections;
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

    enum BearMovementStage
    {
        DetectPlayer = 1,
        BearRoar = 3,
        FogBlow = 5,
        Run = 7,
        WaitForJump ,
        Jump 

    }
    BearMovementStage BearStage = BearMovementStage.DetectPlayer;

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
            //Debug
            //Bear.GetComponent<Animator>().SetTrigger("tRun");
            Bear.GetComponent<SpriteRenderer>().color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
            //Debug


            Debug.Log("in");
            Bear.GetComponent<Rigidbody2D>().velocity = new Vector2(11.0f, 0.0f);

            //if touch Jump check point
            if (GameObject.Find("Bear").GetComponent<Transform>().position.x >= BearCheckPointTransform[1].position.x)
            {
                BearStage++;
            }
        }
        else if (BearStage == BearMovementStage.WaitForJump)
        {
            //減速
            Debug.Log("減速");
            Bear.GetComponent<Rigidbody2D>().velocity = new Vector2(7.0f, 0.0f);

            if (GameObject.Find("Bear").GetComponent<Transform>().position.x >= BearCheckPointTransform[2].position.x)
            {
                BearStage++;
            }

        }
        else if (BearStage == BearMovementStage.Jump)
        {
            //抬頭
            Debug.Log("抬頭");
            Bear.GetComponent<Rigidbody2D>().velocity = new Vector2(8f, 0.0f);
            Bear.GetComponent<Transform>().Rotate(0.0f, 0.0f, 3.0f);


            if (GameObject.Find("Bear").GetComponent<Transform>().position.x >= BearCheckPointTransform[3].position.x)
            {
                BearStage++;
            }

        }

        else if (BearStage == BearMovementStage.Jump + 1)
        {
            Debug.Log("結束下牆");
            Bear.GetComponent<Rigidbody2D>().gravityScale = 50;
            Bear.GetComponent<Rigidbody2D>().angularVelocity = 50;
            Bear.GetComponent<Rigidbody2D>().velocity = new Vector2(12.0f, 0.0f);
        }


    }


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

        for (float f = 0; f < 1.6; f += 0.03f)
        {
            Trap1.transform.localScale = new Vector2(Trap1.transform.localScale.x + 0.03f, Trap1.transform.localScale.y + 0.03f);
            Trap1.transform.position = new Vector2(Trap1.transform.position.x, Trap1.transform.position.y + 0.01f);

            Trap2.transform.localScale = new Vector2(Trap2.transform.localScale.x + 0.03f, Trap2.transform.localScale.y + 0.03f);
            Trap2.transform.position = new Vector2(Trap2.transform.position.x, Trap2.transform.position.y + 0.01f);

            Plant.transform.position = new Vector2(Plant.transform.position.x, Plant.transform.position.y + 0.02f);
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

    private static float WrapAngle(float angle)
    {
        angle %= 360;
        if (angle > 180)
            return angle - 360;

        return angle;
    }



}
