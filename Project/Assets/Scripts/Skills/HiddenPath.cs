using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenPath : MonoBehaviour
{
    public bool isTinyPath;

    private bool playerIn;
    private bool set;

    private Collider2D inCol;

    // Use this for initialization
    void Start()
    {
        playerIn = false;
        set = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isTinyPath && playerIn)
        {
            if( inCol )
            {
                if (!inCol.gameObject.GetComponentInParent<StateChanger>().isSad)
                {
                    inCol.gameObject.GetComponentInParent<PlayerMovement>().respawn();
                }
            }
            
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.name == "Body")
        {
            playerIn = false;
        }
    }

    //void OnTriggerEnter2D(Collider2D col)
    //{
    //    if (col.gameObject.name == "Body")
    //    {
    //        if (isTinyPath)
    //        {
    //            if (!col.gameObject.GetComponentInParent<StateChanger>().isSad)
    //            {
    //                col.gameObject.GetComponentInParent<PlayerMovement>().respawn();
    //            }
    //        }
    //        else
    //        {
    //            if (GetComponent<BoxCollider2D>().isTrigger && !set)
    //            {
    //                playerIn = true;
    //            }
    //            else
    //            {
    //                set = false;
    //            }
    //        }
    //    }
    //}

    void OnTriggerEnter2D(Collider2D col)
    {

        if (col.gameObject.name == "Body")
        {
            inCol = col;

            if (GetComponent<BoxCollider2D>().isTrigger && !set)
            {
                playerIn = true;
            }
            else
            {
                set = false;
            }


        }
    }

    public bool playerIsTrapped()
    {
        return playerIn;
    }

    public void setTrapped(bool trapped)
    {
        playerIn = trapped;
        set = true;
    }
}
