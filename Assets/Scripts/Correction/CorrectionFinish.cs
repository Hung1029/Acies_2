using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorrectionFinish : MonoBehaviour
{
    private SpriteRenderer CorrectionRenderer;

    private const int SpriteNUM = 40;
    [SerializeField]
    private Sprite[] StartSprite = new Sprite[SpriteNUM];

    private int SpriteCount = 0;

    private bool GazeTrigger = false;
    private bool canDetectTrigger = false;

    public bool bCorrectionFinish = false;
    void Start()
    {
        CorrectionRenderer = this.GetComponent<SpriteRenderer>();
    }

    public void DetectFinish()
    {

        if (canDetectTrigger && GazeTrigger && !bCorrectionFinish)
        {
            canDetectTrigger = false;

            StopAllCoroutines();
            StartCoroutine(PlayerCorrectionFinishIEnumerator());
        }
        else if (canDetectTrigger && !GazeTrigger && !bCorrectionFinish)
        {
            canDetectTrigger = false;

            StopAllCoroutines();
            StartCoroutine(PlayerCorrectionFinishReverseIEnumerator());
        }
    }


    public void reset()
    {
        SpriteCount = 0;
        bCorrectionFinish = false;
    }

    IEnumerator PlayerCorrectionFinishIEnumerator()
    {
        for (; SpriteCount < SpriteNUM; SpriteCount++)
        {
            CorrectionRenderer.sprite = StartSprite[SpriteCount];

            yield return new WaitForSeconds(2.0f / SpriteNUM);
        }
        SpriteCount = 39;
        if (SpriteCount == 39)
            bCorrectionFinish = true;
    }


    IEnumerator PlayerCorrectionFinishReverseIEnumerator()
    {
        for (; SpriteCount >= 0; SpriteCount--)
        {
            CorrectionRenderer.sprite = StartSprite[SpriteCount];

            yield return new WaitForSeconds(2.0f / SpriteNUM);
        }
        SpriteCount = 0;
    }


    public void FadeIn()
    {
        StopAllCoroutines();
        StartCoroutine(PlayerCorrectionFadeInIEnumerator());

    }
    public void FadeOut()
    {
        StopAllCoroutines();
        StartCoroutine(PlayerCorrectionFadeOutIEnumerator());

    }


    IEnumerator PlayerCorrectionFadeInIEnumerator()
    {
        CorrectionRenderer.color = new Color(CorrectionRenderer.color.r, CorrectionRenderer.color.b, CorrectionRenderer.color.g, 0.0f);

        for (float a = 0.0f; CorrectionRenderer.color.a < 1.0f; a += 0.1f)
        {
            CorrectionRenderer.color = new Color(CorrectionRenderer.color.r, CorrectionRenderer.color.b, CorrectionRenderer.color.g, a);

            yield return new WaitForSeconds(0.015f);
        }
        CorrectionRenderer.color = new Color(CorrectionRenderer.color.r, CorrectionRenderer.color.b, CorrectionRenderer.color.g, 1.0f);

    }


    IEnumerator PlayerCorrectionFadeOutIEnumerator()
    {
        CorrectionRenderer.color = new Color(CorrectionRenderer.color.r, CorrectionRenderer.color.b, CorrectionRenderer.color.g, 1.0f);

        for (float a = 1.0f; CorrectionRenderer.color.a > 0.0f; a -= 0.1f)
        {
            CorrectionRenderer.color = new Color(CorrectionRenderer.color.r, CorrectionRenderer.color.b, CorrectionRenderer.color.g, a);

            yield return new WaitForSeconds(0.015f);
        }
        CorrectionRenderer.color = new Color(CorrectionRenderer.color.r, CorrectionRenderer.color.b, CorrectionRenderer.color.g, 0.0f);

    }

    private void OnTriggerEnter2D(Collider2D other)
    {

       if (other.name == "GazeTest")
        {

            GazeTrigger = true;
            canDetectTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {

        if (other.name == "GazeTest")
        {

            GazeTrigger = false;
            canDetectTrigger = true;

        }
    }
}
