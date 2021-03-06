﻿using System.Collections;
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

    private SpriteRenderer ChildSprite;

    [System.NonSerialized]
    public bool _bLightBlowEnable = true;

    public GameObject Target;


    // Start is called before the first frame update
    void Start()
    {
        RockSprite = GetComponent<SpriteRenderer>();
        
        if(this.gameObject.transform.childCount > 0)
            ChildSprite = this.gameObject.transform.GetChild(0).GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // set light color
        float t = Mathf.PingPong(Time.time, 1.0f) / 1.0f;
        if (_bLightBlowEnable == true)
        {
            Target.GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>().intensity += t;
        }
        else
        {
            Target.GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>().intensity = 1.5f;
        }
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
        _bLightBlowEnable = false;
        for (float i = 255; i > 0; i -= 10)
        {
            if (this.gameObject.transform.childCount > 0)
                ChildSprite.color = new Color(ChildSprite.color.r, ChildSprite.color.g, ChildSprite.color.b, (float)i / 225);
            RockSprite.color = new Color(RockSprite.color.r, RockSprite.color.g, RockSprite.color.b, (float)i / 225);
            yield return new WaitForSeconds(0.005f);
        }
        Destroy(this.gameObject);
    }

}
