using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testCamera : MonoBehaviour
{
    Vector2 test;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        test = new Vector2(this.transform.position.x + 0.05f, this.transform.position.y);
        if (this.transform.position.x >= 104.7 && this.transform.position.y <= 7.2f)
        {
            test = new Vector2(test.x , test.y + 0.05f);
        }

        this.transform.position = test;


    }
}
