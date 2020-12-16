using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingTarget : MonoBehaviour
{
    //what we are following
    public Transform target;

    //zeros out the velocity
    Vector3 velocity = Vector3.zero;


    //time to follow target
    public float smoothTime = 0.15f;

    //enable and set the maximum Y value
    public bool YMaxEnabled = false;
    public float YMaxValue = 0;

    //enable and set the minimum Y value
    public bool YMinEnabled = false;
    public float YMinValue = 0;

    //enable and set the maximum X value
    public bool XMaxEnabled = false;
    public float XMaxValue = 0;


    //enable and set the minimum X value
    public bool XMinEnabled = false;
    public float XMinValue = 0;

    //Camera focus On different object
    bool bCameraFocusOtherObj = false;


    void FixedUpdate()
    {
        if (bCameraFocusOtherObj == false)
        {
            //target position
            Vector3 targetPos = target.position;


            //vertical
            if (YMinEnabled && YMaxEnabled)
                targetPos.y = Mathf.Clamp(target.position.y, YMinValue, YMaxValue);

            else if (YMinEnabled)
                targetPos.y = Mathf.Clamp(target.position.y, YMinValue, target.position.y);

            else if (YMaxEnabled)
                targetPos.y = Mathf.Clamp(target.position.y, target.position.y, YMaxValue);

            //horizontal
            if (XMinEnabled && XMaxEnabled)
                targetPos.x = Mathf.Clamp(target.position.x, XMinValue, XMaxValue);

            else if (XMinEnabled)
                targetPos.x = Mathf.Clamp(target.position.x, XMinValue, target.position.x);

            else if (XMaxEnabled)
                targetPos.x = Mathf.Clamp(target.position.x, target.position.x, XMaxValue);





            //align the camera and the targets z position
            targetPos.z = transform.position.z;

            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
        }
        else
        {
            //when Camera focuse player can't move
            GameObject.Find("Player").GetComponent<PlayerMovement>().canMove = false;
        }

    }




    public void ShortFollowing(float time , Vector3 ObjPosition)
    {
        bCameraFocusOtherObj = true;
        StartCoroutine(ShortFollowingIEnumerator(time,ObjPosition));


    }

    IEnumerator ShortFollowingIEnumerator(float time, Vector3 ObjPosition)
    {
        ObjPosition.z = transform.position.z;

        //go to object position 
        while (Vector3.Distance(transform.position , ObjPosition) >= 1.0f )
        {
            transform.position = Vector3.SmoothDamp(transform.position, ObjPosition, ref velocity, 0.5f);
            yield return null;
        }

        StartCoroutine(Shake(1.5f,0.08f));

        yield return new WaitForSeconds(time);

        //reset Camera bool 
        bCameraFocusOtherObj = false;

        //reset Player move bool
        GameObject.Find("Player").GetComponent<PlayerMovement>().canMove = true;
    }

    public IEnumerator Shake(float duration, float magnitude) // during time and strength of shake
    {
        Vector3 originalPos = transform.localPosition;

        float elapsed = 0.0f; //timer

        while (elapsed<duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;

            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null; //before another IEnumerator loop, wait for next frame drawed
        }

        transform.localPosition = originalPos;
    }

}
