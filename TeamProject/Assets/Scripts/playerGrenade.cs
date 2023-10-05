using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrenade : MonoBehaviour
{
    [Header("References")]
    public Transform cam;
    public Transform attackPoint;
    public GameObject objectToThrow;

    [Header("Settings")]
    public int totalThrows;
    public float throwCooldown;

    [Header("Throwing")]
    public float throwForce;
    public float throwUpwwardForce;

    bool readyToThrow;
    private void Start()
    {
        readyToThrow = true; 
    }
    private void Update()
    {
        if(Input.GetButtonDown("throwable") && readyToThrow && totalThrows > 0)
        {
            StartCoroutine(Throw());
        }
    }
    IEnumerator Throw()
    {
        readyToThrow = false;
        GameObject projectile = Instantiate(objectToThrow, attackPoint.position, cam.rotation);
        
        Rigidbody projectilerb = projectile.GetComponent<Rigidbody>();
        
        Vector3 forceToAdd = cam.transform.forward * throwForce + transform.up * throwUpwwardForce;
        
        projectilerb.AddForce(forceToAdd, ForceMode.Impulse);
        
        totalThrows--;
        
        yield return new WaitForSeconds(throwCooldown);
        readyToThrow = true;
    }
    public void addThrowsMax(int addThrows)
    {
        totalThrows += addThrows;
    }
}
