using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box_Level2 : MonoBehaviour
{
    //take key or not
    [System.NonSerialized]
    public bool _bTouchBox = false;

    //take skill or not
    [System.NonSerialized]
    public bool _bTakeSkill = false;

    //skill mark
    //[SerializeField]
    //private GameObject SkillMark;
    //private SpriteRenderer SkillMarkSprite;


    private SpriteRenderer BoxSprite;
    // Start is called before the first frame update
    void Start()
    {
        BoxSprite = GetComponent<SpriteRenderer>();

        //SkillMarkSprite = SkillMark.GetComponent<SpriteRenderer>();
    }

    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Player")
            _bTouchBox = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.name == "Player") 
            _bTouchBox = false;
    }


    public void FadeOutBox()
    {
        StartCoroutine(FadeOutBoxIEnumerator());
    }


    IEnumerator FadeOutBoxIEnumerator()
    {
        for (float i = 255; i > 0; i -= 10)
        {
            BoxSprite.color = new Color(BoxSprite.color.r, BoxSprite.color.g, BoxSprite.color.b, (float)i / 225);
            yield return new WaitForSeconds(0.005f);
        }
        BoxSprite.color = new Color(BoxSprite.color.r, BoxSprite.color.g, BoxSprite.color.b, 0.0f);

        //skill mark fade out
        yield return new WaitForSeconds(2.0f);

        /*for (float i = 255; i > 0; i -= 10)
        {
            SkillMarkSprite.color = new Color(SkillMarkSprite.color.r, SkillMarkSprite.color.g, SkillMarkSprite.color.b, (float)i / 225);
            yield return new WaitForSeconds(0.005f);
        }*/
        //Destroy(SkillMark);
        Destroy(this.gameObject);
    }
}
