using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrativeTrigger : MonoBehaviour
{
    [TextArea(3, 10)]
    public string text;

    private int triggerNum;
    HeadsUpGUI h;

    private bool shown;

    private bool showAgain;

    private int counter;

    // Use this for initialization
    void Start()
    {
        string name = gameObject.name;
        string f = name.Substring(name.IndexOf("(") + 1, 1);
        shown = false;
        //int.TryParse(f, out triggerNum);
        //h = GameObject.FindObjectOfType<HeadsUpGUI>() as HeadsUpGUI;
        showAgain = false;
        counter = 0;
    }

    void Update()
    {
        if (h == null)
        {
            h = GameObject.FindObjectOfType<HeadsUpGUI>() as HeadsUpGUI;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.name.Equals("Body"))
        {
            if (h != null && !shown)
            {
                h.showDialog(text);
                shown = true;
            }
            //else
            //Debug.Log("HeadsUpCanvas not available to show dialog. You need to start from a menu screen to have the HeadsUpCanvas available.");
        }
            
    }

    void OnTriggerStay2D(Collider2D col)
    {
        //Debug.Log("Counter: " + counter++);
        HeadsUpGUI.DisplayControls("Press T to Repeat Text");
        if (Input.GetKeyDown(KeyCode.T))
        {
            //Debug.Log("ShowAgain: " + showAgain);
            if (showAgain)
            {
                h.showDialog(text);
                showAgain = false;
            }
            else
            {
                showAgain = true;
            }
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        HeadsUpGUI.HideControls();
    }
}
