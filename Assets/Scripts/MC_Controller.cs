using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MC_Controller : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    Animator animator;
    public float horizontal { get; set; }
    public float vertical { get; set; }

    // Setting Music Power
    public int maxPower = 5;
    int currentPower;

    enum State
    {
        Moving
    }
    State state = State.Moving;

    // Setting Invincible Time
    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;

    // Setting Movement
    public float speed = 3.0f;

    // Property for Power
    public int Power
    {
        get { return currentPower; }
    }

    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        currentPower = maxPower;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Timer for Invincible Period.
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }

        // Read input in Update
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
    }

    void FixedUpdate()
    {
        if (state == State.Moving)
        {
            MoveState();
        }
    }

    void MoveState()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        if (horizontal > 0)
        {
            animator.SetFloat("Move_X", 1);
            animator.SetFloat("Move_Y", 0);
        }
        else if (horizontal < 0)
        {
            animator.SetFloat("Move_X", -1);
            animator.SetFloat("Move_Y", 0);
        }
        else
        {
            if (vertical > 0)
            {
                animator.SetFloat("Move_X", 0);
                animator.SetFloat("Move_Y", 1);
            }
            else
            {
                animator.SetFloat("Move_X", 0);
                animator.SetFloat("Move_Y", -1);
            }
        }

        rigidbody2d.MovePosition(position);
    }

    public void ChangePower(int amount)
    {
        // Check if MC is Invincible.
        if (amount < 0 && isInvincible)
        {
            return;
        }

        if (amount < 0)
        {
            isInvincible = true;
            invincibleTimer = timeInvincible;
        }

        currentPower = Mathf.Clamp(currentPower + amount, 0, maxPower);

        // Assuming UIPowerBar is a singleton instance
        UIPowerBar.instance.SetValue(currentPower / (float)maxPower);
    }
    public void Teleport(Vector3 position)
    {
        transform.position = position;
       
    }
}
