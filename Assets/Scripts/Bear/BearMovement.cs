using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearMovement : MonoBehaviour
{
    Animator animator;
    ParticleSystem ps;

    //Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        animator = this.gameObject.GetComponent<Animator>();
        ps = this.gameObject.GetComponentInChildren<ParticleSystem>();

        //camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    void Howl()
    {
        animator.SetTrigger("tHowl");

        StopCoroutine(HowlRippleIEnumerator());
        StartCoroutine(HowlRippleIEnumerator());
    }

    IEnumerator HowlRippleIEnumerator()
    {
        yield return new WaitForSeconds(0.8f);

        ps.Play();

        //StopCoroutine(camera.GetComponent<FollowingTarget>().Shake(3.0f, 0.08f));
        //StartCoroutine(camera.GetComponent<FollowingTarget>().Shake(3.0f, 0.08f));

    }



}
