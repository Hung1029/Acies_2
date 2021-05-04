using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSkill : MonoBehaviour
{
    //Animator
    public Animator animator;
    private float animatorCounter = 0.0f;

    //PlayerMovement Script
    PlayerMovement MovementScript;

    //MagicLight
    [SerializeField]
    private GameObject MagicLightObj;
    [System.NonSerialized]
    public magicLight MagicLightScript;

    //Scene Management
    Scene currentScene;

    [System.NonSerialized]
    public static bool bCanTriggerSkill = true;

    //Skill
    public static int CURRENTSKILL = 0;

    //Skill 2 
    [System.NonSerialized]
    public bool CanUseSkill2 = false;

    //RT LT button hold
    public static bool bButtonTrigger_right = false;
    public static bool bButtonTrigger_left = false;
    float fButtonInterval_right = 0.0f;
    float fButtonInterval_left = 0.0f;



    // Start is called before the first frame update
    void Start()
    {
        currentScene = SceneManager.GetActiveScene();

        MagicLightScript = MagicLightObj.GetComponent<magicLight>();

        MovementScript = this.gameObject.GetComponent<PlayerMovement>();

        
    }

   


    public void StartSkillAnimate()
    {
        animator.SetTrigger("isSkill");
     
    }


    public void ResetAnimateToIdle()
    {
        animator.Play("Player_idle", -1, 0f);

    }


    public int DetectSkillKeyDown()
    {
        int SkillNUM = 0;
        if (Input.GetButtonDown("skillOne") && bCanTriggerSkill)
        {
            SkillNUM = 1;
        }
        else if (Input.GetButtonDown("skillTwo") && CanUseSkill2 && bCanTriggerSkill)
        {
            SkillNUM = 2;
        }

        return SkillNUM;
    }



/*
    private void Update()
    {
        //RT button button hold caculating 
        fButtonInterval_right += Time.deltaTime;

        //LT button button hold caculating 
        fButtonInterval_left += Time.deltaTime;

    }



    //change left return -1; change right return -1; no change return 0 
    public int DetectSkillChangeKeyHold()
    {
        

        if (Input.GetAxisRaw("ChangeSkillAttribute_right") == 1 && fButtonInterval_right > 0.5f)
        {

            fButtonInterval_right = 0.0f;
            bButtonTrigger_right = true;
        }
        else
        {
            bButtonTrigger_right = false;
        }

        

       

        if (Input.GetAxisRaw("ChangeSkillAttribute_left") == 1 && fButtonInterval_left > 0.5f)
        {

            fButtonInterval_left = 0.0f;
            bButtonTrigger_left = true;
        }
        else
        {
            bButtonTrigger_left = false;
        }


        //return
        if (bButtonTrigger_left)
        {
            return -1;
        }
        else if (bButtonTrigger_right)
        {
            return 1;
        }

        else 
        {
            return 0;
        }

        
    }*/

}
