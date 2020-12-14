using System.Collections;
using System.Collections.Generic;

using Tobii.Gaming;
using UnityEngine;

public class CorrectionGazePoint : MonoBehaviour
{
    private Vector3 lastGazePoint = Vector3.zero;
    private GazePoint gazePoint;
    public float gazePointCanMoveRange = 0.015f;
    private float fGaze_timer = 0.0f;
   

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
        fGaze_timer += Time.deltaTime;
        gazePoint = TobiiAPI.GetGazePoint();

        if (fGaze_timer >= 0.025f)
        {
            Vector3 gazeOnScreen = Camera.main.ScreenToWorldPoint(gazePoint.Screen);

            //check if updat point    
            if (((gazeOnScreen.x - lastGazePoint.x) * (gazeOnScreen.x - lastGazePoint.x) + (gazeOnScreen.y - lastGazePoint.y) * (gazeOnScreen.y - lastGazePoint.y)) > gazePointCanMoveRange * gazePointCanMoveRange)
                transform.position = new Vector2(gazeOnScreen.x, gazeOnScreen.y);
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
