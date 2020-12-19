using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectBearCollision : MonoBehaviour
{
    [System.NonSerialized]
    public bool _bSkillOneTrigger = false;

    public ParticleSystem RockDamageParticle;

    bool bBroken = false;

    void Update()
    {
        if (_bSkillOneTrigger && !bBroken)
        {
            bBroken = true;
            StartCoroutine(FloorBroken());
        }

    }



    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.name == "Bear")
        {
            _bSkillOneTrigger = true;

        }
    }

    IEnumerator FloorBroken()
    {
        yield return new WaitForSeconds(0.9f);
        RockDamageParticle.Play();
        Destroy(this.gameObject);

    }
}
