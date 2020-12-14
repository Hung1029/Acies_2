using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorrectionStart : MonoBehaviour
{
    private Animator CorrectionStartAnimator;

    private SpriteRenderer CorrectionRenderer;

    private const int SpriteNUM = 25;
    [SerializeField]
    private Sprite[] StartSprite = new Sprite[SpriteNUM];

    private int SpriteCount = 0;

    private bool GazeTrigger = false;
    private bool canDetectTrigger = false;


    [SerializeField]
    private GameObject CorrectionFinishObj;
    private CorrectionFinish CorrectionFinishScript;

    [System.NonSerialized]
    public bool bCorrectionFinish = false;

   


    // Start is called before the first frame update
    void Start()
    {
        CorrectionStartAnimator = this.GetComponent<Animator>();
        CorrectionRenderer = this.GetComponent<SpriteRenderer>();
        CorrectionFinishScript = CorrectionFinishObj.GetComponent<CorrectionFinish>();

    }
    


    // Update is called once per frame
    void Update()
    {
        bCorrectionFinish = CorrectionFinishScript.bCorrectionFinish;
    }

    public void reset()
    {
        Debug.Log("in");
        
        SpriteCount = 0;
        bCorrectionFinish = false;

        CorrectionFinishScript.reset();

    }

    public void DetectStart()
    {

        if (canDetectTrigger && GazeTrigger && !CorrectionFinishScript.bCorrectionFinish)
        {
            canDetectTrigger = false;

            StopAllCoroutines();
            StartCoroutine(PlayerCorrectionStartIEnumerator());
        }
        else if (canDetectTrigger && !GazeTrigger && !CorrectionFinishScript.bCorrectionFinish)
        {
            canDetectTrigger = false;

            StopAllCoroutines();
            StartCoroutine(PlayerCorrectionStartReverseIEnumerator());
        }

        CorrectionFinishScript.DetectFinish();

        
    }


        IEnumerator PlayerCorrectionStartIEnumerator()
    {
        for (; SpriteCount < SpriteNUM; SpriteCount++)
        {
            CorrectionRenderer.sprite = StartSprite[SpriteCount];

            yield return new WaitForSeconds(1.0f / SpriteNUM);
        }
        SpriteCount = 24;
    }


    IEnumerator PlayerCorrectionStartReverseIEnumerator()
    {
        for (; SpriteCount >= 0; SpriteCount--)
        {
            CorrectionRenderer.sprite = StartSprite[SpriteCount];

            yield return new WaitForSeconds(0.5f / SpriteNUM);
        }
        SpriteCount = 0;
    }

    public void FadeIn()
    {
        StopAllCoroutines();
        StartCoroutine(PlayerCorrectionFadeInIEnumerator());
        CorrectionFinishScript.FadeIn();

    }
    public void FadeOut()
    {
        StopAllCoroutines();
        StartCoroutine(PlayerCorrectionFadeOutIEnumerator());
        CorrectionFinishScript.FadeOut();

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
       

        if (other.name == "GazeTest" )
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
