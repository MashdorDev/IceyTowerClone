using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController: MonoBehaviour {
    public float moveSpeed = 0.5f;
    public float jumpForce = 45f;
    public Transform groundCheck;
    public float groundCheckRadius;
    public string groundTag = "Platform";
    public float massDecreaseFactor = 0.5f;
    public float pillarBounceAngle = 45f;

    private bool isJumping = false;
    private Rigidbody2D rb;
    private bool facingRight = true;
    private bool isGrounded;
    private float initialMass;
    private float previousSpeed;

    public float maxSpeed = 1700f;
    public float moveForce = 90f;
    public float HorizontalJumpFactor = 250f;
    public float forceJumpLimit = 1000f;
    public float bounceFactor = 2.00f;

    public float spinTorque = 100f;

    private Animator animator;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        initialMass = rb.mass;
        previousSpeed = rb.velocity.magnitude;

        animator = GetComponent<Animator>();
    }

    private void Update() {
        Collider2D groundCollider = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius);
        if (groundCollider != null && groundCollider.tag == groundTag) {
            isGrounded = true;
        } else {
            isGrounded = false;
        }

        if (Input.GetButtonDown("Jump") && !isJumping && isGrounded) {
            rb.constraints = RigidbodyConstraints2D.None;
            Vector2 jumpPower = new Vector2(0f, jumpForce);
            rb.AddForce(jumpPower);
            rb.AddTorque(spinTorque);
            isJumping = true;
            isGrounded = false;
            animator.SetBool("IsJumping", true);
        }

        if(!isGrounded) animator.SetBool("IsJumping", true);

    }

    private void FixedUpdate() {
        float h = Input.GetAxis("Horizontal");

        if (Mathf.Abs(h * rb.velocity.x) < maxSpeed) rb.AddForce(h * moveForce * Vector2.right);

        if (Mathf.Abs(h) <= 0.05)
        {
         rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, 0.1f), rb.velocity.y);

        }

        if ((h > 0 && !facingRight) || (h < 0 && facingRight)) Flip();

        if (isJumping) {
            if(rb.velocity.x == 0){
                animator.SetBool("IsJumpingV", true );
                rb.constraints &= ~RigidbodyConstraints2D.None;
                float totalJumpForce = jumpForce * 200;
                rb.AddForce(Vector2.up * totalJumpForce);
                isJumping = false;
                return;
            } else {
                animator.SetBool("IsJumping", true );
                rb.constraints &= ~RigidbodyConstraints2D.None;
                float totalJumpForce = jumpForce + Mathf.Abs(rb.velocity.x) * HorizontalJumpFactor;
                rb.AddForce(Vector2.up * totalJumpForce);
                isJumping = false;
            }
        }

        float speed = rb.velocity.magnitude;
        if (speed > previousSpeed) {
            float targetMass = Mathf.Max(0.1f, rb.mass - (speed - previousSpeed) * massDecreaseFactor);
            rb.mass = Mathf.Lerp(rb.mass, targetMass, Time.deltaTime);
        } else if (speed < previousSpeed) {
            float targetMass = Mathf.Min(initialMass, rb.mass + (previousSpeed - speed) * massDecreaseFactor);
            rb.mass = Mathf.Lerp(rb.mass, targetMass, Time.deltaTime);
        }
        previousSpeed = speed;

        if (isGrounded && !isJumping) {
            rb.mass = initialMass;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        RotateInDirectionOfSpeed();
    }

    private void RotateInDirectionOfSpeed() {
        if (!isGrounded ) {
            float rotationAngle = - rb.velocity.x * 8;
            transform.rotation = Quaternion.Euler(0, 0, rotationAngle);
        }
    }

    private void Flip() {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.tag == "Pillar") {
            Flip();
            Vector2 rev = new Vector2(rb.velocity.x * bounceFactor * Mathf.Cos(pillarBounceAngle * Mathf.Deg2Rad), rb.velocity.y * bounceFactor * Mathf.Sin(pillarBounceAngle * Mathf.Deg2Rad));
            rb.AddForce(rev, ForceMode2D.Impulse);
            rb.mass *= massDecreaseFactor;
        }

        if (col.gameObject.tag == "Platform") {
            if (!isJumping) {
                transform.rotation = Quaternion.Euler(0, 0, 0);
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                animator.SetBool("IsJumping", false);
                animator.SetBool("IsJumpingV", false);

            }
        }
    }
}