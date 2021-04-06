using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchingJigsaw : MonoBehaviour
{
    public GameObject Matchtemplate;

    private float intervalDistance = 0.5f;

    [System.NonSerialized]
    public bool bMatch = false;


    public bool DetectIfMatch()
    {
        float distance = Vector2.Distance(Matchtemplate.transform.position, this.transform.position);

        //if put jigsaw in the template
        if (distance < intervalDistance)
        {
            StartCoroutine( MoveToTemplate());
            bMatch = true;
            return true;
        }
        else
        {
            bMatch = false;
            return false;
        }
        
    }

    //lerp to template position
    IEnumerator MoveToTemplate()
    {

        for (; (transform.position.x > Matchtemplate.transform.position.x + 0.001f || transform.position.x < Matchtemplate.transform.position.x - 0.001f) && (transform.position.y > Matchtemplate.transform.position.y + 0.001f || transform.position.y < Matchtemplate.transform.position.y - 0.001f);)
        {
            float fVectorX = Matchtemplate.transform.position.x - transform.position.x;
            float fVectorY = Matchtemplate.transform.position.y - transform.position.y;

            transform.position = new Vector2(transform.position.x + fVectorX * 0.25f, transform.position.y + fVectorY * 0.25f);

            yield return new WaitForSeconds(0.05f);
        }
        transform.position = new Vector2(Matchtemplate.transform.position.x, Matchtemplate.transform.position.y);


        yield return null;
    }




}
