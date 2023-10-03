using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class playerController : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] CharacterController controller;
    [SerializeField] ParticleSystem jumpparticles;

    [Header("----- Player Stats -----")]
    [Range(1, 10)][SerializeField] float HP = 10;
    [Range(0, 100)][SerializeField] float stamina = 100;
    [Range(3, 10)][SerializeField] float playerSpeed = 7;
    [Range(1, 10)][SerializeField] float jumpHeight = 2.7f;
    [Range(-35, -10)][SerializeField] float gravityValue = -25;

    private Vector3 move;
    private Vector3 playerVelocity;
    private bool isGrounded;
    private int jumpedTimes;
    int initMaxJumps = 2;
    int maxJumps;
    float maxHP;
    float baseSpeed;
    float maxStam;
    public bool sprintCooldown;

    void Start()
    {
        maxHP = HP;
        baseSpeed = playerSpeed;
        maxStam = stamina;
        sprintCooldown = false;
        GameManager.instance.healthRedFillAmt = HP / maxHP;
        GameManager.instance.healthYelFillAmt = HP / maxHP;
        GameManager.instance.stamBlueFillAmt = stamina / maxStam;
        GameManager.instance.stamYelFillAmt = stamina / maxStam;
        spawnPlayer();
    }

    void Update()
    {
        Movement();
        if (Input.GetKey(KeyCode.LeftShift) && sprintCooldown == false)
        {
            sprint();
        }
        else
        {
            stamRestore();
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            TakeDamage(1);
        }
        GameManager.instance.moveHPBar();
        GameManager.instance.moveStamBar();
    }

    void Movement()
    {
        Debug.Log(playerVelocity.y);
        // Set player's Y velocity to 0 when grounded and reset jumpedTimes num
        isGrounded = controller.isGrounded;
        if (isGrounded && playerVelocity.y < 0)
        {
            maxJumps = initMaxJumps;
            jumpedTimes = 0;
            playerVelocity.y = 0f;
        }

        // Player WASD movement
        move = Input.GetAxis("Horizontal") * transform.right +
            Input.GetAxis("Vertical") * transform.forward;

        controller.Move(move * playerSpeed * Time.deltaTime);

        // set maxJumps to 1 if you haven't jumped and you start falling
        if (!isGrounded && playerVelocity.y < -1 && jumpedTimes == 0)
            maxJumps = 1;

        // Add jump velocity to player's Y value
        if (Input.GetButtonDown("Jump") && jumpedTimes < maxJumps && stamina > 20)
        {
            // Set player particles
            if(jumpedTimes >= 1)
            {
                Vector3 jumpoffset = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
                Instantiate(jumpparticles, jumpoffset, jumpparticles.transform.rotation);
            }
            updateStam(20);
            jumpedTimes++;

            // physics equation to get the exact velocity based on desired height and gravity
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravityValue);
        }


        // Add gravity to player's Y velocity and make him move
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    public void TakeDamage(float damageAmount)
    {
        updateHealth(damageAmount);

        if (HP <= 0)
        {
            GameManager.instance.loseScreen();
        }
    }

    public void updateHealth(float amount)
    {
        GameManager.instance.healthYelFillAmt = (float)HP / 10;
        
        HP -= amount;

        //If we introduce an upgrade system to add more max hp, I'll update this
        Mathf.Clamp(HP, 0, 10);

        GameManager.instance.healthRedFillAmt = (float)HP / 10;

        GameManager.instance.healthRed.fillAmount = GameManager.instance.healthRedFillAmt;
    }

    public void sprint()
    {
        if(isGrounded)
        {
            updateStam(Time.deltaTime * 40);
            playerSpeed = baseSpeed * 2.5f;
        }
        if (stamina <= 0)
        {
            StartCoroutine(sprintExhaust());
        }
    }

    public void stamRestore()
    {
        if (stamina < 100 && isGrounded)
        {
            playerSpeed = baseSpeed;
            updateStam(-(Time.deltaTime * 30));
        }
    }

    public void updateStam(float amount)
    {
        GameManager.instance.stamYelFillAmt = stamina / maxStam;
        stamina -= amount;

        GameManager.instance.stamBlueFillAmt = stamina / maxStam;
        GameManager.instance.stamBlue.fillAmount = GameManager.instance.stamBlueFillAmt;
    }

    IEnumerator sprintExhaust()
    {
        sprintCooldown = true;
        yield return new WaitForSeconds(3);
        sprintCooldown = false;
    }

    public void spawnPlayer()
    {
        HP = maxHP;
        GameManager.instance.healthRedFillAmt = 1;
        GameManager.instance.healthYelFillAmt = 1;
        GameManager.instance.healthYel.fillAmount = 1;
        GameManager.instance.healthRed.fillAmount = 1;

        controller.enabled = false;
        transform.position = GameManager.instance.playerSpawnPOS.transform.position;
        controller.enabled = true;
    }
}