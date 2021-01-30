using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimatedSprite : MonoBehaviour {

    [Header("Settings")]
    public Sprite[] sprites;
    public int speed;//单位：帧
    public const bool useFixedUpdate = true;
    public bool useScaledNativeSize = false;
    public Sprite Sprite
    {
        get
        {
            if (renderer != null)
                return renderer.sprite;
            else if (image != null)
                return image.sprite;
            return null;
        }
    }

    private SpriteRenderer renderer;
    private Image image;
    private int frame = 0;
    private int currentSprite = 0;

	void Start ()
    {
        renderer = gameObject.GetComponent<SpriteRenderer>();
        /*if (renderer == null)
            renderer = gameObject.AddComponent<SpriteRenderer>();*/
        image = gameObject.GetComponent<Image>();

	}
	
	
	void Update ()
    {
		
	}

    void FixedUpdate()
    {
        if(useFixedUpdate)
        {
            if(frame >= speed)
            {
                NextSprite();
                frame = 0;
            }

            frame++;
        }
    }

    public void NextSprite()
    {
        currentSprite++;
        if (currentSprite > sprites.Length - 1)
            currentSprite = 0;
        if (renderer != null)
            renderer.sprite = sprites[currentSprite];
        if(image != null)
            image.sprite = sprites[currentSprite];

        if (useScaledNativeSize)
            SetNativeSize();
    }

    public void SetNativeSize()
    {
        Sprite spr = this.Sprite;
        float width = spr.rect.width;
        float height = spr.rect.height;

        /*float x = 
        float newWidth*/

        if (image != null)
            image.SetNativeSize();
    }
}
