using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Dialogue : MonoBehaviour {
    
    public GameObject Key_A;
    public Sprite picture;
    private Renderer A_renderer;
    private bool canTalk = false;

    [System.NonSerialized]
    public bool bTouchPlayer = false;

    // Start is called before the first frame update
    void Start () {
        A_renderer = Key_A.GetComponent<Renderer> ();
        A_renderer.material.color = new Color (A_renderer.material.color.r, A_renderer.material.color.g, A_renderer.material.color.b, 0);
       
    }
    
    
    // Update is called once per frame
    void Update () {

    }

    public void TriggerDialgue () {
        //dialogue.name = "薇妲";
        //dialogue.sentences = new string[2];
        //dialogue.sentences[0] = "姐姐你看書櫃移開了!!!";
        //dialogue.sentences[1] = "陪我進去嘛 拜託拜託";
        //FindObjectOfType<DialogueManager> ().StartDialogue (dialogue, picture);
       
    }

    private void OnTriggerEnter2D (Collider2D other) {
      
        if(other.name == "Player")
        {
            //fade in block
            StopAllCoroutines (); //stop last style
            StartCoroutine (fadeIn ());

            bTouchPlayer = true;
        }

        
    }

    private void OnTriggerExit2D (Collider2D other) {
        if (other.name == "Player")
        {
            //fade out block
            StopAllCoroutines (); //stop last style
            StartCoroutine (fadeOut ());

            bTouchPlayer = false;
        }
            
    }

    IEnumerator fadeIn () {

        
        float opacity = A_renderer.material.color.a;
        for (int i = 0; opacity < 1.0f; i++) {
            opacity += 0.1f;
            A_renderer.material.color = new Color (A_renderer.material.color.r, A_renderer.material.color.g, A_renderer.material.color.b, opacity);
            yield return null;
        }
        
    }
   

    IEnumerator fadeOut () {
        
        float opacity = A_renderer.material.color.a;
        for (int i = 0; opacity > 0.0f; i++) {
            opacity -= 0.1f;
            A_renderer.material.color = new Color (A_renderer.material.color.r, A_renderer.material.color.g, A_renderer.material.color.b, opacity);
            yield return null;
        }
        A_renderer.material.color = new Color(A_renderer.material.color.r, A_renderer.material.color.g, A_renderer.material.color.b, 0.0f);

    }

}