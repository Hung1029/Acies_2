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

    //Skill
    public static int CURRENTSKILL = 0;

    //Skill 2 
    [System.NonSerialized]
    public bool CanUseSkill2 = false;
    


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
        if (Input.GetButtonDown("skillOne"))
        {
            SkillNUM = 1;
        }
        else if (Input.GetButtonDown("skillTwo") && CanUseSkill2)
        {
            SkillNUM = 2;
        }

        return SkillNUM;
    }

}
