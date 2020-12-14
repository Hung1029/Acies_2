using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clue : MonoBehaviour
{
    [SerializeField]
    private Text clue;

    public void SetText(string text)
    {
        clue.text = text;
    }

    public void FadeIn()
    {
        StopAllCoroutines();
        StartCoroutine(FadeIn_IEnumerator());
     
    }

    public void FadeOut()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOut_IEnumerator());

    }

    public void ImmediateOut()
    {
        clue.color = new Color(clue.color.r, clue.color.g, clue.color.b, 0.0f);
    }


    IEnumerator FadeIn_IEnumerator()
    {
        for (; clue.color.a < 1.0f; )
        {
            clue.color= new Color(clue.color.r, clue.color.g, clue.color.b, clue.color.a + 0.1f);
            yield return new WaitForSeconds(0.04f);
        }
    }

    IEnumerator FadeOut_IEnumerator()
    {
        for (; clue.color.a > 0.0f; )
        {
            clue.color = new Color(clue.color.r, clue.color.g, clue.color.b, clue.color.a - 0.1f);
            yield return new WaitForSeconds(0.04f);
        }
    }


}
