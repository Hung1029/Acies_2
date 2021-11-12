using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDistory : MonoBehaviour
{
    public static DontDistory instance;

    public static DontDistory Instance { get { return instance; } }

    [System.NonSerialized]
    public bool bSkill1Des_open = false;
    [System.NonSerialized]
    public bool bSkill1Des_finish = false;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void reset_state()
    {
        bSkill1Des_open = false;
        bSkill1Des_finish = false;

        Destroy(this.gameObject);
    }
}
