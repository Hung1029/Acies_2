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


    public SpriteRenderer SpriteToSwitch;


    private SpriteRenderer BoxSprite;

    private GameObject[] ChildGameObject;
    // Start is called before the first frame update
    void Start()
    {
        BoxSprite = GetComponent<SpriteRenderer>();

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
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;


        //enable childrens gameobject
        for (int i = 0; i < this.gameObject.transform.childCount; i++)
        {
            this.gameObject.transform.GetChild(i).gameObject.SetActive(true);
        }



    }


}
