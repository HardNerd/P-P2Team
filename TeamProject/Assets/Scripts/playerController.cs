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

    [Header("----- Gun Stats -----")]
    [SerializeField] public List<GunStats> GunList = new List<GunStats>();
    [SerializeField] GameObject gunModel;
    [SerializeField] float shootRate;
    [SerializeField] float shootDamage;
    [SerializeField] int shootDistance;
    [SerializeField] float reloadTime;
    [SerializeField] AudioClip shootSound;

    private Vector3 move;
    private Vector3 playerVelocity;
    private bool isGrounded;
    private int jumpedTimes;
    private bool isShooting;
    public bool isReloading;
    int maxJumps = 2;
    float maxHP;
    float baseSpeed;
    float maxStam;
    public int selectedGun;
    public bool sprintCooldown;
    //bool isPlayingSoundEffect;

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
        GunSelector();
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


        if (Input.GetButton("Fire1") && !isShooting && !GameManager.instance.isPause && !isReloading)
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
        if (Input.GetButtonDown("Jump") && jumpedTimes < maxJumps && stamina > 20)
        {
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

    IEnumerator shoot()
    {
        if (GunList.Count > 0)
        {
            if (GunList[selectedGun].loadedAmmo > 0)
            {


                isShooting = true;
                GunList[selectedGun].loadedAmmo--;
                GameManager.instance.ammoUpdate(GunList[selectedGun].loadedAmmo, GunList[selectedGun].maxAmmoCarried);
                AudioSource.PlayClipAtPoint(shootSound, transform.position);

                RaycastHit hitInfo;
                Ray ray = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f));

                if (Physics.Raycast(ray, out hitInfo, shootDistance))
                {
                    IDamage damageable = hitInfo.collider.GetComponent<IDamage>();
                    Instantiate(GunList[selectedGun].hitEffect, hitInfo.point, GunList[selectedGun].hitEffect.transform.rotation);
                    if (damageable != null)
                        damageable.TakeDamage(shootDamage);
                }

                yield return new WaitForSeconds(shootRate);
                isShooting = false;
            }
        }
    }
   

    public void GunPickup(GunStats gun)
    {
        GunList.Add(gun);
        shootDamage = gun.shootDamage;
        shootDistance = gun.shootDistance;
        shootRate = gun.shootRate;
        shootSound = gun.gunSound;
        reloadTime = gun.reloadTime;

        gunModel.GetComponent<MeshFilter>().sharedMesh = gun.model.GetComponent <MeshFilter>().sharedMesh;
        gunModel.GetComponent<Renderer>().sharedMaterial = gun.model.GetComponent<Renderer>().sharedMaterial;

        selectedGun = GunList.Count - 1;
        GameManager.instance.ammoUpdate(GunList[selectedGun].loadedAmmo, GunList[selectedGun].maxAmmoCarried);
    }

    void  GunSelector()
    {
        if(Input.GetAxis("Mouse ScrollWheel") > 0 && selectedGun < GunList.Count - 1)
        {
            selectedGun++;
            GunChange();
        }
       else if (Input.GetAxis("Mouse ScrollWheel") < 0 && selectedGun > 0)
        {
            selectedGun--;
            GunChange();
        }
    }

    void GunChange()
    {
        shootDamage = GunList[selectedGun].shootDamage;
        shootDistance = GunList[selectedGun].shootDistance;
        shootRate = GunList[selectedGun].shootRate;
        shootSound = GunList[selectedGun].gunSound;
        reloadTime = GunList[selectedGun].reloadTime;
        GameManager.instance.ammoUpdate(GunList[selectedGun].loadedAmmo, GunList[selectedGun].maxAmmoCarried);

        gunModel.GetComponent<MeshFilter>().sharedMesh = GunList[selectedGun].model.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<Renderer>().sharedMaterial = GunList[selectedGun].model.GetComponent<Renderer>().sharedMaterial;
    }
}