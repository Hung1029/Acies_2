using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ropeway : MonoBehaviour
{


    [System.NonSerialized]
    public bool _bRopewayMoving = false;



    void Update()
    {
      
    }


    public void RopewayDown()
    {
        StartCoroutine("RopewayDownIEnumerator");
    }



    IEnumerator RopewayDownIEnumerator()
    {
       
        _bRopewayMoving = true;
        for (float y = transform.position.y; y > -2.89f; y -= 0.0125f)
        {
            transform.position = new Vector2(transform.position.x,y);
            yield return new WaitForSeconds(0.005f);
        }

        yield return new WaitForSeconds(2.0f);

        for (float y = transform.position.y; y < 1.03; y += 0.0125f)
        {
            transform.position = new Vector2(transform.position.x, y);
            yield return new WaitForSeconds(0.005f);
        }

        _bRopewayMoving = false;
    }


}
