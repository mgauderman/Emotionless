using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator anim;
    private float blinkTimer;
    private float blinkTime;
    private string jump, dead, blink, dir, shrink, xvel, yvel, punch;
    private const int W = 0, S = 1, A = 2, D = 3;
    private PlayerMovement p;
    private Rigidbody2D rigi;
    //public float x, y, z;
    private StateChanger st;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        st = GetComponent<StateChanger>();
        blinkTime = Random.Range(3f, 7f);

        //get all parameter names
        for (int i = 0; i < anim.parameters.Length; i++)
        {
            string a = anim.parameters[i].name;
            string lower = a.ToLower();
            if (lower.Contains("dir"))
                dir = a;
            else if (lower.Contains("blink"))
                blink = a;
            else if (lower.Contains("jump"))
                jump = a;
            else if (lower.Contains("dead"))
                dead = a;
            else if (lower.Contains("shrunk"))
                shrink = a;
            else if (lower.Contains("x"))
                xvel = a;
            else if (lower.Contains("y"))
                yvel = a;
            else if (lower.Contains("punch"))
                punch = a;
            //else
            //Debug.Log("Parameter " + a + " not accounted for in Animation Controller.");
        }
        p = GetComponent<PlayerMovement>();
        rigi = GetComponent<Rigidbody2D>();

        anim.SetInteger(dir, S);
    }

    public void Die()
    {
        anim.SetBool(dead, true);
        //Debug.Log("set dead to true");
    }

    public void Punch()
    {
        anim.SetBool(punch, true);
    }

    public void SetHat(string color)
    {
        switch (color)
        {
            case ColorController.red: // layer 2
                anim.SetLayerWeight(2, 1);
                anim.SetLayerWeight(3, 0);
                anim.SetLayerWeight(4, 0);
                anim.SetLayerWeight(5, 1);
                break;
            case ColorController.blue: // layer 3
                anim.SetLayerWeight(2, 0);
                anim.SetLayerWeight(3, 1);
                anim.SetLayerWeight(4, 0);
                anim.SetLayerWeight(6, 1);
                break;
            case ColorController.yellow: // layer 4
                anim.SetLayerWeight(2, 0);
                anim.SetLayerWeight(3, 0);
                anim.SetLayerWeight(4, 1);
                anim.SetLayerWeight(7, 1);
                break;
            case "none":
                anim.SetLayerWeight(2, 0);
                anim.SetLayerWeight(3, 0);
                anim.SetLayerWeight(4, 0);
                anim.SetLayerWeight(5, 1);
                anim.SetLayerWeight(6, 1);
                anim.SetLayerWeight(7, 1);
                break;
        }
    }

    void Update()
    {
        anim.SetFloat(xvel, rigi.velocity.x);
        anim.SetFloat(yvel, rigi.velocity.y);

        // Sad mode
        anim.SetBool(shrink, st.isSad);
        anim.SetLayerWeight(5, st.isAngry ? 1 : 0);
        anim.SetLayerWeight(6, st.isSad ? 1 : 0);
        anim.SetLayerWeight(7, st.isHappy ? 1 : 0);

        bool isDead =
            anim.GetCurrentAnimatorStateInfo(anim.GetLayerIndex("Happy")).IsName("Die");

        if (anim.GetBool(punch))
            anim.SetBool(punch, false);

        if (Input.GetKey(KeyCode.J) && st.isAngry)
            Punch();

        //death
        if (isDead)
            anim.SetBool(dead, false);

        //directions of eyes and body
        else if (!p.isDead())
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKey(KeyCode.W)) // up
            {
                anim.SetInteger(dir, W);
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKey(KeyCode.S)) // down
            {
                anim.SetInteger(dir, S);  
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKey(KeyCode.A)) // left
            {
                anim.SetInteger(dir, A);
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKey(KeyCode.D)) // right
            {
                anim.SetInteger(dir, D);
            }
        }

        //jumping
        if (Input.GetKey(KeyCode.Space) && !anim.GetBool(jump)
            && st.isHappy)
        {
            anim.SetBool(jump, true);
        }
        else
        {
            anim.SetBool(jump, false);
        }

        //blinking
        blinkTimer += Time.deltaTime;
        if (blinkTimer >= blinkTime)
        {
            anim.SetBool(blink, true);
            blinkTimer = 0f;
            blinkTime = Random.Range(3f, 7f);
        }
        else
        {
            anim.SetBool(blink, false);
        }
    }
}
