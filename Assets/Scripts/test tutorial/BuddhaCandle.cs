using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuddhaCandle : MonoBehaviour
{
    [System.NonSerialized]
    public VitaTriggerDetect VitaDetect;
    [System.NonSerialized]
    public bool bTriggerFinish = false;

    bool bLastDetectState = false;


    private bool canDetectTrigger = false;

    private SpriteRenderer CandleRenderer;
    //bool bChangeColor = false;
    //bool bChangeColorFinish = false;

    //int iAddColorTimeCounter = 0;

    //float fGameGatheringTime = 1.0f;


    private const int SpriteNUM = 36;
    [SerializeField]
    private Sprite[] StartSprite = new Sprite[SpriteNUM];

    private int SpriteCount = 0;

    IEnumerator StartCandleIEnumerator;
    IEnumerator ReverseCandleIEnumerator;


    void Start()
    {
        CandleRenderer = GetComponent<SpriteRenderer>();
        VitaDetect = this.GetComponent<VitaTriggerDetect>();
    }



    public void  DetectCandleFinish()
    {
        //Debug.Log(VitaDetect._bSkillTrigger);
        //if state change, start another coroutinges
        if (bLastDetectState != VitaDetect._bSkillTrigger)
        {
            canDetectTrigger = true;
        }

        //Debug.Log(_bSkillOneTrigger);
        if (canDetectTrigger && VitaDetect._bSkillTrigger && !bTriggerFinish)
        {
            canDetectTrigger = false;

            StopAllCoroutines();
            StartCoroutine(ChangeCandleColorIEnumerator(3.0f));
                
        }

        else if (canDetectTrigger && !VitaDetect._bSkillTrigger && !bTriggerFinish)
        {
            canDetectTrigger = false;

            StopAllCoroutines();
            StartCoroutine(ChangeBackCandleColorIEnumerator(3.0f));
          

        }

        //record last state
        bLastDetectState = VitaDetect._bSkillTrigger;

        //return bTriggerFinish;
    }

    


    public IEnumerator ChangeCandleColorIEnumerator(float duration)
    {
        //Debug.Log("in");

        for (; SpriteCount < SpriteNUM; SpriteCount++)
        {
            //Debug.Log("SpriteCount : " + SpriteCount);
            CandleRenderer.sprite = StartSprite[SpriteCount];

            yield return new WaitForSeconds(duration / SpriteNUM);

            if (SpriteCount == 15)
                bTriggerFinish = true;

        }

        SpriteCount = SpriteNUM - 1;
        CandleRenderer.sprite = StartSprite[SpriteCount];
    }



    public IEnumerator ChangeBackCandleColorIEnumerator(float duration)
    {

        for (; SpriteCount >= 0; SpriteCount--)
        {
            CandleRenderer.sprite = StartSprite[SpriteCount];

            yield return new WaitForSeconds(duration / SpriteNUM);
        }
        SpriteCount = 0;
        //CandleRenderer.sprite = StartSprite[SpriteCount];

        bTriggerFinish = false;
    }

    public void ResetCandle()
    {
        StopAllCoroutines();
        StartCoroutine(CandleReset(1.0f));

    }
    public IEnumerator CandleReset(float duration)
    {
        //Debug.Log(this.gameObject.name +"  " + SpriteCount);
        for (; SpriteCount >= 0; SpriteCount--)
        {
            CandleRenderer.sprite = StartSprite[SpriteCount];
            yield return new WaitForSeconds(duration / SpriteNUM);

        }
        SpriteCount = 0;

        canDetectTrigger = false;

        bTriggerFinish = false;

        bLastDetectState = false;
    }

}
