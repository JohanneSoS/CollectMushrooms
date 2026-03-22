using System;
using UnityEngine;
using System.Collections;

public class WolfMovement : MonoBehaviour
{
    private Transform target;
    private Vector2 moveDirection;
    Rigidbody2D rb;
    [SerializeField] private Animator wolfAnim;
    [SerializeField] private SpriteRenderer wolfSprite;
    
    [SerializeField] private float movementSpeed;
    [SerializeField] private bool isRunning;
    [SerializeField] private bool isChasing;
    [SerializeField] private float chasingDuration;
    [SerializeField] private float chasingCooldown;
    [SerializeField] private int damage;
    [SerializeField] private float radius;
    private bool playerHovering = false;
    private bool canChase = true;
    public LayerMask chasingTargetLayer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        wolfAnim = GetComponent<Animator>();
        GlobalEventManager.OnDayStart.AddListener(Despawn);
    }

    void Start()
    {
        target = GameObject.Find("Character").transform;
    }

    void Update()
    {
        if (isChasing)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            moveDirection = direction;
            if (playerHovering)
            {
                StartCoroutine(BounceBack(direction));
                InflictDamage();
            }
        }

        if (Physics2D.OverlapCircle(transform.position, radius, chasingTargetLayer) && !isChasing && !isRunning && canChase)
        {
            isChasing = true;
            isRunning = true;
            GlobalEventManager.OnChasing.Invoke();
            StartCoroutine(WaitForChasingDuration());
        }
        CheckRunningState();
    }

    void FixedUpdate()
    {
        if (isRunning) {
            rb.linearVelocity = new Vector2(moveDirection.x, moveDirection.y) * movementSpeed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerHovering = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerHovering = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    IEnumerator BounceBack(Vector3 direction)
    {
        float defaultMovementSpeed = movementSpeed;
        isChasing = false;
        movementSpeed = movementSpeed * 3;
        moveDirection = -direction;
        yield return new WaitForSeconds(0.2f);
        isRunning = false;
        movementSpeed = defaultMovementSpeed;
        moveDirection = direction;
        StartCoroutine (WaitForChasingCooldown());
    }

    IEnumerator WaitForChasingDuration()
    {
        yield return new WaitForSeconds(chasingDuration);
        isChasing = false;
        isRunning = false;
        StartCoroutine (WaitForChasingCooldown());
    }

    IEnumerator WaitForChasingCooldown()
    {
        canChase = false;
        yield return new WaitForSeconds(chasingCooldown);
        canChase = true;
    }

    public void CheckRunningState()
    {
        if (isRunning)
        {
            wolfAnim.SetBool("isRunning", true);
        }
        else
        {
            wolfAnim.SetBool("isRunning", false);
        }
    }

    private void Despawn()
    {
        Destroy(gameObject);
    }

    private void InflictDamage()
    {
        GlobalEventManager.ApplyDamage.Invoke(damage);
    }
}
