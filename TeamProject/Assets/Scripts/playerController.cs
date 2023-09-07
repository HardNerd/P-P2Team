using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField] Image healthRed;
    [SerializeField] Image healthYel;
    private float healthFillAmount;
    private float lastFillAmount;

    void Start()
    {
        healthFillAmount = HP / 10;
        lastFillAmount = HP / 10;
    }

    void Update()
    {
        Movement();
        if (Input.GetKeyDown(KeyCode.Return))
        {
            takeDamage(1);
        }
        moveHPBar();

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

    public void takeDamage(int damageAmount)
    {
        updateHealth(damageAmount);

        if (HP <= 0)
        {
            GameManager.instance.loseScreen();
        }
    }

    public void updateHealth(int amount)
    {
        float lastHP = HP;
        HP -= amount;

        //If we introduce an upgrade system to add more max hp, I'll update this
        Mathf.Clamp(HP, 0, 10);

        healthFillAmount = (float)HP / 10;
        lastFillAmount = (float)lastHP / 10;

        Debug.Log(healthFillAmount);

        healthRed.fillAmount = healthFillAmount;
    }

    public void moveHPBar()
    {
        if (healthYel.fillAmount > healthRed.fillAmount)
        {
            healthYel.fillAmount -= (lastFillAmount - healthFillAmount) * Time.deltaTime;
        }
    }

    public void spawnPlayer()
    {
        HP = 10;
        healthFillAmount = 1;
        healthYel.fillAmount = healthFillAmount;
        healthRed.fillAmount = healthFillAmount;
        lastFillAmount = 1;

        //include respawn here;
    }
}
