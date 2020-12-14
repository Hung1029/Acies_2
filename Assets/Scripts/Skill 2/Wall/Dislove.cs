using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dislove : MonoBehaviour
{
    private float DisolveTimer = 0.0f;
    private Material material;

    public bool canDisolve = false;


    // Start is called before the first frame update
    void Start()
    {
        material = this.gameObject.GetComponent<SpriteRenderer>().material;
    }

   


    public void FadeOut()
    {
        DisolveTimer += Time.deltaTime;
        float fade = material.GetFloat("_Fade");

        if (DisolveTimer > 0.1f)
        {
            material.SetFloat("_Fade", fade - 0.05f);
            DisolveTimer = 0.0f;
        }

        if (fade<=0.0f)
        {
             material.SetFloat("_Fade", 0.0f);
             Destroy(this.gameObject);
        }
       
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "VitaSoul")
        {
            canDisolve = true;
        }
       
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.name == "VitaSoul")
        {
            canDisolve = false;
        }
    }




}
