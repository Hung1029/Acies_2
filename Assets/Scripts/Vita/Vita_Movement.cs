using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vita_Movement : MonoBehaviour
{
    //1.66 + 0.19 = 1.85
    private float stoppingDistance = 2.5f;
    private Transform target;
    private float speed = 10.0f;
    private bool bFaceRight = true;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

   public void FollowObj()
    {
        if (Mathf.Abs(transform.position.x - target.position.x) > stoppingDistance)
        {
            
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(target.position.x , transform.position.y), speed * Time.deltaTime); //only move x
        }

        if ((transform.position.x - target.position.x) > 0 && bFaceRight)
        {
            transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
            bFaceRight = false;
        }
        else if((transform.position.x - target.position.x) < 0 && !bFaceRight)
        {
            transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
            bFaceRight = true;
        }

    }
}
