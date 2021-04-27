using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;
using UnityEngine.UI;

public class SkillManager_v2 : MonoBehaviour
{
    //player
    private GameObject Player;
    private PlayerSkill SkillScript;
    private PlayerMovement PlayerMovementScript;
    private GameObject magicLightObj;
    private magicLight magicLightScript;

    //vita
    private GameObject VitaSoul;
    private Transform VitaTransform;
    private GazeMovement VitaParticleGazeScript;
    private VitaSoul_particle VitaParticleScript;
    private SpriteRenderer VitaSoulRenderer;
    private Animator VitaSoulBG;

    //UI
    [SerializeField]
    private Image SkillIcon;
    [SerializeField]
    private Image SkillName;


    //Skill time
    public static float fLightUpTime = 5.0f;
    float fNeedGatheringTime = 2.0f;
    float fCanGathingTime;
    float fCanGazeTime = 7.0f;

    //gaze
    Vector3 gazeOnScreen;
    Vector3 VitaPosition;

    //timer
    float VitaSoulGatheringTimer = 0.0f;
    float VitaSoulCanGazeTimer = 0.0f;

    /////IEnumerator
    private IEnumerator ObjMoveTo;
    private IEnumerator FadeOutUI;
    private IEnumerator FadeInUI;


    //Skill 2 
    Skill2Moveable[] ObjectsMoveable;
    private int iPickUpIndex = -1;
    private int iCurrentLookIndex = -1;

    enum SkillStageNUM
    {
        DetectSkillButton,
        StartSkill,
        Gathering,
        FinishLLghtUpVita

    }

    //SkillStage
    SkillStageNUM SkillStage = SkillStageNUM.DetectSkillButton;
    int SkillNUM;

    int CurrentSkillNUM;

    // Start is called before the first frame update
    void Start()
    {
        //Player
        Player = GameObject.Find("Player");
        PlayerMovementScript = Player.GetComponent<PlayerMovement>();
        magicLightObj = GameObject.Find("magicLight");
        magicLightScript = magicLightObj.GetComponent<magicLight>();

        //VitaSoul
        VitaSoul = GameObject.Find("VitaSoul");
        VitaTransform = VitaSoul.GetComponent<Transform>();
        VitaParticleGazeScript = VitaSoul.GetComponent<GazeMovement>();
        VitaParticleScript = VitaSoul.GetComponent<VitaSoul_particle>();
        VitaSoulBG = GameObject.Find("VitaParticle").GetComponent<Animator>();
        VitaSoulRenderer = VitaSoul.GetComponent<SpriteRenderer>();

        SkillScript = Player.GetComponent<PlayerSkill>();

        fCanGathingTime = magicLightScript.fRaiseHand + fLightUpTime;

       



    }

    private void FixedUpdate()
    {
        gazeOnScreen = Camera.main.ScreenToWorldPoint(TobiiAPI.GetGazePoint().Screen);
        VitaPosition = VitaSoul.GetComponent<Transform>().position;
    }


