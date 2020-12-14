using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

    public Text nameText;
    public Text dialogueText;
    public Animator animator;
    public Image Image;
    private Queue<string> sentences;

    [System.NonSerialized]
    public static bool bFinishDialogue = true;

    

    // Start is called before the first frame update
    void Start () {
        sentences = new Queue<string> ();

        
    }

    public void StartDialogue (Dialogue dialogue, Sprite picture) {
        
        bFinishDialogue = false; //Dialogue NonFinish

        
        animator.SetBool ("isOpen", true); //set animator bool

        Image.sprite = picture; //set CG picture

        nameText.text = dialogue.name; //set npc name

        sentences.Clear (); // clear previous sentences

        //queue up sentences
        foreach (string sentence in dialogue.sentences) {
            sentences.Enqueue (sentence);
        }

        DisplayNextSentence ();
    }

    public void DisplayNextSentence () {
        if (sentences.Count == 0) {
            EndDialogue ();
            return;
        }
       
        string sentence = sentences.Dequeue ();
        StopAllCoroutines (); //stop last type sentence
        StartCoroutine (TypeSentence (sentence));
    }

    //set type style
    IEnumerator TypeSentence (string sentence) {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray ()) {
            dialogueText.text += letter;
            yield return new WaitForSeconds (0.03f);
        }
    }

    public void EndDialogue () {
        bFinishDialogue = true; //Dialogue Finish
        animator.SetBool ("isOpen", false); //set animator bool
    }

    void Update () {
        if (Input.GetButtonDown ("NPC_trigger")) {
            DisplayNextSentence ();
        }

    }

}