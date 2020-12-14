using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager_Correction : MonoBehaviour
{
    [SerializeField]
    private GameObject[] Correction = new GameObject[5];

    private CorrectionStart[] CorrectionScript = new CorrectionStart[5];

    int count = 0;
    int CorrectionNUM = 0;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            CorrectionScript[i] = Correction[i].GetComponent<CorrectionStart>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        

       //Fade In
        if (count%3 == 0)
        {
            CorrectionScript[CorrectionNUM%5].FadeIn();
            count++;
        }
        else if (count % 3 == 1)
        {
           CorrectionScript[CorrectionNUM % 5].DetectStart();
            if (CorrectionScript[CorrectionNUM % 5].bCorrectionFinish)
            {
                count++;
            }
        }
        else if (count % 3 == 2 )
        {
            CorrectionScript[CorrectionNUM % 5].FadeOut();
            CorrectionScript[CorrectionNUM % 5].reset();
            count++;
            CorrectionNUM++;
        }

       
    }
}
