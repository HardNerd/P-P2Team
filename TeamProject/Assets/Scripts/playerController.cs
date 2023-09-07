using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerController : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] CharacterController controller;
    [SerializeField] Image healthRed;
    [SerializeField] Image healthYel;

    [Header("----- Player Stats -----")]
    [Range(1, 10)] [SerializeField] int HP = 10;
    [Range(3, 10)] [SerializeField] float playerSpeed = 7;
    [Range(1, 10)] [SerializeField] float jumpHeight = 2.7f;
    [Range(-35, -10)] [SerializeField] float gravityValue = -25;

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

    private float healthFillAmount;
    private float lastFillAmount;
    int origHP;

    void Start()
    {
        origHP = HP;
        healthFillAmount = HP / 10;
        lastFillAmount = HP / 10;
        spawnPlayer();
    }

    void Update()
    {
        Movement();
        if (Input.GetKeyDown(KeyCode.Return))
        {
            takeDamage(1);
        }
        moveHPBar();


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
            if (healthYel.fillAmount - healthRed.fillAmount > .2)
                healthYel.fillAmount = Mathf.Lerp(healthYel.fillAmount, healthRed.fillAmount, Time.deltaTime);
            else
                healthYel.fillAmount -= (lastFillAmount - healthFillAmount) * Time.deltaTime * 2;
        }
    }

    public void spawnPlayer()
    {
        HP = origHP;
        healthFillAmount = 1;
        healthYel.fillAmount = healthFillAmount;
        healthRed.fillAmount = healthFillAmount;
        lastFillAmount = 1;

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
                damageable.takeDamage(shootDamage);
        }

        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }
}
