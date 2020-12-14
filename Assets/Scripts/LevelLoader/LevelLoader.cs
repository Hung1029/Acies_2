using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;

    // Update is called once per frame
    void Update()
    {
       
      
    }

    public void LoadNextLevel(string SceneName)
    {
          StartCoroutine(LoadLevelDelay(SceneName));
        

    }

    IEnumerator LoadLevelDelay(string SceneName)
    {
        transition.SetTrigger("Start");
        
        yield return new WaitForSeconds(1.0f);

        
        SceneManager.LoadScene(SceneName);
        
    }

}
