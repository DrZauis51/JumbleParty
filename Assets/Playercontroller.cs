using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Playercontroller : MonoBehaviour
{
    enum PlayerState {
        Moving,
        Jumping,
        Falling,
        Idle
    }

    private PlayerState currentState = PlayerState.Moving;
    private bool isGrounded;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;
    
    private const float MAX_VEL_X = 15;
    private const float IN_AIR_PCT = 0.5f;

    public float xSpeed;
    public float jumpStrength;

    void Start()
    {
        isGrounded = false;
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        
    }

    /// <summary>
    /// Method <c>MoveX</c> enables x inputs and movement.
    /// <param>percent</param>: A range of 0.0f to 1.0f for movement strength.
    /// </summary>
    void MoveX(float percent) {
        if (percent < 0.0f || percent > 1.0f) {
            throw new ArgumentException(String.Format("{0} is not in the range of [0.0f, 1.0f].", percent), "percent");
        }

        

        float xHat = new Vector2(Input.GetAxis("Horizontal"), 0).normalized.x;
        float vx = xHat * xSpeed * percent;
        if (xHat != 0) {
            sr.flipX = xHat > 0;
            anim.Play("Rat");
        } else {
            anim.StopPlayback();
        }
        rb.AddForce(transform.right * vx);
        // limit x velocity
        rb.velocity = new Vector2(Vector2.ClampMagnitude(rb.velocity, MAX_VEL_X).x, rb.velocity.y);
    }

    void StartFalling() {
        // start falling state if the velocity is negative
        const float NEGATIVE_VEL = -0.5f;
        if (rb.velocity.y < NEGATIVE_VEL) {
            isGrounded = false;
            currentState = PlayerState.Falling;
        }
    }

    void MoveState() {
        
        MoveX(1.0f);
        // jump input
        Debug.Log(Input.GetAxis("jump"));
        float yHat = new Vector2(0, Input.GetAxis("jump")).normalized.y;
        if (isGrounded && yHat == 1) {
            currentState = PlayerState.Jumping;
        }
        
        StartFalling();
    }

    void JumpState() {
        MoveX(IN_AIR_PCT);
        
        if (isGrounded) {
            // add vertical force
            float vy = jumpStrength;
            rb.AddForce(transform.up * vy);
            isGrounded = false;
        }
        
        StartFalling();
    }

    void FallingState() {
        MoveX(IN_AIR_PCT);

        bool jumpEnabled = isGrounded && rb.velocity.y <= 0;
        if (jumpEnabled) {
            currentState = PlayerState.Moving;
        }
    }


    void FixedUpdate()
    {
        if (currentState == PlayerState.Moving) {
            MoveState();
        } else if (currentState == PlayerState.Jumping) {
            JumpState();
        } else if (currentState == PlayerState.Falling) {
            FallingState();
        }
        Debug.Log(currentState);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground") {
            isGrounded = true;
        }

        if (collision.gameObject.tag == "Enemy") {
            // Respawn
            SceneManager.LoadScene(0);
        }
    }
}
