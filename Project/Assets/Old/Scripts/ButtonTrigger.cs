using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTrigger : MonoBehaviour
{
    public GameObject[] Walls;
    public Sprite unpressed;
    public Sprite pressed;
    AudioSource a;

    // Use this for initialization
    void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = unpressed;
        a = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player") && gameObject.GetComponent<SpriteRenderer>().sprite != pressed)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = pressed;
            a.Play();
            foreach (GameObject wall in Walls)
            {
                wall.SetActive(false);
            }
        }
    }
}