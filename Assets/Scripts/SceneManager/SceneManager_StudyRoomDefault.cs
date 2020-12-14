using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager_StudyRoomDefault : MonoBehaviour
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
    private GameObject VitaSoul;
    private VitaSoul_particle VitaSoulScript;

    [SerializeField]
    private GameObject Clue;
    private Clue ClueScript;

    void Awake()
    {
        //If never enter Study Room, don't use this script
        if (GameDataManager.instance.bEnterStudyRoom)
            this.enabled = true;
        

    }
    // Start is called before the first frame update
    void Start()
    {
        //control player movement
        PlayerMovementScript = Player.GetComponent<PlayerMovement>();
        PlayerTriggerScript = Player.GetComponent<PlayerTriggerDetect>();

        //control Vita dialogue , movement
        VitaDialogueScript = Vita.GetComponent<NPC_Dialogue>();
        VitaMovementScript = Vita.GetComponent<Vita_Movement>();

        //controlChangeScene
        levelLoaderScript = levelLoader.GetComponent<LevelLoader>();

        //control Vita Soul
        VitaSoulScript = VitaSoul.GetComponent<VitaSoul_particle>();


        //Defluat setting
        BookCase.transform.position = new Vector2(-8.2f, BookCase.transform.position.y);

        Player.transform.position = new Vector2(-3.58f, -1.89f);

        Vita.SetActive(false);
        VitaSoul.SetActive(true);

        VitaSoulScript.MoveToward(new Vector2(VitaSoul.transform.position.x, -0.54f));
        VitaSoulScript.MoveSpeed = 7.0f;

        //control Clue
        ClueScript = Clue.GetComponent<Clue>();


        VitaSoulScript.VitaSpriteFadeIn();
    }

    // Update is called once per frame
    void Update()
    {
        VitaSoulScript.FollowObj();
       

    }
}
