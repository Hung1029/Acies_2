using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    //Animator
    private Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        animator = this.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
     
    }

    public void GrowUp()
    {
        
        animator.SetTrigger("tGrowUp");
        //StartCoroutine("GrowUpIEnumerator");
        
    }


    //GrowUp IEnumerator
     IEnumerator GrowUpIEnumerator()
    {
        for(float y = transform.position.y; y < -0.73; y+=0.05f)
        {
            transform.position = new Vector2(transform.position.x, y);
            yield return new WaitForSeconds(0.05f);
        }
       
    }



}
