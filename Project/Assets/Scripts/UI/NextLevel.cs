using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class NextLevel : MonoBehaviour
{
    private int thisLevel, nextLevel, lastLevel;
    private static bool didSad = false;
    private string levelName;
    //private string levelName, nextLevelName;
    HeadsUpGUI h;
    MetricManager m;
    MusicManager mu;

    #if UNITY_EDITOR
    private readonly KeyCode[] keyCodes =
        {
            KeyCode.Alpha1,
            KeyCode.Alpha2,
            KeyCode.Alpha3,
            KeyCode.Alpha4,
            KeyCode.Alpha5,
            KeyCode.Alpha6,
            KeyCode.Alpha7,
            KeyCode.Alpha8,
            KeyCode.Alpha9,
            KeyCode.Alpha0
        };

    private static readonly string[] sceneNames =
        {
            "Alpha_Happy_Lvl1",
            "Alpha_Happy_2",
            "Gold_Happy_3",
            "Alpha_Happy_4",
            "Alpha_Sad_1",
            "Alpha_Sad_2",
            "Alpha_Sad_3",
            "Alpha_Angry_1",
            "Alpha_Angry_2",
            "Alpha_Angry_3",
            "Alpha_Scene_1",
            "Main_Area"
        };
    #endif

    // Use this for initialization
    void Start()
    {
        thisLevel = SceneManager.GetActiveScene().buildIndex;
        levelName = SceneManager.GetActiveScene().name;
        nextLevel = thisLevel + 1;
        lastLevel = thisLevel - 1;
        if (thisLevel == 6)// 6 is main menu thing
        {
            if (gameObject.name.Contains("Happy")) //the first happy level is 2
                nextLevel = 2;
            else if (gameObject.name.Contains("Angry")) //first angry level is 10
                nextLevel = 10;
        }
        else if (thisLevel == 9) //if we're at the last sad level, we want to go back to the menu
        {
            nextLevel = 6;
            didSad = true;
        }
        else if (thisLevel == 10) //if we're at the first angry level, our previous level should be 6
            lastLevel = 6;
    }
	
    // Update is called once per frame
    void Update()
    {
        if (h == null)
        {
            h = GameObject.FindObjectOfType<HeadsUpGUI>();
            if (h != null)
            {
                //Debug.Log(thisLevel + " sent to headsupgui");
                h.setLevel(thisLevel);
            }
        }

        if (m == null)
        {
            m = GameObject.FindObjectOfType<MetricManager>();
        }
        if (mu == null)
        {
            mu = GameObject.FindObjectOfType<MusicManager>();
        }

        float time = 0f;
        if (Input.anyKeyDown)
            time = Time.timeSinceLevelLoad;

        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            //go forward a level
            time = Time.timeSinceLevelLoad;
            if (nextLevel < SceneManager.sceneCountInBuildSettings)
            {
                LoadLevel(nextLevel, 1, time);
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            //go back a level
            time = Time.timeSinceLevelLoad;
            if (lastLevel > 0)
            {
                LoadLevel(lastLevel, -1, time);
            }
        }

        #if UNITY_EDITOR
        for (int i = 0; i < keyCodes.Length; i++)
        {
            if (Input.GetKeyDown(keyCodes[i]))
            {
                string s = sceneNames[i];
                if (i == (keyCodes.Length - 1))
                {
                    Debug.Log("You will not be able to shortcut to other levels from here.");
                }
                LoadLevel(s, 2, time);
            }
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            string s = sceneNames[sceneNames.Length - 2];
            LoadLevel(s, 2, time);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            string s = sceneNames[sceneNames.Length - 1];
            LoadLevel(s, 2, time);
        }
        #endif
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            float time = Time.timeSinceLevelLoad;
            if (nextLevel < SceneManager.sceneCountInBuildSettings)
            {
                LoadLevel(nextLevel, 0, time);
            }
            //while (SceneManager.sceneLoaded)
        }
    }

    void LoadLevel(int level, int skip, float time)
    {
        //Debug.Log("load level");
        SetLevel(skip, level, time);
        SceneManager.LoadScene(level);
    }

    void LoadLevel(string s, int skip, float time)
    {
        //Debug.Log("load level by name");
        SceneManager.LoadScene(s);
        int level = SceneManager.GetSceneByName(s).buildIndex;
        SetLevel(skip, level, time);
    }

    void SetLevel(int skip, int level, float time)
    {
        //Debug.Log("set level");
        m.setLevel(skip, level, time, levelName);
        h.setLevel(level);
        ColorController.ChangeColor("default");
        AreaController.LevelChange();
    }
}
