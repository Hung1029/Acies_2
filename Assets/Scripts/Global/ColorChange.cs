using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChange : MonoBehaviour
{
    SpriteRenderer sprite;


    // Start is called before the first frame update
    void Start()
    {
        sprite = this.GetComponent<SpriteRenderer>();
      
    }


    public void ColorChanging( Color tagetColor, float duration, float fPreWaitTime = 0.0f)
    {

        StopAllCoroutines();
        StartCoroutine(ColorChangingIEnumerator( tagetColor,  duration, fPreWaitTime));
    }
    
    IEnumerator ColorChangingIEnumerator(Color tagetColor, float duration, float fPreWaitTime = 0.0f)
    {

        yield return new WaitForSeconds(fPreWaitTime);

        Color originalColor = sprite.color;
        bool bColorFinishChang = false;
        float t = 0;

        while (!bColorFinishChang)
        {
           
            sprite.color = Color.Lerp(originalColor, tagetColor, t);

            if (t < 1)
            {
                t += Time.deltaTime / duration;
                bColorFinishChang = false;
            }
            else
            {
                bColorFinishChang = true;
            }


            yield return null;
        }

    }


    
    


}
