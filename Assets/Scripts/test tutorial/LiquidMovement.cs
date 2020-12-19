using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidMovement : MonoBehaviour
{

    bool bTouchFloor = false;
    float Timer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if particle touch floor, start count time
        if (bTouchFloor)
        {
            Timer += Time.deltaTime;
        }
        //reset timer
        else
        {
            Timer = 0.0f;
        }

        //if touch floor more than 3s, disable collider
        if (Timer >= 3.0f)
        {
            this.gameObject.GetComponent<CircleCollider2D>().enabled = false;
        }


        //if lower than y = 0, destory particle
        if (this.gameObject.GetComponent<Transform>().position.y <= -1.0f )
        {
            Destroy(this.gameObject);
        }


    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.gameObject.layer == LayerMask.NameToLayer("Floor"))
        {
            bTouchFloor = true;
        }
    }



}
