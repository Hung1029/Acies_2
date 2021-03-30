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
    


    // Start is called before the first frame update
    void Start()
    {
        VitaParticleScript = Vita.GetComponent<VitaSoul_particle>();

        MovingPlatformTriggerColorChange = MovingPlatform.GetComponentInChildren<ColorChange>();

        WaterWheelScript = WaterWheel.GetComponent<WaterWheel>();
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


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


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


}
