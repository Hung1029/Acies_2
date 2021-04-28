using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadLoader : MonoBehaviour
{
    Animator LoaderAnimator;

    private static PlayerDeadLoader instance;

    private void Awake()
    {
        //check for the instance of the object
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }




    // Start is called before the first frame update
    void Start()
    {
        LoaderAnimator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TransitionAfterTime(float time)
    {
        StartCoroutine(TransitionAfterTimeIEnumerator(time));
    }

    IEnumerator TransitionAfterTimeIEnumerator(float time)
    {
        yield return new WaitForSeconds(time);
        LoaderAnimator.SetTrigger("Start");
    }

}
