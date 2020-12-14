using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock_Level2 : MonoBehaviour
{

    public int _iGraphicNum;

    [SerializeField]
    private SkillManager _SkillManagerScript;

    [System.NonSerialized]
    public bool _bTrigger =false;


    private SpriteRenderer RockSprite;

    // Start is called before the first frame update
    void Start()
    {
        RockSprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.name == "VitaSoul")
        {
            //set graphic num
            _SkillManagerScript.iCurrentGraphic = _iGraphicNum + 1;

            _bTrigger = true;

        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.name == "VitaSoul")
        {
            _bTrigger = false;
        }
    }

    public void DestroyRock()
    {
        StartCoroutine(DestroyRockIEnumerator());
    }


    IEnumerator DestroyRockIEnumerator()
    {
        for (float i = 255; i > 0; i -= 10)
        {
            RockSprite.color = new Color(RockSprite.color.r, RockSprite.color.g, RockSprite.color.b, (float)i / 225);
            yield return new WaitForSeconds(0.005f);
        }
        Destroy(this.gameObject);
    }

}
