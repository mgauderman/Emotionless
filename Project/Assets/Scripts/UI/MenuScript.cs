using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuScript : MonoBehaviour
{

    GameObject mainMenu, settingsPanel, creditsPanel, controlsPanel, headsUpGUI;
    PlayerMovement player;
    //public static int scene_number;

    // Use this for initialization
    void Start()
    {
        GameObject.DontDestroyOnLoad(gameObject);
        //scene_number = 1;
        mainMenu = GameObject.Find("MainMenu");
        mainMenu.SetActive(true);
        settingsPanel = GameObject.Find("AudioPanel");
        settingsPanel.SetActive(false);
        creditsPanel = GameObject.Find("CreditsPanel");
        creditsPanel.SetActive(false);
        controlsPanel = GameObject.Find("ControlsPanel");
        controlsPanel.SetActive(false);
        headsUpGUI = GameObject.Find("HeadsUpCanvas");
        DontDestroyOnLoad(headsUpGUI);
        SceneManager.LoadScene(1);
    }
	
    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            GameObject p = GameObject.Find("Player");
            if (p != null)
                player = p.GetComponent<PlayerMovement>();
        }
        else
        {
            if (mainMenu.activeSelf)
            {
                if (!player.isDead())
                    player.DisallowMovement();
            }
        }
    }

    public void Controls()
    {
        controlsPanel.SetActive(true);
    }

    public void Audio()
    {
        settingsPanel.SetActive(true);
    }

    public void Resume()
    {
        if (player != null)
            player.AllowMovement();
    }

    public void Exit()
    {
        #if UNITY_EDITOR
        EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
