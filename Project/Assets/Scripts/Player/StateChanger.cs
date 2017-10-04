using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateChanger : MonoBehaviour
{

    public bool isHappy;
    public bool isSad;
    public bool isAngry;

    public const int Happy = 20;
    public const int Sad = 20;
    public const int Angry = 20;

    private bool isReallySad;

    void Start()
    {
        if( isSad )
        {
            isSad = false;
            isReallySad = true;
        }
        else
        {
            isReallySad = false;
        }
    }

    public bool isQuiteSad()
    {
        return isReallySad;
    }

    void Update()
    {
        if( Input.GetKeyDown(KeyCode.K) && isReallySad ) { // when player presses K after they have finished sad world, can be small at will
            if( isSad )
            {
                isSad = false;
            }
            else
            {
                isSad = true;
            }
        }
        int score = HeadsUpGUI.getScore();
        //isHappy = (score >= (int)(Happy * 0.75));
        //isSad = (score >= (int)((Happy * 0.75) + (Sad * 0.75)));
        //isAngry = (score >= (int)((Happy * 0.75) + (Sad * 0.75) + (Angry * 0.75)));
    }
}
