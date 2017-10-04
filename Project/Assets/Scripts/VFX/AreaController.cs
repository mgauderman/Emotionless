using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaController : MonoBehaviour
{
    public bool isRed, isBlue, isYellow;
    public float timeActive;

    private string color;

    private Color changeW;
    // color walls will change when go on this area
    private Color changeB;
    // color background will change when on this area

    //private AudioSource a;

    private bool isFading;
    private float fadeTimer;

    private bool onArea;
    private bool firstTime;
    // is true only if the player has never activated this area before
    //  (important cuz it will zoom out if they haven't)
    private float firstTimer;
    private float ft;
    // starting amount of firstTimer
    private Vector3 camPos;
    private GameObject player;
    private bool nextLevel;

    private bool isZoomedOut;

    void Start()
    {
        //a = GetComponent<AudioSource>();

        if (isYellow)
        {
            changeW = ColorController.yw;
            changeB = ColorController.yb;
            color = "yellow";
            ColorController.changeW = ColorController.yw;
            ColorController.changeB = ColorController.yb;
        }
        else if (isBlue)
        {
            changeW = ColorController.bw;
            changeB = ColorController.bb;
            color = "blue";
            ColorController.changeW = ColorController.bw;
            ColorController.changeB = ColorController.bb;
        }
        else
        {
            changeW = ColorController.rw;
            changeB = ColorController.rb;
            color = "red";
            ColorController.changeW = ColorController.rw;
            ColorController.changeB = ColorController.rb;
        }
        setActive(false);

        isFading = false;
        fadeTimer = timeActive;

        // these deal with zooming out on first time on this area
        firstTime = true;
        firstTimer = 4.0f;
        ft = firstTimer;
        onArea = false;



        isZoomedOut = false;

        gameObject.GetComponent<SpriteRenderer>().color = changeW;
        player = GameObject.Find("Player");
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.name == "WallColorChanger") // This is where the player enters an emotional state after entering an area
        {
            BoxCollider2D cb = col.GetComponent<BoxCollider2D>();
            if (cb.bounds.max.x - 0.25f < GetComponent<BoxCollider2D>().bounds.max.x &&
                cb.bounds.max.y - 0.25f < GetComponent<BoxCollider2D>().bounds.max.y &&
                cb.bounds.min.x + 0.25f > GetComponent<BoxCollider2D>().bounds.min.x &&
                cb.bounds.min.y + 0.25f > GetComponent<BoxCollider2D>().bounds.min.y)
            {
                //Debug.Log("Setting on area = true");
                onArea = true;
            }

            if (firstTimer <= 2.0f)
            {
                ColorController.ChangeColor(color);
                col.gameObject.GetComponentInParent<AnimationController>().SetHat(color);
                setActive(true);
                isFading = false;
                changeState(true);
                //col.gameObject.GetComponentInParent<PlayerMovement>().setRespawn(transform.position);
                col.gameObject.transform.parent.GetComponent<PlayerMovement>().setRespawn(transform.position + new Vector3(1.0f, -1.0f, 0.0f));
            }
            ///a.Play();
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (firstTimer <= 0.0f)
        {
            onArea = false;
            if (col.gameObject.name == "WallColorChanger")
            {
                isFading = true;
                MetricManager.AddToAreaCount(gameObject.name, gameObject.transform.position.x, gameObject.transform.position.y);
            }
        }
        HeadsUpGUI.HideControls();
    }

    void setActive(bool active)
    {
        if (active)
        {
            if (ColorController.fadeTimer < timeActive)
            {
                ColorController.fadeTimer = timeActive;
                ColorController.timeActive = timeActive;
            }
        }
        foreach (Transform child in transform)
        {
            if (child.gameObject.CompareTag("Path"))
            {
                foreach (Transform child2 in child.transform)
                {
                    if (active)
                    {
                        if (child2.gameObject.CompareTag("Path"))
                        {
                            child2.gameObject.GetComponent<SpriteRenderer>().color = changeB;
                            foreach (Transform child3 in child2.transform)
                            {
                                child3.gameObject.GetComponent<SpriteRenderer>().color = changeB;
                            }
                        }
                        else if (child2.gameObject.CompareTag("HiddenWall"))
                        {
                            child2.gameObject.GetComponent<SpriteRenderer>().color = ColorController.t;
                            child2.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
                        }
                    }
                    else
                    {
                        if (child2.gameObject.CompareTag("Path"))
                        {
                            child2.gameObject.GetComponent<SpriteRenderer>().color = ColorController.t;
                            foreach (Transform child3 in child2.transform)
                            {
                                child3.gameObject.GetComponent<SpriteRenderer>().color = ColorController.t;
                            }
                            if (child2.GetComponent<HiddenPath>().playerIsTrapped()) // if the player is trapped in a hidden path at end of timer
                            {
                                player.GetComponent<PlayerMovement>().respawn();
                                child2.GetComponent<HiddenPath>().setTrapped(false);
                            }
                        }
                        else if (child2.gameObject.CompareTag("HiddenWall"))
                        {
                            child2.gameObject.GetComponent<SpriteRenderer>().color = ColorController.ow;
                            child2.gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
                        }
                    }
                }
            }
        }
    }

    void Update()
    {
        if (onArea)
        {
            //Debug.Log("Inside update, firstTime: " + firstTime);
        }
       
        if (!firstTime && onArea)
        {
            HeadsUpGUI.DisplayControls("Press F to Zoom Out");
            if (Input.GetKeyDown(KeyCode.F))
            {
                GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
                if (firstTimer < 0.0f && !isZoomedOut)
                {
                    camera.GetComponent<Camera>().orthographicSize = 25;
                    isZoomedOut = true;
                    player.GetComponent<PlayerMovement>().setCanMove(false, true);
                }
                else if (firstTimer < 0.0f && isZoomedOut)
                {
                    camera.GetComponent<Camera>().orthographicSize = 6.5f;
                    isZoomedOut = false;
                    player.GetComponent<PlayerMovement>().setCanMove(true, false);
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (player == null)
        {
            player = GameObject.Find("player");
        }
        if (player.GetComponent<PlayerMovement>().CustomCamera())
        {
            firstTime = false;
            firstTimer = 0.0f;
        }
        //Debug.Log("right before if");
        if (firstTime && onArea) // if the player is going on this area for the first time then zoom out and in over firstTimer seconds
        {
            //Debug.Log("right after if");
            if (firstTimer == ft)
            {
                player.GetComponent<PlayerMovement>().setCanMove(false, true); // freeze player until camera is done zooming out and in
                camPos = GameObject.FindGameObjectWithTag("MainCamera").transform.position;
            }
            firstTimer -= Time.deltaTime;
            if (firstTimer >= 3.0f) // zoom out
            {
                float oSize = Mathf.Lerp(6.5f, 30.0f, (1 - (firstTimer - 3.0f)) / 1.0f);
                float cPosx = Mathf.Lerp(camPos.x, transform.position.x, (1 - (firstTimer - 3.0f)) / 1.0f);
                float cPosy = Mathf.Lerp(camPos.y, transform.position.y, (1 - (firstTimer - 3.0f)) / 1.0f);
                GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
                camera.transform.position = new Vector3(cPosx, cPosy, camPos.z);
                camera.GetComponent<Camera>().orthographicSize = oSize;
            }
            else if (firstTimer <= 0.0f)
            {
                player.GetComponent<PlayerMovement>().setCanMove(true, false); // freeze player until camera is done zooming out and in
                firstTime = false;
            }
            else if (firstTimer < 1.0f)
            {
                float oSize = Mathf.Lerp(30.0f, 6.5f, (1 - (firstTimer)) / 1.0f);
                float cPosx = Mathf.Lerp(transform.position.x, camPos.x, (1 - (firstTimer)) / 1.0f);
                float cPosy = Mathf.Lerp(transform.position.y, camPos.y, (1 - (firstTimer)) / 1.0f);
                GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
                camera.transform.position = new Vector3(cPosx, cPosy, camPos.z);
                camera.GetComponent<Camera>().orthographicSize = oSize;
            }
        }

        if (isFading)
        {
            fadeTimer -= Time.deltaTime;

            // This first section is to make the background/walls/paths fade
            float rp = Mathf.Lerp(changeB.r, ColorController.ob.r, 1.0f - (fadeTimer / timeActive)); // these three are for hidden paths
            float gp = Mathf.Lerp(changeB.g, ColorController.ob.g, 1.0f - (fadeTimer / timeActive));
            float bp = Mathf.Lerp(changeB.b, ColorController.ob.b, 1.0f - (fadeTimer / timeActive));
            Color pathc = new Color(rp, gp, bp, 255); // only do this if you want the path to fade 

            foreach (Transform child in transform) // change the color of the hidden paths for this area
            {
                if (child.gameObject.CompareTag("Path"))
                {
                    foreach (Transform child2 in child.transform)
                    {
                        if (child2.gameObject.CompareTag("Path"))
                        {
                            child2.gameObject.GetComponent<SpriteRenderer>().color = pathc;
                            foreach (Transform child3 in child2.transform)
                            {
                                child3.gameObject.GetComponent<SpriteRenderer>().color = pathc;
                            }
                        }
                        //else if (child2.gameObject.CompareTag("HiddenWall"))
                        //{
                        //    child2.gameObject.GetComponent<SpriteRenderer>().color = pathc;
                        //}

                    }
                }
            }

            //ColorController.ChangeColor(wallc, backgroundc); // change the color of everything else

            // This section is when the timer runs out (when you exit the emotional state
            if (fadeTimer <= 0)
            {
                isFading = false;
                fadeTimer = timeActive;
                setActive(false);
                //ColorController.ChangeColor("default");
                if (ColorController.fadeTimer <= 0.0f)
                {
                    FindObjectOfType<AnimationController>().SetHat("none");
                    changeState(false);
                }
            }
        }
        else
        {
            fadeTimer = timeActive;
        }
    }

    public static void LevelChange()
    {
        //LevelChange() = true;
    }

    void changeState(bool enterState)
    {
        if (isYellow)
        {
            player.GetComponent<StateChanger>().isHappy = enterState;
        }
        else if (isBlue)
        {
            player.GetComponent<StateChanger>().isSad = enterState;
        }
        else if (isRed)
        {
            player.GetComponent<StateChanger>().isAngry = enterState;
        }
    }
}

//    public bool isRed, isBlue, isYellow, isActive;
//    private string n;
//    private Color c = new Color32();
//    private Color b = new Color32();
//    private Color w = new Color32();
//    private static Color r = new Color32(100, 9, 0, 255);
//    private static Color redb = new Color32(160, 14, 0, 255);
//    private static Color bl = new Color32(33, 35, 89, 255);
//    private static Color blueb = new Color32(44, 47, 121, 255);
//    private static Color y = new Color32(153, 131, 20, 255);
//    private static Color yellowb = new Color32(204, 175, 27, 255);
//    private bool isFadingBack = false;
//    private AudioSource a;
//    private const int red = 0, yellow = 1, blue = 2;
//    // Use this for initialization
//    void Start()
//    {
//        a = GetComponent<AudioSource>();
//        if (isYellow)
//        {
//            n = ColorController.yellow;
//            c = y;
//            b = yellowb;
//            w = ColorController.y;
//        }
//        if (isRed)
//        {
//            n = ColorController.red;
//            c = r;
//            b = redb;
//            w = ColorController.r;
//        }
//        if (isBlue)
//        {
//            n = ColorController.blue;
//            c = bl;
//            b = blueb;
//            w = ColorController.b;
//        }
//        gameObject.GetComponent<SpriteRenderer>().color = c;
//        SetButtonsActive(false);
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (isFadingBack)
//        {

//        }
//    }

//    void OnTriggerEnter2D(Collider2D col)
//    {
//        if (col.gameObject.name == "WallColorChanger")
//        {
//            setActive();
//            a.Play();
//            ColorController.ChangeColor(n);
//            SetButtonsActive(true);
//        }
//    }

//    void OnTriggerExit2D(Collider2D col)
//    {
//        if (col.gameObject.name == "WallColorChanger")
//        {
//            isFadingBack = true;
//        }
//    }

//    void SetButtonsActive(bool active)
//    {
//        foreach (Transform child in transform)
//        {
//            if (child.gameObject.name == "Button")
//            {
//                child.gameObject.SetActive(active);
//                child.gameObject.GetComponent<SpriteRenderer>().color = b;
//            }
//            else
//            {
//                if (!active)
//                {
//                    child.gameObject.GetComponent<SpriteRenderer>().color = w;
//                }
//                else
//                {
//                    child.gameObject.GetComponent<SpriteRenderer>().color = c;
//                    //child.gameObject.GetComponent<BoxCollider2D>().enabled = false;
//                }
//            }
//        }
//    }

//    void setActive()
//    {
//        foreach (AreaController a in FindObjectsOfType<AreaController>())
//        {
//            if (!this.Equals(a))
//                a.SetButtonsActive(false);
//        }
//    }
//}
