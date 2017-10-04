using UnityEngine;
using System.Collections;

// @NOTE the attached sprite's position should be "top left" or the children will not align properly
// Strech out the image as you need in the sprite render, the following script will auto-correct it when rendered in the game
 
// Generates a nice set of repeated sprites inside a streched sprite renderer
// @NOTE Vertical only, you can easily expand this to horizontal with a little tweaking
public class RepeatSpriteBoundary : MonoBehaviour
{
    SpriteRenderer sprite;
    int dir = -1;
    private const int l = 0, u = 1, r = 2, d = 3;
    private bool isHorizontal;

    void Update()
    {
    }

    void Awake()
    {
        foreach (SpriteRenderer s in GetComponentsInChildren<SpriteRenderer>())
        {
            repeatSprite(s);
        }
    }

    void repeatSprite(SpriteRenderer obj)
    {
        GameObject repeat = obj.gameObject;
        //Debug.Log(repeat.name + " " + repeat.transform.parent.name + repeat.transform.position.x + "," + repeat.transform.position.y);
        isHorizontal = Mathf.Approximately(90f, repeat.transform.eulerAngles.z)
        || Mathf.Approximately(-90f, repeat.transform.eulerAngles.z)
        || Mathf.Approximately(270f, repeat.transform.eulerAngles.z)
        || Mathf.Approximately(90f, repeat.transform.parent.eulerAngles.z)
        || Mathf.Approximately(-90f, repeat.transform.parent.eulerAngles.z)
        || Mathf.Approximately(270f, repeat.transform.parent.eulerAngles.z);
        // Get the current sprite with an unscaled size
        sprite = repeat.GetComponent<SpriteRenderer>();
        //sprite.material.SetTextureScale(sprite.name, new Vector2(2, 2));
        //sprite.sprite.texture.wrapMode = TextureWrapMode.Repeat;
        Vector2 spriteSize = new Vector2(sprite.bounds.size.x / repeat.transform.lossyScale.x,
                                 sprite.bounds.size.y / repeat.transform.lossyScale.y);
        if (isHorizontal)
            spriteSize = new Vector2(sprite.bounds.size.x / repeat.transform.localScale.x,
                sprite.bounds.size.y / repeat.transform.localScale.y);
 
        if (isHorizontal)
        {
            return;
        }
        // Generate a child prefab of the sprite renderer
        GameObject childPrefab = new GameObject();
        SpriteRenderer childSprite = childPrefab.AddComponent<SpriteRenderer>();
        childPrefab.transform.Rotate(new Vector3(0, 0, repeat.transform.eulerAngles.z));
        childPrefab.transform.position = repeat.transform.position;
        childSprite.flipY = sprite.flipY;
        childSprite.sprite = sprite.sprite;
        childPrefab.tag = "Wall";
 
        // Loop through and spit out repeated tiles
        GameObject child;
        float nbSpritesX = sprite.bounds.size.x / spriteSize.x;
        float nbSpritesY = sprite.bounds.size.y / spriteSize.y;

        //Debug.Log(sprite.sprite.vertices);

        /*Debug.Log(isHorizontal + "Sprite size: " + sprite.sprite.bounds.size.x + " y: " + sprite.sprite.bounds.size.y
            + "Sprite bounds: " + sprite.bounds.size.x + " y: " + sprite.bounds.size.y
            + "Transform: " + repeat.transform.localScale.x + " y: " + repeat.transform.localScale.y
            + "Size = " + spriteSize.x + "," + spriteSize.y + "\n" +
            "number of sprites = " + nbSpritesX + ", y " + nbSpritesY);
            */
        
        if (nbSpritesX == 1 && nbSpritesY == 1)
        {
            return;
            //TODO: for weird rotated 270 parent paths and their children:
//            if (repeat.transform.parent.name.Equals("Paths"))
//                return;
//            else
//            {
//                nbSpritesX = Mathf.Round(repeat.transform.parent.localScale.x);
//                nbSpritesY = Mathf.Round(repeat.transform.parent.localScale.y);
//            }
        }
        //if it's a minipath
        if (repeat.transform.lossyScale.x < 1 || repeat.transform.lossyScale.y < 1)
        {
            //get the smaller dimension
            float min = Mathf.Min(repeat.transform.localScale.x, repeat.transform.localScale.y);
            childPrefab.transform.localScale = new Vector2(min, min);
            nbSpritesX = Mathf.Round(repeat.transform.parent.localScale.x);
            nbSpritesY = Mathf.Round(repeat.transform.parent.localScale.y);
            //// me
            //verticalSpritesX = Mathf.Round(nbSpritesX * (1 + min)) * nbSpritesX;
            //verticalSpritesY = 3 * nbSpritesY;
            //Vector2 VerticalSpriteSize = new Vector2()
            //// end me
            nbSpritesX = 3 * nbSpritesX;
            nbSpritesY = Mathf.Round(nbSpritesY * (1 + min)) * nbSpritesY;
            spriteSize = new Vector2((sprite.bounds.size.x * childPrefab.transform.localScale.x) / repeat.transform.parent.localScale.x,
                (sprite.bounds.size.y * childPrefab.transform.localScale.y) / repeat.transform.parent.localScale.y);
            //Debug.Log(nbSpritesX + " " + nbSpritesY + ", " + spriteSize.x + " " + spriteSize.y);
        }

        for (int x = 0; x < nbSpritesX; x++)
        {
            for (int y = 0; y < nbSpritesY; y++)
            {
                child = Instantiate(childPrefab) as GameObject;
                if (isHorizontal)
                {
                    child.transform.parent = repeat.transform;
                    child.transform.localPosition = new Vector3(spriteSize.x * x, -spriteSize.y * y, 0);
                }
                else if (Mathf.Approximately(repeat.transform.eulerAngles.z, 180f))
                {
                    //Debug.Log("Upside down!");
                    child.transform.localPosition = new Vector3(repeat.transform.position.x - spriteSize.x * x,
                        repeat.transform.position.y - spriteSize.y * y, 0);
                    child.transform.parent = repeat.transform;
                }
                else
                {
                    child.transform.localPosition = new Vector3(repeat.transform.position.x + spriteSize.x * x,
                        repeat.transform.position.y - spriteSize.y * y, 0);
                    child.transform.parent = repeat.transform;
                }
                string log = "child's x and y = x = " + x + " / " + (repeat.transform.localPosition.x + spriteSize.x * x) +
                             ", y = " + y + " / " + (repeat.transform.localPosition.y - spriteSize.y * y
                             + "\n vs. x = " + child.transform.position.x + " y: " + child.transform.position.y);
                //Debug.Log(log);
                child.transform.parent = repeat.transform;
            }
        }
 
        // Set the parent last on the prefab to prevent transform displacement
        childPrefab.transform.parent = repeat.transform;
 
        // Disable the currently existing sprite component since its now a repeated image
        sprite.enabled = false;
    }
}