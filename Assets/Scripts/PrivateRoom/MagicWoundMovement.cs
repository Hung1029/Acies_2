using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class MagicWoundMovement : MonoBehaviour
{
    [SerializeField]
    private Light2D pointLight;

    [SerializeField]
    private GameObject Explosion;
    private SpriteRenderer ExplosionRenderer;

    private SpriteRenderer MagicWoundRenderer;

    private Transform transform;
    public bool bfinishShine = false;
    public bool bfinishFadeOut = false;
    bool test = true;
    //bool test = true;
    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<Transform>();
        ExplosionRenderer = Explosion.GetComponent<SpriteRenderer>();
        MagicWoundRenderer = this.GetComponent<SpriteRenderer>();


    }

    
    void Update()
    {
        
    }

    public void ChangeShortingLayer(string LayerName)
    {
        MagicWoundRenderer.sortingLayerName = LayerName;
    }


    public void MagicWoundAnimate()
    {
        StopAllCoroutines();
        StartCoroutine(LightUp());
        StartCoroutine(MovementIEnumerator());

    }
    IEnumerator LightUp()
    {
        //light up magic wound
        yield return new WaitForSeconds(0.5f);
        for (; pointLight.intensity < 1.0f;)
        {
            pointLight.intensity += 0.05f; 
           yield return new WaitForSeconds(0.028f);
        }


    }


    IEnumerator MovementIEnumerator()
    {
        //magicWound rising
        yield return new WaitForSeconds(0.5f);
        for (; transform.position.y < 0.568f; )
        {
            transform.position = transform.position + new Vector3(0.0f,0.03f,0.0f);
            yield return new WaitForSeconds(0.028f);
        }

        //Explosion
        yield return new WaitForSeconds(1.5f);
        pointLight.intensity = 0; //close light magic wound
        for (; Explosion.transform.localScale.x < 1.8f;)
        {
            Explosion.transform.localScale = new Vector3(Explosion.transform.localScale.x + 0.1f, Explosion.transform.localScale.y + 0.1f, Explosion.transform.localScale.z);
            yield return new WaitForSeconds(0.0f);
        }
        bfinishShine = true;

        

    }
    public void MagicWoundFadeOutShine()
    {
        StopAllCoroutines();
        StartCoroutine(MagicWoundFadeOutShineIEnumerator());

    }

    IEnumerator MagicWoundFadeOutShineIEnumerator()
    {
        //after explosion
        yield return new WaitForSeconds(2.0f);
        pointLight.intensity = 1.0f; //light up magic wound
        for (; ExplosionRenderer.color.a > 0.0f;)
        {
            ExplosionRenderer.color = new Vector4(ExplosionRenderer.color.r, ExplosionRenderer.color.g, ExplosionRenderer.color.b, ExplosionRenderer.color.a - 0.01f);
            yield return new WaitForSeconds(0.02f);
        }


        bfinishFadeOut = true;
    }


}
