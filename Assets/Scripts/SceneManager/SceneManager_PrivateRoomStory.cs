using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager_PrivateRoomStory : MonoBehaviour
{
     [SerializeField]
    private GameObject Player;
    public Sprite PlayerCV;
    private Animator PlayerAni;
    private PlayerMovement PlayerMovementScript;
    private PlayerTriggerDetect PlayerTriggerScript;
    private Transform PlayerTrans;
    private float TransSpeed = 3.5f;

    [SerializeField]
    private GameObject Vita;
    public Sprite VitaCV;
    private Transform VitaTrans;
    private NPC_Dialogue VitaDialogueScript;
    private Vita_Movement VitaMovementScript;

    private Dialogue Dialogue;
   
    private int AnimatorCount = 0;

    [SerializeField]
    private GameObject Box;
    private SpriteRenderer BoxRenderer;
    [SerializeField]
    private Sprite Box_close;

    [SerializeField]
    private GameObject VitaSoul;
    private VitaSoul_particle VitaSoulScript;

    //magice Wound
    [SerializeField]
    private GameObject MagicWound;
    private Transform MagicWoundTrans;
    private MagicWoundMovement MagicWoundScript;

    [SerializeField]
    private GameObject levelLoader;
    private LevelLoader levelLoaderScript;

    //UI set
    [SerializeField]
    private GameObject CGMove;
    private VitaCGMovement CGMoveScript;

    // Start is called before the first frame update
    void Start()
    {
        //Have ever Enter
        GameDataManager.instance.bEnterPrivateRoom = true;

        //control Vita dialogue , movement
        VitaTrans = Vita.GetComponent<Transform>();
        VitaDialogueScript = Vita.GetComponent<NPC_Dialogue>();
        VitaMovementScript = Vita.GetComponent<Vita_Movement>();

        //control Player
        PlayerTrans = Player.GetComponent<Transform>();
        PlayerMovementScript = Player.GetComponent<PlayerMovement>();
        PlayerTriggerScript = Player.GetComponent<PlayerTriggerDetect>();
        PlayerAni = Player.GetComponent<Animator>();

        //new dialogue
        Dialogue = new Dialogue();

        //box 
        BoxRenderer = Box.GetComponent<SpriteRenderer>();

        //magic wound
        MagicWoundScript = MagicWound.GetComponent<MagicWoundMovement>();
        MagicWoundTrans = MagicWound.GetComponent<Transform>();

        //control Vita Soul
        VitaSoulScript = VitaSoul.GetComponent<VitaSoul_particle>();

        //controlChangeScene
        levelLoaderScript = levelLoader.GetComponent<LevelLoader>();

        //UI
        CGMoveScript = CGMove.GetComponent<VitaCGMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if(AnimatorCount <=5)
            VitaMovementScript.DetectTurnFace();

        //wait for scene transition
        if (AnimatorCount == 0)
        {
           StartCoroutine(AfterSceneTransistion());
           PlayerMovementScript.canMove_skill = false;
        }
       
        //player move in scene
        if(AnimatorCount == 1)
        {
            PlayerTrans.position = PlayerTrans.position + new Vector3(5.0f * Time.deltaTime, 0.0f,0.0f);
            PlayerAni.SetFloat("Speed", 1.0f);
            StartCoroutine(PlayerMoveInScene());
        }

        //player can move and wait to trigger
        else if (AnimatorCount == 2)
        {
            if (PlayerTriggerScript.bTrigger) //if trigger StoryTrigger
            {
                AnimatorCount++;
            }
        }

        //set player's and vita's position
        else if (AnimatorCount == 3)
        {
            
            PlayerMovementScript.canMove_skill = false;
            PlayerTrans.position = PlayerTrans.position + new Vector3(TransSpeed * Time.deltaTime, 0.0f, 0.0f);
           
            if (PlayerTrans.position.x >= 1.29f)
            {
                PlayerMovementScript.TurnFace();
                PlayerAni.SetFloat("Speed", 0.0f); //stop runing animator
                AnimatorCount++;
            }
        }

        //start dialogue
        else if(AnimatorCount == 4)
        {
            Dialogue.name = "薇妲";
            Dialogue.sentences = new string[2];
            Dialogue.sentences[0] = "欸？";
            Dialogue.sentences[1] = "這裡是哪裡呀？太酷了吧";
            CGMoveScript.SetRecTransformX(62.98596f);
            FindObjectOfType<DialogueManager>().StartDialogue(Dialogue, VitaCV);
            AnimatorCount++;
        }

        else if (AnimatorCount == 5 && DialogueManager.bFinishDialogue)
        {
            Dialogue.name = "莉妲";
            Dialogue.sentences = new string[1];
            Dialogue.sentences[0] = "是爺爺的密室嗎？";
            CGMoveScript.SetRecTransformX(145.7859f);
            FindObjectOfType<DialogueManager>().StartDialogue(Dialogue, PlayerCV);
            AnimatorCount++;
        }

        else if (AnimatorCount == 6 && DialogueManager.bFinishDialogue)
        {
            Dialogue.name = "薇妲";
            Dialogue.sentences = new string[1];
            CGMoveScript.SetRecTransformX(62.98596f);
            Dialogue.sentences[0] = "哇啊！我們快來探險！莉妲你看！那是什麼？";
            FindObjectOfType<DialogueManager>().StartDialogue(Dialogue, VitaCV);
            AnimatorCount++;
        }

        else if (AnimatorCount == 7 && DialogueManager.bFinishDialogue)
        {
            Dialogue.name = "莉妲";
            Dialogue.sentences = new string[1];
            Dialogue.sentences[0] = "不要亂碰！要是弄壞就糟了";
            CGMoveScript.SetRecTransformX(145.7859f);
            FindObjectOfType<DialogueManager>().StartDialogue(Dialogue, PlayerCV);
            AnimatorCount++;
        }
       

        else if (AnimatorCount == 8 && DialogueManager.bFinishDialogue)
        {
            Dialogue.name = "薇妲";
            Dialogue.sentences = new string[1];
            Dialogue.sentences[0] = "欸？來不及了";
            CGMoveScript.SetRecTransformX(62.98596f);
            FindObjectOfType<DialogueManager>().StartDialogue(Dialogue, VitaCV);
            AnimatorCount++;
        }

        else if (AnimatorCount == 9 && DialogueManager.bFinishDialogue)
        {
            StartCoroutine(WaitToNextAnimate(1.0f));
        }

        //open box + MagicWoundAnimate
        else if (AnimatorCount == 10)
        {
            BoxRenderer.sprite = Box_close;
            MagicWoundScript.MagicWoundAnimate();
            AnimatorCount++;

        }

        //wait next animation
        else if (AnimatorCount == 11 && MagicWoundScript.bfinishShine)
        {
            StartCoroutine(WaitToNextAnimate(1.0f));
        }


        //set vita disappear
        else if (AnimatorCount == 12 && MagicWoundScript.bfinishShine)
        {
            Vita.SetActive(false); 

            Dialogue.name = "薇妲";
            Dialogue.sentences = new string[1];
            Dialogue.sentences[0] = "阿！！！！";
            CGMoveScript.SetRecTransformX(62.98596f);
            FindObjectOfType<DialogueManager>().StartDialogue(Dialogue, VitaCV);
            
            AnimatorCount++;
        }

        //set Dialogue
        else if (AnimatorCount == 13 && DialogueManager.bFinishDialogue)
        {
            Dialogue.name = "莉妲";
            Dialogue.sentences = new string[1];
            Dialogue.sentences[0] = "薇妲！！！";
            CGMoveScript.SetRecTransformX(145.7859f);
            FindObjectOfType<DialogueManager>().StartDialogue(Dialogue, PlayerCV);
            
            AnimatorCount++;
        }

        else if (AnimatorCount == 14 && DialogueManager.bFinishDialogue)
        {
            MagicWoundScript.MagicWoundFadeOutShine();
            AnimatorCount++;
        }

        //wait next animation
        else if (AnimatorCount == 15 && MagicWoundScript.bfinishFadeOut)
        {
            StartCoroutine(WaitToNextAnimate(0.5f));
        }

        //magicWound fallDown set
        else if (AnimatorCount == 16)
        {
            MagicWound.GetComponent<Rigidbody2D>().gravityScale = 1.0f;
            MagicWound.AddComponent<BoxCollider2D>();
            MagicWoundTrans.eulerAngles = new Vector3(0.0f, 0.0f, 53.165f );
            MagicWoundScript.ChangeShortingLayer("Player"); //stand in front of box
            AnimatorCount++;
        }

        //wait next animation
        else if (AnimatorCount == 17 && MagicWoundScript.bfinishFadeOut)
        {
            StartCoroutine(WaitToNextAnimate(2.0f));
        }

        //set Dialogue
        else if (AnimatorCount == 18)
        {
            //close magicWound collider
            MagicWound.GetComponent<BoxCollider2D>().isTrigger = true;
            MagicWound.GetComponent<Rigidbody2D>().gravityScale = 0.0f;

            Dialogue.name = "莉妲";
            Dialogue.sentences = new string[2];
            Dialogue.sentences[0] = "薇妲？你去哪了？";
            Dialogue.sentences[1] = "別跟我鬧喔！你快點出來！薇妲！";
            CGMoveScript.SetRecTransformX(145.7859f);
            FindObjectOfType<DialogueManager>().StartDialogue(Dialogue, PlayerCV);

            AnimatorCount++;
        }

        else if (AnimatorCount == 19 && DialogueManager.bFinishDialogue)
        {
            Dialogue.name = "薇妲";
            Dialogue.sentences = new string[1];
            Dialogue.sentences[0] = "莉妲！";
            CGMoveScript.SetRecTransformX(62.98596f);
            FindObjectOfType<DialogueManager>().StartDialogue(Dialogue, VitaCV);

            AnimatorCount++;
        }

        else if (AnimatorCount == 20 && DialogueManager.bFinishDialogue)
        {
            Dialogue.name = "莉妲";
            Dialogue.sentences = new string[1];
            Dialogue.sentences[0] = "薇妲？你在哪裡？";
            CGMoveScript.SetRecTransformX(145.7859f);
            FindObjectOfType<DialogueManager>().StartDialogue(Dialogue, PlayerCV);

            AnimatorCount++;
        }

        else if (AnimatorCount == 21 && DialogueManager.bFinishDialogue)
        {
            Dialogue.name = "薇妲";
            Dialogue.sentences = new string[1];
            CGMoveScript.SetRecTransformX(62.98596f);
            Dialogue.sentences[0] = "這裡！我在這裡";
            FindObjectOfType<DialogueManager>().StartDialogue(Dialogue, VitaCV);

            AnimatorCount++;
        }

        else if (AnimatorCount == 22 && DialogueManager.bFinishDialogue)
        {
            StartCoroutine(WaitToNextAnimate(1.0f));
            VitaSoul.SetActive(true);            
        }

        //Vita movement
        else if (AnimatorCount == 23 && !VitaSoulScript.bMoveFinish)
        {
            VitaSoulScript.MoveToward(new Vector2(-1.75f,-0.54f));
            //if finish move toward
            if (VitaSoulScript.bMoveFinish)
            {
                VitaSoulScript.VitaSpriteFadeIn();
                AnimatorCount++;
                VitaSoulScript.bMoveFinish = false;
            }
                
        }
        else if (AnimatorCount == 24)
        {
            StartCoroutine(WaitToNextAnimate(1.0f));
        }

        //set Dialogue
        else if (AnimatorCount == 25 && DialogueManager.bFinishDialogue)
        {
            Dialogue.name = "莉妲";
            Dialogue.sentences = new string[1];
            CGMoveScript.SetRecTransformX(145.7859f);
            Dialogue.sentences[0] = "啊！薇妲？你怎麼變成這樣？";
            FindObjectOfType<DialogueManager>().StartDialogue(Dialogue, PlayerCV);

            AnimatorCount++;
        }
        
        else if (AnimatorCount == 26 && DialogueManager.bFinishDialogue)
        {
            Dialogue.name = "薇妲";
            Dialogue.sentences = new string[1];
            CGMoveScript.SetRecTransformX(62.98596f);
            Dialogue.sentences[0] = "一回過神就變成這樣啦！哈哈哈！好好玩喔";
            FindObjectOfType<DialogueManager>().StartDialogue(Dialogue, VitaCV);

            VitaSoulScript.MoveSpeed = 15.0f; //set vita soul new move speed

            AnimatorCount++;
        }

        //Vita movement
        else if (AnimatorCount == 27 && DialogueManager.bFinishDialogue &&!VitaSoulScript.bMoveFinish)
        {
            VitaSoulScript.MoveToward(new Vector2(-5.13f, 0.9f));
            //if finish move toward
            if (VitaSoulScript.bMoveFinish)
            {
                VitaSoulScript.bMoveFinish = false;
                AnimatorCount++;
            }
                
        }

        else if (AnimatorCount == 28 && !VitaSoulScript.bMoveFinish)
        {
            VitaSoulScript.MoveToward(new Vector2(4.15f, 0.96f));
            //if finish move toward
            if (VitaSoulScript.bMoveFinish)
            {
                VitaSoulScript.bMoveFinish = false;
                AnimatorCount++;
            }
        }

        else if (AnimatorCount == 29 && !VitaSoulScript.bMoveFinish)
        {
            VitaSoulScript.MoveToward(new Vector2(-6.91f, 2.89f));
            //if finish move toward
            if (VitaSoulScript.bMoveFinish)
            {
                VitaSoulScript.bMoveFinish = false;
                AnimatorCount++;
            }
        }
        else if (AnimatorCount == 30 && !VitaSoulScript.bMoveFinish)
        {
            VitaSoulScript.MoveToward(new Vector2(-1.75f, -0.54f));
            //if finish move toward
            if (VitaSoulScript.bMoveFinish)
            {
                VitaSoulScript.bMoveFinish = false;
                AnimatorCount++;
            }
        }

        //set Dialogue
        else if (AnimatorCount == 31 )
        {
            Dialogue.name = "莉妲";
            Dialogue.sentences = new string[1];
            Dialogue.sentences[0] = "別鬧了！快下來，我們去找爺爺，他一定知道發生什麼事";
            FindObjectOfType<DialogueManager>().StartDialogue(Dialogue, PlayerCV);

            VitaSoulScript.MoveSpeed = 10.0f; //set vita soul new move speed

            //set trigger name Wall_left
            PlayerTriggerScript.TriggerName = "Wall_left"; //player wait for trigger's name


            AnimatorCount++;
        }

        else if (AnimatorCount >= 32 && DialogueManager.bFinishDialogue)
        {
            PlayerMovementScript.canMove_skill = true;
            VitaSoulScript.FollowObj();
            if (PlayerTriggerScript.bTrigger)
            {
                levelLoaderScript.LoadNextLevel("StudyRoom");
            }
            
        }

        

    }

    IEnumerator AfterSceneTransistion()
    {
        yield return new WaitForSeconds(2.0f);
        AnimatorCount++;
        StopAllCoroutines();
    }


    IEnumerator PlayerMoveInScene()
    {
        yield return new WaitForSeconds(0.8f);
        AnimatorCount++;
        PlayerMovementScript.canMove_skill = true;
        PlayerTriggerScript.TriggerName = "StoryTrigger"; //player wait for trigger's name
        StopAllCoroutines();
    }

    IEnumerator WaitToNextAnimate(float time)
    {
        yield return new WaitForSeconds(time);
        AnimatorCount++;
        StopAllCoroutines();
    }

   

}
