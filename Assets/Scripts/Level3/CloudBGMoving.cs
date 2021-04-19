using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudBGMoving : MonoBehaviour
{
    public GameObject[] Cloud;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var item in Cloud)
        {
            item.transform.Translate(Vector2.left * Time.deltaTime ,Space.World);


            if (item.transform.localPosition.x < -55.2f)
            {
                item.transform.localPosition = new Vector2(80f , item.transform.localPosition.y);
            }
        }
    }
}
