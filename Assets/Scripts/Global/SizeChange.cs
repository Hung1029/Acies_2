using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeChange : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SizeChanging(float fMultiple, float duration, float fPreWaitTime = 0.0f)
    {

        StopAllCoroutines();
        StartCoroutine(SizeChangingIEnumerator(fMultiple, duration, fPreWaitTime));
    }

    IEnumerator SizeChangingIEnumerator(float fMultiple, float duration, float fPreWaitTime = 0.0f)
    {

        yield return new WaitForSeconds(fPreWaitTime);

        Vector3 originalScale = transform.localScale;


        bool bScaleFinishChang = false;
        float t = 0;

        while (!bScaleFinishChang)
        {
            transform.localScale = Vector3.Lerp(originalScale , new Vector3(originalScale.x * fMultiple, originalScale.y * fMultiple , originalScale.z) , t);

            if (t < 1)
            {
                t += Time.deltaTime / duration;
                bScaleFinishChang = false;
            }
            else
            {
                bScaleFinishChang = true;
            }


            yield return null;
        }

    }


}
