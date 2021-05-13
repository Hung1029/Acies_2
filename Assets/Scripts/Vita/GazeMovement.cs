using System.Collections;
using System.Collections.Generic;
using Tobii.Gaming;
using UnityEngine;

public class GazeMovement : MonoBehaviour
{

    private Vector3 lastGazePoint = Vector3.zero;
    private GazePoint gazePoint;
    public float gazePointCanMoveRange = 0.015f;
    private float fGaze_timer = 0.0f;
    [System.NonSerialized]
    public bool bVitaSoulCanGaze = false;

    //find object with tag can skill
    GameObject[] ObjectCanSkill;
    int iNearCanSkillObjNUM = -1;
    float fNearDistance = 0.75f;

    // Start is called before the first frame update
    void Start()
    {
        gazePoint = TobiiAPI.GetGazePoint();
        Vector3 gazeOnScreen = Camera.main.ScreenToWorldPoint(gazePoint.Screen);
        lastGazePoint = gazeOnScreen;


    }

    // Update is called once per frame
    void Update()
    {
        ObjectCanSkill = GameObject.FindGameObjectsWithTag("CanSkill");


        fGaze_timer += Time.deltaTime;
        gazePoint = TobiiAPI.GetGazePoint();

        if (fGaze_timer >= 0.025f && bVitaSoulCanGaze)
        {
            
            Vector3 gazeOnScreen = Camera.main.ScreenToWorldPoint(gazePoint.Screen);
            
            //check if updat point    
            if (((gazeOnScreen.x - lastGazePoint.x) * (gazeOnScreen.x - lastGazePoint.x) + (gazeOnScreen.y - lastGazePoint.y) * (gazeOnScreen.y - lastGazePoint.y)) > gazePointCanMoveRange * gazePointCanMoveRange)
            {
                //if gaze point near can skill object, change it position                 
                for (int i = 0; i < ObjectCanSkill.Length && PlayerSkill.CURRENTSKILL == 1; i++)
                {
                    if (Vector2.Distance(gazeOnScreen, ObjectCanSkill[i].GetComponent<Transform>().position) < fNearDistance && ObjectCanSkill[i].GetComponentInChildren<VitaTriggerDetect>().bCanBeDetect)
                    {
                        iNearCanSkillObjNUM = i;
                        break;
                    }
                }

                //change transform position
                if (iNearCanSkillObjNUM != -1 )
                {
                     transform.position = new Vector2(ObjectCanSkill[iNearCanSkillObjNUM].GetComponent<Transform>().position.x, ObjectCanSkill[iNearCanSkillObjNUM].GetComponent<Transform>().position.y);
                }

                else
                    transform.position = new Vector2(gazeOnScreen.x, gazeOnScreen.y);

                iNearCanSkillObjNUM = -1;
            }
                
            lastGazePoint = gazeOnScreen;

            //clear timer
            fGaze_timer = 0.0f;
        }

        

    }

    float map(float s, float low1, float high1, float low2, float high2)
    {
        return low2 + (s - low1) * (high2 - low2) / (high1 - low1);
    }

   


}
