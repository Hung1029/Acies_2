using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    //what we are following
    public Transform PlayerTransform;

    //TargetTransform and adjustment
    Vector2 TargetTransform;
    float TargetTransformAdjust_X = 0;
    float TargetTransformAdjust_Y = 0;

    public Transform MainCamera;

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
        public GameObject CheckPoint;

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

    public CheckPointInformation[] CheckPointInfo;

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
        if (PlayerTransform.position.x < CheckPointInfo[1].CheckPoint.GetComponentInParent<Transform>().position.x)
        {
            currentPlayerArea = 0;
        }
        else
        {
            for (int i = 0; i < CheckPointInfo.Length -1; i++)
            {
                if (PlayerTransform.position.x > CheckPointInfo[i].CheckPoint.GetComponentInParent<Transform>().position.x && PlayerTransform.position.x < CheckPointInfo[i + 1].CheckPoint.GetComponentInParent<Transform>().position.x)
                {
                    currentPlayerArea = i;
                    break;
                }
                else if (i == CheckPointInfo.Length - 2)
                {
                    currentPlayerArea = CheckPointInfo.Length - 1;
                    break;
                }
            }
        }

        //if change area
        if (lastPlayerArea != currentPlayerArea)
        {
            //disable last enable parallax script
            for (int j = 0; j < CheckPointInfo[lastPlayerArea].ParallaxSprite.Length; j++)
            {
                CheckPointInfo[lastPlayerArea].ParallaxSprite[j].GetComponent<ParallaxBackground>().enabled = false;
            }

            //enable parallax script
            for (int j = 0; j < CheckPointInfo[currentPlayerArea].ParallaxSprite.Length; j++)
            {
                CheckPointInfo[currentPlayerArea].ParallaxSprite[j].GetComponent<ParallaxBackground>().enabled = true;
            }


        }

        //update last player area
        lastPlayerArea = currentPlayerArea;

        //Debug.Log("currentPlayerArea = "+currentPlayerArea);
        //Debug.Log("point 1 = " + CheckPointInfo[0].CheckPoint.GetComponentInParent<Transform>().position.x);
        //Debug.Log("point 2 = " + CheckPointInfo[1].CheckPoint.GetComponentInParent<Transform>().position.x);
       // Debug.Log("player = " + PlayerTransform.position.x);
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
            if (CheckPointInfo[currentPlayerArea].YMinEnabled && CheckPointInfo[currentPlayerArea].YMaxEnabled)
                PlayerTransformPos.y = Mathf.Clamp(TargetTransform.y, CheckPointInfo[currentPlayerArea].YMinValue, CheckPointInfo[currentPlayerArea].YMaxValue);

            else if (CheckPointInfo[currentPlayerArea].YMinEnabled)
                PlayerTransformPos.y = Mathf.Clamp(TargetTransform.y, CheckPointInfo[currentPlayerArea].YMinValue, TargetTransform.y);

            else if (CheckPointInfo[currentPlayerArea].YMaxEnabled)
                PlayerTransformPos.y = Mathf.Clamp(TargetTransform.y, TargetTransform.y, CheckPointInfo[currentPlayerArea].YMaxValue);

            //horizontal
            if (CheckPointInfo[currentPlayerArea].XMinEnabled && CheckPointInfo[currentPlayerArea].XMaxEnabled)
                PlayerTransformPos.x = Mathf.Clamp(TargetTransform.x, CheckPointInfo[currentPlayerArea].XMinValue, CheckPointInfo[currentPlayerArea].XMaxValue);

            else if (CheckPointInfo[currentPlayerArea].XMinEnabled)
                PlayerTransformPos.x = Mathf.Clamp(TargetTransform.x, CheckPointInfo[currentPlayerArea].XMinValue, TargetTransform.x);

            else if (CheckPointInfo[currentPlayerArea].XMaxEnabled)
                PlayerTransformPos.x = Mathf.Clamp(TargetTransform.x, TargetTransform.x, CheckPointInfo[currentPlayerArea].XMaxValue);


            //Not following Player X
            if (!Follow_X)
            {
                //reset x position
                PlayerTransformPos.x = MainCamera.transform.position.x;
            }

            //Not following Player Y
            if (!Follow_Y)
            {
                //reset y position
                PlayerTransformPos.y = MainCamera.transform.position.y;
            }

            //align the camera and the PlayerTransforms z position
            PlayerTransformPos.z = MainCamera.transform.position.z;

           
            MainCamera.transform.position = Vector3.SmoothDamp(MainCamera.transform.position, PlayerTransformPos, ref velocity, smoothTime);
            
            //Debug.Log(Follow_Y);
        }

        else
        {
            //when Camera focuse player can't move
            GameObject.Find("Player").GetComponent<PlayerMovement>().canMove = false;
        }
    }

    public void ChangeCameraProjectionSize(Camera MainCamera, float fSizeValue, float fTransformTime)
    {
        StartCoroutine(ChangeCameraProjectionSizeIEnumerator(MainCamera, fSizeValue, fTransformTime));

    }

    IEnumerator ChangeCameraProjectionSizeIEnumerator(Camera MainCamera, float fSizeValue, float fTransformTime)
    {
        if (fTransformTime > 0.0f)
        {
            float fOriginalValue = MainCamera.orthographicSize;
            float fCameraChangePositionTimer = 0.0f;

            while (MainCamera.orthographicSize != fSizeValue)
            {
                Camera.main.orthographicSize = Mathf.Lerp(fOriginalValue, fSizeValue, fCameraChangePositionTimer);
                fCameraChangePositionTimer += Time.deltaTime / fTransformTime;

                yield return null;
            }
        }
        else if(fTransformTime == 0.0f)
        {
            Camera.main.orthographicSize = fSizeValue;
        }

    }



    public void ChangeCamraFollowingTargetPosition(float x, float y, float fTransformTime, bool KeepXFollow , bool KeepYFollow )
    {
        StartCoroutine(ChangeCamraFollowingTargetPositionIEnumerator(x, y, fTransformTime, KeepXFollow, KeepYFollow));
    }

    IEnumerator ChangeCamraFollowingTargetPositionIEnumerator(float x, float y, float fTransformTime, bool KeepXFollow , bool KeepYFollow)
    {
        //Debug.Log("start ChangeCamraFollowingTargetPositionIEnumerator ");
        Follow_X = false;
        Follow_Y = false;

        if (fTransformTime > 0.0f)
        {
            Vector3 v2NewValue = new Vector3();

            Vector3 v2OriginalPosition = new Vector3(MainCamera.position.x, MainCamera.position.y, MainCamera.position.z);
            Vector3 v2TargetPosition = new Vector3(v2OriginalPosition.x + x, y, MainCamera.position.z);
            

            float fCameraChangePositionTimer = 0.0f;

            while (Vector2.Distance(MainCamera.transform.position, v2TargetPosition) >= 0.001f)
            {
                v2NewValue = Vector2.Lerp(v2OriginalPosition, v2TargetPosition, fCameraChangePositionTimer);

                fCameraChangePositionTimer += Time.deltaTime / fTransformTime;

                //TargetTransformAdjust_X = v2NewValue.x;
                //TargetTransformAdjust_Y = v2NewValue.y;


                //setting camera 

                //Adjust target position
                TargetTransform = PlayerTransform.position;
                TargetTransform = new Vector2(v2NewValue.x, v2NewValue.y);
                
                //PlayerTransform position
                Vector3 TransformPos = TargetTransform;

                TransformPos.z = MainCamera.transform.position.z;

                MainCamera.transform.position = Vector3.SmoothDamp(MainCamera.transform.position, TransformPos, ref velocity, smoothTime);
            

                yield return null;

            }
            MainCamera.transform.position = v2TargetPosition;
            TargetTransformAdjust_X = x;
        }

        else if (fTransformTime == 0.0f)
        {
            TargetTransformAdjust_X = x;
            TargetTransformAdjust_Y = y;
        }


        Follow_X = KeepXFollow;
        Follow_Y = KeepYFollow;

        /*.Log("finish ChangeCamraFollowingTargetPositionIEnumerator ");*/
    }


    public void ShortFollowing(float time, Vector3 ObjPosition)
    {
       // Debug.Log("ShortFollowing");
        //set Player can't
        GameObject.Find("Player").GetComponent<PlayerMovement>().canMove = false;
        GameObject.Find("Player").GetComponent<Animator>().SetFloat("Speed", 0.0f);

        bCameraFocusOtherObj = true;
        StartCoroutine(ShortFollowingIEnumerator(time, ObjPosition));


    }

    IEnumerator ShortFollowingIEnumerator(float time, Vector3 ObjPosition)
    {
        ObjPosition.z = MainCamera.position.z;


        //avoid camera lower then restriced range
        if (ObjPosition.y < CheckPointInfo[currentPlayerArea].YMinValue)
        {
            ObjPosition.y = CheckPointInfo[currentPlayerArea].YMinValue;
        }

        //go to object position 
        while (Vector3.Distance(MainCamera.position, ObjPosition) >= 1.0f)
        {
            MainCamera.position = Vector3.SmoothDamp(MainCamera.position, ObjPosition, ref velocity, 0.5f);
            yield return null;
        }

        yield return new WaitForSeconds(time);

        //reset Camera bool 
        bCameraFocusOtherObj = false;

        //reset Player move bool
        GameObject.Find("Player").GetComponent<PlayerMovement>().canMove = true;
        //Debug.Log("ShortFollowing finish");
    }

    public void SetXYFollowing(bool KeepXFollow , bool KeepYFollow )
    {
        Follow_X = KeepXFollow;
        Follow_Y = KeepYFollow;
    }


    public void BackToFollowPlayer()
    {
        TargetTransformAdjust_X = 0;
        TargetTransformAdjust_Y = 0;
        Follow_X = true;
        Follow_Y = true;
    }


    public IEnumerator Shake(float fPreWaitTime, float duration, float magnitude) // during time and strength of shake
    {
        //Debug.Log("Shake");
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
