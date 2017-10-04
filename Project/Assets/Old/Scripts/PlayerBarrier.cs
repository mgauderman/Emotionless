using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBarrier : MonoBehaviour
{

    public GameObject barrier;
    private float barrierActiveTime = 1f;
    private float barrierDelay = 5f;
    private bool isDelayed = false;

    // Use this for initialization
    void Start()
    {
        barrier.SetActive(false);
    }
	
    // Update is called once per frame
    void Update()
    {
        if (!isDelayed)
        {
            if (barrier.activeSelf)
                barrierActiveTime -= Time.deltaTime;
            else if ((Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.Q)) && !isDelayed)
            {
                barrier.SetActive(true);
                //GetComponent<PlayerHealth>().TakeDamage(8.0f);
            }
            if (barrierActiveTime <= 0f)
            {
                barrier.SetActive(false);
                isDelayed = true;
            }
        }
        else
        {
            barrier.SetActive(false);
            barrierDelay -= Time.deltaTime;
        }
        if (barrierDelay <= 0f)
        {
            isDelayed = false;
            barrierActiveTime = 1f;
            barrierDelay = 5f;
        }
    }
}
