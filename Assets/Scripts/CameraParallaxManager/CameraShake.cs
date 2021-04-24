using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{

    public Transform MainCamera;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Shake(float fPreWaitTime, float duration, float magnitude)
    {
        StartCoroutine(ShakeIEnumerator( fPreWaitTime,  duration,  magnitude));
    }


    IEnumerator ShakeIEnumerator(float fPreWaitTime, float duration, float magnitude) // during time and strength of shake
    {
        Debug.Log("Shake");
        yield return new WaitForSeconds(fPreWaitTime);


        Vector3 originalPos = MainCamera.localPosition;

        float elapsed = 0.0f; //timer

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;

            float y = Random.Range(-1f, 1f) * magnitude;

            MainCamera.localPosition = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null; //before another IEnumerator loop, wait for next frame drawed
        }

        MainCamera.localPosition = originalPos;
    }


}
