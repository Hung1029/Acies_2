using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key_Level2 : MonoBehaviour
{
    //touch key or not
    [System.NonSerialized]
    public bool _bTouchKey = false;

    //take key or not
    [System.NonSerialized]
    public bool _bTakeKey = false;


    private SpriteRenderer KeySprite;
    // Start is called before the first frame update
    void Start()
    {
        KeySprite = GetComponent<SpriteRenderer>();
    }



    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.name == "Player")
            _bTouchKey = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.name == "Player")
            _bTouchKey = false;
    }


    public void FadeOutKey()
    {
        StartCoroutine(FadeOutKeyIEnumerator());
    }


    IEnumerator FadeOutKeyIEnumerator()
    {
        for (float i = 255; i > 0; i -= 10)
        {
            KeySprite.color = new Color(KeySprite.color.r, KeySprite.color.g, KeySprite.color.b, (float)i / 225);
            yield return new WaitForSeconds(0.005f);
        }
        Destroy(this.gameObject);
    }

}
