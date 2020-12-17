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
    public GameObject BearMovementCheckPoint;
    public Vector2[] BearCheckPointTransform;


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

        Debug.Log(BearMovementCheckPoint.transform.childCount);
        /*for (int i = 0; i < BearMovementCheckPoint.transform.childCount; i++)
        {

            //BearCheckPointTransform[i] = BearMovementCheckPoint.transform.GetChild(i).GetComponent<Transform>().position;

            //Vector2 v = BearMovementCheckPoint.transform.GetChild(i).GetComponent<Transform>().position;
            //BearCheckPointTransform[i].position = BearMovementCheckPoint.transform.TransformPoint(BearMovementCheckPoint.transform.GetChild(i).GetComponent<Transform>().position);
            //BearCheckPointTransform[i].position = v;
            Debug.Log(BearMovementCheckPoint.transform.GetChild(i).GetComponent<Transform>().position);
        }*/
        Debug.Log(BearMovementCheckPoint.transform.GetChild(0).GetComponent<Transform>().position);
        Debug.Log(BearMovementCheckPoint.transform.GetChild(1).GetComponent<Transform>().position);




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


    }


    ////reset candle
    void resetCandle()
    {
        GameCandle[0].GetComponent<BuddhaCandle>().ResetCandle(); 
        GameCandle[1].GetComponent<BuddhaCandle>().ResetCandle(); 
        GameCandle[2].GetComponent<BuddhaCandle>().ResetCandle(); 
        GameCandle[3].GetComponent<BuddhaCandle>().ResetCandle(); 
    }



    ///



    ////Cloud

    IEnumerator ChangeCloudColorIEnumerator(GameObject Cloud)
    {
        SpriteRenderer[] childrenCloud = Cloud.GetComponentsInChildren<SpriteRenderer>();

        for (float a = childrenCloud[0].color.a; a > 0.0f; a -= 0.05f)
        {
            foreach (SpriteRenderer sprite in childrenCloud)
            {
                sprite.color = new Vector4(sprite.color.r, sprite.color.g, sprite.color.b, a);
            }

            yield return new WaitForSeconds(0.05f);
        }

        Destroy(Cloud);
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

}
