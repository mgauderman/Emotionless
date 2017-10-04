using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerHealth : MonoBehaviour
{
    public GameObject finalText;
    public GameObject finalTrigger; 
    public GameObject happyBar;
    public GameObject angryBar;
    public GameObject sadBar;
    private float enemyDamage = 20.0f;
    public float numbingDamage = 2f;
    public GameObject barrier;
    public Slider[] sliders;
    readonly string[] emotions = { "Happy", "Sad", "Angry" };
    private Slider happySlider;
    private Slider angrySlider;
    private Slider sadSlider;
    private float finalHappy, finalSad;//, finalAngry;

    // Use this for initialization
    void Start()
    {
        happySlider = happyBar.GetComponent<Slider>();
        angrySlider = angryBar.GetComponent<Slider>();
        sadSlider = sadBar.GetComponent<Slider>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            AudioSource a = GetComponent<AudioSource>();
            a.Play();

            if( col.gameObject.CompareTag("Happy"))
            {
                happySlider.value += enemyDamage;
            }
            else if (col.gameObject.CompareTag("Angry"))
            {
                angrySlider.value += enemyDamage;
            }
            else if (col.gameObject.CompareTag("Sad"))
            {
                sadSlider.value += enemyDamage;
            }
            Destroy(col.gameObject);
            
            //bool active = barrier.activeSelf;
            //string tag = col.gameObject.tag;
            //foreach (Slider s in sliders)
            //{
            //    if (s.name.Contains(tag))
            //    {
            //        Debug.Log("yay");
            //        Destroy(col.gameObject);
            //        if (s.value + enemyDamage <= s.maxValue)
            //            s.value += enemyDamage;

            //    }
            //}
        }
    }
    
    void FixedUpdate()
    {
        if( transform.position.x >= finalTrigger.transform.position.x ) {
            finalHappy = happySlider.value;
            finalSad = sadSlider.value;
            //finalAngry = angrySlider.value;

            TextMesh tm = finalText.GetComponent<TextMesh>();
            if (finalHappy == 100.0f && finalSad != 100.0f)
            {
                tm.text = "I stayed positive about the situation and tried \n" +
                    "to make some small talk with them about the latest \n" +
                    "Movies. Turns out, this worked wonders and they \n" +
                    "asked you if you wanted to go see the newest Star \n" +
                    "Wars movie with them on Friday night! Of course I \n" +
                    "said yes. Now it's time to start getting worried \n" +
                    "about Friday night...";
            }
            else if( finalHappy != 100.0f && finalSad == 100.0f )
            {
                tm.text = "I could tell this was not going well and \n" +
                    "and just succumbed to defeat. I knew that \n" +
                    "nothing would ever come of this so I just \n" +
                    "started crying, right there in front of \n" +
                    "the person I cared most for in the world. \n" +
                    "They tried to comfort me and kissed me on \n" +
                    "the cheek. Maybe not all was lost...";
            }
            else if( finalHappy == 100.0f && finalSad == 100.0f )
            {
                tm.text = "My emotions were too conflicting. \n" +
                    "I didn't know how to respond but I was \n" +
                    "feeling way too emotional to keep my mouth \n" +
                    "shut. So what did I do? I yelled I CAN'T TAKE \n" +
                    "THIS ANYMORE, and started walking the other \n" +
                    "way, away from the person I loved. So much for \n" +
                    "my love life.";
            }
            else
            {
                tm.text = "I didn't know how to react so I just \n" +
                    "kept my mouth shut and continued walking next \n" +
                    "to them with an undeniable awkward silence \n" +
                    "between us that only wrenched us further apart. \n" +
                    "I was hopeless.";
            }
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        //Debug.Log("trigger");
        //int damage = col.tag;
        //if (col.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        //{
        //    bool active = barrier.activeSelf;
        //    string name = col.gameObject.tag;
        //    EmotionSlider cur;
        //    if (bars.TryGetValue(name + "Bar", out cur))
        //    {
        //        Destroy(col.gameObject);
        //        if (!active)
        //        {
        //            if (cur.absorb(enemyDamage))
        //                Die(name);
        //            else
        //                cur.slid.value = cur.Current;
        //        }
        //    }
        //}
    }

    void Die(string name)
    {
        GameObject[] e;
        foreach (string s in emotions)
        {
            e = GameObject.FindGameObjectsWithTag(s);
            foreach (GameObject enemy in e)
                enemy.GetComponent<EnemyMovement>().PlayerDead(s);
        }
        //GameObject.Find("GameManager").playerDead();
        Destroy(gameObject);
        //Instantiate(dieExplosion, transform.position, transform.rotation);
    }
}