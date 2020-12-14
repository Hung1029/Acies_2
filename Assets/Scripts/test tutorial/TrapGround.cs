using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapGround : MonoBehaviour
{
    bool _bStandOnGroundTrigger ;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        JumpBack();
    }



    void JumpBack()
    {
        if (_bStandOnGroundTrigger == false && transform.rotation.z!=0 )
        {
            //Debug.Log("斜的");
        }
        Debug.Log(_bStandOnGroundTrigger);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.name == "Player")
        {
            _bStandOnGroundTrigger = true;

        }
    }


    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider.name == "Player")
        {
            _bStandOnGroundTrigger = false;

        }
    }


}
