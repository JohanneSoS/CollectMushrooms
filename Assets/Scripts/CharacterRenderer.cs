using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterRenderer : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    private int lastDirection;
    public bool isRunning;
    public bool isFlipped;

    public float frameRate;
    private float idleTime;

    [SerializeField] private List<Sprite> runNSprites;
    [SerializeField] private List<Sprite> runNESprites;
    [SerializeField] private List<Sprite> runESprites;
    [SerializeField] private List<Sprite> runSESprites;
    [SerializeField] private List<Sprite> runSSprites;
    
    public void FlipSprite(string direction)
    {
        if (direction == "right" && !isFlipped)
        {
            spriteRenderer.flipX = true;
            isFlipped = true;
        }
        else if (direction == "left" && isFlipped)
        {
            spriteRenderer.flipX = false;
            isFlipped = false;
        }
    }

    public List<Sprite> GetSpriteDirection(Vector2 direction)
    {
        List<Sprite> selectedSprites = null;

        if (direction.y > 0)
        {
            if (Math.Abs(direction.x) > 0)
            {
                selectedSprites = runNESprites;
            }
            else
            {
                selectedSprites = runNSprites;
            }
        }
        else if (direction.y < 0)
        {
            if (Math.Abs(direction.x) > 0)
            {
                selectedSprites = runSESprites;
            }
            else
            {
                selectedSprites = runSSprites;
            }
        }

        if (direction.y == 0)
        {
            if (Math.Abs(direction.x) > 0)
            {
                selectedSprites = runESprites;
            }
        }

        return selectedSprites;
    }

    public void UpdateSprite(List<Sprite> directionSprites)
    {
        if (directionSprites != null)
        {
            float playTime = Time.time - idleTime;
            int totalFrames = (int)(playTime * frameRate);
            int frame = totalFrames % directionSprites.Count; 
            spriteRenderer.sprite = directionSprites[frame];
        }
        else
        {
            idleTime = Time.time;
        }
    }
}
