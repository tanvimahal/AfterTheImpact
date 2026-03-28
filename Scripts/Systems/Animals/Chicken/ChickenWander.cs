using UnityEngine;

public class ChickenWander : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 0.5f;
    public float minMoveTime = 1f;
    public float maxMoveTime = 2f;
    public float idleTime = 1.2f;
    public float wanderRadius = 2f;

    private Rigidbody2D rb;
    private Vector2 startPosition;
    private Vector2 moveDirection;
    private float timer;
    private bool isMoving;
    private SpriteRenderer sr;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
        sr = GetComponent<SpriteRenderer>();
        StartIdle();
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            if (isMoving)
                StartIdle();
            else
                StartMoving();
        }

        // Keep sheep within a radius
        float distance = Vector2.Distance(transform.position, startPosition);
        if (distance > wanderRadius)
        {
            moveDirection = (startPosition - (Vector2)transform.position).normalized;
        }
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            rb.linearVelocity = moveDirection * moveSpeed;

            if (Mathf.Abs(moveDirection.x) > 0.1f)
            {
                sr.flipX = moveDirection.x < 0;
            }
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    void StartMoving()
    {
        isMoving = true;
        timer = Random.Range(minMoveTime, maxMoveTime);
        moveDirection = Random.insideUnitCircle.normalized;
    }

    void StartIdle()
    {
        isMoving = false;
        timer = idleTime;
        rb.linearVelocity = Vector2.zero;
    }
}