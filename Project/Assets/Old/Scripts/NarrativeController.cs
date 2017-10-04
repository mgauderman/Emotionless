using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NarrativeController : MonoBehaviour
{
    /*public struct pair
    {
        public int x;
        public string line;

        public pair(int p1, string p2)
        {
            x = p1;
            line = p2;
        }
    }

    private Text text;
    private GameObject player;
    int pos;
    private pair[] story =
        {
            new pair(1, "You are walking around campus with your friend that you \n" +
                "have had a HUGE crush on ever since the start  of the year. Like \n" +
                "seriously they are the CUTEST person you have ever met and just \n" +
                "are everything you have ever wanted."),
            new pair(2, "two"),
            new pair(3, "three"),
            new pair(4, "four"),
            new pair(5, "five"),
            new pair(6, "six"),
            new pair(7, "seven"),
            new pair(8, "eight"),
            new pair(80, "nine"),
            new pair(90, "ten")
        };
    
    // Use this for initialization
    void Start()
    {
        text = GetComponent<Text>();
        player = GameObject.FindGameObjectWithTag("Player");
        pos = 0;


        /*
        foreach (var p in story)
        {
            text.text = p.line;
            //this is your object that you want to have the UI element hovering over
            GameObject WorldObject;
 
            //this is the ui element
            RectTransform UI_Element;
 
            //first you need the RectTransform component of your canvas
            RectTransform CanvasRect = Canvas.GetComponent<RectTransform>();
 
            //then you calculate the position of the UI element
            //0,0 for the canvas is at the center of the screen, whereas WorldToViewPortPoint treats the lower left corner as 0,0. Because of this, you need to subtract the height / width of the canvas * 0.5 to get the correct position.
 
            Vector2 ViewportPosition = GetComponent<Camera>().WorldToViewportPoint(WorldObject.transform.position);
            Vector2 WorldObject_ScreenPosition = new Vector2(
                                                     ((ViewportPosition.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
                                                     ((ViewportPosition.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)));
 
            //now you can set the position of the ui element
            UI_Element.anchoredPosition = WorldObject_ScreenPosition;
            Instantiate(text, spawnPosition, Quaternion.identity);
        }
        */
    //}
	
    // Update is called once per frame
    // don't need this because can show text in-game
    //void Update()
    //{
    //    pair curr = story[pos];
    //    if (player)
    //    {
    //        if (player.transform.position.x >= curr.x)
    //        {
    //            text.text = curr.line;
    //            pos++;
    //        }
    //    }
    //}
}
