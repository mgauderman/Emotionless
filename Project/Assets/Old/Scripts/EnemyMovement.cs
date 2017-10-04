using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private Transform target;
    public float speed = 2f;
    public GameObject triggerBox;
    private float minDistance = 1f;
    private float shrinkrate = 2.0f;
    private float range;
    private bool playerDead = false;
    private bool attackState;
    private float elapsedTime;
    private float attackTime; // if attackTime > 0, enemy won't change directions
    private Vector3 direction;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        playerDead = false;
        attackState = false;
        elapsedTime = 0.0f;
        attackTime = 0.0f;
        direction = new Vector2(0.0f, 0.0f);  
    }

    void FixedUpdate()
    {
        elapsedTime += Time.deltaTime;
        if(elapsedTime > 1.0f)
        {
            elapsedTime -= 1.0f;
        }
        if (playerDead)
        {
            return;
        }
        range = Vector2.Distance(transform.position, target.position);
        
        if (range > minDistance && !playerDead)
        {
            if (!attackState) // if enemy is in neutral state
            {
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + 2.0f * Mathf.Cos(elapsedTime * 2 * Mathf.PI), transform.position.y + 2.0f * Mathf.Sin(elapsedTime * 2 * Mathf.PI)), speed * Time.deltaTime);


                //if ( ((vertical && triggerBox.transform.position.x <= target.transform.position.x) ||
                //    !vertical && triggerBox.transform.position.y >= target.transform.position.y)
                //    && (Mathf.Abs(transform.position.x - target.position.x) < 0.5f || Mathf.Abs(transform.position.y - target.position.y) < 0.5f) )
                if (triggerBox.transform.position.x <= target.transform.position.x && (Mathf.Abs(transform.position.x - target.position.x) < 0.5f || Mathf.Abs(transform.position.y - target.position.y) < 0.5f))
                {
                    attackState = true;
                    MoveEnemy();
                    attackTime = 0.5f;
                }
            }
            else if(attackState) {
                Vector3 shrink = new Vector3(shrinkrate * .001f, shrinkrate * .001f, shrinkrate * .001f); // Shrink the enemy a bit
                transform.localScale -= shrink;
                GetComponent<PolygonCollider2D>().transform.localScale = transform.localScale;
                minDistance -= 0.003f;

                speed += 0.012f; // Speed up the enemy a bit

                MoveEnemy();              
            }
        }
        if (transform.localScale.x < 0.2f)
        {
            Destroy(gameObject);
            Destroy(this);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Numb"))
        {
            shrinkrate++;
            Destroy(col.gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Numb"))
        {
            shrinkrate++;
            Destroy(col.gameObject);
        }
    }

    public void PlayerDead(string message)
    {
        playerDead = true;
    }

    void MoveEnemy()
    {
        // Move the enemy
        attackTime -= Time.deltaTime;
        if (attackTime < 0.0f)
        {
            attackTime = 0.0f;
        }
        //transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime); 

        // Calculate the direction if in line with player
        if (attackTime == 0.0f && Mathf.Abs(transform.position.x - target.position.x) < 0.5f)
        {
            if (transform.position.y - target.position.y > 0.0f)
            {
                direction.y = -1.0f;
                direction.x = 0.0f;
            }
            else
            {
                direction.y = 1.0f;
                direction.x = 0.0f;
            }
        }

        else if (attackTime == 0.0f && Mathf.Abs(transform.position.y - target.position.y) < 0.5f)
        {
            if (transform.position.x - target.position.x > 0.0f)
            {
                direction.x = -1.0f;
                direction.y = 0.0f;
            }
            else
            {
                direction.x = 1.0f;
                direction.y = 0.0f;
            }
        }
        transform.position = Vector2.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);
    }
}
