using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Sting : MonoBehaviour
{
    public AudioClip sting;
    static AudioSource stingSource;

    void Start()
    {
    }

    void Update()
    {
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
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            PlaySting(sting);
        }
    }

    public static void PlaySting(AudioClip s)
    {
        if (stingSource != null)
        {
            stingSource.clip = s;
            stingSource.Play();
        }
    }
}