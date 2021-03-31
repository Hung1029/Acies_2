using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill2Moveable : MonoBehaviour
{
    private Transform VitaSoulTrans;

    private GameObject Skill2PickedOutLine;
    SpriteRenderer PickedOut;


    public bool bIsAJigsaw  = false;

    // Start is called before the first frame update
    void Start()
    {
        VitaSoulTrans = GameObject.Find("VitaSoul").GetComponent<Transform>();

        //create outline
        Skill2PickedOutLine = new GameObject();
        Skill2PickedOutLine.name = "PickOutline";
        Skill2PickedOutLine.transform.parent = this.gameObject.transform;
        Skill2PickedOutLine.transform.position = this.gameObject.transform.position;
        Skill2PickedOutLine.transform.localScale = new Vector3(1.1f, 1.1f,0.0f);        
        PickedOut =  Skill2PickedOutLine.AddComponent<SpriteRenderer>();
        this.gameObject.GetComponent<SpriteRenderer>().sortingOrder = PickedOut.sortingOrder + 1;
        PickedOut.sprite = this.gameObject.GetComponent<SpriteRenderer>().sprite;
        PickedOut.color = new Color(PickedOut.color.r, PickedOut.color.g, PickedOut.color.b,0.0f);

    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void FollowingVitaPosition()
    {
        transform.position = VitaSoulTrans.position;
    }

    public void PickUp()
    {
        PickedOut.color = new Color(PickedOut.color.r, PickedOut.color.g, PickedOut.color.b, 1.0f);
    }
    public void PickDown()
    {
        PickedOut.color = new Color(PickedOut.color.r, PickedOut.color.g, PickedOut.color.b, 0.0f);
    }

}
