using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_Level2 : MonoBehaviour
{
    [System.NonSerialized]
    public bool _bIsOnColor = false;



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
        if (other.name == "VitaSoul")
        {
            this.gameObject.GetComponent<SpriteRenderer>().color = new Vector4(this.gameObject.GetComponent<SpriteRenderer>().color.r, this.gameObject.GetComponent<SpriteRenderer>().color.g, this.gameObject.GetComponent<SpriteRenderer>().color.b, 0.0f);
            _bIsOnColor = true;
        }
            
    }


}
