using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager_Level3 : MonoBehaviour
{

    public GameObject Vita;

    public MovingPlatform MovingPlatform;
    public SkillOneTriggerIcon PlatformTrigger;
    private ColorChange MovingPlatformTriggerColorChange;
    


    private VitaSoul_particle VitaParticleScript;

    //private float ftimer_moving  = 0.0f;
    //private float ftimer_water = 0.0f;

    //player
    private PlayerSkill playerSkillScript;

    //water wheel
    [SerializeField]
    private GameObject WaterWheel;
    private WaterWheel WaterWheelScript;

    [SerializeField]
    public SkillOneTriggerIcon WaterWheelTrigger;

    //particle system
    [SerializeField]
    private ParticleSystem Splash;

    //plant
    [SerializeField]
    private Plant PlantScript;


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
    public SpriteRenderer StatusSprite;
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

    //Skill 1 description
    public SkillDiscription SkillDescription_1;
    bool bSkill1Des_open = true;
    bool bSkill1Des_finish = false;

    public SpriteRenderer[] StatusMaterial;

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

        //skill 1 description
        if(bSkill1Des_open == false && GameObject.Find("Player").transform.position.x >= -7.35f)
        {
            StartCoroutine(SkillDescription_1.FadeIn());
            GameObject.Find("Player").GetComponent<PlayerMovement>().canMove = false;
            GameObject.Find("Player").GetComponent<Animator>().SetFloat("Speed", Mathf.Abs(0));
            bSkill1Des_open = true;
        }
        else if (bSkill1Des_open == true && bSkill1Des_finish　== false && Input.GetButtonDown("Submit") && SkillDescription_1.SkillDiscriptionSet[SkillDescription_1.SkillDiscriptionSet.Length -1].image.color.a ==1)
        {
            StopCoroutine(SkillDescription_1.FadeIn());
            StartCoroutine(SkillDescription_1.FadeOut());
            GameObject.Find("Player").GetComponent<PlayerMovement>().canMove = true;
            bSkill1Des_finish = true;
        }



        //move platform
        PlatformTrigger.DetectFinish();
        if (MovingPlatform._bCanMove == false && PlatformTrigger.bTriggerFinish)
        {
            MovingPlatform._bCanMove = true;
        }



        //water wheel rotate
        WaterWheelTrigger.DetectFinish();
        if (WaterWheelScript._bIsRotate == false && WaterWheelTrigger.bTriggerFinish)
        {
           
            Splash.Play();
            WaterWheelScript.PlayWaterWheelRotate();
            PlantScript.GrowUp();

            //set camera 
            this.gameObject.GetComponent<CameraManager>().ShortFollowing(2.0f, PlantScript.GetComponentInParent<Transform>().position);
            StartCoroutine(this.gameObject.GetComponent<CameraManager>().Shake(1.0f, 1.5f, 0.08f));

            

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


                for (int i = 0; i < StatusMaterial.Length; i++)
                {
                    StartCoroutine(ColorChangingIEnumerator(i, new Color(1.0f, 1.0f, 1.0f, 0.0f), 2.0f));
                }
                

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
            StatusSprite.color = CColor[array[i]];
            yield return new WaitForSeconds(2.0f);
        }

        //set jigsaw color to white
        for (int i = 0; i < StatusMatchingJigsawScript.Length; i++)
        {
            StatusMatchingJigsawScript[i].GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            StatusSprite.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
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
    IEnumerator ColorChangingIEnumerator(int iIndex, Color tagetColor, float duration)
    {
        bool bColorFinishChang = false;
        float t = 0;

        while (!bColorFinishChang)
        {
            
            StatusMaterial[iIndex].color = Color.Lerp(StatusMaterial[iIndex].color, tagetColor, t);

            if (t < duration)
            {
                t += Time.deltaTime / duration;
                bColorFinishChang = false;
            }
            else
            {
                bColorFinishChang = true;
            }

            yield return null;
        }

    }


}
