using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager_Skill1 : MonoBehaviour
{

    [SerializeField]
    private GameObject magicLightObj;
    private magicLight magicLightScript;

    [SerializeField]
    private GameObject VitaSoul;
    private VitaSoul_particle VitaParticleScript;


    // Start is called before the first frame update
    void Start()
    {
        magicLightScript = magicLightObj.GetComponent<magicLight>();

        VitaParticleScript = VitaSoul.GetComponent<VitaSoul_particle>();


    }

    // Update is called once per frame
    void Update()
    {
        //Light up Vita
        if (magicLightScript.LightUpVita)
        {
            VitaParticleScript.LightUpVita(magicLightScript.magicLt.color);
            VitaParticleScript.SkillNUM = PlayerSkill.CURRENTSKILL;
        }


     





    }
}
