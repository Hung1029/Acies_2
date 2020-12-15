using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadSkillDirection : MonoBehaviour
{
    [System.NonSerialized]
    public bool bPlayerTouch = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Player")
        {
            bPlayerTouch = true;
        }
       
    }


}
