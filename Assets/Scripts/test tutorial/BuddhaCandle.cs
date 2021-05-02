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


    private const int SpriteNUM = 36;
    [SerializeField]
    private Sprite[] StartSprite = new Sprite[SpriteNUM];

    private int SpriteCount = 0;


    void Start()
    {
        CandleRenderer = GetComponent<SpriteRenderer>();
    }



    public bool DetectCandleFinish()
    {
        if (bChangeColorFinish == false)
        {
            Debug.Log(_bSkillOneTrigger);
            if (_bSkillOneTrigger && bChangeColor == false && PlayerSkill.CURRENTSKILL == 1)
            {
                Debug.Log("in");
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


    public IEnumerator ChangeCandleColorIEnumerator()
    {

        for (; SpriteCount < SpriteNUM; SpriteCount++)
        {
            Debug.Log(SpriteCount);
            CandleRenderer.sprite = StartSprite[SpriteCount];

            yield return new WaitForSeconds(3.0f / SpriteNUM);

            if (SpriteCount == 34)
                bChangeColorFinish = true;

        }
        SpriteCount = SpriteNUM - 1;

        bChangeColorFinish = true;

        /*// change format
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
        bChangeColorFinish = true;*/
    }



    public IEnumerator ChangeBackCandleColorIEnumerator()
    {

        for (; SpriteCount >= 0; SpriteCount--)
        {
            CandleRenderer.sprite = StartSprite[SpriteCount];

            yield return new WaitForSeconds(3.0f / SpriteNUM);
        }
        SpriteCount = 0;

        /* // change format
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
         }*/
    }


    public IEnumerator reset()
    {

        for (; SpriteCount >= 0; SpriteCount--)
        {
            CandleRenderer.sprite = StartSprite[SpriteCount];

            yield return new WaitForSeconds(3.0f / SpriteNUM);
        }
        SpriteCount = 0;

    _bSkillOneTrigger = false;

    
    bChangeColor = false;
    bChangeColorFinish = false;
}


    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log(other.name);
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
