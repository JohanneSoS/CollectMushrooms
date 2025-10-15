using System;
using UnityEngine;

public class CharacterRenderer : MonoBehaviour
{
    //public static readonly string[] staticDirections = {"StaticN", "StaticNE", "StaticE", "StaticSE", "StaticS", "StaticSW", "StaticW", "StaticNW"};
    //public static readonly string[] runDirections = {"RunN", "RunNE", "RunE", "RunSE", "RunS", "RunSW", "RunW", "RunNW"};

    [SerializeField] private Animator anim;
    [SerializeField] private SpriteRenderer spriteRenderer;
    //private int lastDirection;
    public bool isRunning;
    public bool isFlipped;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void CheckRunningState()
    {
        if (isRunning)
        {
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }
    }

    public void FlipSprite(string direction)
    {
        if (direction == "right")
        {
            spriteRenderer.flipX = true;
            isFlipped = true;
        }
        else if (direction == "left")
        {
            spriteRenderer.flipX = false;
            isFlipped = false;
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
