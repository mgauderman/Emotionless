using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillWall : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
		
    }
	
    // Update is called once per frame
    void Update()
    {
        //if( GetComponent<BoxCollider2D>().)
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.name == "Body")
        {
            if (Input.GetKey(KeyCode.J))
            {
                if (col.gameObject.GetComponentInParent<StateChanger>().isAngry)
                {
                    col.gameObject.GetComponentInParent<AnimationController>().Punch();
                    gameObject.GetComponentInParent<Animator>().SetTrigger("destroy");
                }
            }
        }
    }
}
