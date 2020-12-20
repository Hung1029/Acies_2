using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    //Player
    private GameObject Player;
    private PlayerMovement PlayerMovementScript;
    private PlayerSkill SkillScript;
    private GameObject magicLightObj;
    private magicLight magicLightScript;


    //VitaSoul
    private GameObject VitaSoul;
    private GazeMovement VitaParticleGazeScript;
    private VitaSoul_particle VitaParticleScript;
    private SpriteRenderer VitaSoulRenderer;
    private Animator VitaSoulBG;




    //UI
    [SerializeField]
    private Image SkillIcon;
    [SerializeField]
    private Image SkillName;

    //Timer
    private float VitaSoulCanGazeTimer = 0.0f;
    private float VitaSoulgGatheringTimer = 0.0f;


    //skill stage number
    enum SkillStageNUM
    {
        DetectSkillButton,
        AfterPressButton,
        Gathering,
        StartGazeControlVita,
        FinishLLghtUpVita
    }

    //SkillStage
    SkillStageNUM SkillStage = SkillStageNUM.DetectSkillButton;
    int SkillNUM;
    int CurrentSkillNUM;

    //Skill time
    float fLightUpTime = 5.0f;
    float fNeedGatheringTime = 1.0f;
    float fCanGazeTime = 0.0f;

    //Skill 2
    [SerializeField]
    private GameObject Skill2Board;
    private SpriteRenderer Skill2Sprite;
    private SpriteRenderer Skill2GroundColor;
    private bool bAllColorOn = false;
    [System.NonSerialized]
    public int iCurrentGraphic = 0; //current use graphic index
    enum Skill2StageNUM
    {
        DetectRockTrigger,
        Skill2InitialSet,
        CheckBlockColor
    }
    Skill2StageNUM Skill2Stage = Skill2StageNUM.DetectRockTrigger;
    
    //Skill 2  Rock
    [SerializeField]
    private Rock_Level2[] RockScript;
    int iCurrentRockNum = 0;


    //gaze
    Vector3 gazeOnScreen;
    Vector3 VitaPosition;

    //Fog detect collider
    [SerializeField]
    private GameObject FogColliderGameObject;
    [SerializeField]
    private GameObject FogColliderGameObjectForPlayer1;
    [SerializeField]
    private GameObject FogColliderGameObjectForPlayer2;
    bool bColliderOnPlayer = true;


    // Start is called before the first frame update
    void Start()
    {
        //Player
        Player = GameObject.Find("Player");
        PlayerMovementScript = Player.GetComponent<PlayerMovement>();
        SkillScript = Player.GetComponent<PlayerSkill>();
        magicLightObj = GameObject.Find("magicLight");
        magicLightScript = magicLightObj.GetComponent<magicLight>();

        //VitaSoul
        VitaSoul = GameObject.Find("VitaSoul");
        VitaParticleGazeScript = VitaSoul.GetComponent<GazeMovement>();
        VitaParticleScript = VitaSoul.GetComponent<VitaSoul_particle>();
        VitaSoulRenderer = VitaSoul.GetComponent<SpriteRenderer>();
        VitaSoulBG = GameObject.Find("VitaParticle").GetComponent<Animator>();


        //Skill 2 
        Skill2Sprite = Skill2Board.GetComponent<SpriteRenderer>();
        Skill2GroundColor = Skill2Board.transform.GetChild(0).GetComponent<SpriteRenderer>();

        fCanGazeTime = magicLightScript.fRaiseHand + fLightUpTime;

        


    }
    void FixedUpdate()
    {
        gazeOnScreen = Camera.main.ScreenToWorldPoint(TobiiAPI.GetGazePoint().Screen);
        VitaPosition = VitaSoul.GetComponent<Transform>().position;
    }



    // Update is called once per frame
    void Update()
    {
        ///////////////////////////////////////////////////////////////////////////////////Detect Skill Button

        SkillNUM = SkillScript.DetectSkillKeyDown(); //Detecting Key Down

        if (SkillStage == SkillStageNUM.DetectSkillButton && SkillNUM != 0 && !PlayerMovementScript.bPlayerMove)
        {
            SkillStage++;
        }

        ///////////////////////////////////////////////////////////////////////////////////Start Skill 

        if (SkillStage == SkillStageNUM.AfterPressButton )
        {
            //start prompt
            VitaParticleScript.PromptFadeIn();

            //start UI
            FadeInUI();

            //set Current Skill
            CurrentSkillNUM = SkillNUM;

            //Change Light 
            SkillScript.MagicLightScript.ChangeLightColor(SkillNUM);
            SkillScript.MagicLightScript.FadeIn();
            SkillScript.MagicLightScript.FadeOutCountDown(fLightUpTime);

            //Player skill animate
            SkillScript.StartSkillAnimate();
            
            //next stage
            SkillStage++;
        }

        ///////////////////////////////////////////////////////////////////////////////////Gathering, check if user look on vita

        if (SkillStage == SkillStageNUM.Gathering )
        {
            
            float Distance = Vector3.Distance(VitaPosition, gazeOnScreen);

            VitaSoulgGatheringTimer += Time.deltaTime;

            if (Distance <= 10.05f)
            {
                VitaParticleScript.animator.SetBool("isGazeing", true);
                VitaSoulBG.SetBool("isGazeing", true);

                //fade out prompt
                if (VitaParticleScript.bPromptExist)
                    VitaParticleScript.PromptFadeOut();

                if (VitaSoulgGatheringTimer >= fNeedGatheringTime) //gathering for 2s
                {
                    //vita animate
                    VitaParticleScript.animator.SetBool("isGazeing", false);
                    VitaSoulBG.SetBool("isGazeing", false);

                    //allow user controll vita
                    VitaParticleGazeScript.bVitaSoulCanGaze = true;

                    //reset timer
                    VitaSoulCanGazeTimer = 0.0f;
                    VitaSoulgGatheringTimer = 0.0f;

                    //player can't move
                    PlayerMovementScript.canMove = false;

                    //next stage
                    SkillStage++;

                }
            }
            else
            {
                //inerrupt gathering, reset timer
                VitaSoulgGatheringTimer = 0.0f;

                VitaSoulCanGazeTimer += Time.deltaTime;


                VitaParticleScript.animator.SetBool("isGazeing", false);
                VitaSoulBG.SetBool("isGazeing", false);


                if (!VitaParticleScript.bPromptExist)
                    VitaParticleScript.PromptFadeIn();

               

            }
        }
       
        ///////////////////////////////////////////////////////////////////////////////////if player move before gathering finished Vita or Can Gaze Time up or pressed skill button again
        if ((PlayerMovementScript.bPlayerMove || VitaSoulCanGazeTimer >= fCanGazeTime || (SkillNUM!=0 && VitaSoulCanGazeTimer > 0.5f)) && SkillStage == SkillStageNUM.Gathering)
        {
            //reset vita animate
            VitaParticleScript.PromptFadeOut();
            VitaParticleScript.animator.SetBool("isGazeing", false);
            VitaSoulBG.SetBool("isGazeing", false);

            //reset gaze
            VitaParticleGazeScript.bVitaSoulCanGaze = false;

            //reset UI
            FadeOutUI();

            //reset player animate 
            SkillScript.ResetAnimateToIdle();
            

            //reset timer
            VitaSoulCanGazeTimer = 0.0f;
            VitaSoulgGatheringTimer = 0.0f;

            //reset magic light and reset can light up vita bool
            SkillScript.MagicLightScript.FadeOut();

            //reset Current skill num
            PlayerSkill.CURRENTSKILL = 0;

            //back stage
            SkillStage = 0;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////Start Gaze Control Vita
        
        //start light up vita
        if (magicLightScript.LightUpVita && SkillStage == SkillStageNUM.StartGazeControlVita)
        {
            

            //set currentSkill
            PlayerSkill.CURRENTSKILL = CurrentSkillNUM;

            //reset timer
            VitaSoulCanGazeTimer = 0.0f;

            VitaParticleScript.LightUpVita(magicLightScript.magicLt.color);
            VitaParticleScript.SkillNUM = PlayerSkill.CURRENTSKILL;

            VitaParticleScript.animator.SetBool("StartSkill", true);

            magicLightScript.LightUpVita = false; // reset bool

            SkillStage++;//next stage

        }
        
        //if never light up count dowm
        else if (SkillStage == SkillStageNUM.StartGazeControlVita)
        {
            VitaSoulCanGazeTimer += Time.deltaTime;
            if (VitaSoulCanGazeTimer > 5.0f || (VitaSoulCanGazeTimer > 0.5f && SkillNUM != 0)) //over 5.0s or after Player raise hand and then press skillbutton
            {
                //reset animate
                if (VitaParticleScript.bPromptExist)
                    VitaParticleScript.PromptFadeOut();

                //player can't move
                PlayerMovementScript.canMove = true;

                VitaParticleGazeScript.bVitaSoulCanGaze = false;
                FadeOutUI();

                //reset player animate
                SkillScript.ResetAnimateToIdle();

                //reset magic light
                SkillScript.MagicLightScript.FadeOut();

                //reset timer
                VitaSoulCanGazeTimer = 0.0f;

                //reset Current skill num
                PlayerSkill.CURRENTSKILL = 0;

                //back stage
                SkillStage = 0;
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////Finish Light Up Vita
        if (SkillStage == SkillStageNUM.FinishLLghtUpVita)
        {
            VitaSoulCanGazeTimer += Time.deltaTime;
            //skill 2  
            if (PlayerSkill.CURRENTSKILL == 2)
            {
               
                //check which rock is trigger
                if (Skill2Stage == Skill2StageNUM.DetectRockTrigger)
                {
                    
                    for (int i = 0; i < RockScript.Length ; i++)
                    {
                        
                        if (RockScript[i]._bTrigger)
                        {
                            Skill2Stage++;
                            iCurrentRockNum = i + 1; //start index with 1
                            
                        }

                    }
                }
                

                //set skill stage initial 
                if (Skill2Stage==Skill2StageNUM.Skill2InitialSet)
                {
                    Skill2Board.transform.GetChild(iCurrentGraphic).gameObject.active = true;

                    //change position = camera position X.Y
                    Skill2Board.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0.0f);

                    //fade in skill 2
                    FadeInBoard();

                                       
                    Skill2Stage++;
                }
                
                
                //start check block
                if (Skill2Stage == Skill2StageNUM.CheckBlockColor)
                {

                    Block_Level2[] BlockCheck = Skill2Board.transform.GetChild(iCurrentGraphic).GetComponentsInChildren<Block_Level2>();
                    bAllColorOn = true;
                    foreach (Block_Level2 item in BlockCheck)
                    {
                        if (item._bIsOnColor == false)
                        {

                            bAllColorOn = false;
                            break;
                        }
                    }
                }

            }

            if (bAllColorOn || VitaSoulCanGazeTimer > 7.0f || SkillNUM != 0) //when time up or push the button again or finish skill2
            {
                //player can move
                PlayerMovementScript.canMove = true; 

               VitaParticleGazeScript.bVitaSoulCanGaze = false;

                //reset particle
                VitaParticleScript.StopSkillAfterTime(0.0f); 

                // reset timer
                VitaSoulCanGazeTimer = 0.0f;

                //reset magic light
                SkillScript.MagicLightScript.FadeOut();

                //reset player animate
                SkillScript.ResetAnimateToIdle();
               
                FadeOutUI();

                VitaParticleScript.animator.SetBool("StartSkill", false);

                VitaSoulRenderer.color = new Color(VitaSoulRenderer.color.r, VitaSoulRenderer.color.g, VitaSoulRenderer.color.b, 1.0f);

                SkillStage = 0;//reset stage

                //skill 2 Board
                if (PlayerSkill.CURRENTSKILL == 2)
                {
                    
                    //reset Skill2 stage 
                    Skill2Stage = Skill2StageNUM.DetectRockTrigger;

                    if (bAllColorOn)
                    {
                        //Destroy Rock
                        RockScript[iCurrentRockNum - 1].DestroyRock();

                    }

                    //reset skill 2 
                    ResetSkill2();

                    //reset current rock num
                    iCurrentRockNum = 0;

                }

                //reset Current skill num
                PlayerSkill.CURRENTSKILL = 0;

            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////Changeing Player, Vita collider for Skill 1
        //change Fog Detect Collider
        if (PlayerSkill.CURRENTSKILL == 0 && bColliderOnPlayer == false)
        {
            //change CircleCollider2D size
            FogColliderGameObject.GetComponent<CircleCollider2D>().radius = 1.4f;

            //open player collider enbled
            FogColliderGameObjectForPlayer1.GetComponent<CircleCollider2D>().enabled = true;
            FogColliderGameObjectForPlayer2.GetComponent<CircleCollider2D>().enabled = true;

            //set to player position
            FogColliderGameObject.transform.position = GameObject.Find("Player").transform.position;


            //set to child of player
            FogColliderGameObject.transform.parent = GameObject.Find("Player").transform;

            bColliderOnPlayer = true;

        }
        else if (PlayerSkill.CURRENTSKILL == 1 && bColliderOnPlayer == true)
        {
            //change CircleCollider2D size
            FogColliderGameObject.GetComponent<CircleCollider2D>().radius = 1.327327f;

            //revert player collider enbled
            FogColliderGameObjectForPlayer1.GetComponent<CircleCollider2D>().enabled = false;
            FogColliderGameObjectForPlayer2.GetComponent<CircleCollider2D>().enabled = false;

            //set to Vita position
            FogColliderGameObject.transform.position = GameObject.Find("VitaSoul").transform.position;

            //set to child of Vita
            FogColliderGameObject.transform.parent = GameObject.Find("VitaSoul").transform;

            bColliderOnPlayer = false;
        }





    }



    //Skill 2
    public void FadeInBoard()
    {
        StartCoroutine(FadeInBoardIEnumerator());
    }


    IEnumerator FadeInBoardIEnumerator()
    {
        SpriteRenderer[] BlockSprite = Skill2Board.transform.GetChild(iCurrentGraphic).GetComponentsInChildren<SpriteRenderer>();

        for (int i = 0; i < 255; i += 10)
        {
            
            //block
            foreach (SpriteRenderer item in BlockSprite)
            {
                item.color = new Color(item.color.r, item.color.g, item.color.b, (float)i / 225);
            }

            //back board
            Skill2Sprite.color = new Color(Skill2Sprite.color.r, Skill2Sprite.color.g, Skill2Sprite.color.b, (float)i / 225);

            yield return new WaitForSeconds(0.005f);
        }

        foreach (SpriteRenderer item in BlockSprite)
        {
            item.color = new Color(item.color.r, item.color.g, item.color.b, 1.0f);
        }
        Skill2Sprite.color = new Color(Skill2Sprite.color.r, Skill2Sprite.color.g, Skill2Sprite.color.b, 1.0f);
        Skill2GroundColor.color = new Color(Skill2GroundColor.color.r, Skill2GroundColor.color.g, Skill2GroundColor.color.b, 1.0f);


    }

    public void ResetSkill2()
    {
        StartCoroutine(FadeOutBoardIEnumerator());


        //reset active graphic
        Skill2Board.transform.GetChild(iCurrentGraphic).gameObject.active = false;

        //reset block boolean
        Block_Level2[] BlockCheck = Skill2Board.transform.GetChild(iCurrentGraphic).GetComponentsInChildren<Block_Level2>();
        foreach (Block_Level2 item in BlockCheck)
        {
            item._bIsOnColor = false;
        }

        bAllColorOn = false; // reset Skill2 boolean

    }


    IEnumerator FadeOutBoardIEnumerator()
    {
        SpriteRenderer[] BlockSprite = Skill2Board.transform.GetChild(iCurrentGraphic).GetComponentsInChildren<SpriteRenderer>();

        for (float i = Skill2Sprite.color.a * 225; i > 0; i -= 10)
        {
            //block
            foreach (SpriteRenderer item in BlockSprite)
            {
                item.color = new Color(item.color.r, item.color.g, item.color.b, (float)i / 225);
            }

            //back board
            Skill2Sprite.color = new Color(Skill2Sprite.color.r, Skill2Sprite.color.g, Skill2Sprite.color.b, (float)i / 225);

            //ground color
            Skill2GroundColor.color = new Color(Skill2GroundColor.color.r, Skill2GroundColor.color.g, Skill2GroundColor.color.b, (float)i / 225);


            yield return new WaitForSeconds(0.005f);
        }

        foreach (SpriteRenderer item in BlockSprite)
        {
            //reset to original color
            item.color = new Color(1.0f, 1.0f, 1.0f, 0.0f); 
        }

        Skill2GroundColor.color = new Color(Skill2GroundColor.color.r, Skill2GroundColor.color.g, Skill2GroundColor.color.b, 0.0f);
        Skill2Sprite.color = new Color(Skill2Sprite.color.r, Skill2Sprite.color.g, Skill2Sprite.color.b, 0.0f);

    }



    //UI
    void FadeInUI()
    {
        //stop UI Ienumerator
        StopCoroutine(FadeInSkillIconIEnumerator());
        StopCoroutine(FadeOutSkillIconIEnumerator());

        StartCoroutine(FadeInSkillIconIEnumerator());
    }

    void FadeOutUI()
    {
        //stop UI Ienumerator
        StopCoroutine(FadeInSkillIconIEnumerator());
        StopCoroutine(FadeOutSkillIconIEnumerator());

        StartCoroutine(FadeOutSkillIconIEnumerator());
    }


    IEnumerator FadeOutSkillIconIEnumerator()
    {
        SkillIcon.color = new Color(SkillIcon.color.r, SkillIcon.color.g, SkillIcon.color.b, 1.0f);
        SkillName.color = new Color(SkillIcon.color.r, SkillIcon.color.g, SkillIcon.color.b, 1.0f);

        for (float a = 1.0f; SkillIcon.color.a > 0.0f; a -= 0.06f)
        {
            SkillIcon.color = new Color(SkillIcon.color.r, SkillIcon.color.g, SkillIcon.color.b, a);
            SkillName.color = new Color(SkillIcon.color.r, SkillIcon.color.g, SkillIcon.color.b, a);
            yield return new WaitForSeconds(0.05f);
        }
        SkillIcon.color = new Color(SkillIcon.color.r, SkillIcon.color.g, SkillIcon.color.b, 0.0f);
        SkillName.color = new Color(SkillIcon.color.r, SkillIcon.color.g, SkillIcon.color.b, 0.0f);
    }



    IEnumerator FadeInSkillIconIEnumerator()
    {


        SkillIcon.color = new Color(SkillIcon.color.r, SkillIcon.color.g, SkillIcon.color.b, 0.0f);
        SkillName.color = new Color(SkillIcon.color.r, SkillIcon.color.g, SkillIcon.color.b, 0.0f);

        yield return new WaitForSeconds(0.8f); //Wait for Raise hand
        for (float a = 0.0f; SkillIcon.color.a < 1.0f; a += 0.06f)
        {
            SkillIcon.color = new Color(SkillIcon.color.r, SkillIcon.color.g, SkillIcon.color.b, a);
            SkillName.color = new Color(SkillIcon.color.r, SkillIcon.color.g, SkillIcon.color.b, a);
            yield return new WaitForSeconds(0.05f);
        }
        SkillIcon.color = new Color(SkillIcon.color.r, SkillIcon.color.g, SkillIcon.color.b, 1.0f);
        SkillName.color = new Color(SkillIcon.color.r, SkillIcon.color.g, SkillIcon.color.b, 1.0f);
    }



    //Other funtion
    void DelayCanGaze(float time)
    {
        StartCoroutine(DelayCanGazeIEnumerator(time));
    }


    IEnumerator DelayCanGazeIEnumerator(float time)
    {

        yield return new WaitForSeconds(time);
        SkillStage++;

    }









}