    void Update()
    {
       
        //skill2
        //if it is moveable object give it outline
        ObjectsMoveable = FindObjectsOfType(typeof(Skill2Moveable)) as Skill2Moveable[];

        ///////////////////////////////////////////////////////////////////////////////////Detect Skill Button
        SkillNUM = SkillScript.DetectSkillKeyDown();

        if (SkillNUM != 0 && SkillStage == SkillStageNUM.DetectSkillButton && !PlayerMovementScript.bPlayerMove) //detect start skill
        {
            SkillStage++;
        }
        ///////////////////////////////////////////////////////////////////////////////////Strat Skill
        if (SkillStage == SkillStageNUM.StartSkill)
        {
            //vita soul stop following
            VitaParticleScript.bCanFollow = false;

            //vita soul move to miagic wound 
            Vector2 V2magicWoundTop = new Vector2(Player.transform.Find("magicLight").GetComponent<Transform>().position.x, Player.transform.Find("magicLight").GetComponent<Transform>().position.y + 0.2f);
            ObjMoveTo = ObjMoveToIEnumerator(VitaTransform, V2magicWoundTop);
            StartCoroutine(ObjMoveTo);


            //start UI
            FadeInUI = FadeInSkillIconIEnumerator();            
            StartCoroutine(FadeInUI);

            //set current skill
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
        ///////////////////////////////////////////////////////////////////////////////////Wait For User Gaze
        if (SkillStage == SkillStageNUM.Gathering)
        {
            float Distance = Vector2.Distance(VitaPosition, gazeOnScreen);
            VitaSoulCanGazeTimer += Time.deltaTime;

            //gathering 
            if (Distance < 2.0f)
            {
                VitaSoulGatheringTimer += Time.deltaTime;
                VitaParticleScript.animator.SetBool("isGazeing", true);
                VitaSoulBG.SetBool("isGazeing", true);
                PlayerMovementScript.canMove = false;

                if (VitaSoulGatheringTimer >= fNeedGatheringTime) //gathering for 2s
                {
                    //stop vita move coroutine
                    StopCoroutine(ObjMoveTo);

                    //vita animate
                    VitaParticleScript.animator.SetBool("isGazeing", false);
                    VitaSoulBG.SetBool("isGazeing", false);

                    //allow user controll vita
                    VitaParticleGazeScript.bVitaSoulCanGaze = true;

                    //reset timer
                    VitaSoulCanGazeTimer = 0.0f;
                    VitaSoulGatheringTimer = 0.0f;

                    //player can't move
                    PlayerMovementScript.canMove = false;

                    //light up vita

                    //set currentSkill
                    PlayerSkill.CURRENTSKILL = CurrentSkillNUM;


                    VitaParticleScript.LightUpVita(magicLightScript.magicLt.color);
                    VitaParticleScript.SkillNUM = PlayerSkill.CURRENTSKILL;

                    VitaParticleScript.animator.SetBool("StartSkill", true);

                    magicLightScript.LightUpVita = false; // reset bool


                    //next stage
                    SkillStage++;
                }

            }

            //not gathering
            else
            {
                VitaSoulGatheringTimer = 0.0f;
                VitaParticleScript.animator.SetBool("isGazeing", false);
                VitaSoulBG.SetBool("isGazeing", false);

            }

            //if gathering are interrupted
            if (( VitaSoulCanGazeTimer >= fCanGathingTime || (Input.GetButtonDown("Cancel") && VitaSoulCanGazeTimer > 0.5f)) )
            {
                
                StartCoroutine(CaneMoveIntervelJump());

                //stop vita move coroutine
                StopCoroutine(ObjMoveTo);

                //reset vita animate
                VitaParticleScript.animator.SetBool("isGazeing", false);
                VitaSoulBG.SetBool("isGazeing", false);

                //reset gaze
                VitaParticleGazeScript.bVitaSoulCanGaze = false;

                //reset UI
                StopCoroutine(FadeInUI);
                FadeOutUI = FadeOutSkillIconIEnumerator();                
                StartCoroutine(FadeOutUI);

                //reset player animate 
                SkillScript.ResetAnimateToIdle();


                //reset timer
                VitaSoulCanGazeTimer = 0.0f;
                VitaSoulGatheringTimer = 0.0f;

                //reset magic light and reset can light up vita bool
                SkillScript.MagicLightScript.FadeOut();

                //vita back to follow
                VitaParticleScript.bCanFollow = true;

                //reset Current skill num
                PlayerSkill.CURRENTSKILL = 0;

                //back stage
                SkillStage = 0;
            }

        }
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////Finish Light Up Vita
        if(SkillStage == SkillStageNUM.FinishLLghtUpVita)
        {
            VitaSoulCanGazeTimer += Time.deltaTime;

            //skill 2 detect
            if (PlayerSkill.CURRENTSKILL == 2)
            {
                //still no object pick
                if (iPickUpIndex < 0)
                {
                    for (int i = 0; i < ObjectsMoveable.Length ; i++)
                    {
                        if (ObjectsMoveable[i].transform.GetComponent<VitaTriggerDetect>()._bSkillTrigger)
                        {
                            ObjectsMoveable[i].PickUp();
                            iCurrentLookIndex = i;
                           
                        }
                        else
                        {
                            ObjectsMoveable[i].PickDown();
                            if (iCurrentLookIndex == i)
                                iCurrentLookIndex = -1;

                        }
                    }

                    //set the pick up index when press submit
                    if (iCurrentLookIndex >= 0 && Input.GetButtonDown("Submit"))
                    {

                        //Pick Down other object
                        for (int i = 0; i < ObjectsMoveable.Length; i++)
                        {
                            if (i == iCurrentLookIndex)
                                continue;
                            ObjectsMoveable[i].PickDown();
                        }

                        iPickUpIndex = iCurrentLookIndex;
                        ObjectsMoveable[iCurrentLookIndex].transform.GetComponent<PolygonCollider2D>().isTrigger  = true;

                        //remember last effective position
                        ObjectsMoveable[iCurrentLookIndex].v2LastPosition = ObjectsMoveable[iCurrentLookIndex].transform.localPosition;

                        ObjectsMoveable[iCurrentLookIndex].transform.rotation = Quaternion.identity;

                        //reset iCurrentLookIndex
                        iCurrentLookIndex = -1;
                    }
                }

                //if already pick an object
                else
                {
                    ObjectsMoveable[iPickUpIndex].FollowingVitaPosition();
                    //cancel the pick or put it in template
                    if (Input.GetButtonDown("Submit") )
                    {
                        //if it is not jigsaw
                        if (!ObjectsMoveable[iPickUpIndex].bIsAJigsaw)
                        {
                            //reset physic movement
                            ObjectsMoveable[iPickUpIndex].transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

                            //disable box collider
                            ObjectsMoveable[iPickUpIndex].transform.GetComponent<PolygonCollider2D>().isTrigger = false;

                            //pick line off
                            ObjectsMoveable[iPickUpIndex].PickDown();

                            //reset pick up object index
                            iPickUpIndex = -1;
                        }

                        //it is a jigsaw, then check the match with template
                        else
                        {
                            
                            //match
                            if (ObjectsMoveable[iPickUpIndex].transform.GetComponent<MatchingJigsaw>().DetectIfMatch())
                            {
                                //Freeze it physic movement
                                ObjectsMoveable[iPickUpIndex].transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

                                //pick line off
                                ObjectsMoveable[iPickUpIndex].PickDown();

                                //reset pick up object index
                                iPickUpIndex = -1;
                            }

                            //not match
                            else
                            {
                                //reset physic movement
                                ObjectsMoveable[iPickUpIndex].transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

                                //disable box collider
                                ObjectsMoveable[iPickUpIndex].transform.GetComponent<PolygonCollider2D>().isTrigger = false;

                                //pick line off
                                ObjectsMoveable[iPickUpIndex].PickDown();

                                //reset pick up object index
                                iPickUpIndex = -1;
                            }

                        }
                        
                    }


                }
                
            }

            if (VitaSoulCanGazeTimer > fCanGazeTime || Input.GetButtonDown("Cancel")) 
            {
                //Finish skill 1
                
                //reset UI
                StopCoroutine(FadeInUI);
                FadeOutUI = FadeOutSkillIconIEnumerator();
                StartCoroutine(FadeOutUI);
                

                StartCoroutine(CaneMoveIntervelJump());
                
                //reset vita follow
                VitaParticleScript.bCanFollow = true;

                //reset gaze
                VitaParticleGazeScript.bVitaSoulCanGaze = false;

                //reset particle
                VitaParticleScript.StopSkillAfterTime(0.0f);

                // reset timer
                VitaSoulCanGazeTimer = 0.0f;

                //reset magic light
                SkillScript.MagicLightScript.FadeOut();

                //reset player animate
                SkillScript.ResetAnimateToIdle();

                VitaParticleScript.animator.SetBool("StartSkill", false);

                VitaSoulRenderer.color = new Color(VitaSoulRenderer.color.r, VitaSoulRenderer.color.g, VitaSoulRenderer.color.b, 1.0f);

                SkillStage = 0;//reset stage

                //reset skill2
                if (PlayerSkill.CURRENTSKILL == 2)
                    resetSkill();

                //reset Current skill num
                PlayerSkill.CURRENTSKILL = 0;
            }

        }

        //detect LT RT hold button
        /* int TriggerButton = SkillScript.DetectSkillChangeKeyHold();
         if(TriggerButton == 1)
             Debug.Log("RT");
         else if (TriggerButton == -1)
             Debug.Log("LT");*/

    }

    IEnumerator CaneMoveIntervelJump()
    {
        yield return new WaitForSeconds(0.2f);
        //player can move
        PlayerMovementScript.canMove = !PlayerMovementScript.canMove;
    }



    public int GetCurrentPickUpStoneNUM()
    {
        
        Debug.Log(iPickUpIndex);
        return iPickUpIndex;
    }


    //reset Skill 2 object
    void resetSkill()
    {

        if(iPickUpIndex >= 0)
        {
            //disable box collider
            ObjectsMoveable[iPickUpIndex].transform.GetComponent<PolygonCollider2D>().isTrigger = false;

            //pick line off
            ObjectsMoveable[iPickUpIndex].PickDown();

            //reset pick up object index
            iPickUpIndex = -1;
        }

        //reset pick up outline
        for (int i = 0; i < ObjectsMoveable.Length; i++)
        {           
            ObjectsMoveable[i].PickDown();

        }

    }


    //obj move to

    IEnumerator ObjMoveToIEnumerator(Transform objTransform, Vector2 v2Position)
    {

        for (; (objTransform.position.x > v2Position.x + 0.001f || objTransform.position.x < v2Position.x - 0.001f) && (objTransform.position.y > v2Position.y + 0.001f || objTransform.position.y < v2Position.y - 0.001f); )
        {
            float fVectorX = v2Position.x - objTransform.position.x;
            float fVectorY = v2Position.y - objTransform.position.y;

            objTransform.position = new Vector2(objTransform.position.x + fVectorX * 0.25f , objTransform.position.y + fVectorY * 0.25f);

            yield return new WaitForSeconds(0.05f);
        }
        objTransform.position = new Vector2(v2Position.x, v2Position.y);
            
    }




    //UI

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
    
}
