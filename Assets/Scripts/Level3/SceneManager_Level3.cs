using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager_Level3 : MonoBehaviour
{
    public VitaTriggerDetect movePlatformTrigger;

    public GameObject Vita;

    public MovingPlatform MovingPlatform;

    private GazeMovement VitaParticleGazeScript;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {   
        Vita.GetComponent<VitaSoul_particle>().FollowObj();       
    }


    // Update is called once per frame
    void Update()
    {

        //if  move platform is trigger
        if (movePlatformTrigger._bSkillTrigger)
            MovingPlatform._bCanMove = true;
        
    }
}
