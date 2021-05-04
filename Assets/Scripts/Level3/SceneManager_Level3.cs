using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public GameObject GameBoard;
    public GameObject[] StartCandle;
    public GameObject[] GameCandle;
    public GameObject BGMask;
    public GameObject levelUI;
    public GameObject levelUITitle;
    public GameObject levelUIText;
    public GameObject StartGame;
    bool bGameFinish = true;
    public Transform VitaSoulGamePos;
    float fLerpTimer = 0.0f;
    Vector2 originalVector2;

    IEnumerator StartCandleIEnumerator;
    IEnumerator ReverseCandleIEnumerator;

    enum BuddhaCandleStateNUM
    {
        Start = 0,
        ChooseNUM = 1,
        RisingUp = 2,
        FadeOutLevelUI = 4,
        Dialogue1 = 6,
        Dialogue2 = 7,
        FinishDialogue = 8,
        FadeInPrompt = 10,
        FadeOutPrompt = 12,

        FadeIn1 = 14,
        FadeIn2 = 16,
        FadeIn3 = 18,
        FadeOut = 20,
        PlayerDetecting = 22,
        CheckAns = 23,
        FinishVitaSetting = 25,
        Finish = 27


    }
    BuddhaCandleStateNUM BuddhaCandleState = BuddhaCandleStateNUM.Start;

    
    //int iBuddhaLevel = 0;
    int iOrder = 0;
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

    //Trigger three status
    public GameObject[] StatusTrigger;
    IEnumerator test;

    //detect drowing 
    public PlayerTriggerDetect DetectDrowingScript;
    bool bDrowing = false;

    //player dead loader
    private PlayerDeadLoader PlayerDeadLoaderScript;

    //Dialogue
    private Dialogue Dialogue;
    private NPC_Dialogue VitaDialogueScript;
    public Sprite PlayerCV;
    public Sprite VitaCV;
    //UI set
    [SerializeField]
    private GameObject CGMove;
    private VitaCGMovement CGMoveScript;

    void Start()
    {
        VitaParticleScript = Vita.GetComponent<VitaSoul_particle>();

        MovingPlatformTriggerColorChange = MovingPlatform.GetComponentInChildren<ColorChange>();

        WaterWheelScript = WaterWheel.GetComponent<WaterWheel>();

        playerSkillScript = GameObject.Find("Player").GetComponent<PlayerSkill>();

        ilightUpArray[0] = -1;
        ilightUpArray[1] = -1;
        ilightUpArray[2] = -1;

        PlayerDeadLoaderScript = GameObject.Find("PlayerDeadLoaderCanvas").GetComponent<PlayerDeadLoader>();

        originalVector2 = GameBoard.transform.localPosition;

        //Dialogue
        VitaDialogueScript = Vita.GetComponent<NPC_Dialogue>();
        //new dialogue
        Dialogue = new Dialogue();

        // UI
        CGMoveScript = CGMove.GetComponent<VitaCGMovement>();

    }

    private void FixedUpdate()
    {   
        Vita.GetComponent<VitaSoul_particle>().FollowObj();       
    }


    // Update is called once per frame
    void Update()
    {
        /////////////////////////detect drowing
        if (DetectDrowingScript.bTrigger && !bDrowing)
        {
            bDrowing = true;

            StopAllCoroutines();

            //player rigibody edit
            GameObject.Find("Player").GetComponent<PlayerMovement>().canMove = false;
            GameObject.Find("Player").GetComponent<Rigidbody2D>().gravityScale = 0.5f;
            GameObject.Find("Player").GetComponent<Rigidbody2D>().drag = 8.0f;
            GameObject.Find("Player").GetComponent<Animator>().SetTrigger("tDrowing");

            //create bubble
            GameObject.Find("Bubble").GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

            PlayerDeadLoaderScript.TransitionAfterTime(3.0f);
            StartCoroutine(ReloadSceneAfterTimeIEnumerator(3.65f));

            //reset UI
            if(this.gameObject.GetComponent<SkillManager_v2>().FadeInUI != null)
            {
                StopCoroutine(this.gameObject.GetComponent<SkillManager_v2>().FadeInUI);
                this.gameObject.GetComponent<SkillManager_v2>().FadeInUI = this.gameObject.GetComponent<SkillManager_v2>().FadeOutSkillIconIEnumerator();
                StartCoroutine(this.gameObject.GetComponent<SkillManager_v2>().FadeInUI);
            }
                

        }
        
        //////////////////////////////////////////////////////////////////////////////////////////////
        //skill 1 description
        if (bSkill1Des_open == false && GameObject.Find("Player").transform.position.x >= -7.35f)
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
        //Debug.Log(Vita.GetComponent<GazeMovement>().bVitaSoulCanGaze);

        StartCandle[0].GetComponent<SkillOneTriggerIcon>().DetectFinish();
        StartCandle[1].GetComponent<SkillOneTriggerIcon>().DetectFinish();
        bool bCandle1 = StartCandle[0].GetComponent<SkillOneTriggerIcon>().bTriggerFinish;
        bool bCandle2 = StartCandle[1].GetComponent<SkillOneTriggerIcon>().bTriggerFinish;

        //bool bCandle1 = true;
        //bool bCandle2 = true;


        //bool bCandle1 = true;
        //bool bCandle2 = true;
        if (bCandle1 && bCandle2 && BuddhaCandleState == BuddhaCandleStateNUM.Start)
        {
            BuddhaCandleState = BuddhaCandleStateNUM.ChooseNUM;
        }

        //Choose 3number between 1-5  
        else if (BuddhaCandleState == BuddhaCandleStateNUM.ChooseNUM)
        {
            for (int i = 0; i < array.Length;)
            {
                bool flag = true;
                int ii = Random.Range(0, 5);
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
            BuddhaCandleState += 1;

            //camera shaking
            StartCoroutine(this.gameObject.GetComponent<CameraManager>().ChangeCameraFollowingPosition(0.0f, 0.5f,new Vector2(GameBoard.transform.position.x, 7.34f)));
            StartCoroutine(this.gameObject.GetComponent<CameraManager>().Shake(0.5f, 3.5f, 0.05f));


            //set player movement, disable trigger skill
            GameObject.Find("Player").GetComponent<PlayerMovement>().canMove = false;
            PlayerSkill.bCanTriggerSkill = false;

        }

        //rising board up
        else if (BuddhaCandleState == BuddhaCandleStateNUM.RisingUp)
        {
            if (GameBoard.transform.localPosition.y != 0.5)
            {
                GameBoard.transform.localPosition = Vector2.Lerp(originalVector2, new Vector2(GameBoard.transform.localPosition.x, 0.5f), fLerpTimer);
                fLerpTimer += Time.deltaTime / 4.0f;
            }

            //next stage
            else if (GameBoard.transform.localPosition.y == 0.5)
            {
                fLerpTimer = 0;
                BuddhaCandleState++;

                //set camera
                StartCoroutine(this.gameObject.GetComponent<CameraManager>().ChangeCameraProjectionSizeIEnumerator(Camera.main,2.5f , 0.75f , 0.5f));
                
                //set background color
                GameObject.Find("Player").GetComponent<ColorChange>().ColorChanging(new Color(1.0f,1.0f,1.0f,0.0f) , 0.5f,0.5f );
                BGMask.GetComponent<ColorChange>().ColorChanging(new Color(0.0f, 0.0f, 0.0f, 0.74f), 0.75f, 0.5f);
                levelUI.GetComponent<ColorChange>().ColorChanging(new Color(1.0f, 1.0f, 1.0f, 1.0f), 1.0f, 1.75f);
                levelUITitle.GetComponent<ColorChange>().ColorChanging(new Color(1.0f, 1.0f, 1.0f, 1.0f), 1.0f, 2.25f);
                levelUIText.GetComponent<ColorChange>().ColorChanging(new Color(1.0f, 1.0f, 1.0f, 1.0f), 1.0f, 2.75f);

                //vita soul stop following player
                VitaParticleScript.bCanFollow = false;
                StartCoroutine(this.gameObject.GetComponent<SkillManager_v2>().ObjMoveToIEnumerator(Vita.transform, VitaSoulGamePos.position));
                StartCoroutine(BuddhaLevelDelayIEnumerator(4.5f));
            }

        }

        //fade out level UI
        else if (BuddhaCandleState == BuddhaCandleStateNUM.FadeOutLevelUI)
        {
            levelUI.GetComponent<ColorChange>().ColorChanging(new Color(1.0f, 1.0f, 1.0f, 0.0f), 1.0f);
            levelUITitle.GetComponent<ColorChange>().ColorChanging(new Color(1.0f, 1.0f, 1.0f, 0.0f), 1.0f);
            levelUIText.GetComponent<ColorChange>().ColorChanging(new Color(1.0f, 1.0f, 1.0f, 0.0f), 1.0f);

            BuddhaCandleState++;
            StartCoroutine(BuddhaLevelDelayIEnumerator(2.0f));
        }

        else if (BuddhaCandleState == BuddhaCandleStateNUM.Dialogue1)
        {
            Dialogue.name = "薇妲";
            Dialogue.sentences = new string[2];
            Dialogue.sentences[0] = "欸？";
            Dialogue.sentences[1] = "這是什麼啊太酷了吧";
            CGMoveScript.SetRecTransformX(62.98596f);
            FindObjectOfType<DialogueManager>().StartDialogue(Dialogue, VitaCV);
            BuddhaCandleState ++;
        }

        else if (BuddhaCandleState == BuddhaCandleStateNUM.Dialogue2 && DialogueManager.bFinishDialogue)
        {
            Dialogue.name = "莉妲";
            Dialogue.sentences = new string[1];
            Dialogue.sentences[0] = "嘿嘿，按下X鍵就可以....討厭拉";
            CGMoveScript.SetRecTransformX(145.7859f);
            FindObjectOfType<DialogueManager>().StartDialogue(Dialogue, PlayerCV);
            BuddhaCandleState++;
        }

        else if (BuddhaCandleState == BuddhaCandleStateNUM.FinishDialogue && DialogueManager.bFinishDialogue)
        {
            //can trigger skill
            PlayerSkill.bCanTriggerSkill = true;

            //set skill manager
            SkillManager_v2.bDirectTriggerVitaSkill = true;

            BuddhaCandleState++;

            StartCoroutine(BuddhaLevelDelayIEnumerator(2.0f));
        }

        else if (BuddhaCandleState == BuddhaCandleStateNUM.FadeInPrompt)
        {
            StartGame.GetComponent<ColorChange>().ColorChanging(new Color(1.0f, 1.0f, 1.0f, 1.0f), 1.0f);

            BuddhaCandleState++;
            StartCoroutine(BuddhaLevelDelayIEnumerator(3.0f));
        }

        else if (BuddhaCandleState == BuddhaCandleStateNUM.FadeOutPrompt)
        {
            StartGame.GetComponent<ColorChange>().ColorChanging(new Color(1.0f, 1.0f, 1.0f, 0.0f), 1.0f);

            BuddhaCandleState++;
            StartCoroutine(BuddhaLevelDelayIEnumerator(2.0f));
        }


        //fade in in order GameCandle 1 
        else if (BuddhaCandleState == BuddhaCandleStateNUM.FadeIn1)
        {
            StartCandleIEnumerator = GameCandle[array[0]].GetComponent<BuddhaCandle>().ChangeCandleColorIEnumerator(2.0f);
            StartCoroutine(StartCandleIEnumerator);

            StartCoroutine(BuddhaLevelDelayIEnumerator(3.0f));
            BuddhaCandleState++;
        }

        //fade in in order GameCandle 2
        else if (BuddhaCandleState == BuddhaCandleStateNUM.FadeIn2)
        {
            if (StartCandleIEnumerator != null)
                StopCoroutine(StartCandleIEnumerator);

            ReverseCandleIEnumerator = GameCandle[array[0]].GetComponent<BuddhaCandle>().ChangeBackCandleColorIEnumerator(1.0f);
            StartCoroutine(ReverseCandleIEnumerator);

            StartCandleIEnumerator = GameCandle[array[1]].GetComponent<BuddhaCandle>().ChangeCandleColorIEnumerator(2.0f);
            StartCoroutine(StartCandleIEnumerator);

            StartCoroutine(BuddhaLevelDelayIEnumerator(3.0f));
            BuddhaCandleState++;
        }

        //fade in in order GameCandle 3 
        else if (BuddhaCandleState == BuddhaCandleStateNUM.FadeIn3)
        {
            if (StartCandleIEnumerator != null)
                StopCoroutine(StartCandleIEnumerator);

            ReverseCandleIEnumerator = GameCandle[array[1]].GetComponent<BuddhaCandle>().ChangeBackCandleColorIEnumerator(1.0f);
            StartCoroutine(ReverseCandleIEnumerator);

            StartCandleIEnumerator = GameCandle[array[2]].GetComponent<BuddhaCandle>().ChangeCandleColorIEnumerator(2.0f);
            StartCoroutine(StartCandleIEnumerator);
            StartCoroutine(BuddhaLevelDelayIEnumerator(3.0f));
            BuddhaCandleState++;

        }

        //fade out 
        else if (BuddhaCandleState == BuddhaCandleStateNUM.FadeOut)
        {
            ReverseCandleIEnumerator = GameCandle[array[2]].GetComponent<BuddhaCandle>().ChangeBackCandleColorIEnumerator(1.0f);
            StartCoroutine(ReverseCandleIEnumerator);

            StartCoroutine(BuddhaLevelDelayIEnumerator(1.5f));
            BuddhaCandleState++;
        }

        //check if Buddha level end
        else if (BuddhaCandleState == BuddhaCandleStateNUM.PlayerDetecting)
        {
            
            //Detect all candle
            for (int i = 0; i < GameCandle.Length; i++)
            {
                GameCandle[i].GetComponent<BuddhaCandle>().DetectCandleFinish();

                //if this light never record
                if (ilightUpArray[0] != i && ilightUpArray[1] != i && ilightUpArray[2] != i && GameCandle[i].GetComponent<BuddhaCandle>().bTriggerFinish)
                {
                    ilightUpArray[iOrder] = i;
                    iOrder++;
                }


            }

            //all number in
            if (ilightUpArray[0] >= 0 && ilightUpArray[1] >= 0 && ilightUpArray[2] >= 0 )
            {
                iOrder = 0;
                BuddhaCandleState++;
            }


        }

        //check candle
        else if (BuddhaCandleState == BuddhaCandleStateNUM.CheckAns)
        {
            bGameFinish = true;
            for (int i = 0; i < 3; i++)
            {
                
                if (ilightUpArray[i] != array[i]) // if is not first one 
                {
                    bGameFinish = false;
                    StartCoroutine(resetCandleIEnumerator());
                    break;
                }
            }

            if(bGameFinish)
                StartCoroutine(BuddhaLevelDelayIEnumerator(0.0f));

            BuddhaCandleState++;

            //Debug.Log("end check");

        }

        else if (BuddhaCandleState == BuddhaCandleStateNUM.FinishVitaSetting)
        {
            SkillManager_v2.bFinishSkill = true;
            BuddhaCandleState++;
            StartCoroutine(BuddhaLevelDelayIEnumerator(2.0f));
        }


        else if (BuddhaCandleState == BuddhaCandleStateNUM.Finish)
        {
            //reset
            SkillManager_v2.bFinishSkill = false;

            if (this.gameObject.GetComponent<SkillManager_v2>().ObjMoveTo != null)
            {
                StopCoroutine(this.gameObject.GetComponent<SkillManager_v2>().ObjMoveTo);
            }


            BGMask.GetComponent<ColorChange>().ColorChanging(new Color(0.0f, 0.0f, 0.0f, 0.0f), 0.75f);

            //set camera
            StartCoroutine(this.gameObject.GetComponent<CameraManager>().ChangeCameraProjectionSizeIEnumerator(Camera.main, 5.0f, 0.75f));

            //set background color
            GameObject.Find("Player").GetComponent<ColorChange>().ColorChanging(new Color(1.0f, 1.0f, 1.0f, 1.0f), 0.5f);

            //set skill manager
            SkillManager_v2.bDirectTriggerVitaSkill = false;

            //vita soul stop following player
            VitaParticleScript.bCanFollow = true;

            //set player movement, disable trigger skill
            //GameObject.Find("Player").GetComponent<PlayerMovement>().canMove = true;

            
            BuddhaCandleState++;

        }
        Debug.Log(GameObject.Find("Player").GetComponent<PlayerMovement>().canMove);
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////// status keep check trigger
        for (int i = 0; i < StatusToLightUp.Length; i++)
        {
            if (iLightUpOrder[i] < 0)
            {
                //detect trigger
                StatusTrigger[i].GetComponent<SkillOneTriggerIcon>().DetectFinish();

                if (StatusTrigger[i].GetComponent<SkillOneTriggerIcon>().bTriggerFinish)
                {
                    //set light up order
                    iLightUpOrder[i] = iCountOrder;
                    iCountOrder++;
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
        BuddhaCandleState += 1;
        //StopAllCoroutines();
    }


    IEnumerator resetCandleIEnumerator()
    {

        //set status icon to red
        for (int i = 0; i < 3; i++)
        {
            Debug.Log(GameCandle[ilightUpArray[i]].name);
            GameCandle[ilightUpArray[i]].GetComponent<SpriteRenderer>().color = Color.red;
        }

        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < 3; i++)
        {
            Debug.Log(ilightUpArray[i]);
            GameCandle[ilightUpArray[i]].GetComponent<ColorChange>().ColorChanging(Color.white, VitaParticleScript.fSkillOneGatheringTime * 0.5f);
        }

        yield return new WaitForSeconds(VitaParticleScript.fSkillOneGatheringTime * 0.5f);

        for (int i = 0; i < 3; i++)
        {
            GameCandle[ilightUpArray[i]].GetComponent<BuddhaCandle>().ResetCandle();
        }
        yield return new WaitForSeconds(1.5f);


        for (int i = 0; i < 3; i++)
        {
            ilightUpArray[i] = -1;
        }



        BuddhaCandleState = BuddhaCandleStateNUM.PlayerDetecting;

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

        //set status icon to red
        for (int i = 0; i < StatusToLightUp.Length; i++)
        {
            StatusTrigger[i].GetComponent<SpriteRenderer>().color = Color.red;
        }

        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < iLightUpOrder.Length; i++)
        {
            StatusTrigger[i].GetComponent<ColorChange>().ColorChanging(Color.white, VitaParticleScript.fSkillOneGatheringTime * 0.5f);
        }

        yield return new WaitForSeconds(VitaParticleScript.fSkillOneGatheringTime * 0.5f);

        for (int i = 0; i < iLightUpOrder.Length; i++)
        {
            StartCoroutine(StatusTrigger[i].GetComponent<SkillOneTriggerIcon>().reset());
        }

        for (int i = 0; i < iLightUpOrder.Length; i++)
        {
            iLightUpOrder[i] = -1;
        }

        iCountOrder = 0;

        /*//set status color to red
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

        iCountOrder = 0;*/

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

    IEnumerator ReloadSceneAfterTimeIEnumerator(float time)
    {
        yield return new WaitForSeconds(time);
        //reload scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
