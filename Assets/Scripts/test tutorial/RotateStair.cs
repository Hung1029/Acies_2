using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateStair : MonoBehaviour
{

    [System.NonSerialized]
    public bool _bIsRotate = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    ////water wheel rotate

    public void StairRotate()
    {
        StartCoroutine("WaterWheelRotateIEnumerator");

    }


    IEnumerator WaterWheelRotateIEnumerator()
    {
        _bIsRotate = true;
        //how many circle
        for (int circle = 0; circle < 1; circle++)
        {
            //rotate angle
            for (float a = 0.0f; a < 370; a++)
            {
                transform.Rotate(0.0f, 0.0f, 1.0f);
                yield return new WaitForSeconds(0.025f);
            }
        }
        _bIsRotate = false;

    }



    ////







}
