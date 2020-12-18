using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraParallaxManager_Level1 : MonoBehaviour
{
    //what we are following
    public Transform PlayerTransform;

    public Transform MainCameraPosition;

    //zeros out the velocity
    Vector3 velocity = Vector3.zero;


    //time to follow PlayerTransform
    public float smoothTime = 0.15f;


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
    int currentPlayerArea = 0;

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
                else if(i == 2)
                {
                    currentPlayerArea = 3;
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


    }



    // Update is called once per frame
    void FixedUpdate()
    {
        if (bCameraFocusOtherObj == false)
        {
            //PlayerTransform position
            Vector3 PlayerTransformPos = PlayerTransform.position;


            //vertical
            if (Info[currentPlayerArea].YMinEnabled && Info[currentPlayerArea].YMaxEnabled)
                PlayerTransformPos.y = Mathf.Clamp(PlayerTransform.position.y, Info[currentPlayerArea].YMinValue, Info[currentPlayerArea].YMaxValue);

            else if (Info[currentPlayerArea].YMinEnabled)
                PlayerTransformPos.y = Mathf.Clamp(PlayerTransform.position.y, Info[currentPlayerArea].YMinValue, PlayerTransform.position.y);

            else if (Info[currentPlayerArea].YMaxEnabled)
                PlayerTransformPos.y = Mathf.Clamp(PlayerTransform.position.y, PlayerTransform.position.y, Info[currentPlayerArea].YMaxValue);

            //horizontal
            if (Info[currentPlayerArea].XMinEnabled && Info[currentPlayerArea].XMaxEnabled)
                PlayerTransformPos.x = Mathf.Clamp(PlayerTransform.position.x, Info[currentPlayerArea].XMinValue, Info[currentPlayerArea].XMaxValue);

            else if (Info[currentPlayerArea].XMinEnabled)
                PlayerTransformPos.x = Mathf.Clamp(PlayerTransform.position.x, Info[currentPlayerArea].XMinValue, PlayerTransform.position.x);

            else if (Info[currentPlayerArea].XMaxEnabled)
                PlayerTransformPos.x = Mathf.Clamp(PlayerTransform.position.x, PlayerTransform.position.x, Info[currentPlayerArea].XMaxValue);



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
