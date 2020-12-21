using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{

    static int PlayerHealthCount = 5;
    float fCanHurtTimer = 2.0f;
    bool bHurrting = false;

    //bool bCanHurt = true;

    [SerializeField]
    private GameObject[] PlayerLifeUIImageArray;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(bHurrting == false)
            fCanHurtTimer += Time.deltaTime;

        /*if (Input.GetKeyDown(KeyCode.N))
        {
            bCanHurt = !bCanHurt;
        }*/

        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetPlayerLife();
        }


    }

    // can call one time or many time
    public void Hurt()
    {

      
        if (fCanHurtTimer > 1.0f && PlayerHealthCount > 0 /*&& bCanHurt*/) // 1s hurt interval and have life
        {
            //stop counting hurt interval timer
            bHurrting = true;
            fCanHurtTimer = 0.0f;

            //loss life
            PlayerHealthCount--;

            //loss life UI
            PlayerLifeUIImageArray[PlayerHealthCount].GetComponent<Image>().color = new Color(PlayerLifeUIImageArray[PlayerHealthCount].GetComponent<Image>().color.r , PlayerLifeUIImageArray[PlayerHealthCount ].GetComponent<Image>().color.g , PlayerLifeUIImageArray[PlayerHealthCount ].GetComponent<Image>().color.b , PlayerLifeUIImageArray[PlayerHealthCount].GetComponent<Image>().color.a/4f);
            PlayerLifeUIImageArray[PlayerHealthCount].transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(PlayerLifeUIImageArray[PlayerHealthCount].transform.GetChild(0).GetComponentInChildren<Image>().color.r, PlayerLifeUIImageArray[PlayerHealthCount].transform.GetChild(0).GetComponentInChildren<Image>().color.g, PlayerLifeUIImageArray[PlayerHealthCount].transform.GetChild(0).GetComponentInChildren<Image>().color.b, PlayerLifeUIImageArray[PlayerHealthCount].transform.GetChild(0).GetComponentInChildren<Image>().color.a / 4f);
            
            //play hurt animation
            StartCoroutine(PlayerGetHurt());
            
        }


    }

    public void ResetPlayerLife()
    {
        if (PlayerHealthCount == 0)
        {
            //Reset life
            PlayerHealthCount = 5;

            for (int i = 0; i < 5; i++)
            {
                //Reset life UI
                PlayerLifeUIImageArray[i].GetComponent<Image>().color = new Color(PlayerLifeUIImageArray[i].GetComponent<Image>().color.r, PlayerLifeUIImageArray[i].GetComponent<Image>().color.g, PlayerLifeUIImageArray[i].GetComponent<Image>().color.b, 255f);
                PlayerLifeUIImageArray[i].transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(PlayerLifeUIImageArray[i].transform.GetChild(0).GetComponentInChildren<Image>().color.r, PlayerLifeUIImageArray[i].transform.GetChild(0).GetComponentInChildren<Image>().color.g, PlayerLifeUIImageArray[i].transform.GetChild(0).GetComponentInChildren<Image>().color.b, 255f);

            }
        }
    }


    IEnumerator PlayerGetHurt()
    {
        //stop Player moving
        GameObject.Find("Player").GetComponent<PlayerMovement>().canMove = false;
        GameObject.Find("Player").GetComponent<Animator>().SetFloat("Speed", Mathf.Abs(0));

        //set animate to idle
        GameObject.Find("Player").GetComponent<PlayerSkill>().ResetAnimateToIdle();

        //trun to red color
        GameObject.Find("Player").GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.5f, 0.5f);

        yield return new WaitForSeconds(2.0f);

        //trun color back
        GameObject.Find("Player").GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f);

        //Player can moving
        GameObject.Find("Player").GetComponent<PlayerMovement>().canMove = true;
        bHurrting = false;

    }


}
