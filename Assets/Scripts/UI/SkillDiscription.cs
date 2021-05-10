using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillDiscription : MonoBehaviour
{
    [System.Serializable]
    public struct SkillDiscriptionImage
    {
        public Image image;
        public float duration;
        public float interval;
    }


    public SkillDiscriptionImage[] SkillDiscriptionSet;


    public SkillDiscriptionImage[] SkillDiscriptionSet2;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator FadeIn1()
    {

        GameObject.Find("Player").GetComponent<PlayerMovement>().canMove_camera = false;
        GameObject.Find("Player").GetComponent<Animator>().SetFloat("Speed", Mathf.Abs(0));
        PlayerSkill.bCanTriggerSkill = false;


        float fTimer = 0.0f;
        for (int i = 0; i < 5; i++)
        {
            fTimer = 0.0f;
            while (SkillDiscriptionSet[i].image.color.a != 1.0f)
            {
                SkillDiscriptionSet[i].image.color = Color.Lerp(new Color(1.0f, 1.0f, 1.0f, 0.0f), new Color(1.0f, 1.0f, 1.0f, 1.0f), fTimer);
                fTimer += Time.deltaTime / SkillDiscriptionSet[i].duration;

                yield return null;
            }
            yield return new WaitForSeconds(SkillDiscriptionSet[i].interval);
            
        }

        while (!Input.GetButtonDown("Submit"))
        {
            yield return null;
        }


        fTimer = 0.0f;
        //fade out 1-1 description
        while (SkillDiscriptionSet[3].image.color.a != 0.0f)
        {
            for (int i = 2; i < 4; i++)
            {
                SkillDiscriptionSet[i].image.color = Color.Lerp(new Color(1.0f, 1.0f, 1.0f, 1.0f), new Color(1.0f, 1.0f, 1.0f, 0.0f), fTimer);

            }
            fTimer += Time.deltaTime / 0.5f;
            yield return null;
        }


        //fade in 1-2 description
        for (int i = 5; i < 7; i++)
        {
            fTimer = 0.0f;
            while (SkillDiscriptionSet[i].image.color.a != 1.0f)
            {
                SkillDiscriptionSet[i].image.color = Color.Lerp(new Color(1.0f, 1.0f, 1.0f, 0.0f), new Color(1.0f, 1.0f, 1.0f, 1.0f), fTimer);
                fTimer += Time.deltaTime / SkillDiscriptionSet[i].duration;

                yield return null;
            }
            yield return new WaitForSeconds(SkillDiscriptionSet[i].interval);
        }

        while (!Input.GetButtonDown("Submit"))
        {
            yield return null;
        }

        fTimer = 0.0f;
        while (SkillDiscriptionSet[0].image.color.a != 0.0f)
        {
            SkillDiscriptionSet[0].image.color = Color.Lerp(new Color(1.0f, 1.0f, 1.0f, 1.0f), new Color(1.0f, 1.0f, 1.0f, 0.0f), fTimer);
            SkillDiscriptionSet[1].image.color = Color.Lerp(new Color(1.0f, 1.0f, 1.0f, 1.0f), new Color(1.0f, 1.0f, 1.0f, 0.0f), fTimer);
            SkillDiscriptionSet[4].image.color = Color.Lerp(new Color(1.0f, 1.0f, 1.0f, 1.0f), new Color(1.0f, 1.0f, 1.0f, 0.0f), fTimer);
            SkillDiscriptionSet[5].image.color = Color.Lerp(new Color(1.0f, 1.0f, 1.0f, 1.0f), new Color(1.0f, 1.0f, 1.0f, 0.0f), fTimer);
            SkillDiscriptionSet[6].image.color = Color.Lerp(new Color(1.0f, 1.0f, 1.0f, 1.0f), new Color(1.0f, 1.0f, 1.0f, 0.0f), fTimer);

            fTimer += Time.deltaTime / 0.5f;
            yield return null;
        }

        GameObject.Find("Player").GetComponent<PlayerMovement>().canMove_camera = true;
        PlayerSkill.bCanTriggerSkill = true;
    }



    public IEnumerator FadeIn2()
    {

        PlayerSkill.bCanTriggerSkill = false;
        GameObject.Find("Player").GetComponent<PlayerMovement>().canMove_camera = false;
        GameObject.Find("Player").GetComponent<Animator>().SetFloat("Speed", Mathf.Abs(0));

        float fTimer = 0.0f;
        for (int i = 0; i < 5; i++)
        {
            fTimer = 0.0f;
            while (SkillDiscriptionSet2[i].image.color.a != 1.0f)
            {
                SkillDiscriptionSet2[i].image.color = Color.Lerp(new Color(1.0f, 1.0f, 1.0f, 0.0f), new Color(1.0f, 1.0f, 1.0f, 1.0f), fTimer);
                fTimer += Time.deltaTime / SkillDiscriptionSet2[i].duration;

                yield return null;
            }
            yield return new WaitForSeconds(SkillDiscriptionSet2[i].interval);

        }

        while (!Input.GetButtonDown("Submit"))
        {
            yield return null;
        }


        fTimer = 0.0f;

        while (SkillDiscriptionSet2[0].image.color.a != 0.0f)
        {
            SkillDiscriptionSet2[0].image.color = Color.Lerp(new Color(1.0f, 1.0f, 1.0f, 1.0f), new Color(1.0f, 1.0f, 1.0f, 0.0f), fTimer);
            SkillDiscriptionSet2[1].image.color = Color.Lerp(new Color(1.0f, 1.0f, 1.0f, 1.0f), new Color(1.0f, 1.0f, 1.0f, 0.0f), fTimer);
            SkillDiscriptionSet2[2].image.color = Color.Lerp(new Color(1.0f, 1.0f, 1.0f, 1.0f), new Color(1.0f, 1.0f, 1.0f, 0.0f), fTimer);
            SkillDiscriptionSet2[3].image.color = Color.Lerp(new Color(1.0f, 1.0f, 1.0f, 1.0f), new Color(1.0f, 1.0f, 1.0f, 0.0f), fTimer);
            SkillDiscriptionSet2[4].image.color = Color.Lerp(new Color(1.0f, 1.0f, 1.0f, 1.0f), new Color(1.0f, 1.0f, 1.0f, 0.0f), fTimer);

            fTimer += Time.deltaTime / 0.5f;
            yield return null;
        }

        GameObject.Find("Player").GetComponent<PlayerMovement>().canMove_camera = true;
        PlayerSkill.bCanTriggerSkill = true;
    }


}
