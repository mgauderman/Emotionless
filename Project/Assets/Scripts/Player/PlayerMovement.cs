using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float playerSpeed = 20;
    public float playerAgility = 10;
    public GameObject wall;
    public AudioClip jumpSting;
    public AudioClip punchSting;
    public AudioClip minimizeSting;
    public AudioClip maximizeSting;

    private AudioSource stingSource;

    //speed player moves
    private float walkSpeed;
    private float curSpeed;
    private Rigidbody2D rigi;
    private int sprintMultiplier;
    private bool sprint;
    private float jumpTimer;
    private bool jumping;
    private const float jumpTime = 0.5f;
    private Vector3 respawnPosition;
    private bool dead;
    private bool isZoomedOut;
    private bool canMove;
    private bool customCamera;

    void Start()
    {
        walkSpeed = (float)(playerSpeed + (playerAgility / 10));
        rigi = GetComponent<Rigidbody2D>();
        sprintMultiplier = 1;
        jumpTimer = jumpTime;
        jumping = false;
        dead = false;
        respawnPosition = transform.position;
        isZoomedOut = false;
        canMove = true;
        customCamera = false;
    }

    public void setCanMove(bool canmove, bool izo)
    { // used to halt player while zooming camera while going over areas
        canMove = canmove;
        isZoomedOut = izo;
    }

    public bool CustomCamera()
    {
        return customCamera;
    }

    void Update()
    {
        // for audio
        if (stingSource == null)
        {
            GameObject temp = GameObject.Find("StingPlayer");
            stingSource = null;
            if (temp != null)
            {
                stingSource = temp.GetComponent<AudioSource>();
                stingSource.loop = false;
            }
        }

        // switch to custom camera controls
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (!customCamera)
                customCamera = true;
            else
                customCamera = false;
        }

        //if (GameObject.GetComponent<MenuScript>().isActiveAndEnabled)
        //dead = true;

        if (Input.GetKeyDown(KeyCode.LeftShift)) // Press the left shift key to do a short dash..this is for debugging only (not in game)
        {
            if (Mathf.Abs(Input.GetAxis("Horizontal") + Input.GetAxis("Vertical")) > 0.75)
            {
                sprintMultiplier = 5;
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!jumping && GetComponent<StateChanger>().isHappy)
            {
                jumping = true;
                stingSource.clip = jumpSting;
                stingSource.Play();
                /// Transform body = transform.FindChild("Body");
                // body.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            }
            //transform.localScale -= new Vector3(0.01f, 0.01f, 0.01f); to fall down
        }

        if( Input.GetKeyDown(KeyCode.K)) // minimize/maximize sound
        {
            if( GetComponent<StateChanger>().isQuiteSad() )
            {
                if( GetComponent<StateChanger>().isSad )
                {
                    stingSource.clip = minimizeSting; 
                }
                else
                {
                    stingSource.clip = maximizeSting;
                }
                stingSource.Play();
            }
        }

        if( Input.GetKeyDown(KeyCode.J) ) // punch sound
        {
            if( GetComponent<StateChanger>().isAngry )
            {
                stingSource.clip = punchSting;
                stingSource.Play();
            }
        }
        if (jumping)
        {
            if (jumpTimer > jumpTime / 2)
            {
                transform.localScale += new Vector3(0.01f, 0.01f, 1.0f);
            }
            else if (jumpTimer < jumpTime / 2) // it IS necessary for this to be an else if statement since it ignores the == case which would make the sprite end up smaller than when it started 
            {
                transform.localScale -= new Vector3(0.01f, 0.01f, 1.0f);
            }
            jumpTimer -= Time.deltaTime;
            if (jumpTimer <= 0.0f)
            {
                jumpTimer = jumpTime;
                jumping = false;
                //Transform body = transform.FindChild("Body");
                //body.gameObject.GetComponent<BoxCollider2D>().enabled = true;
            }
        }
        else
        {
            if (sprintMultiplier > 1)
            {
                sprintMultiplier--;
            }
        }

        //makes it so that the arrow keys don't allow the player to move. Just WASD
//        if (Input.GetKey(KeyCode.UpArrow) ||
//            Input.GetKey(KeyCode.DownArrow) ||
//            Input.GetKey(KeyCode.LeftArrow) ||
//            Input.GetKey(KeyCode.RightArrow))
//            return;

        curSpeed = walkSpeed * sprintMultiplier;

        if (!dead && canMove)
            rigi.velocity = new Vector2(Mathf.Lerp(0, Input.GetAxis("Horizontal") * curSpeed, 0.4f),
                Mathf.Lerp(0, Input.GetAxis("Vertical") * curSpeed, 0.4f));
        else
        {
            rigi.velocity = new Vector2(0, 0);
        }
        //transform.Translate(0, 0, transform.position.y - transform.position.x);

        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
        // if (transform.position.x >= 0.0f && transform.position.x <= 55.0f) // make sure camera doesn't go too far to left
        // 
        if (!isZoomedOut && !customCamera)
        {
            camera.transform.position = new Vector3(transform.position.x, camera.transform.position.y, camera.transform.position.z);
            // }
            if (transform.position.y - camera.transform.position.y > 1.5)
            {
                camera.transform.position = new Vector3(camera.transform.position.x,
                    (transform.position.y - 1.5f),
                    camera.transform.position.z);
            }
            else if (transform.position.y - camera.transform.position.y < -1.5)
            {
                camera.transform.position = new Vector3(camera.transform.position.x,
                    (transform.position.y + 1.5f),
                    camera.transform.position.z);
            }
        }

        // for custom camera movement
        if (customCamera)
        {
            //if( Input.GetKey(KeyCode.LeftArrow)
            // Mathf.Lerp(Input.GetAxis("CameraVertical")*
            //camera.GetComponent<Camera>().orthographicSize = Mathf.Lerp(6.5f, 25.0f, Input.Get)
            camera.transform.position = new Vector3(camera.transform.position.x + 0.2f * Input.GetAxis("CameraHorizontal"),
                camera.transform.position.y + 0.2f * Input.GetAxis("CameraVertical"), camera.transform.position.z);
        }
        if (Input.GetKey(KeyCode.I))
        {
            camera.GetComponent<Camera>().orthographicSize -= 0.07f;
        }
        if (Input.GetKey(KeyCode.U))
        {
            camera.GetComponent<Camera>().orthographicSize += 0.07f;
        }




        // an alternative way of moving using forces
        //float horizontalMovement = Input.GetAxis("Horizontal");

        //float verticalMovement = Input.GetAxis("Vertical");

        //Vector2 moveVector = new Vector2(horizontalMovement, verticalMovement);

        //GetComponent<Rigidbody2D>().AddForce(moveVector * 10.0f);

        if (Input.GetKey(KeyCode.A)) // Rotate player to left
        {
            //transform.localRotation = Quaternion.Euler(0, 180, 0);
            //Vector3 cur = wall.transform.localPosition;
            //wall.transform.localPosition = new Vector3(cur.x, cur.y, -1);
            //            wall.transform.position.Set(wall.transform.position.x, wall.transform.position.y, 1);
        }
        else if (Input.GetKey(KeyCode.D)) // Rotate player to right
        {
            //transform.localRotation = Quaternion.Euler(0, 0, 0);
            //Vector3 cur = wall.transform.localPosition;
            //wall.transform.localPosition = new Vector3(cur.x, cur.y, 1);
        }
        //if (Input.GetKeyDown(KeyCode.F))
        //{
        //    if (!isZoomedOut)
        //    {
        //        camera.GetComponent<Camera>().orthographicSize = 25;
        //        isZoomedOut = true;
        //    }
        //    else
        //    {
        //        camera.GetComponent<Camera>().orthographicSize = 6.5f;
        //        updateCamera();
        //        isZoomedOut = false;
        //    }
            
            
        //}
    }

    public void updateCamera()
    {
        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
        camera.transform.position = new Vector3(transform.position.x, transform.position.y, camera.transform.position.z);
    }

    public void respawn()
    {
        transform.position = respawnPosition;
        dead = false;
        updateCamera();
        MetricManager.AddToRespawnCount();
        //Debug.Log("No longer dead, respawn method finished");
    }

    public void setRespawn(Vector3 respawnP)
    {
        respawnPosition = respawnP;
    }

    public bool isJumping()
    {
        return jumping;
    }

    public bool isDead()
    {
        return dead;
    }

    public void DisallowMovement()
    {
        dead = true;
    }

    public void AllowMovement()
    {
        dead = false;
    }

    public void Die()
    {
        if (!dead)
        {
            GetComponent<AnimationController>().Die();
            //Debug.Log("Called animation Die");
            //MetricManager.AddToRespawnCount();
            dead = true;
        }
    }
}


