using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager_Level2 : MonoBehaviour
{
    [SerializeField]
    private GameObject VitaSoul;
    private VitaSoul_particle VitaParticleScript;
    private GazeMovement VitaParticleGazeScript;

    [SerializeField]
    private Key_Level2 KeyScript;

    [SerializeField]
    private Box_Level2 BoxScript;

    //particle system
    [SerializeField]
    private ParticleSystem Splash;

    //plant
    [SerializeField]
    private GameObject Plant;
    bool bPlantGrow = false;

    //Pop Skill Direction UI
    [SerializeField]
    private SkillDirection SkillDirectionUI;
    bool bRead = false;


    //Drop
    public ParticleSystem Drop;




    // Start is called before the first frame update
    void Start()
    {
        VitaParticleScript = VitaSoul.GetComponent<VitaSoul_particle>();
        VitaParticleGazeScript = VitaSoul.GetComponent<GazeMovement>();

        VitaParticleScript.MoveSpeed = 9.5f;

    }

    // Update is called once per frame
    void Update()
    {

        if (!VitaParticleGazeScript.bVitaSoulCanGaze)
        {
            VitaParticleScript.FollowObj();
        }


        ///take key
        if(KeyScript)
            if (KeyScript._bTouchKey && KeyScript._bTakeKey == false)
            {
                KeyScript.FadeOutKey();
                KeyScript._bTakeKey = true;
            }

        ///take skill 
        if (BoxScript)
            if (BoxScript._bTouchBox && KeyScript._bTakeKey  && BoxScript._bTakeSkill == false)
            {
                BoxScript.FadeOutBox();
                BoxScript._bTakeSkill = true;


                ////Skill Direction UI fade in
                SkillDirectionUI.GetComponent<Animator>().SetTrigger("tStart");
                //SkillDirectionUI.StartCoroutine(SkillDirectionUI.FadeInTextMainTitleIEnumerator());

                //player can't move
                GameObject.Find("Player").GetComponent<PlayerMovement>().canMove = false;
                GameObject.Find("Player").GetComponent<Animator>().SetFloat("Speed", Mathf.Abs(0));

                bRead = true;
            }

        
        //skill Directio UI fade out when pressA Key
        if (bRead && Input.GetKeyDown(KeyCode.A))
        {
            SkillDirectionUI.StartCoroutine(SkillDirectionUI.FadeOutTextMainTitleIEnumerator());

            //player can move
            GameObject.Find("Player").GetComponent<PlayerMovement>().canMove = true;

            //reset Reading bool if need to read again
            //bRead = false;

            //can use skill 2
            GameObject.Find("Player").GetComponent<PlayerSkill>().CanUseSkill2 = true;

        }



        //clear rock -> water drop -> plant grow
        if (!GameObject.Find("item-rock2-1") && !bPlantGrow)
        {
            StartCoroutine(DelayPlantGrow());
            Plant.GetComponent<EdgeCollider2D>().enabled = true;

            Drop.Play();

            bPlantGrow = true;
        }

        IEnumerator DelayPlantGrow(){

            yield return new WaitForSeconds(2.0f);
            Plant.GetComponent<Animator>().SetTrigger("tGrowUp");

        }


        ///





    }
}
