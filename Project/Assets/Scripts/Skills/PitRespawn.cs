using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitRespawn : MonoBehaviour
{
    private bool isdead;
    // Use this for initialization
    void Start()
    {
    }
	
    // Update is called once per frame
    void Update()
    {
		
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.name == "Body")
        {
            BoxCollider2D bc = col.gameObject.GetComponent<BoxCollider2D>();
            // if (!col.gameObject.GetComponentInParent<PlayerMovement>().isJumping() &&
            // if the player's body is fully contained within the pit (and they aren't jumping),
            // respawn them
            PlayerMovement p = col.gameObject.transform.parent.GetComponent<PlayerMovement>();
            float offsetx = (bc.bounds.max.x - bc.bounds.min.x) * 0.25f;
            float offsety = (bc.bounds.max.y - bc.bounds.min.y) * 0.25f;
            if (!p.isJumping() && !p.isDead() &&
                bc.bounds.max.x - offsetx < GetComponent<BoxCollider2D>().bounds.max.x &&
                bc.bounds.max.y - offsety < GetComponent<BoxCollider2D>().bounds.max.y &&
                bc.bounds.min.x + offsetx > GetComponent<BoxCollider2D>().bounds.min.x &&
                bc.bounds.min.y + offsety > GetComponent<BoxCollider2D>().bounds.min.y)
            {
                
                //Debug.Log("Death");
                col.gameObject.GetComponentInParent<PlayerMovement>().Die();
            }
        }
    }
}
