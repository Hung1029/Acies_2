using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillDirection : MonoBehaviour
{
    // Start is called before the first frame update
    public Text TextMainTitle;
    public Text TextTextSkillName;
    public Text TextDirection;
    public RawImage ImageButtonToContinue;

    Image ImageDirectionBG;

    void Start()
    {
        ImageDirectionBG = this.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    

    

    public IEnumerator FadeOutTextMainTitleIEnumerator()
    {
        //TextMainTitle.color = new Color(TextMainTitle.color.r, TextMainTitle.color.g, TextMainTitle.color.b, 1.0f);
        //TextTextSkillName.color = new Color(TextMainTitle.color.r, TextMainTitle.color.g, TextMainTitle.color.b, 1.0f);
        //TextDirection.color = new Color(TextMainTitle.color.r, TextMainTitle.color.g, TextMainTitle.color.b, 1.0f);
        //ImageDirectionBG.color = new Color(ImageDirectionBG.color.r, ImageDirectionBG.color.g, ImageDirectionBG.color.b, 1.0f);
        for (float a = 1.0f; TextMainTitle.color.a > 0.0f; a -= 0.06f)
        {
            TextMainTitle.color = new Color(TextMainTitle.color.r, TextMainTitle.color.g, TextMainTitle.color.b, a);
            TextTextSkillName.color = new Color(TextMainTitle.color.r, TextMainTitle.color.g, TextMainTitle.color.b, a);
            TextDirection.color = new Color(TextMainTitle.color.r, TextMainTitle.color.g, TextMainTitle.color.b, a);
            ImageDirectionBG.color = new Color(ImageDirectionBG.color.r, ImageDirectionBG.color.g, ImageDirectionBG.color.b, a);
            ImageButtonToContinue.color = new Color(ImageButtonToContinue.color.r, ImageButtonToContinue.color.g, ImageButtonToContinue.color.b, a);
            yield return new WaitForSeconds(0.025f);
        }
        TextMainTitle.color = new Color(TextMainTitle.color.r, TextMainTitle.color.g, TextMainTitle.color.b, 0.0f);
        TextTextSkillName.color = new Color(TextMainTitle.color.r, TextMainTitle.color.g, TextMainTitle.color.b, 0.0f);
        TextDirection.color = new Color(TextMainTitle.color.r, TextMainTitle.color.g, TextMainTitle.color.b, 0.0f);
        ImageDirectionBG.color = new Color(ImageDirectionBG.color.r, ImageDirectionBG.color.g, ImageDirectionBG.color.b, 0.0f);
        ImageButtonToContinue.color = new Color(ImageButtonToContinue.color.r, ImageButtonToContinue.color.g, ImageButtonToContinue.color.b, 0.0f);

    }



    public IEnumerator FadeInTextMainTitleIEnumerator()
    {


        //TextMainTitle.color = new Color(TextMainTitle.color.r, TextMainTitle.color.g, TextMainTitle.color.b, 0.0f);
        //TextTextSkillName.color = new Color(TextMainTitle.color.r, TextMainTitle.color.g, TextMainTitle.color.b, 0.0f);
        //TextDirection.color = new Color(TextMainTitle.color.r, TextMainTitle.color.g, TextMainTitle.color.b, 0.0f);
        //ImageDirectionBG.color = new Color(ImageDirectionBG.color.r, ImageDirectionBG.color.g, ImageDirectionBG.color.b, 0.0f);

        for (float a = 0.0f; TextMainTitle.color.a < 1.0f; a += 0.06f)
        {
            TextMainTitle.color = new Color(TextMainTitle.color.r, TextMainTitle.color.g, TextMainTitle.color.b, a);
            TextTextSkillName.color = new Color(TextMainTitle.color.r, TextMainTitle.color.g, TextMainTitle.color.b, a);
            TextDirection.color = new Color(TextMainTitle.color.r, TextMainTitle.color.g, TextMainTitle.color.b, a);
            ImageDirectionBG.color = new Color(ImageDirectionBG.color.r, ImageDirectionBG.color.g, ImageDirectionBG.color.b, a);
            ImageButtonToContinue.color = new Color(ImageButtonToContinue.color.r, ImageButtonToContinue.color.g, ImageButtonToContinue.color.b, a);

            yield return new WaitForSeconds(0.025f);
        }
        TextMainTitle.color = new Color(TextMainTitle.color.r, TextMainTitle.color.g, TextMainTitle.color.b, 1.0f);
        TextTextSkillName.color = new Color(TextMainTitle.color.r, TextMainTitle.color.g, TextMainTitle.color.b, 1.0f);
        TextDirection.color = new Color(TextMainTitle.color.r, TextMainTitle.color.g, TextMainTitle.color.b, 1.0f);
        ImageDirectionBG.color = new Color(ImageDirectionBG.color.r, ImageDirectionBG.color.g, ImageDirectionBG.color.b, 1.0f);
        ImageButtonToContinue.color = new Color(ImageButtonToContinue.color.r, ImageButtonToContinue.color.g, ImageButtonToContinue.color.b, 1.0f);

    }

}
