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

    // Start is called before the first frame update
    void Start()
    {

    }


    //check if Trigger Camera check point
    void Update()
    {

        //check all collider
        for (int i = 0; i < Info.Length; i++)
        {
            if (Info[i].CheckPointEnabled)
            {
                //if Player touch collider
                if (Info[i].CheckPointTriggerScript.bTouchPlayer)
                {
                    //Player face lefts => go to left area
                    if (PlayerTransform.localScale.x > 0.0f)
                    {
                        

                        //disable last enable parallax script
                        for (int j = 0; j < Info[currentPlayerArea].ParallaxSprite.Length; j++)
                        {
                            Info[currentPlayerArea].ParallaxSprite[j].GetComponent<ParallaxBackground>().enabled = false;
                        }

                        currentPlayerArea--;

                        //enable parallax script
                        for (int j = 0; j < Info[currentPlayerArea].ParallaxSprite.Length; j++)
                        {
                            Info[currentPlayerArea].ParallaxSprite[j].GetComponent<ParallaxBackground>().enabled = true;
                        }

                    }
                    //Player face Right => go to right area                    
                    else
                    {
                        //disable last enable parallax script
                        for (int j = 0; j < Info[currentPlayerArea].ParallaxSprite.Length; j++)
                        {
                            Info[currentPlayerArea].ParallaxSprite[j].GetComponent<ParallaxBackground>().enabled = false;
                        }

                        currentPlayerArea++;

                        //enable parallax script
                        for (int j = 0; j < Info[currentPlayerArea].ParallaxSprite.Length; j++)
                        {
                            Info[currentPlayerArea].ParallaxSprite[j].GetComponent<ParallaxBackground>().enabled = true;
                        }
                    }

                    //reset check point bool
                    Info[i].CheckPointTriggerScript.bTouchPlayer = false;
                }
            }
        }
    }



    // Update is called once per frame
    void FixedUpdate()
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

    
}
