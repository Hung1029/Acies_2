using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamingUI : MonoBehaviour
{
    public GameObject SkillSetting;

    public Image Attribute1Image;
    public Image Attribute2Image;


    //color
    Color cGray = new Color(0.33f, 0.33f, 0.33f);
    Color cWhite = new Color(1.0f, 1.0f, 1.0f);


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        

        //open/close skill setting page
        if (Input.GetButtonDown("SkillSettingPlatform"))
        {
            if (SkillSetting.activeSelf)
            {
                SkillSetting.SetActive(false);
            }

            else
            {
                SkillSetting.SetActive(true);
            }
                
        }



        //set skill attribute
        if (SkillSetting.activeSelf )
        {
           
            //right
            if (Input.GetAxisRaw("ChangeSkillAttribute_right") == 1)
            {
                Attribute1Image.color = cWhite;
                Attribute2Image.color = cGray;
            }

            //left
            else if(Input.GetAxisRaw("ChangeSkillAttribute_left") == 1)
            {
                Attribute1Image.color = cGray;
                Attribute2Image.color = cWhite;
            }

        }


    }

   


}
