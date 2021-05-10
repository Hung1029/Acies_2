using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager_Level3 : MonoBehaviour
{

    public GameObject Vita;

    public MovingPlatform MovingPlatform;
    public SkillOneTriggerIcon PlatformTrigger;

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
    public Animator SmokeAnimator;
    bool bGameFinish = true;
    public Transform VitaSoulGamePos;
    public GameObject Skill2Token;
    float fLerpTimer = 0.0f;
    Vector2 originalVector2;

    IEnumerator StartCandleIEnumerator;
    IEnumerator ReverseCandleIEnumerator;

    enum BuddhaCandleStateNUM
    {

        Pause = -1,

        Start = 0,
        ChooseNUM = 1,
        RisingUp = 2,
        FadeOutLevelUI = 4,

        FadeInPrompt = 6, //6
        FadeOutPrompt = 8, //8
        FadeIn1 = 10,//10
        FadeIn2 = 12,//12
        FadeIn3 = 14,//14
        FadeOut = 16,//16

        Dialogue1 = 18,//18
        Dialogue2 = 19,//19
        Dialogue3 = 20,//20
        Dialogue4 = 21,//21
        FinishDialogue = 22,//22


        

        PlayerDetecting = 24,
        CheckAns = 25,
        FinishVitaSetting = 27,
        Finish = 29,
        RisingDown = 31,

    }
    BuddhaCandleStateNUM BuddhaCandleState = BuddhaCandleStateNUM.Start;
    bool bBuddhaRestart = false;

    //int iBuddhaLevel = 0;
    int iOrder = 0;
    int[] array = new int[3]; //set candle number
    int[] ilightUpArray = new int[3]; //set  candle light up number


    //Status Jisaw
    public MatchingJigsaw[] StatusMatchingJigsawScript;
    public SpriteRenderer StatusSprite;
    public GameObject MainStatusTrigger;
    bool CanReStart = false;
    public ColorChange[] MainStatusTriggerSprite;
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
    SkillDiscription SkillDescription_1;
    bool bSkill1Des_open = true;
    bool bSkill1Des_finish = false;

    public SpriteRenderer[] StatusMaterial;

    //Trigger three status
    public GameObject[] StatusTrigger;
    public ColorChange[] StatusLight;
    bool[] bColorCanChange = new bool[3];
    bool[] bLastColorState = new bool[3];

    //detect drowing 
    public PlayerTriggerDetect DetectDrowingScript;
    bool bDrowing = false;

    //player dead loader
    private PlayerDeadLoader PlayerDeadLoaderScript;

    //Dialogue
    private Dialogue Dialogue;
    public Sprite PlayerCV;
    public Sprite VitaCV;
    //UI set
    [SerializeField]
    private GameObject CGMove;
    private VitaCGMovement CGMoveScript;

    void Start()
    {
        VitaParticleScript = Vita.GetComponent<VitaSoul_particle>();


        WaterWheelScript = WaterWheel.GetComponent<WaterWheel>();

        playerSkillScript = GameObject.Find("Player").GetComponent<PlayerSkill>();

        ilightUpArray[0] = -1;
        ilightUpArray[1] = -1;
        ilightUpArray[2] = -1;

        PlayerDeadLoaderScript = GameObject.Find("PlayerDeadLoaderCanvas").GetComponent<PlayerDeadLoader>();

        originalVector2 = GameBoard.transform.localPosition;

        //new dialogue
        Dialogue = new Dialogue();

        // UI
        CGMoveScript = CGMove.GetComponent<VitaCGMovement>();

        for (int i = 0; i < bColorCanChange.Length; i++)
        {
            bColorCanChange[i] = true;
        }

        for (int i = 0; i < bLastColorState.Length; i++)
        {
            bLastColorState[i] = false;
        }

        SkillDescription_1 = GameObject.Find("SkillDirection").GetComponent<SkillDiscription>();
    }

    private void FixedUpdate()
    {   
        Vita.GetComponent<VitaSoul_particle>().FollowObj();       
    }


    // Update is called once per frame
    void Update()
    {
        //test
        if (Input.GetKeyDown(KeyCode.T))
        {
            Skill2Token.SetActive(true);
            StartCoroutine(skillTokenTrigger());
        }



        /////////////////////////detect drowing
        if (DetectDrowingScript.bTrigger && !bDrowing)
        {
            bDrowing = true;

            StopAllCoroutines();

            //player rigibody edit
            GameObject.Find("Player").GetComponent<PlayerMovement>().canMove_skill = false;
            GameObject.Find("Player").GetComponent<Rigidbody2D>().gravityScale = 0.5f;
            GameObject.Find("Player").GetComponent<Rigidbody2D>().drag = 8.0f;
            GameObject.Find("Player").GetComponent<Animator>().SetTrigger("tDrowing");

            //create bubble
            GameObject.Find("Bubble").GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

            PlayerDeadLoaderScript.TransitionAfterTime(3.0f);
            StartCoroutine(ReloadSceneAfterTimeIEnumerator(3.65f));

            //reset UI
            this.gameObject.GetComponent<SkillManager_v2>().SkillIconColorChange.ColorChanging(new Color(1.0f, 1.0f, 1.0f, 0.0f), 0.5f);
            this.gameObject.GetComponent<SkillManager_v2>().SkillNameColorChange.ColorChanging(new Color(1.0f, 1.0f, 1.0f, 0.0f), 0.5f);
          


        }

        //////////////////////////////////////////////////////////////////////////////////////////////
        //skill 1 description
        if (bSkill1Des_open == false && GameObject.Find("Player").transform.position.x >= -7.35f)
        {
            StartCoroutine(SkillDescription_1.FadeIn());
            GameObject.Find("Player").GetComponent<PlayerMovement>().canMove_skill = false;
            GameObject.Find("Player").GetComponent<Animator>().SetFloat("Speed", Mathf.Abs(0));
            bSkill1Des_open = true;
        }
        else if (bSkill1Des_open == true && bSkill1Des_finish　== false && Input.GetButtonDown("Submit") && SkillDescription_1.SkillDiscriptionSet[SkillDescription_1.SkillDiscriptionSet.Length -1].image.color.a ==1)
        {

            StopCoroutine(SkillDescription_1.FadeIn());
            StartCoroutine(SkillDescription_1.FadeOut());
            GameObject.Find("Player").GetComponent<PlayerMovement>().canMove_skill = true;
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
        
        StartCandle[0].GetComponent<SkillOneTriggerIcon>().DetectFinish();
        StartCandle[1].GetComponent<SkillOneTriggerIcon>().DetectFinish();
        bool bCandle1 = StartCandle[0].GetComponent<SkillOneTriggerIcon>().bTriggerFinish;
        bool bCandle2 = StartCandle[1].GetComponent<SkillOneTriggerIcon>().bTriggerFinish;

        //bool bCandle1 = true;
        //bool bCandle2 = true;

        if (bCandle1 && bCandle2 && BuddhaCandleState == BuddhaCandleStateNUM.Start)
        {
            //set vita stop skill
            SkillManager_v2.bFinishSkill = true;
            BuddhaCandleState = BuddhaCandleStateNUM.ChooseNUM;
        }

        //Choose 3number between 1-5  
        else if (BuddhaCandleState == BuddhaCandleStateNUM.ChooseNUM)
        {
            //reset vita stop skill
            SkillManager_v2.bFinishSkill = false;

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
            StartCoroutine(this.gameObject.GetComponent<CameraManager>().ChangeCameraFollowingPosition(0.0f, 0.5f, new Vector2(GameBoard.transform.position.x, 7.34f)));
            StartCoroutine(this.gameObject.GetComponent<CameraManager>().Shake(0.5f, 3.5f, 0.05f));


            //set player movement, disable trigger skill
            GameObject.Find("Player").GetComponent<PlayerMovement>().canMove_skill = false;
            GameObject.Find("Player").GetComponent<Animator>().SetFloat("Speed", 0.0f);
            PlayerSkill.bCanTriggerSkill = false;

            //set smoke
            SmokeAnimator.SetTrigger("Start");

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
                StartCoroutine(this.gameObject.GetComponent<CameraManager>().ChangeCameraProjectionSizeIEnumerator(Camera.main, 2.5f, 0.75f, 0.5f));

                //set background color
                GameObject.Find("Player").GetComponent<ColorChange>().ColorChanging(new Color(1.0f, 1.0f, 1.0f, 0.0f), 0.5f, 0.5f);
                BGMask.GetComponent<ColorChange>().ColorChanging(new Color(0.0f, 0.0f, 0.0f, 0.74f), 0.75f, 0.5f);
                levelUI.GetComponent<ColorChange>().ColorChanging(new Color(1.0f, 1.0f, 1.0f, 1.0f), 1.0f, 1.75f);
                levelUITitle.GetComponent<ColorChange>().ColorChanging(new Color(1.0f, 1.0f, 1.0f, 1.0f), 1.0f, 2.25f);
                levelUIText.GetComponent<ColorChange>().ColorChanging(new Color(1.0f, 1.0f, 1.0f, 1.0f), 1.0f, 2.75f);

                //vita soul stop following player
                VitaParticleScript.bCanFollow = false;
                StartCoroutine(this.gameObject.GetComponent<SkillManager_v2>().ObjMoveToIEnumerator(Vita.transform, VitaSoulGamePos));
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
            Dialogue.sentences = new string[1];
            Dialogue.sentences[0] = "哇啊!怎麼突然出現那麼大塊的石頭";
            CGMoveScript.SetRecTransformX(62.98596f);
            FindObjectOfType<DialogueManager>().StartDialogue(Dialogue, VitaCV);
            BuddhaCandleState++;
        }

        else if (BuddhaCandleState == BuddhaCandleStateNUM.Dialogue2 && DialogueManager.bFinishDialogue)
        {
            Dialogue.name = "莉妲";
            Dialogue.sentences = new string[2];
            Dialogue.sentences[0] = "旁邊好像寫了一些文字";
            Dialogue.sentences[1] = "「按下X使用技能點亮圖紋」";
            CGMoveScript.SetRecTransformX(145.7859f);
            FindObjectOfType<DialogueManager>().StartDialogue(Dialogue, PlayerCV);
            BuddhaCandleState++;
        }

        else if (BuddhaCandleState == BuddhaCandleStateNUM.Dialogue3 && DialogueManager.bFinishDialogue)
        {
            Dialogue.name = "薇妲";
            Dialogue.sentences = new string[1];
            Dialogue.sentences[0] = "什麼意思阿?";
            CGMoveScript.SetRecTransformX(62.98596f);
            FindObjectOfType<DialogueManager>().StartDialogue(Dialogue, VitaCV);
            BuddhaCandleState++;
        }

        else if (BuddhaCandleState == BuddhaCandleStateNUM.Dialogue4 && DialogueManager.bFinishDialogue)
        {
            Dialogue.name = "莉妲";
            Dialogue.sentences = new string[1];
            Dialogue.sentences[0] = "總之，我們先來試試看吧!";
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
            //StartGame.GetComponent<ColorChange>().ColorChanging(new Color(1.0f, 1.0f, 1.0f, 1.0f), 1.0f);

            BuddhaCandleState++;

            BuddhaCandleState++;
            //StartCoroutine(BuddhaLevelDelayIEnumerator(3.0f));
        }

        else if (BuddhaCandleState == BuddhaCandleStateNUM.FadeOutPrompt)
        {
            //StartGame.GetComponent<ColorChange>().ColorChanging(new Color(1.0f, 1.0f, 1.0f, 0.0f), 1.0f);

            BuddhaCandleState++;
            BuddhaCandleState++;
            //StartCoroutine(BuddhaLevelDelayIEnumerator(2.0f));
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

            //reset game start candle
            StartCoroutine(StartCandle[0].GetComponent<SkillOneTriggerIcon>().reset(1.0f));
            StartCoroutine(StartCandle[1].GetComponent<SkillOneTriggerIcon>().reset(1.0f));

            //first time in light up
            if (!bBuddhaRestart)
            {
                StartCoroutine(BuddhaLevelDelayIEnumerator(1.5f));
                BuddhaCandleState++;
            }
            else
            {
                StartCoroutine(BuddhaLevelDelayIEnumerator(1.5f));
                BuddhaCandleState = BuddhaCandleStateNUM.PlayerDetecting -1 ;

            }

            

        }

        //check if Buddha level end
        else if (BuddhaCandleState == BuddhaCandleStateNUM.PlayerDetecting)
        {

            //Detect Game start candle , to restart the game
            if (bCandle1 && bCandle2)
            {
                bBuddhaRestart = true;
                BuddhaCandleState = BuddhaCandleStateNUM.FadeIn1;
            }


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
            if (ilightUpArray[0] >= 0 && ilightUpArray[1] >= 0 && ilightUpArray[2] >= 0)
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

            if (bGameFinish)
                StartCoroutine(BuddhaLevelDelayIEnumerator(0.0f));

            BuddhaCandleState++;


        }

        else if (BuddhaCandleState == BuddhaCandleStateNUM.FinishVitaSetting)
        {
            SkillManager_v2.bFinishSkill = true;
            BuddhaCandleState++;

            //reset game start candle
            StartCoroutine(StartCandle[0].GetComponent<SkillOneTriggerIcon>().LightUp(1.0f));
            StartCoroutine(StartCandle[1].GetComponent<SkillOneTriggerIcon>().LightUp(1.0f));


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

            BuddhaCandleState++;

            StartCoroutine(BuddhaLevelDelayIEnumerator(1.5f));

            ////set game board original position
            //originalVector2 = GameBoard.transform.localPosition;

            ////set camera
            //StartCoroutine(this.gameObject.GetComponent<CameraManager>().Shake(2.0f, 3.5f, 0.05f));
        }



        else if (BuddhaCandleState == BuddhaCandleStateNUM.RisingDown)
        {
           /* //set smoke
            if (GameBoard.transform.localPosition.y == 0.5f)
                SmokeAnimator.SetTrigger("Start");

            if (GameBoard.transform.localPosition.y != -4.23f)
            {
                GameBoard.transform.localPosition = Vector2.Lerp(originalVector2, new Vector2(GameBoard.transform.localPosition.x, -4.23f), fLerpTimer);
                fLerpTimer += Time.deltaTime / 4.0f;
            }

            //next stage
            else if (GameBoard.transform.localPosition.y == -4.23f)
            {
                fLerpTimer = 0;
                BuddhaCandleState++;
                */

                //set player movement, disable trigger skill
                GameObject.Find("Player").GetComponent<PlayerMovement>().canMove_skill = true;

                //reset camera
                this.gameObject.GetComponent<CameraManager>().ResetCamera();

                Skill2Token.SetActive(true);

                BuddhaCandleState++;
            /*
            }*/

            Skill2Token.GetComponent<ColorChange>().ColorChanging(new Color(1.0f, 1.0f, 1.0f, 1.0f) , 1.5f);

        }


        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////// status keep check trigger
        ///

        //main status if re-start clue
        if (CanReStart)
        {
            MainStatusTrigger.GetComponent<SkillOneTriggerIcon>().DetectFinish();
            if (MainStatusTrigger.GetComponent<SkillOneTriggerIcon>().bTriggerFinish)
            {
                StartCoroutine(StatusClueIEnumerator());
                CanReStart = false;
            }
            
        }



        for (int i = 0; i < StatusToLightUp.Length; i++)
        {
            if (iLightUpOrder[i] < 0)
            {

                //detect trigger
                StatusTrigger[i].GetComponent<SkillOneTriggerIcon>().DetectFinish();

                //if jigsaw is finish
                if (StatusTrigger[i].GetComponent<SkillOneTriggerIcon>().bTriggerFinish && MatchingJigsawFinish)
                {
                    //set light up order
                    iLightUpOrder[i] = iCountOrder;
                    iCountOrder++;
                }

                //if jigsaw is not finish, reset skill trigger 
                else if (StatusTrigger[i].GetComponent<SkillOneTriggerIcon>().bTriggerFinish && !MatchingJigsawFinish)
                {
                    StartCoroutine(StatusTrigger[i].GetComponent<SkillOneTriggerIcon>().reset(1.5f));

                    Color color = StatusLight[i].GetComponent<SpriteRenderer>().color;
                    StatusLight[i].GetComponent<ColorChange>().ColorChanging(new Color(color.r, color.g, color.b, 0.0f), 1.5f);
                    bColorCanChange[i] = false;
                    bLastColorState[i] = false;
                }

                //color change
                if (bLastColorState[i] != StatusTrigger[i].GetComponent<SkillOneTriggerIcon>().VitaDetect._bSkillTrigger)
                {
                    bColorCanChange[i] = true;
                }

                if (bColorCanChange[i] && StatusTrigger[i].GetComponent<SkillOneTriggerIcon>().VitaDetect._bSkillTrigger && !StatusTrigger[i].GetComponent<SkillOneTriggerIcon>().bReseting && !MatchingJigsawFinish)
                {
                    bColorCanChange[i] = false;
                    Color color = StatusLight[i].GetComponent<SpriteRenderer>().color;
                    StatusLight[i].GetComponent<ColorChange>().ColorChanging(new Color(color.r, color.g, color.b, 1.0f) , 3.0f);

                }

                if (bColorCanChange[i] && !StatusTrigger[i].GetComponent<SkillOneTriggerIcon>().VitaDetect._bSkillTrigger && !StatusTrigger[i].GetComponent<SkillOneTriggerIcon>().bReseting && !MatchingJigsawFinish)
                {
                    bColorCanChange[i] = false;
                    Color color = StatusLight[i].GetComponent<SpriteRenderer>().color;
                    StatusLight[i].GetComponent<ColorChange>().ColorChanging(new Color(color.r, color.g, color.b, 0.0f), 3.0f);

                }

                bLastColorState[i] = StatusTrigger[i].GetComponent<SkillOneTriggerIcon>().VitaDetect._bSkillTrigger;

            }

        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        //get skill 2
        if (Skill2Token)
            if (Skill2Token.GetComponent<PlayerTrigger>()._bPlayerTrigger)
            {
                playerSkillScript.CanUseSkill2 = true;
                StartCoroutine(skillTokenTrigger());
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

    //skill 2 token trigger
    IEnumerator skillTokenTrigger()
    {
        Skill2Token.GetComponent<ColorChange>().ColorChanging(new Color(1.0f, 1.0f, 1.0f, 0.0f), 1.5f);
        Skill2Token.GetComponent<SizeChange>().SizeChanging( 0.0f , 1.5f);


        yield return new WaitForSeconds(2.0f);

        Destroy(Skill2Token);
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


    bool bFirstTime = false;
    IEnumerator StatusClueIEnumerator()
    {
        if (bFirstTime == false)
        {
            bFirstTime = true;

            yield return new WaitForSeconds(1.0f);

            //set color
            MainStatusTrigger.SetActive(true);
            for (int i = 0; i < MainStatusTriggerSprite.Length; i++)
            {
                MainStatusTriggerSprite[i].GetComponent<ColorChange>().ColorChanging(new Color(1.0f, 1.0f, 1.0f, 1.0f), 2.0f);
            }

            StartCoroutine(MainStatusTrigger.GetComponent<SkillOneTriggerIcon>().LightUp(1.5f));

            GameObject.Find("Status_godlight").GetComponent<ColorChange>().ColorChanging(new Color(1.0f, 1.0f, 1.0f, 1.0f), 2.0f);

            yield return new WaitForSeconds(3.0f);

            for (int i = 0; i < StatusLight.Length; i++)
            {
                Color color = StatusLight[i].GetComponent<SpriteRenderer>().color;
                StatusLight[i].GetComponent<ColorChange>().ColorChanging(new Color(color.r, color.g, color.b, 1.0f), 3.0f);
            }

            yield return new WaitForSeconds(4.0f);

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
        }
        // light up status light in particular color order
        for (int i = 0; i < array.Length; i++)
        {
            GameObject.Find("Status_godlight").GetComponent<ColorChange>().ColorChanging(CColor[array[i]], 1.0f);
            yield return new WaitForSeconds(2.5f);
        }

        //set jigsaw color to white
        for (int i = 0; i < StatusMatchingJigsawScript.Length; i++)
        {
            GameObject.Find("Status_godlight").GetComponent<ColorChange>().ColorChanging(new Color(1.0f, 1.0f, 1.0f, 1.0f), 2.0f);

        }

        StartCoroutine(MainStatusTrigger.GetComponent<SkillOneTriggerIcon>().reset(1.5f));

        CanReStart = true;
        
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
