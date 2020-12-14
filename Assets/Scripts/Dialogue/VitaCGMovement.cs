using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VitaCGMovement : MonoBehaviour
{
    
    private Image image;
    private RectTransform rectTransform;
    // Start is called before the first frame update
    void Start()
    {
        image = this.GetComponent<Image>();
        rectTransform = this.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        
      

    }

    public void SetRecTransformX(float x)
    {
        rectTransform.position = new Vector2(x, rectTransform.position.y);
    }

}
