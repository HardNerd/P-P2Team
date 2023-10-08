using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class playerController : MonoBehaviour, IDamage, IPhysics, IDataPersistence
{
    
    [Header("----- Components -----")]
    [SerializeField] CharacterController controller;
    [SerializeField] ParticleSystem jumpParticles;
    [SerializeField] InventoryObjects Inventory;
    private int deathCount = 0;

    [Header("----- Player Stats -----")]
    [Range(1, 10)][SerializeField] float HP = 10;
    [Range(0, 100)][SerializeField] float stamina = 100;
    [Range(3, 10)][SerializeField] float playerSpeed = 7;
    [SerializeField] float coyoteTime; // small delay allowing player to jump after being grounded
    [Range(1, 10)][SerializeField] float minJumpHeight;
    [SerializeField] float jumpStartTime;
    [Range(-35, -10)][SerializeField] float gravityValue = -25;
    [Range(1, 10)][SerializeField] int pushBackResolve;
    [SerializeField] float boosetedSpeed;
    [SerializeField] float speedCoolDown;

    // Player movement
    private Vector3 move;
    private Vector3 playerVelocity;
    public Vector2 player2DVelocity;
    Vector2 previousFramePos = Vector2.zero;
    float baseSpeed;
    
    float maxStam;
    public bool sprintCooldown;
    
    private Vector3 pushBack;
   
    // Player Jump
    private bool isGrounded;
    private int jumpedTimes;
    private float coyoteTimeCounter;
    private float jumpTime;
    private bool isJumping;
    int initMaxJumps = 1;
    int maxJumps;

    float maxHP;
    public float healthPcakValue = 10;

    // Player mutators
    bool canSprint = true;
    bool canJump = true; // double jump is handled with initMaxJumps
    bool canDash;

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
        //might cause issues when trying to load in player after going through save data
        //since it is trying to spawn player before new player spawn location is set
        spawnPlayer();
    }

    void Update()
    {
        Movement();
        
        if (Input.GetKey(KeyCode.LeftShift) && sprintCooldown == false && canSprint)
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
        if (pushBack.magnitude > 0.01f)
        {
            //pushBack = Vector3.Lerp(pushBack, Vector3.zero, Time.deltaTime * pushBackResolve);
            pushBack.x = Mathf.Lerp(pushBack.x, 0, Time.deltaTime * pushBackResolve);
            pushBack.y = Mathf.Lerp(pushBack.y, 0, Time.deltaTime * pushBackResolve * 3);
            pushBack.z = Mathf.Lerp(pushBack.z, 0, Time.deltaTime * pushBackResolve);
        }

        // Set player's Y velocity to 0 when grounded and reset jumpedTimes num
        isGrounded = controller.isGrounded;
        if (isGrounded && playerVelocity.y < 0)
        {
            maxJumps = initMaxJumps;
            jumpedTimes = 0;
            playerVelocity.y = 0f;
            coyoteTimeCounter = coyoteTime;
        }

        // Player WASD movement
        move = Input.GetAxis("Horizontal") * transform.right +
            Input.GetAxis("Vertical") * transform.forward;

        controller.Move(move * playerSpeed * Time.deltaTime);

        // If player walks off a platform
        if (!isGrounded && playerVelocity.y < -1 && jumpedTimes == 0)
        {
            coyoteTimeCounter -= Time.deltaTime;

            // set maxJumps to 1 if you haven't jumped, you start falling and coyoteTime is up
            if (coyoteTimeCounter <= 0f)
                maxJumps = initMaxJumps - 1;
        }

        // Add jump velocity to player's Y value
        if (Input.GetButtonDown("Jump") && jumpedTimes < maxJumps && stamina > 20 && canJump)
        {
            isJumping = true;
            jumpTime = jumpStartTime;

            // Set player particles
            if(jumpedTimes >= 1)
            {
                Vector3 jumpoffset = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
                Instantiate(jumpParticles, jumpoffset, jumpParticles.transform.rotation);
            }
            updateStam(20);
            jumpedTimes++;

            // physics equation to get the exact velocity based on desired height and gravity
            playerVelocity.y = Mathf.Sqrt(minJumpHeight * -2f * gravityValue);
        }
        if (Input.GetButton("Jump") && isJumping)
        {
            if (jumpTime > 0)
            {
                playerVelocity.y = Mathf.Sqrt(minJumpHeight * -2f * gravityValue);
                jumpTime -= Time.deltaTime;
            }
            else
                isJumping = false;
        }

        if (Input.GetButtonUp("Jump"))
            isJumping = false;

        // Add gravity to player's Y velocity and make him move
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move((playerVelocity + pushBack) * Time.deltaTime);

        // Calculate player2DVelocity for bullet prediction
        player2DVelocity = (new Vector2(transform.position.x, transform.position.z) - previousFramePos) / Time.deltaTime;
        previousFramePos = new Vector2(transform.position.x, transform.position.z);
    }

    public void TakeDamage(float damageAmount)
    {
        updateHealth(damageAmount);

        if (HP <= 0)
        {
            deathCount++;
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
        pushBack.x = 0; 
        pushBack.y = 0; 
        pushBack.z = 0;
        transform.position = GameManager.instance.playerSpawnPOS.transform.position;
        controller.enabled = true;
    }
    public void physics(Vector3 direction)
    {
        pushBack += direction;
    }

    public void healthPickup()
    {
        if(Input.GetButton("Interact"))
        {
            if (HealthPickup.hasPickedUpHealthPack)
            {
                HP = HP + healthPcakValue;
                HealthPickup.hasPickedUpHealthPack = false;
                GameManager.instance.healthRedFillAmt = (float)HP / 10;

                GameManager.instance.healthRed.fillAmount = GameManager.instance.healthRedFillAmt;
                if (HP + healthPcakValue > maxHP)
                {
                    HP = maxHP;
                }
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        Item items = other.GetComponent<Item>();
        if(items != null)
        {
            Inventory.AddItem(items.item);
            GameManager.instance.displayInventory.DisplayItem();
            Destroy(other.gameObject);
        }
        if(other.CompareTag("SpeedBoost"))
        {
            playerSpeed = boosetedSpeed;
            StartCoroutine(SpeedBoostDuration());
        }


    }

    IEnumerator SpeedBoostDuration()
    {
        yield return new WaitForSeconds(speedCoolDown);
        playerSpeed = baseSpeed;
    }

    //private void OnApplicationQuit()
    //{
    //    Inventory.Container.Clear();
    //}

    void IDataPersistence.LoadData(GameData data)
    {
        
        this.deathCount = data.deathCount;
    }

    void IDataPersistence.SaveData(GameData data)
    {
        data.deathCount = this.deathCount;
    }
}

