using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockPlayerHurt_Level2 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.collider.name == "Player")
            GameObject.Find("Player").GetComponent<PlayerHealth>().Hurt();
            
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.name == "Player")
            GameObject.Find("Player").GetComponent<PlayerHealth>().Hurt();

    }

}
