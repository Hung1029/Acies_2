using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallTriggeExplo : MonoBehaviour
{

    bool bBearStepOnRock = false;
    bool bBearRunOnRock = false;
    bool bPlayPS = false;



    ParticleSystem RockDamagePS;

    // Start is called before the first frame update
    void Start()
    {
        RockDamagePS = this.GetComponentInChildren<ParticleSystem>();
        //Debug.Log(RockDamagePS);
    }

    // Update is called once per frame
    void Update()
    {
        if (bPlayPS == false && ((GameObject.Find("Bear").GetComponent<SpriteRenderer>().sprite.name == "1-3_5")  && bBearStepOnRock || bBearRunOnRock))
        {
            //Debug.Log(this.gameObject.name + "  Start");
            RockDamagePS.transform.eulerAngles = Quaternion.Inverse(this.gameObject.transform.rotation) * new Vector3(0.0f, 0.0f, 45f);
            RockDamagePS.Play();
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(this.gameObject.GetComponent<SpriteRenderer>().color.r, this.gameObject.GetComponent<SpriteRenderer>().color.g, this.gameObject.GetComponent<SpriteRenderer>().color.b, 0.0f);
            StartCoroutine(CountDownForDestroy(3.0f));
            bPlayPS = true;
        }
    }


    //detect bear
    private void OnTriggerStay2D(Collider2D collision)
    {
        //Debug.Log(collision.name);

        if ( collision.name == "FallingRockExploDetect")
        {
            //Debug.Log(this.gameObject.name + "  Trigger");
            bBearStepOnRock = true;
        }

        if (collision.name == "DetectPlayer")
        {
            bBearRunOnRock = true;
        }

    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        //Debug.Log(collision.name);

        if (collision.name == "FallingRockExploDetect")
        {
            //Debug.Log(this.gameObject.name + "  Trigger");
            bBearStepOnRock = false;
        }

        if (collision.name == "DetectPlayer")
        {
            bBearRunOnRock = false;
        }

    }


    IEnumerator CountDownForDestroy( float time)
    {
        //Debug.Log(this.gameObject.name + "  Destroy");
        yield return new WaitForSeconds(time);

        Destroy(this.gameObject);
    }

}
