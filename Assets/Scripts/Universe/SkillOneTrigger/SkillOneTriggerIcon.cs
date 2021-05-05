using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillOneTriggerIcon : MonoBehaviour
{
    private SpriteRenderer TriggerRenderer;

    private const int SpriteNUM = 50;
    [SerializeField]
    private Sprite[] StartSprite = new Sprite[SpriteNUM];

    private int SpriteCount = 0;

    private bool canDetectTrigger = false;

    [System.NonSerialized]
    public bool bTriggerFinish = false;


    [System.NonSerialized]
    public VitaTriggerDetect VitaDetect;

    bool bLastDetectState = false;

    [System.NonSerialized]
    public bool bReseting = false;


    void Start()
    {
        TriggerRenderer = this.GetComponent<SpriteRenderer>();
        VitaDetect = this.GetComponent<VitaTriggerDetect>();
    }

    public void DetectFinish()
    {
       
        //if state change, start another coroutinges
        if (bLastDetectState != VitaDetect._bSkillTrigger)
        {
            canDetectTrigger = true;
        }


        if (canDetectTrigger && VitaDetect._bSkillTrigger && !bTriggerFinish  && !bReseting)
        {
            canDetectTrigger = false;

            StopAllCoroutines();
            StartCoroutine(PlayerTriggerFinishIEnumerator());
        }
        else if (canDetectTrigger && !VitaDetect._bSkillTrigger && !bTriggerFinish && !bReseting)
        {
            canDetectTrigger = false;

            StopAllCoroutines();
            StartCoroutine(PlayerTriggerFinishReverseIEnumerator());
        }

        //record last state
        bLastDetectState = VitaDetect._bSkillTrigger;

    }


    //public void reset()
    //{
    //    SpriteCount = 0;
    //    bTriggerFinish = false;
    //}

    IEnumerator PlayerTriggerFinishIEnumerator()
    {
        //Debug.Log("PlayerTriggerFinishIEnumerator");

        for (; SpriteCount < SpriteNUM; SpriteCount++)
        {
            TriggerRenderer.sprite = StartSprite[SpriteCount];

            yield return new WaitForSeconds(3.0f / SpriteNUM);

            if (SpriteCount == 34)
                bTriggerFinish = true;

        }
        SpriteCount = SpriteNUM -1;
        
    }


    IEnumerator PlayerTriggerFinishReverseIEnumerator()
    {
        //Debug.Log("PlayerTriggerFinishIEnumerator");
        for (; SpriteCount >= 0; SpriteCount--)
        {
            TriggerRenderer.sprite = StartSprite[SpriteCount];

            yield return new WaitForSeconds(3.0f / SpriteNUM);
        }
        SpriteCount = 0;
    }

    public IEnumerator reset()
    {
        
        for (; SpriteCount >= 0; SpriteCount--)
        {
            TriggerRenderer.sprite = StartSprite[SpriteCount];

            yield return new WaitForSeconds(3.0f / SpriteNUM);
        }
        SpriteCount = 0;


        canDetectTrigger = false;

        bTriggerFinish = false;

        bLastDetectState = false;
    }

    public IEnumerator reset(float fDuration)
    {
        //reset at once, vita can't interrupt
        bReseting = true;
        canDetectTrigger = false;

        bTriggerFinish = false;

        bLastDetectState = false;

        for (; SpriteCount >= 0; SpriteCount--)
        {
            TriggerRenderer.sprite = StartSprite[SpriteCount];

            yield return new WaitForSeconds(fDuration / SpriteNUM);
        }
        SpriteCount = 0;
        bReseting = false;
    }

    public IEnumerator LightUp(float fDuration)
    {
        //Debug.Log("PlayerTriggerFinishIEnumerator");

        for (; SpriteCount < SpriteNUM; SpriteCount++)
        {
            TriggerRenderer.sprite = StartSprite[SpriteCount];

            yield return new WaitForSeconds(fDuration / SpriteNUM);

            if (SpriteCount == 34)
                bTriggerFinish = true;

        }
        SpriteCount = SpriteNUM - 1;
    }

}
