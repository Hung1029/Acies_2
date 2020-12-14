using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ropeway : MonoBehaviour
{


    [System.NonSerialized]
    public bool _bRopewayMoving = false;

    public void RopewayDown()
    {
        StartCoroutine("RopewayDownIEnumerator");
    }



    IEnumerator RopewayDownIEnumerator()
    {
        _bRopewayMoving = true;
        for (float y = transform.position.y; y > -3.95f; y -= 0.05f)
        {
            transform.position = new Vector2(transform.position.x,y);
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(2.0f);

        for (float y = transform.position.y; y < 2.693; y += 0.05f)
        {
            transform.position = new Vector2(transform.position.x, y);
            yield return new WaitForSeconds(0.05f);
        }

        _bRopewayMoving = false;
    }

}
