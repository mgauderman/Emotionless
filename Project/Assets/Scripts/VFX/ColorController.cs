using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorController : MonoBehaviour
{

    private static GameObject[] walls;
    private static GameObject[] buttons;
    public static Color rw = new Color32(155, 56, 46, 255);
    // red wall
    public static Color rb = new Color32(234, 79, 58, 255);
    // red background
    public static Color bw = new Color32(48, 52, 160, 255);
    // blue wall
    public static Color bb = new Color32(114, 138, 182, 255);
    // blue background
    public static Color yw = new Color32(255, 219, 34, 255);
    // yellow wall
    public static Color yb = new Color32(255, 253, 171, 255);
    // yellow background
    public static Color ow = new Color32(255, 255, 255, 255);
    // original wall
    public static Color ob = new Color32(0, 0, 0, 255);
    // original background
    public static Color iw = new Color32(200, 200, 200, 255);
    // intermediate wall color (snaps to wall after this)
    public static Color ib = new Color32(50, 50, 50, 255);
    // intermediate background color (snaps to black after this)
    public static Color t = new Color32(0, 0, 0, 0);
    public static float fadeTimer = 0.0f;
    public static float timeActive = 0.0f;
    public static Color changeW;
    public static Color changeB;
    public const string red = "red";
    public const string blue = "blue";
    public const string yellow = "yellow";
    public static int numCoins = 0;


    // Use this for initialization
    void Start()
    {
        walls = GameObject.FindGameObjectsWithTag("Wall");
        ChangeColor(ow, ob);
        reset();
    }
	
    // Update is called once per frame
    void FixedUpdate()
    {
        if (fadeTimer > 0.0f)
        {
            fadeTimer -= Time.deltaTime;
            if (fadeTimer > 0)
            {
                // This first section is to make the background/walls/paths fade
                float rwall = Mathf.Lerp(changeW.r, ColorController.ow.r, 1.0f - (fadeTimer / timeActive)); // these three are for walls
                float gwall = Mathf.Lerp(changeW.g, ColorController.ow.g, 1.0f - (fadeTimer / timeActive));
                float bwall = Mathf.Lerp(changeW.b, ColorController.ow.b, 1.0f - (fadeTimer / timeActive));
                float rback = Mathf.Lerp(changeB.r, ColorController.ob.r, 1.0f - (fadeTimer / timeActive)); // these three are for background
                float gback = Mathf.Lerp(changeB.g, ColorController.ob.g, 1.0f - (fadeTimer / timeActive));
                float bback = Mathf.Lerp(changeB.b, ColorController.ob.b, 1.0f - (fadeTimer / timeActive));

                Color wallc = new Color(rwall, gwall, bwall, 255);
                Color backgroundc = new Color(rback, gback, bback, 255);

                ColorController.ChangeColor(wallc, backgroundc); // change the color of everything else
            }
            else
            {
                ColorController.ChangeColor("default");
            }
        }
        else
        {
            ColorController.ChangeColor("default");
        }
    }

    public static void ChangeColor(string name)
    {
        Color wall = new Color();
        Color background = new Color();
        switch (name)
        {
            case red:
                wall = rw;
                background = rb;
                break;
            case blue:
                wall = bw;
                background = bb;
                break;
            case yellow:
                wall = yw;
                background = yb;
                break;
            case "default":
                wall = ow;
                background = ob;
                break;
        }
        //ChangeColor(wall, background);
    }

    public static void ChangeColor(Color wall, Color background)
    {
        foreach (GameObject w in walls)
        {
            w.GetComponent<SpriteRenderer>().color = wall;
        }
        Camera.main.backgroundColor = background;
    }

    public void reset()
    {
        fadeTimer = 0.0f;
        timeActive = 0.0f;
    }
}