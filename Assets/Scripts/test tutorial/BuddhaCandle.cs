using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuddhaCandle : MonoBehaviour
{
    [System.NonSerialized]
    public bool _bSkillOneTrigger = false;

    private SpriteRenderer CandleRenderer;
    bool bChangeColor = false;
    bool bChangeColorFinish = false;
    int iAddColorTimeCounter = 0;

    float fGameGatheringTime = 1.0f;


    void Start()
    {
        CandleRenderer = GetComponent<SpriteRenderer>();



    }



    public bool DetectCandleFinish()
    {
        if (bChangeColorFinish == false)
        {
            if (_bSkillOneTrigger && bChangeColor == false && PlayerSkill.CURRENTSKILL == 1)
            {

                StopCoroutine("ChangeBackCandleColorIEnumerator");
                StartCoroutine("ChangeCandleColorIEnumerator");
                bChangeColor = true;
            }

            else if (_bSkillOneTrigger == false && bChangeColor)
            {

                StopCoroutine("ChangeCandleColorIEnumerator"); // stop change color
                StartCoroutine("ChangeBackCandleColorIEnumerator"); // turn back color
                bChangeColor = false;
            }


        }
        return bChangeColorFinish;
    }

    public void  ResetCandle()
    {
        iAddColorTimeCounter = 0;
        bChangeColorFinish = false;
        CandleRenderer.color = new Color(0.9294118f, 0.9294118f, 0.9294118f, 1.0f );
    }


    IEnumerator ChangeCandleColorIEnumerator()
    {
       
        // change format
        float fr_goal = 1.0f;
        float fg_goal = 0.987f;
        float fb_goal = 0f;
        float fa_goal = 1.0f;

        //set ever time to change
        float fr = (fr_goal - 0.9294118f) / 20;
        float fg = (fg_goal - 0.9294118f) / 20f;
        float fb = (fb_goal - 0.9294118f) / 20;
        float fa = (fa_goal - 1.0f) / 20;

        for (; iAddColorTimeCounter < 20; iAddColorTimeCounter++)
        {
                       
            CandleRenderer.color = new Color(CandleRenderer.color.r + fr, CandleRenderer.color.g, CandleRenderer.color.b , CandleRenderer.color.a);         
            CandleRenderer.color = new Color(CandleRenderer.color.r , CandleRenderer.color.g + fg, CandleRenderer.color.b, CandleRenderer.color.a);         
            CandleRenderer.color = new Color(CandleRenderer.color.r, CandleRenderer.color.g, CandleRenderer.color.b + fb, CandleRenderer.color.a);           
            CandleRenderer.color = new Color(CandleRenderer.color.r, CandleRenderer.color.g, CandleRenderer.color.b, CandleRenderer.color.a + fa);          
            yield return new WaitForSeconds(fGameGatheringTime / 20f); 
        }
        bChangeColorFinish = true;
    }



    IEnumerator ChangeBackCandleColorIEnumerator()
    {

        // change format
        float fr_goal = 1.0f;
        float fg_goal = 0.987f;
        float fb_goal = 0f;
        float fa_goal = 1.0f;

        //set ever time to change
        float fr = (fr_goal - 0.9294118f) / 20;
        float fg = (fg_goal - 0.9294118f) / 20f;
        float fb = (fb_goal - 0.9294118f) / 20;
        float fa = (fa_goal - 1.0f) / 20;

        
        for (; iAddColorTimeCounter > 0; iAddColorTimeCounter--)
        {
            CandleRenderer.color = new Color(CandleRenderer.color.r - fr, CandleRenderer.color.g, CandleRenderer.color.b, CandleRenderer.color.a);
            CandleRenderer.color = new Color(CandleRenderer.color.r, CandleRenderer.color.g - fg, CandleRenderer.color.b, CandleRenderer.color.a);
            CandleRenderer.color = new Color(CandleRenderer.color.r, CandleRenderer.color.g, CandleRenderer.color.b - fb, CandleRenderer.color.a);
            CandleRenderer.color = new Color(CandleRenderer.color.r, CandleRenderer.color.g, CandleRenderer.color.b, CandleRenderer.color.a - fa);
            yield return new WaitForSeconds(fGameGatheringTime / 20f);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "VitaSoul")
        {
            _bSkillOneTrigger = true;

        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.name == "VitaSoul")
        {
            _bSkillOneTrigger = false;

        }
    }
}
