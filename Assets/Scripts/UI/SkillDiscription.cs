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


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator FadeIn()
    {
        for (int i = 0; i < SkillDiscriptionSet.Length; i++)
        {
            float fTimer = 0.0f;
            while (SkillDiscriptionSet[i].image.color.a != 1.0f)
            {
                SkillDiscriptionSet[i].image.color = Color.Lerp(new Color(1.0f, 1.0f, 1.0f, 0.0f), new Color(1.0f, 1.0f, 1.0f, 1.0f), fTimer);
                fTimer += Time.deltaTime / SkillDiscriptionSet[i].duration;

                yield return null;
            }
            yield return new WaitForSeconds(SkillDiscriptionSet[i].interval);
            
        }
    }

    public IEnumerator FadeOut()
    {
        float fTimer = 0.0f;
        while (SkillDiscriptionSet[0].image.color.a != 0.0f)
        {
            for (int i = 0; i < SkillDiscriptionSet.Length; i++)
            {
                SkillDiscriptionSet[i].image.color = Color.Lerp(new Color(1.0f, 1.0f, 1.0f, 1.0f), new Color(1.0f, 1.0f, 1.0f, 0.0f), fTimer);
                
            }
            fTimer += Time.deltaTime / 0.5f;
            yield return null;
        }

    }


}
