using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    [Header("----- Components -----")]
    [SerializeField] CharacterController controller;

    [Header("----- Player Stats -----")]
    [Range(1, 10)] [SerializeField] int HP;
    [Range(3, 10)] [SerializeField] float playerSpeed;
    [Range(1, 15)] [SerializeField] float jumpHeight; // default: 2.5
    [Range(-35, -10)] [SerializeField] float gravityValue; // default: -25

    //[Header("----- Gun Stats -----")]
    //[SerializeField] float shootRate;
    //[SerializeField] int shootDamage;
    //[SerializeField] int shootDistance;

    private Vector3 move;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private int jumpedTimes;
    int maxJumps = 2;

    void Start()
    {
        
    }

    void Update()
    {
        Movement();
    }

    void Movement()
    {        
        // Set player's Y velocity to 0 when grounded and reset jumpedTimes num
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            jumpedTimes = 0;
            playerVelocity.y = 0f;
        }

        // Player WASD movement
        move = Input.GetAxis("Horizontal") * transform.right +
            Input.GetAxis("Vertical") * transform.forward;

        controller.Move(move * playerSpeed * Time.deltaTime);

        // Add jump force to player's Y velocity
        if (Input.GetButtonDown("Jump") && jumpedTimes < maxJumps)
        {
            jumpedTimes++;
            playerVelocity.y = jumpHeight;
        }

        // Add gravity to player's Y velocity and make him move
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}
