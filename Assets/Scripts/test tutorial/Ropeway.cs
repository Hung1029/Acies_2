using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ropeway : MonoBehaviour
{


    [System.NonSerialized]
    public bool _bRopewayMoving = false;

    [System.NonSerialized]
    public bool _bLightBlowEnable = true;

    public GameObject Target;

    void Update()
    {
        // set light color
        float t = Mathf.PingPong(Time.time, 1.0f) / 1.0f;
        if (_bLightBlowEnable == true)
        {
            Target.GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>().intensity += t;
        }
        else
        {
            Target.GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>().intensity = 1.5f;
        }
    }


    public void RopewayDown()
    {
        StartCoroutine("RopewayDownIEnumerator");
    }



    IEnumerator RopewayDownIEnumerator()
    {
       
        _bRopewayMoving = true;
        _bLightBlowEnable = false;
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
