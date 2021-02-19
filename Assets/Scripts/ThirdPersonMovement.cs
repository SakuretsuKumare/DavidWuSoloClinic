using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This code was used from Brackey's Third Person Movement in Unity
video https://www.youtube.com/watch?v=4HpC--2iowE and also his gravity system in
Brackey's First Person Movement In Unity video https://www.youtube.com/watch?v=_QajrabyTJc
*/

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController controller;
    public Animator anim;
    public Transform cam;
    public float speed = 6f;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    public float gravity = -9.81f;
    public float jumpHeight = 2f;
    Vector3 velocity;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    bool isGrounded;

    // Update is called once per frame
    void Update()
    {
        // Creates a ground mask sphere to check if the player is touching the ground.
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Assigns player movement to the horizontal and vertical inputs.
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        // Set the Float value of the Animator.
        float magnitude = direction.magnitude;
        anim.SetFloat("MoveSpeed", magnitude * speed);

        // If our player is moving; more than a speed of 0
        if (magnitude >= 0.1f)
        {
            // Gets the angle of the camera and rotates the player to the camera.
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            // The player moves in the direction of the camera.
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            // Moves the player.
            controller.Move(moveDirection * speed * Time.deltaTime);
        }
        // Checks if the player is on the ground, then you can jump.
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * 2f * -gravity);
        }

        anim.SetBool("isGrounded", isGrounded);

        // Player is being affected by gravity.
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
