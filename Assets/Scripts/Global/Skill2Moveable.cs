using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill2Moveable : MonoBehaviour
{
    private Transform VitaSoulTrans;

    private GameObject Skill2PickedOutLine;
    SpriteRenderer PickedOut;

    [System.NonSerialized]
    public Vector2 v2LastPosition;

    public Quaternion v2LastRotation;

    //public Animator SmokeAniControllor;

    public bool bIsAJigsaw  = false;

    bool bObjectNotEffective = false;

    GameObject smoke ;

    bool bCanCheckInEffective =true;


    // Start is called before the first frame update
    void Start()
    {

        v2LastPosition = this.transform.localPosition;
        v2LastRotation = this.transform.localRotation;

        VitaSoulTrans = GameObject.Find("VitaSoul").GetComponent<Transform>();

        //create outline
        Skill2PickedOutLine = new GameObject();
        Skill2PickedOutLine.name = "PickOutline";
        Skill2PickedOutLine.transform.parent = this.gameObject.transform;
        Skill2PickedOutLine.transform.position = this.gameObject.transform.position;
        Skill2PickedOutLine.transform.localScale = new Vector3(1.1f, 1.1f,0.0f);
        Skill2PickedOutLine.transform.localRotation = Quaternion.identity;
        PickedOut =  Skill2PickedOutLine.AddComponent<SpriteRenderer>();
        PickedOut.sortingLayerName = "PlayItem";
        this.gameObject.GetComponent<SpriteRenderer>().sortingOrder = PickedOut.sortingOrder + 1;
        PickedOut.sprite = this.gameObject.GetComponent<SpriteRenderer>().sprite;
        PickedOut.material.shader = Shader.Find("GUI/Text Shader");
        PickedOut.color = Color.white;
        PickedOut.color = new Color(PickedOut.color.r, PickedOut.color.g, PickedOut.color.b,0.0f);



        //create smoke 
        smoke = new GameObject();
        smoke.name = "smoke";
        smoke.transform.parent = this.gameObject.transform;
        smoke.AddComponent<SpriteRenderer>();
        smoke.GetComponent<SpriteRenderer>().color = new Color(0.0f, 0.0f, 0.0f,1.0f);
        smoke.GetComponent<SpriteRenderer>().sortingLayerName = "PlayItem";
        smoke.AddComponent<Animator>();
        //smoke.GetComponent<Animator>().runtimeAnimatorController = < UnityEditor.Animations.AnimatorController > SmokeAniControllor;

#if UNITY_EDITOR
        smoke.GetComponent<Animator>().runtimeAnimatorController = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEditor.Animations.AnimatorController>("Assets/Sprites/Skill2_final/Smoke.controller");
#endif
        //smoke.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Sprites/Skill2_final/Smoke.controller");



        smoke.transform.localPosition = new Vector2(0.0f, 0.0f);

    }

    // Update is called once per frame
    void Update()
    {
        //check if gameobject in the effective area when it is not in pick up mode 
        if (bObjectNotEffective && PickedOut.color.a == 0.0f && bCanCheckInEffective)
        {
            bCanCheckInEffective = false;
            bObjectNotEffective = false;
            StartCoroutine(BackToLastPositionIEnumerator());

        }
        

    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "invalidAreaForMovableObj")
        {
            bObjectNotEffective = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "invalidAreaForMovableObj")
        {
            bObjectNotEffective = false;
        }
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


    IEnumerator BackToLastPositionIEnumerator()
    {
        //Freeze it physic movement
        this.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

        //unvisble
        this.GetComponent<VitaTriggerDetect>().bCanTrigger = false;
        this.GetComponent<SpriteRenderer>().color = new Color(this.GetComponent<SpriteRenderer>().color.r, this.GetComponent<SpriteRenderer>().color.g, this.GetComponent<SpriteRenderer>().color.b, 0.0f);
        
        //start smoke animation
        smoke.GetComponent<Animator>().SetTrigger("tSmoke");


        yield return new WaitForSeconds(1.0f);
        
        //move obj to last availble position, rotation
        this.transform.localPosition = new Vector2(v2LastPosition.x , v2LastPosition.y + 1.0f) ;
        this.transform.rotation = v2LastRotation;


        //start animation
        smoke.GetComponent<Animator>().SetTrigger("tSmoke");


        yield return new WaitForSeconds(0.5f);

        //Restore it physic movement
        this.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

        //visble
        this.GetComponent<SpriteRenderer>().color = new Color(this.GetComponent<SpriteRenderer>().color.r, this.GetComponent<SpriteRenderer>().color.g, this.GetComponent<SpriteRenderer>().color.b, 1.0f);
        this.GetComponent<VitaTriggerDetect>().bCanTrigger = true;

        bCanCheckInEffective = true;
    }


}
