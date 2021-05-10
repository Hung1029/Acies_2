using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager_StudyRoomStory : MonoBehaviour
{
    [SerializeField]
    private GameObject BookCase;

    [SerializeField]
    private GameObject Player;
    private PlayerMovement PlayerMovementScript;
    private PlayerTriggerDetect PlayerTriggerScript;

    [SerializeField]
    private GameObject Vita;
    private NPC_Dialogue VitaDialogueScript;
    private Vita_Movement VitaMovementScript;

    [SerializeField]
    private GameObject levelLoader;
    private LevelLoader levelLoaderScript;

    [SerializeField]
    private GameObject Clue;
    private Clue ClueScript;

    //Vita Ask Rita to go to Private Room
    private bool bVitaAsk = false;

    private int AnimatorCount = 0;
    private bool changeScene = false;

    //Dialogue
    private Dialogue dialogue;
    [SerializeField]
    private Sprite VitaCV;

    //UI set
    [SerializeField]
    private GameObject CGMove;
    private VitaCGMovement CGMoveScript;
    void Awake()
    {
        
        //If ever enter Study Room, BaseCase don't move
        if (!GameDataManager.instance.bEnterStudyRoom)
            this.enabled = true;
        

    }


    // Start is called before the first frame update
    void Start()
    {
        GameDataManager.instance.bEnterStudyRoom = true;

        //control player movement
        PlayerMovementScript = Player.GetComponent<PlayerMovement>();
        PlayerTriggerScript = Player.GetComponent<PlayerTriggerDetect>();

        //control Vita dialogue , movement
        VitaDialogueScript = Vita.GetComponent<NPC_Dialogue>();
        VitaMovementScript = Vita.GetComponent<Vita_Movement>();

        //control Clue
        ClueScript = Clue.GetComponent<Clue>();

        //controlChangeScene
        levelLoaderScript = levelLoader.GetComponent<LevelLoader>();

        //new dialogue
        dialogue = new Dialogue();

        //UI
        CGMoveScript = CGMove.GetComponent<VitaCGMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        //BookCase Swap
        if (AnimatorCount == 0)
        {
            //play the book case animator at the first time enter           
            PlayerMovementScript.canMove_skill = false;
            StartCoroutine(BookCaseSwap(3.0f));
            
            AnimatorCount++;
        }

        //Vita ask to go Private Room
        else if (AnimatorCount == 1 && bVitaAsk)
        {
            //VitaDialogueScript.TriggerDialgue();
            dialogue.name = "薇妲";
            dialogue.sentences = new string[2];
            dialogue.sentences[0] = "姐姐你看書櫃移開了!!!";
            dialogue.sentences[1] = "陪我進去嘛 拜託拜託";
            CGMoveScript.SetRecTransformX(62.98596f);
            FindObjectOfType<DialogueManager>().StartDialogue(dialogue, VitaCV);
            AnimatorCount++;
        }

        //Vita finish talking, Rita can move
        else if (AnimatorCount == 2 && DialogueManager.bFinishDialogue)
        {
            ClueScript.SetText("左右移動蘑菇頭，移動莉妲");
            ClueScript.FadeIn();
            PlayerMovementScript.canMove_skill = true;
            AnimatorCount++;
        }
        
        //Give move clue
        else if (AnimatorCount == 3 &&  Mathf.Abs(Input.GetAxis("Horizontal")) > 0.0f)
        {
            ClueScript.FadeOut();
            PlayerTriggerScript.TriggerName = "Aisle"; // set player trigger detect object name
            AnimatorCount++;
        }


        //Vita follow Rita 
        if (AnimatorCount >= 4)
        {
            VitaMovementScript.FollowObj();
            
        }

    }
    
    IEnumerator BookCaseSwap(float AnimateTime)
    {
        // AnimateTime = X_Range*FrameWaitTime/X_Movement 
        float FrameWaitTime = 0.01f;
        float X_Traget = -8.2f;
        float X_Range = X_Traget - BookCase.transform.position.x;
        float X_Movement = (X_Range * FrameWaitTime) / AnimateTime ;
        yield return new WaitForSeconds(1.8f); //wait for transition
        for (; BookCase.transform.position.x > X_Traget; )
        {
            BookCase.transform.position = new Vector2(BookCase.transform.position.x + X_Movement, BookCase.transform.position.y);
            yield return new WaitForSeconds(FrameWaitTime);
        }


        //after book case open 
        yield return new WaitForSeconds(1.0f);
        bVitaAsk = true;
        

    }


    }
