using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraParallaxManager_Level1 : MonoBehaviour
{
    //what we are following
    public Transform PlayerTransform;

    //TargetTransform and adjustment
    Vector2 TargetTransform;
    float TargetTransformAdjust_X = 0;
    float TargetTransformAdjust_Y = 0;

    public Transform MainCameraPosition;

    //zeros out the velocity
    Vector3 velocity = Vector3.zero;


    //time to follow PlayerTransform
    public float smoothTime = 0.15f;

    //follow player which axis
    [System.NonSerialized]
    public bool Follow_X = true;
    [System.NonSerialized]
    public bool Follow_Y = true;


    [System.Serializable]
    public struct CheckPointInformation
    {
        //enable check point
        public bool CheckPointEnabled;
        public CheckPointTrigger CheckPointTriggerScript;

        //enable and set the maximum Y value
        public bool YMaxEnabled;
        public float YMaxValue;

        //enable and set the minimum Y value
        public bool YMinEnabled;
        public float YMinValue;

        //enable and set the maximum X value
        public bool XMaxEnabled;
        public float XMaxValue;


        //enable and set the minimum X value
        public bool XMinEnabled;
        public float XMinValue;

        //Sprite Parallax in this area
        public ParallaxBackground[] ParallaxSprite;

    }

    public CheckPointInformation[] Info;

    //Player On Which Part of Game
    [System.NonSerialized]
    public int currentPlayerArea = 0;

    int lastPlayerArea = 0;

    //Camera focus On different object
    bool bCameraFocusOtherObj = false;

    // Start is called before the first frame update
    void Start()
    {
        


    }


    //check if Trigger Camera check point
    void Update()
    {

        //Adjust target position
        TargetTransform = PlayerTransform.position;
        TargetTransform = new Vector2(TargetTransform.x + TargetTransformAdjust_X, TargetTransform.y + TargetTransformAdjust_Y);

        //check where player stand
        if (PlayerTransform.position.x < Info[1].CheckPointTriggerScript.GetComponentInParent<Transform>().position.x)
        {
            currentPlayerArea = 0;
        }
        else
        {
            for (int i = 1; i < Info.Length-1; i++)
            {
                if (PlayerTransform.position.x > Info[i].CheckPointTriggerScript.GetComponentInParent<Transform>().position.x && PlayerTransform.position.x < Info[i + 1].CheckPointTriggerScript.GetComponentInParent<Transform>().position.x)
                {
                    currentPlayerArea = i;
                    break;
                }
                else if(i == Info.Length - 2)
                {
                    currentPlayerArea = Info.Length - 1;
                    break;
                }
            }
        }

        //if change area
        if (lastPlayerArea !=  currentPlayerArea)
        {
            //disable last enable parallax script
            for (int j = 0; j < Info[lastPlayerArea].ParallaxSprite.Length; j++)
            {
                Info[lastPlayerArea].ParallaxSprite[j].GetComponent<ParallaxBackground>().enabled = false;
            }

            //enable parallax script
            for (int j = 0; j < Info[currentPlayerArea].ParallaxSprite.Length; j++)
            {
                Info[currentPlayerArea].ParallaxSprite[j].GetComponent<ParallaxBackground>().enabled = true;
            }


        }

        //update last player area
        lastPlayerArea = currentPlayerArea;

        Debug.Log("currentPlayerArea = " + currentPlayerArea);
    }



    // Update is called once per frame
    void FixedUpdate()
    {
        //Adjust target position
        TargetTransform = PlayerTransform.position;
        TargetTransform = new Vector2(TargetTransform.x + TargetTransformAdjust_X, TargetTransform.y + TargetTransformAdjust_Y);


        if (bCameraFocusOtherObj == false)
        {
            //PlayerTransform position
            Vector3 PlayerTransformPos = TargetTransform;

           
            //vertical
            if (Info[currentPlayerArea].YMinEnabled && Info[currentPlayerArea].YMaxEnabled)
                PlayerTransformPos.y = Mathf.Clamp(TargetTransform.y, Info[currentPlayerArea].YMinValue, Info[currentPlayerArea].YMaxValue);

            else if (Info[currentPlayerArea].YMinEnabled)
                PlayerTransformPos.y = Mathf.Clamp(TargetTransform.y, Info[currentPlayerArea].YMinValue, TargetTransform.y);

            else if (Info[currentPlayerArea].YMaxEnabled)
                PlayerTransformPos.y = Mathf.Clamp(TargetTransform.y, TargetTransform.y, Info[currentPlayerArea].YMaxValue);

            //horizontal
            if (Info[currentPlayerArea].XMinEnabled && Info[currentPlayerArea].XMaxEnabled)
                PlayerTransformPos.x = Mathf.Clamp(TargetTransform.x, Info[currentPlayerArea].XMinValue, Info[currentPlayerArea].XMaxValue);

            else if (Info[currentPlayerArea].XMinEnabled)
                PlayerTransformPos.x = Mathf.Clamp(TargetTransform.x, Info[currentPlayerArea].XMinValue, TargetTransform.x);

            else if (Info[currentPlayerArea].XMaxEnabled)
                PlayerTransformPos.x = Mathf.Clamp(TargetTransform.x, TargetTransform.x, Info[currentPlayerArea].XMaxValue);


            //Not following Player X
            if (!Follow_X)
            {
                //reset x position
                PlayerTransformPos.x = MainCameraPosition.transform.position.x;
            }

            //Not following Player Y
            if (!Follow_Y)
            {
                //reset y position
                PlayerTransformPos.y = MainCameraPosition.transform.position.y;
            }

            //align the camera and the PlayerTransforms z position
            PlayerTransformPos.z = MainCameraPosition.transform.position.z;

            MainCameraPosition.transform.position = Vector3.SmoothDamp(MainCameraPosition.transform.position, PlayerTransformPos, ref velocity, smoothTime);
        }

        else
        {
            //when Camera focuse player can't move
            GameObject.Find("Player").GetComponent<PlayerMovement>().canMove = false;
        }
    }

    public void ChangeCameraProjectionSize( Camera MainCamera , float fSizeValue, float fTransformTime )
    {
        StartCoroutine(ChangeCameraProjectionSizeIEnumerator(MainCamera, fSizeValue, fTransformTime));    

    }

    IEnumerator ChangeCameraProjectionSizeIEnumerator(Camera MainCamera, float fSizeValue, float fTransformTime)
    {

      
        float fTranformValue = 0.05f;
        while (MainCamera.orthographicSize <= fSizeValue)
        {
            MainCamera.orthographicSize +=  fTranformValue;
            
            yield return new WaitForSeconds(fTransformTime / (fSizeValue / fTranformValue));
        }

    }



    public void ChangeCamraFollowingTargetPosition(float x, float y, float fTransformTime, bool KeepXFollow = true, bool KeepYFollow = true)
    {
        StartCoroutine(ChangeCamraFollowingTargetPositionIEnumerator(x, y, fTransformTime, KeepXFollow, KeepYFollow));
    }

    IEnumerator ChangeCamraFollowingTargetPositionIEnumerator(float x, float y, float fTransformTime, bool KeepXFollow = true, bool KeepYFollow = true)
    {

        float fTranformValueX = (x - TargetTransformAdjust_X) / (fTransformTime / 0.03f);
        float fTranformValueY = (y - TargetTransformAdjust_Y) / (fTransformTime / 0.03f);

        while (Mathf.Abs(TargetTransformAdjust_X - x) >= 0.5f || Mathf.Abs(TargetTransformAdjust_Y - y) >= 0.5f)
        {
            TargetTransformAdjust_X += fTranformValueX;
            TargetTransformAdjust_Y += fTranformValueY;

            yield return new WaitForSeconds(0.03f);
        }

        Follow_X = KeepXFollow;
        Follow_Y = KeepYFollow;


    }


    public void ShortFollowing(float time, Vector3 ObjPosition)
    {
        //set Player can't
        GameObject.Find("Player").GetComponent<PlayerMovement>().canMove = false ;
        GameObject.Find("Player").GetComponent<Animator>().SetFloat("Speed", 0.0f);

        bCameraFocusOtherObj = true;
        StartCoroutine(ShortFollowingIEnumerator(time, ObjPosition));


    }

    IEnumerator ShortFollowingIEnumerator(float time, Vector3 ObjPosition)
    {
        ObjPosition.z = MainCameraPosition.position.z;


        //avoid camera lower then restriced range
        if (ObjPosition.y < Info[currentPlayerArea].YMinValue )
        {
            ObjPosition.y = Info[currentPlayerArea].YMinValue;
        }

        //go to object position 
        while (Vector3.Distance(MainCameraPosition.position, ObjPosition) >= 1.0f)
        {
            MainCameraPosition.position = Vector3.SmoothDamp(MainCameraPosition.position, ObjPosition, ref velocity, 0.5f);
            yield return null;
        }

        yield return new WaitForSeconds(time);

        //reset Camera bool 
        bCameraFocusOtherObj = false;

        //reset Player move bool
        GameObject.Find("Player").GetComponent<PlayerMovement>().canMove = true;
    }

    public IEnumerator Shake(float fPreWaitTime ,float duration, float magnitude) // during time and strength of shake
    {
        yield return new WaitForSeconds(fPreWaitTime);


        Vector3 originalPos = MainCameraPosition.localPosition;

        float elapsed = 0.0f; //timer

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;

            float y = Random.Range(-1f, 1f) * magnitude;

            MainCameraPosition.localPosition = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null; //before another IEnumerator loop, wait for next frame drawed
        }

        MainCameraPosition.localPosition = originalPos;
    }


}
