using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeadsUpGUI : MonoBehaviour
{
    private static GameObject collection, pauseMenu;
    private static Text score;
    private GameObject dialogPanel, controlsPanel;
    private Text dialog;
    private static Text controls;
    private int level;
    private GameObject player;
    private float playerX, initialX;
    private bool created;

    void Awake()
    {
        if (!created)
        {
            DontDestroyOnLoad(this.gameObject);
            created = true;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Use this for initialization
    void Start()
    {
        pauseMenu = GameObject.Find("PauseMenu");
        pauseMenu.SetActive(false);
        collection = GameObject.Find("Collection");
        score = collection.GetComponentInChildren<Text>();
        if (ColorController.numCoins > 0)
        {
            score.text = ColorController.numCoins.ToString();
            collection.SetActive(true);
        }
        else
            collection.SetActive(false);
       
        dialogPanel = GameObject.Find("DialogPanel");
        dialog = GameObject.Find("Dialog").GetComponent<Text>() as Text;
        dialogPanel.SetActive(false);
        controlsPanel = GameObject.Find("ControlsPanel (1)");
        controlsPanel.SetActive(false);
        controls = GameObject.Find("ControlsText").GetComponent<Text>() as Text;
        controls.enabled = false;
        setLevel(1);
    }
	
    // Update is called once per frame
    void Update()
    {        
        if (player == null)
        {
            player = GameObject.Find("Player");
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
            if (pauseMenu.activeSelf)
                player.GetComponent<PlayerMovement>().DisallowMovement();
            else
                player.GetComponent<PlayerMovement>().AllowMovement();
        }


        //dialog window
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (dialogPanel.activeSelf)
            {
                dialogPanel.SetActive(false);
                player.GetComponent<PlayerMovement>().AllowMovement();
            }
        }
    }

    public void showDialog(string text)
    {
        //Debug.Log("Level " + level + ", trigger " + num);
        //if (!(level >= 0 && level < lines.Count && num >= 0 && num < lines[level].Count))
        //{
        //    Debug.Log("Something went wrong with showing dialog! Level " + level + ", trigger " + num);
        //    return;
        //}
        //string toDisplay = lines[level][num].getCurrentString();
        if (text != null)
        {
            dialog.text = text;
            player.GetComponent<PlayerMovement>().DisallowMovement();
            dialogPanel.SetActive(true);
        }
    }

    public static void DisplayControls(string text)
    {
        if (text != null)
        {
            controls.text = text;
            controls.enabled = true;
        }
    }

    public static void HideControls()
    {
        if (controls.enabled)
            controls.enabled = false;
    }

    public static void UpdateScore()
    {
        ColorController.numCoins++;
        score.text = ColorController.numCoins.ToString();
        if (ColorController.numCoins >= 1)
        {
            collection.SetActive(true);
        }
    }

    public static int getScore()
    {
        return ColorController.numCoins;
    }

    public void setLevel(int level)
    {
        this.level = level - 1;
        player = null;
    }
}






//    private readonly List<List<Dialog>> lines =
//        new List<List<Dialog>>
//        {
//            new List<Dialog> // level 1
//            {
//                new Dialog("Welcome to the world I live in."),
//                new Dialog("I’ve never really felt ANYTHING before."),
//                new Dialog("I’ve been here for a long time, just standing, walking through halls, not really getting anywhere."),
//                new Dialog("I need to get out. I can’t stand being here much longer, empty hallways and dead ends everywhere I turn."),
//                new Dialog("But how? How can I escape?"),
//                new Dialog("What is this? It hovered and spun, and I took it."),
//                new Dialog("I felt a rush of something inside of me. I should collect more of these!"),
//                new Dialog(false, "Where is this blinking area taking me?", "I didn't make it across the path, and I got sent back! I should try that again."),
//                new Dialog(false, "This was new. This was a change. Something fluttered inside me."),
//                new Dialog(true, "So many dead ends. I feel like I have no purpose.")
//            },
//            new List<Dialog> // happy 1
//            {
//                new Dialog("Look! More of those spinny things that let me feel. And another one of those flashing areas, where to now?")
//            },
//            new List<Dialog> // happy 2
//            {
//                new Dialog(false, "Oh no! How can I go forward with this pit of despair in the way?"),
//                new Dialog(false, "I feel so...happy! I just want to jump! (try pressing 'SPACE')")
//            },
//            new List<Dialog> // happy 3
//            {
//                new Dialog(false, "It seems like I can only jump while I am happy (until the background fades)"),
//                new Dialog(false, "Whenever I fall down one of these pits or get stuck in a hidden path, I think I respawn at the last flashing area I touched, so I need to be careful!")
//            },
//            new List<Dialog> // happy 4
//            {
//                new Dialog(false, "Even though the emotion is over...I still feel happy...is this permanent?")
//            },
//            new List<Dialog>
//            { // main place
//                //new Dialog()
//            },
//            new List<Dialog> // sad 1
//            {
//                new Dialog(false, "What is this? This isn't happy...this feels like the opposite...maybe that's why I am small?")
//            },
//            new List<Dialog> // sad 2
//            {
//                //new Dialog()
//            },
//            new List<Dialog> // sad 3
//            {
//                new Dialog(false, "I feel empowered...I feel like I think I can now control this 'sadness' (try pressing 'K')")
//            },
//            new List<Dialog> // angry 1
//            {
//                new Dialog(false, "WHAT THE HECK IS THIS, I DIDN'T ASK TO FEEL LIKE THIS, I JUST WANT TO BREAK THINGS (try pressing 'J')")
//            },
//            new List<Dialog> // angry 2
//            {
//                //new Dialog()
//            },
//            new List<Dialog> // angry 3
//            {
//                new Dialog(false, "I've finally broken free of this endless maze...and I have emerged a new being. I can feel and control my emotions. I can't believe I am finally free! <3")
//            }
//
//        };
//
//    public class Dialog
//    {
//        public List<string> strs { get; }
//
//        private int current;
//
//        private bool repeatLast;
//
//        public Dialog(params string[] text)
//        {
//            strs = new List<string>();
//            for (int i = 0; i < text.Length; i++)
//            {
//                strs.Add(text[i]);
//            }
//            //repeatLast = strs.Count < 2;
//            repeatLast = false;
//            current = 0;
//        }
//
//        public Dialog(bool rep, params string[] text)
//            : this(text)
//        {
//            repeatLast = rep;
//        }
//
//        public string getCurrentString()
//        {
//            if (current < strs.Count)
//            {
//                current++;
//                return strs[current - 1];
//            }
//            else if (repeatLast)
//                return strs[strs.Count - 1];
//            else
//                return null;
//        }
//
//        public void addString(string s)
//        {
//            strs.Add(s);
//        }
//    }