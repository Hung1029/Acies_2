using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Short_Cut : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        //reload scene
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

            //reset dontdistory
            GameObject.FindObjectOfType<DontDistory>().reset_state();
        }

        //switch to scene1
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SceneManager.LoadScene(4);
        }

        //switch to scene2
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SceneManager.LoadScene(5);
        }

        //switch to scene3
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SceneManager.LoadScene(6);
        }






        if (Input.GetKeyDown(KeyCode.S))
        {
            SceneManager.LoadScene(0);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            SceneManager.LoadScene(1);
        }


        if (Input.GetKeyDown(KeyCode.P))
        {
            SceneManager.LoadScene(2);
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            SceneManager.LoadScene(3);
        }
    }
}
