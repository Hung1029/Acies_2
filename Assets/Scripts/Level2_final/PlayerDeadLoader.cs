using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadLoader : MonoBehaviour
{
    Animator LoaderAnimator;

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
