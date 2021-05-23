using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager_OutDoorStory : MonoBehaviour
{
    //Grandpa
    [SerializeField]
    private GameObject Gandpa;
    private NPC_Dialogue GandpaScript;
    public Sprite GrandpaCV;

    //Vita
    public GameObject VitaSoul;
    private VitaSoul_particle VitaSoulScript;
    public Sprite VitaCV;

    //Player
    public Sprite PlayerCV;

    private int AnimatorCount = 0;

    //Dialogue
    private Dialogue dialogue;

      //UI set
    [SerializeField]
    private GameObject CGMove;
    private VitaCGMovement CGMoveScript;

    public PlayerTriggerDetect rightBoundary;

    private LevelLoader levelLoaderScript;
    [SerializeField]
    private GameObject levelLoader;

    // Start is called before the first frame update
    void Start()
    {
        GandpaScript = Gandpa.GetComponent<NPC_Dialogue>();

        VitaSoulScript = VitaSoul.GetComponent<VitaSoul_particle>();
        VitaSoulScript.MoveSpeed = 8.0f;

        dialogue = new Dialogue();

        //UI
        CGMoveScript = CGMove.GetComponent<VitaCGMovement>();

        VitaSoulScript.VitaSpriteFadeIn();

        //controlChangeScene
        levelLoaderScript = levelLoader.GetComponent<LevelLoader>();
    }

    // Update is called once per frame
    void Update()
    {
        VitaSoulScript.FollowObj();

        if (AnimatorCount == 0 && GandpaScript.bTouchPlayer && Input.GetKeyDown(KeyCode.A))
        {
            dialogue.name = "莉妲";
            dialogue.sentences = new string[1];
            dialogue.sentences[0] = "爺爺，不好了！";
            CGMoveScript.SetRecTransformX(145.7859f);
            FindObjectOfType<DialogueManager>().StartDialogue(dialogue, PlayerCV);


            AnimatorCount++;
        }

        else if (AnimatorCount == 1 && DialogueManager.bFinishDialogue)
        {
            dialogue.name = "爺爺";
            dialogue.sentences = new string[1];
            dialogue.sentences[0] = "是莉妲啊？怎麼了嗎？";
            CGMoveScript.SetRecTransformX(145.7859f);
            FindObjectOfType<DialogueManager>().StartDialogue(dialogue, GrandpaCV);

            AnimatorCount++;
        }

        else if (AnimatorCount == 2 && DialogueManager.bFinishDialogue)
        {
            dialogue.name = "莉妲";
            dialogue.sentences = new string[1];
            dialogue.sentences[0] = "是這樣的…";
            FindObjectOfType<DialogueManager>().StartDialogue(dialogue, PlayerCV);

            AnimatorCount++;
        }

        else if (AnimatorCount == 3 && DialogueManager.bFinishDialogue)
        {
            dialogue.name = "薇妲";
            dialogue.sentences = new string[1];
            dialogue.sentences[0] = "爺爺～快看看我，我現在竟然可以飛欸！";
            CGMoveScript.SetRecTransformX(62.98596f);
            FindObjectOfType<DialogueManager>().StartDialogue(dialogue, VitaCV);

            AnimatorCount++;
        }

        else if (AnimatorCount == 4 && DialogueManager.bFinishDialogue)
        {
            dialogue.name = "爺爺";
            dialogue.sentences = new string[1];
            dialogue.sentences[0] = "這是薇妲？怎麼變成這樣？";
            CGMoveScript.SetRecTransformX(145.7859f);
            FindObjectOfType<DialogueManager>().StartDialogue(dialogue, GrandpaCV);

            AnimatorCount++;
        }

        else if (AnimatorCount == 5 && DialogueManager.bFinishDialogue)
        {
            dialogue.name = "莉妲";
            dialogue.sentences = new string[1];
            dialogue.sentences[0] = "薇妲碰到書房裡的這個東西，就變成這樣了，爺爺怎麼辦？";
            FindObjectOfType<DialogueManager>().StartDialogue(dialogue, PlayerCV);

            AnimatorCount++;
        }

        else if (AnimatorCount == 6 && DialogueManager.bFinishDialogue)
        {
            dialogue.name = "爺爺";
            dialogue.sentences = new string[1];
            dialogue.sentences[0] = "如果我沒猜錯，法鈴開啟了傳送門將薇妲的身體傳到別的地方了，只要找到身體的所在，薇妲就可以回到身體裡";
            FindObjectOfType<DialogueManager>().StartDialogue(dialogue, GrandpaCV);

            AnimatorCount++;
        }

        else if (AnimatorCount == 7 && DialogueManager.bFinishDialogue)
        {
            dialogue.name = "莉妲";
            dialogue.sentences = new string[1];
            dialogue.sentences[0] = "真的嗎？那我們該怎麼做？";
            FindObjectOfType<DialogueManager>().StartDialogue(dialogue, PlayerCV);

            AnimatorCount++;
        }

        else if (AnimatorCount == 8 && DialogueManager.bFinishDialogue)
        {
            dialogue.name = "爺爺";
            dialogue.sentences = new string[1];
            dialogue.sentences[0] = "你們前往斯克爾都吧！這個法鈴是我從斯克爾族長手中拿到的，他或許會知道更詳細的原因";
            FindObjectOfType<DialogueManager>().StartDialogue(dialogue, GrandpaCV);

            AnimatorCount++;
        }

        else if (AnimatorCount == 9 && DialogueManager.bFinishDialogue)
        {
            dialogue.name = "莉妲";
            dialogue.sentences = new string[1];
            dialogue.sentences[0] = "斯克爾都……是那個傳說中受神龍眷顧的城市嗎？";
            FindObjectOfType<DialogueManager>().StartDialogue(dialogue, PlayerCV);

            AnimatorCount++;
        }

        else if (AnimatorCount == 10 && DialogueManager.bFinishDialogue)
        {
            dialogue.name = "爺爺";
            dialogue.sentences = new string[1];
            dialogue.sentences[0] = "沒錯，我會給斯克爾族長寫一封信，事情緊急，你們快點出發吧！";
            FindObjectOfType<DialogueManager>().StartDialogue(dialogue, GrandpaCV);

            AnimatorCount++;
        }

        if (rightBoundary.bTrigger)
        {
            levelLoaderScript.LoadNextLevel("Level1_final");
        }


    }
}
