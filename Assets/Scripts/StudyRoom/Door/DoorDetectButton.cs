using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorDetectButton : MonoBehaviour
{
    private GameObject Clue;
    private Clue ClueScript;


    private GameObject levelLoader;
    private LevelLoader levelLoaderScript;

    public string NextLevelName;
    public string ClueWord;

    private bool bEnter = false; 

    // Start is called before the first frame update
    void Start()
    {
        Clue = GameObject.Find("Clue");
        ClueScript = Clue.GetComponent<Clue>();

        levelLoader = GameObject.Find("LevelLoader");
        levelLoaderScript = levelLoader.GetComponent<LevelLoader>();

    }

    // Update is called once per frame
    void Update()
    {
        if (bEnter&& Input.GetKeyDown(KeyCode.B))
        {
            ClueScript.ImmediateOut();
            levelLoaderScript.LoadNextLevel(NextLevelName);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Player" )
        {
            ClueScript.SetText(ClueWord);
            ClueScript.FadeIn();

            bEnter = true;

            
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.name == "Player")
        {
            ClueScript.SetText(ClueWord);
            ClueScript.FadeOut();

            bEnter = false;
        }
    }
}
