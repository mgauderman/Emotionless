using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{

    private Vector3 orig_pos;
    
    // Use this for initialization
    void Start()
    {
        orig_pos = this.gameObject.transform.position;
    }
    
    // Update is called once per frame
    void Update()
    {
        Vector3 newpos = orig_pos + new Vector3(0, Mathf.Sin(Time.time * 7) / 16, 0);
        transform.position = newpos;//Vector3.Lerp (transform.position, newpos, .1f);\
        transform.Rotate(new Vector3(0, 3, 0));
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            MetricManager.AddToCollectionCount();
            HeadsUpGUI.UpdateScore();
            Destroy(this.gameObject);
        }
    }
}
