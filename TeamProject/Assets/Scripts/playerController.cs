using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerController : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] CharacterController controller;

    [Header("----- Player Stats -----")]
    [Range(1, 10)][SerializeField] int HP = 10;
    [Range(3, 10)][SerializeField] float playerSpeed = 7;
    [Range(1, 10)][SerializeField] float jumpHeight = 2.7f;
    [Range(-35, -10)][SerializeField] float gravityValue = -25;

    [Header("----- Gun Stats -----")]
    [SerializeField] float shootRate = 2;
    [SerializeField] int shootDamage = 1;
    [SerializeField] int shootDistance = 15;

    private Vector3 move;
    private Vector3 playerVelocity;
    private bool isGrounded;
    private int jumpedTimes;
    private bool isShooting;
    int maxJumps = 2;
    int maxHP;

    void Start()
    {
        maxHP = HP;
        GameManager.instance.healthRedFillAmt = HP / maxHP;
        GameManager.instance.healthYelFillAmt = HP / maxHP;
        spawnPlayer();
    }

    void Update()
    {
        Movement();
        if (Input.GetKeyDown(KeyCode.Return))
        {
            TakeDamage(1);
        }
        GameManager.instance.moveHPBar();


        if (Input.GetButton("Fire1") && !isShooting)
            StartCoroutine(shoot());
    }

    void Movement()
    {
        // Set player's Y velocity to 0 when grounded and reset jumpedTimes num
        isGrounded = controller.isGrounded;
        if (isGrounded && playerVelocity.y < 0)
        {
            jumpedTimes = 0;
            playerVelocity.y = 0f;
        }

        // Player WASD movement
        move = Input.GetAxis("Horizontal") * transform.right +
            Input.GetAxis("Vertical") * transform.forward;

        controller.Move(move * playerSpeed * Time.deltaTime);

        // Add jump velocity to player's Y value
        if (Input.GetButtonDown("Jump") && jumpedTimes < maxJumps)
        {
            jumpedTimes++;

            // physics equation to get the exact velocity based on desired height and gravity
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravityValue);
        }

        // Add gravity to player's Y velocity and make him move
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    public void TakeDamage(int damageAmount)
    {
        updateHealth(damageAmount);

        if (HP <= 0)
        {
            GameManager.instance.loseScreen();
        }
    }

    public void updateHealth(int amount)
    {
        GameManager.instance.healthYelFillAmt = (float)HP / 10;
        
        HP -= amount;

        //If we introduce an upgrade system to add more max hp, I'll update this
        Mathf.Clamp(HP, 0, 10);

        GameManager.instance.healthRedFillAmt = (float)HP / 10;

        GameManager.instance.healthRed.fillAmount = GameManager.instance.healthRedFillAmt;
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

    IEnumerator shoot()
    {
        isShooting = true;

        RaycastHit hitInfo;
        Ray ray = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f));

        if (Physics.Raycast(ray, out hitInfo, shootDistance))
        {
            IDamage damageable = hitInfo.collider.GetComponent<IDamage>();

            if (damageable != null)
                damageable.TakeDamage(shootDamage);
        }

        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }
}