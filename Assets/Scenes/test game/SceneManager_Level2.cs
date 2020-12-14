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
    private Plant PlantScript;


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
        if(GameObject.Find("Key"))
            if (KeyScript._bTouchKey && KeyScript._bTakeKey == false)
            {
                KeyScript.FadeOutKey();
                KeyScript._bTakeKey = true;
            }

        ///take skill 
        if (GameObject.Find("Box"))
            if (BoxScript._bTouchBox && KeyScript._bTakeKey  && BoxScript._bTakeSkill == false)
            {
                BoxScript.FadeOutBox();
                BoxScript._bTakeSkill = true;
            }


        //clear rock -> water drop -> plant grow
        if (!GameObject.Find("Rock3"))
        {
            Splash.Play();
            PlantScript.GrowUp();
        }



        ///


    }
}
