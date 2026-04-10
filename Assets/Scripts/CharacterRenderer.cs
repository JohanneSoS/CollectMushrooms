using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterRenderer : MonoBehaviour
{
    //public static readonly string[] staticDirections = {"StaticN", "StaticNE", "StaticE", "StaticSE", "StaticS", "StaticSW", "StaticW", "StaticNW"};
    //public static readonly string[] runDirections = {"RunN", "RunNE", "RunE", "RunSE", "RunS", "RunSW", "RunW", "RunNW"};

    //[SerializeField] private Animator anim;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private int lastDirection;
    public bool isRunning;
    public bool isFlipped;
    public bool isSwimming;
    

    public float frameRate;
    private float idleTime;

    [SerializeField] private List<Sprite> runNSprites;
    [SerializeField] private List<Sprite> runNESprites;
    [SerializeField] private List<Sprite> runESprites;
    [SerializeField] private List<Sprite> runSESprites;
    [SerializeField] private List<Sprite> runSSprites;
    
    private void Awake()
    {
        //anim = GetComponent<Animator>();
    }

    public void CheckRunningState()
    {
        /*if (isRunning)
        {
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }

        if (isSwimming)
        {
            anim.SetFloat("runningSpeed", 0.4f);
        }
        else
        {
            anim.SetFloat("runningSpeed", 1f);
        }*/
    }

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
            // else idle
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
    
    /*public void SetDirection(Vector2 direction)
    {
        string[] directionArray = null;

        if (direction.magnitude < 0.01f)
        {
            directionArray = staticDirections;
        }
        else
        {
            directionArray = runDirections;  
            lastDirection = DirectionToIndex(direction, 8);
        }
        
        anim.Play(directionArray[lastDirection]);
    }

    public static int DirectionToIndex(Vector2 dir, int sliceCount)
    {
        Vector2 normDir = dir.normalized;
        float step = 360f / sliceCount;
        float halfstep = step * 2;
        float angle = Vector2.SignedAngle(Vector2.up, normDir);
        angle += halfstep;
        if (angle < 0)
        {
            angle += 360f;
        }
        float stepCount = angle / step;
        return Mathf.FloorToInt(stepCount);
    }*/
    
    
}
