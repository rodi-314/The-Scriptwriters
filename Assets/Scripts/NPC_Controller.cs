using RPGM.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NPC_Controller : MonoBehaviour
{
    public float speed;
    public bool vertical;
    private Vector2 originalPos;
    private Vector2 targetPos;
    Animator animator;
    // ChangeTime and the Timer is used to change NPC direction, so that it walks back and forth.
    public float changeTime = 3.0f;

    private Rigidbody2D rigidbody2D;
    private float timer;
    private int direction = 1;
    private float originalSpeed;
    private bool returningToPath = false;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        timer = changeTime;
        originalSpeed = speed;
        originalPos = rigidbody2D.position;
        targetPos = originalPos;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!returningToPath)
        {
            timer -= Time.deltaTime;

            if (timer < 0)
            {
                direction = -direction;
                timer = changeTime;
            }
        }
    }

    void FixedUpdate()
    {
        if (!returningToPath)
        {
            Vector2 position = rigidbody2D.position;

            if (vertical)
            {
                position.y += Time.deltaTime * speed * direction;
                targetPos.y = originalPos.y + (direction * speed * changeTime);
                animator.SetFloat("Move X", 0);
                animator.SetFloat("Move Y", direction);
            }
            else
            {
                position.x += Time.deltaTime * speed * direction;
                targetPos.x = originalPos.x + (direction * speed * changeTime);
                animator.SetFloat("Move X", direction);
                animator.SetFloat("Move Y", 0);
            }

            rigidbody2D.MovePosition(position);
        }
        else
        {
            // Move NPC back to its original path smoothly
            Vector2 newPosition = Vector2.MoveTowards(rigidbody2D.position, targetPos, speed * Time.deltaTime);
            rigidbody2D.MovePosition(newPosition);

            if (Vector2.Distance(rigidbody2D.position, targetPos) < 0.1f)
            {
                returningToPath = false;
            }
        }
    }

    void OnCollisionStay2D(Collision2D other)
    {
        MC_Controller player = other.gameObject.GetComponent<MC_Controller>();

        if (player != null)
        {
            speed = 0;
            // Freeze position to prevent NPC from being pushed
            rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        MC_Controller player = other.gameObject.GetComponent<MC_Controller>();

        if (player != null)
        {
            speed = originalSpeed;
            // Unfreeze position but keep rotation frozen
            rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            returningToPath = true;
        }
    }
}
