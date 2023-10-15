using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class cameraMovement : MonoBehaviour
{
    [SerializeField] int sensitivity = 300;

    [SerializeField] int lockXRotMin = -90;
    [SerializeField] int lockXRotMax = 90;

    [SerializeField] bool invertY;

    float xRotation;

    void Start()
    {
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
            GameManager.instance.statePause();
        transform.localRotation = Quaternion.Euler(transform.parent.forward);
    }

    void Update()
    {
        // Input
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensitivity;

        if (invertY)
            xRotation += mouseY;
        else
            xRotation -= mouseY;

        // Clamp rotation and rotate camera on X-Axis
        xRotation = Mathf.Clamp(xRotation, lockXRotMin, lockXRotMax);
        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        // Rotate player on Y-Axis
        transform.parent.Rotate(Vector3.up * mouseX);
    }
}
