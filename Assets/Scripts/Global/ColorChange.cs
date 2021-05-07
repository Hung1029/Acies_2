using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorChange : MonoBehaviour
{
    SpriteRenderer sprite;
    Image image;

    // Start is called before the first frame update
    void Start()
    {
        sprite = this.GetComponent<SpriteRenderer>();
        image = this.GetComponent<Image>();
    }


    public void ColorChanging( Color tagetColor, float duration, float fPreWaitTime = 0.0f)
    {

        StopAllCoroutines();
        StartCoroutine(ColorChangingIEnumerator( tagetColor,  duration, fPreWaitTime));
    }
    
    IEnumerator ColorChangingIEnumerator(Color tagetColor, float duration, float fPreWaitTime = 0.0f)
    {

        yield return new WaitForSeconds(fPreWaitTime);

        Color originalColor = new Color();

        if (sprite != null)
            originalColor = sprite.color;

        else if (image != null)
            originalColor = image.color;

        else
            Debug.LogError("No image or Sprite");

        bool bColorFinishChang = false;
        float t = 0;

        while (!bColorFinishChang)
        {
            if (sprite != null)
                sprite.color = Color.Lerp(originalColor, tagetColor, t);
            else if (image != null)
                image.color = Color.Lerp(originalColor, tagetColor, t);

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
